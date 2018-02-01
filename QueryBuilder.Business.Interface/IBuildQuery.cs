namespace QueryBuilder.Business.Interface {
  public interface IBuildQuery {
    string Build();
    IBuildQuery Comment(string comment);
    IBuildQuery Select(string columnName);
    IBuildQuery From(string tableName, string alias);
    IBuildQuery InnerJoin(IBuildInnerJoinQuery innerJoinQueryBuilder);
    IBuildQuery Where(IBuildWhereQuery whereQueryBuilder);
    IBuildQuery GroupBy(string columnName);
    IBuildQuery Having(IBuildHavingQuery havingQueryBuilder);
    string GetTableAlias();
  }
}