using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QueryBuilder.Business.Interface;
using QueryBuilder.Business.Interface.Operators;

namespace QueryBuilder.Business {
  public class WhereQueryBuilder : IBuildWhereQuery {
    private readonly LogicalOperator _LogicalOperator;
    private readonly List<string> _Conditions;
    private readonly List<string> _AndConditions;

    public LogicalOperator LogicalOperator => _LogicalOperator;
    public List<string> Conditions => _Conditions;

    public WhereQueryBuilder(LogicalOperator logicalOperator, params string[] conditions) {
      _LogicalOperator = logicalOperator;
      _Conditions = new List<string>(conditions);
      _AddBaseCondition(logicalOperator);
      _AndConditions = new List<string>();
    }

    private void _AddBaseCondition(LogicalOperator logicalOperator) {
      string baseCondition = (logicalOperator == LogicalOperator.AND).ToString().ToUpper();

      if (logicalOperator == LogicalOperator.OR && _Conditions.Count == 0) {
        baseCondition = (true).ToString().ToUpper();
      }

      _Conditions.Insert(0, baseCondition);
    }

    public string Build() {
      StringBuilder whereQuery = new StringBuilder("WHERE ");
      whereQuery.Append(_CreateConditions());
      whereQuery.Append(_CreateAndCondition());

      return whereQuery.ToString();
    }

    private string _CreateConditions() {
      if (!_Conditions.Any()) return null;

      StringBuilder conditions = new StringBuilder();
      conditions.Append("(");

      foreach (string condition in _Conditions) {
        conditions.Append($"{condition}");

        if (!_IsLastItem(_Conditions, condition)) {
          string locicalOperator = _LogicalOperator == LogicalOperator.AND ? "AND" : "OR";

          conditions.Append($"{Environment.NewLine}\t{locicalOperator} ");
        }
        else {
          conditions.Append($"){Environment.NewLine}");
        }
      }

      return conditions.ToString();
    }

    private string _CreateAndCondition() {
      if (!_AndConditions.Any()) return null;

      StringBuilder andConditionQuery = new StringBuilder();

      foreach (string andCondition in _AndConditions) {
        andConditionQuery.Append($"\tAND {andCondition}{Environment.NewLine}");
      }

      return andConditionQuery.ToString();
    }

    private bool _IsLastItem(List<string> collection, string item) {
      return collection.IndexOf(item) == collection.Count - 1;
    }

    public IBuildWhereQuery And(string condition) {
      if (!string.IsNullOrWhiteSpace(condition)) _AndConditions.Add(condition);

      return this;
    }
  }
}