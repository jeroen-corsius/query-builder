using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QueryBuilder.Business.Interface;
using QueryBuilder.Business.Interface.Operators;

namespace QueryBuilder.Business {
  public class HavingQueryBuilder : IBuildHavingQuery {
    private readonly LogicalOperator _LogicalOperator;
    private readonly List<string> _Conditions;

    public HavingQueryBuilder(string statement)
      : this(LogicalOperator.AND, statement) {
      
    }

    public HavingQueryBuilder(LogicalOperator logicalOperator, params string[] conditions) {
      if (conditions.Any(string.IsNullOrWhiteSpace)) throw new Exception("Unable to instantiate HavingQueryBuilder without statement(s)");

      _LogicalOperator = logicalOperator;
      _Conditions = new List<string>(conditions);
    }

    public string Build() {
      StringBuilder havingQuery = new StringBuilder("HAVING ");
      havingQuery.Append(_CreateConditions());

      return havingQuery.ToString();
    }

    private string _CreateConditions() {
      if (!_Conditions.Any()) return null;

      StringBuilder conditions = new StringBuilder();
      
      foreach (string condition in _Conditions) {
        conditions.Append($"{condition}");

        if (!_IsLastItem(_Conditions, condition)) {
          string locicalOperator = _LogicalOperator == LogicalOperator.AND ? "AND" : "OR";

          conditions.Append($"{Environment.NewLine}\t{locicalOperator} ");
        }
        else {
          conditions.Append($"{Environment.NewLine}");
        }
      }

      return conditions.ToString();
    }

    private bool _IsLastItem(List<string> collection, string item) {
      return collection.IndexOf(item) == collection.Count - 1;
    }
  }
}