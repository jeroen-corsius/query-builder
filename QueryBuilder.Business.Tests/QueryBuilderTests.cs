using System;
using NUnit.Framework;
using QueryBuilder.Business.Interface;
using QueryBuilder.Business.Interface.Operators;

namespace QueryBuilder.Business.Tests {
  [TestFixture]
  public class QueryBuilderTests {
    [Test]
    public void ThrowsException_When_Empty() {
      string expectedExceptionMessage = "Unable to build empty query";

      Exception exception = Assert.Throws<Exception>(() => new QueryBuilder().Build());

      Assert.AreEqual(expectedExceptionMessage, exception.Message);
    }

    [Test]
    public void BuildsQuery_WithSingleComment() {
      string comment = "Test comment";
      string expectedQuery = $"-- {comment}\r\n";

      QueryBuilder queryBuilder = new QueryBuilder();
      queryBuilder.Comment(comment);

      string actualQuery = queryBuilder.Build();

      Assert.AreEqual(expectedQuery, actualQuery);
    }

    [Test]
    public void BuildsQuery_WithMultipleComments() {
      string comment1 = "Test comment 1";
      string comment2 = "Test comment 2";
      string expectedQuery = $"-- {comment1}\r\n-- {comment2}\r\n";

      QueryBuilder queryBuilder = new QueryBuilder();
      queryBuilder.Comment(comment1);
      queryBuilder.Comment(comment2);

      string actualQuery = queryBuilder.Build();

      Assert.AreEqual(expectedQuery, actualQuery);
    }

    [Test]
    public void BuildsQuery_WithSingleSelect() {
      string columnName = "ColumnName";
      string expectedQuery = $"SELECT\r\n\t{columnName}\r\n";

      QueryBuilder queryBuilder = new QueryBuilder();
      queryBuilder.Select(columnName);

      string actualQuery = queryBuilder.Build();

      Assert.AreEqual(expectedQuery, actualQuery);
    }

    [Test]
    public void BuildsQuery_WithMultipleSelect() {
      string columnName1 = "ColumnName1";
      string columnName2 = "ColumnName2";
      string expectedQuery = $"SELECT\r\n\t{columnName1},\r\n\t{columnName2}\r\n";

      QueryBuilder queryBuilder = new QueryBuilder();
      queryBuilder
        .Select(columnName1)
        .Select(columnName2);

      string actualQuery = queryBuilder.Build();

      Assert.AreEqual(expectedQuery, actualQuery);
    }

    [Test]
    public void BuildsQuery_WithFrom() {
      string tableName = "TableName";
      string alias = "tn";
      string expectedQuery = $"FROM {tableName} {alias}\r\n";

      QueryBuilder queryBuilder = new QueryBuilder();
      queryBuilder.From(tableName, alias);

      string actualQuery = queryBuilder.Build();

      Assert.AreEqual(expectedQuery, actualQuery);
    }

    [Test]
    public void BuildsQuery_WithInnerJoin() {
      string tableName = "bc_core_doubleclickcampaignmanager_lld.ref_paid_search ps";
      string onStatement = "i.segment_value_1 = ps.legacykeywordid";
      string andStatement = "i.event_type = 'CONVERSION'";
      string expectedQuery = $"INNER JOIN {tableName}\r\n\tON {onStatement}\r\n\tAND {andStatement}\r\n";

      QueryBuilder queryBuilder = new QueryBuilder();
      queryBuilder.InnerJoin(
        new InnerJoinQueryBuilder(tableName)
        .On(onStatement)
        .And(andStatement)
      );

      string actualQuery = queryBuilder.Build();

      Assert.AreEqual(expectedQuery, actualQuery);
    }

    [Test]
    public void BuildsQuery_WithWhere() {
      string firstCondition = "i.event_type = 'CONVERSION'";
      string secondCondition = "i.segment_value_1 = ps.legacykeywordid";
      string expectedQuery = $"WHERE (TRUE\r\n\tAND {firstCondition})\r\n\tAND {secondCondition}\r\n";

      QueryBuilder queryBuilder = new QueryBuilder();
      queryBuilder.Where(
        new WhereQueryBuilder(LogicalOperator.AND, firstCondition)
        .And(secondCondition)
      );

      string actualQuery = queryBuilder.Build();

      Assert.AreEqual(expectedQuery, actualQuery);
    }

    [Test]
    public void BuildsQuery_WithGroupBy() {
      string tableName = "ColumnName";
      string expectedQuery = $"GROUP BY {tableName}\r\n";

      QueryBuilder queryBuilder = new QueryBuilder();
      queryBuilder.GroupBy(tableName);

      string actualQuery = queryBuilder.Build();

      Assert.AreEqual(expectedQuery, actualQuery);
    }

    [Test]
    public void BuildsQuery_WithHaving() {
      string havingStatement = "SUM(CASE i.event_type WHEN 'CLICK' THEN 1 ELSE 0 END) >= 12";
      string expectedQuery = $"HAVING {havingStatement}\r\n";

      QueryBuilder queryBuilder = new QueryBuilder();
      queryBuilder.Having(new HavingQueryBuilder(havingStatement));

      string actualQuery = queryBuilder.Build();

      Assert.AreEqual(expectedQuery, actualQuery);
    }

    [Test]
    public void Build_ResultsInResettingQueryBuilder() {
      string expectedExceptionMessage = "Unable to build empty query";

      IBuildQuery queryBuilder = new QueryBuilder()
      .Comment("Comment")
        .Select("Select1")
        .Select("Select2")
        .From("From", "Alias")
        .InnerJoin(new InnerJoinQueryBuilder("InnerJoin")
          .On("On")
          .And("And"))
        .Where(new WhereQueryBuilder(LogicalOperator.AND, "Condition1", "Condition2")
          .And("And")
          .And("And"))
        .GroupBy("GroupBy")
        .Having(new HavingQueryBuilder("Having"));

      string firstBuildResult = queryBuilder.Build();
      Exception exception = Assert.Throws<Exception>(() => queryBuilder.Build());

      Assert.IsNotEmpty(firstBuildResult);
      Assert.AreEqual(expectedExceptionMessage, exception.Message);
    }
  }
}