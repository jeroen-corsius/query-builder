using System.Collections.Generic;
using QueryBuilder.Business.Interface.Operators;

namespace QueryBuilder.Business.Interface {
  public interface IBuildWhereQuery {
    LogicalOperator LogicalOperator { get; }
    List<string> Conditions { get; }
    string Build();
    IBuildWhereQuery And(string condition);
  }
}