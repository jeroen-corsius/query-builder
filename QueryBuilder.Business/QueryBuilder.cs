using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QueryBuilder.Business.Interface;
using QueryBuilder.Business.Interface.Operators;

namespace QueryBuilder.Business {
  public class QueryBuilder : IBuildQuery {
    private List<string> _Comment;
    private List<string> _Select;
    private string _From;
    private List<IBuildInnerJoinQuery> _InnerJoin;
    private IBuildWhereQuery _Where;
    private string _GroupBy;
    private IBuildHavingQuery _Having;
    private string _Alias;

    public QueryBuilder() {
      _Comment = new List<string>();
      _Select = new List<string>();
      _InnerJoin = new List<IBuildInnerJoinQuery>();
    }

    public string Build() {
      if (_IsEmpty()) throw new Exception("Unable to build empty query");

      StringBuilder query = new StringBuilder();
      query.Append(_CreateCommentStatements());
      query.Append(_CreateSelectStatements());
      query.Append(_CreateFromStatement());
      query.Append(_CreateInnerJoinStatements());
      query.Append(_CreateWhereStatements());
      query.Append(_CreateGroupByStatement());
      query.Append(_CreateHavingStatement());

      _Reset();

      return query.ToString();
    }

    public IBuildQuery Comment(string comment) {
      _Comment.Add(comment);

      return this;
    }

    public IBuildQuery Select(string columnName) {
      _Select.Add(columnName);

      return this;
    }

    public IBuildQuery From(string tableName, string alias) {
      _From = tableName;
      _Alias = alias;

      return this;
    }

    public IBuildQuery InnerJoin(IBuildInnerJoinQuery innerJoinQueryBuilder) {
      if (innerJoinQueryBuilder != null) _InnerJoin.Add(innerJoinQueryBuilder);

      return this;
    }

    public IBuildQuery Where(IBuildWhereQuery whereQueryBuilder) {
      if (whereQueryBuilder == null) return this;

      if (_Where == null) {
        _Where = whereQueryBuilder;
      }
      else {
        switch (whereQueryBuilder.LogicalOperator) {
          case LogicalOperator.AND:
            foreach (string condition in whereQueryBuilder.Conditions) {
              _Where.And(condition);
            }
            break;
          default:
          throw new NotImplementedException($"LogicalOperator '{whereQueryBuilder.LogicalOperator}' is not implemented");
        }
      }
      
      return this;
    }

    public IBuildQuery GroupBy(string columnName) {
      _GroupBy = columnName;

      return this;
    }

    public IBuildQuery Having(IBuildHavingQuery havingQueryBuilder) {
      _Having = havingQueryBuilder;

      return this;
    }

    public string GetTableAlias() {
      return _Alias;
    }

    private void _Reset() {
      _Comment = new List<string>();
      _Select = new List<string>();
      _From = null;
      _InnerJoin = new List<IBuildInnerJoinQuery>();
      _Where = null;
      _GroupBy = null;
      _Having = null;
    }

    private string _CreateCommentStatements() {
      if (!_Comment.Any()) return null;

      StringBuilder comments = new StringBuilder();

      foreach (string comment in _Comment) {
        comments.Append($"-- {comment}{Environment.NewLine}");
      }

      return comments.ToString();
    }

    private string _CreateSelectStatements() {
      if (!_Select.Any()) return null;

      StringBuilder selectStatements = new StringBuilder();
      selectStatements.AppendLine("SELECT");

      foreach (string select in _Select) {
        selectStatements.Append($"\t{select}");

        if (!_IsLastItem(_Select, select)) {
          selectStatements.Append(',');
        }

        selectStatements.Append(Environment.NewLine);
      }

      return selectStatements.ToString();
    }

    private string _CreateFromStatement() {
      if (string.IsNullOrWhiteSpace(_From) || string.IsNullOrWhiteSpace(_Alias)) return null;

      return $"FROM {_From} {_Alias}{Environment.NewLine}";
    }

    private string _CreateInnerJoinStatements() {
      if (!_InnerJoin.Any()) return null;

      StringBuilder innerJoinStatements = new StringBuilder();

      foreach (InnerJoinQueryBuilder innerJoinQueryBuilder in _InnerJoin) {
        innerJoinStatements.Append(innerJoinQueryBuilder.Build());
      }

      return innerJoinStatements.ToString();
    }

    private string _CreateWhereStatements() {
      if (_Where == null) return null;

      return _Where.Build();
    }

    private string _CreateGroupByStatement() {
      if (string.IsNullOrWhiteSpace(_GroupBy)) return null;

      return $"GROUP BY {_GroupBy}{Environment.NewLine}";
    }

    private string _CreateHavingStatement() {
      if (_Having == null) return null;

      return _Having.Build();
    }

    private bool _IsLastItem(List<string> collection, string item) {
      return collection.IndexOf(item) == collection.Count - 1;
    }

    private bool _IsEmpty() {
      return 
        !_Comment.Any() && 
        !_Select.Any() && string.IsNullOrWhiteSpace(_From) && 
        !_InnerJoin.Any() && 
        _Where == null && string.IsNullOrWhiteSpace(_GroupBy) && 
        _Having == null;
    }
  }
}