using System;
using System.Text;
using QueryBuilder.Business.Interface;

namespace QueryBuilder.Business {
  public class InnerJoinQueryBuilder : IBuildInnerJoinQuery {
    private readonly string _TableName;
    private string _On;
    private string _And;

    public InnerJoinQueryBuilder(string tableName) {
      if (string.IsNullOrWhiteSpace(tableName)) throw new Exception("Unable to instantiate InnerJoinQueryBuilder without table name");

      _TableName = tableName;
    }

    public string Build() {
      StringBuilder innerJoinQuery = new StringBuilder();
      innerJoinQuery.Append(_CreateInnerJoinStatement());
      innerJoinQuery.Append(_CreateOnStatement());
      innerJoinQuery.Append(_CreateAndStatement());

      return innerJoinQuery.ToString();
    }

    private string _CreateInnerJoinStatement() {
      return $"INNER JOIN {_TableName}{Environment.NewLine}";
    }

    private string _CreateOnStatement() {
      return $"\tON {_On}{Environment.NewLine}";
    }

    private string _CreateAndStatement() {
      if (string.IsNullOrWhiteSpace(_And)) return null;

      return $"\tAND {_And}{Environment.NewLine}";
    }

    public IBuildInnerJoinQuery On(string onStatement) {
      _On = onStatement;

      return this;
    }

    public IBuildInnerJoinQuery And(string andStatement) {
      _And = andStatement;

      return this;
    }
  }
}