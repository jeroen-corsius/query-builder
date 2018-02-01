namespace QueryBuilder.Business.Interface {
  public interface IBuildInnerJoinQuery {
    string Build();
    IBuildInnerJoinQuery On(string onStatement);
    IBuildInnerJoinQuery And(string andStatement);
  }
}