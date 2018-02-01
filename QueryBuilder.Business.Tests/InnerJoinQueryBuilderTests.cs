using System;
using NUnit.Framework;

namespace QueryBuilder.Business.Tests {
  [TestFixture]
  public class InnerJoinQueryBuilderTests {
    [Test]
    public void ThrowsException_When_Empty() {
      string expectedExceptionMessage = "Unable to instantiate InnerJoinQueryBuilder without table name";

      Exception exception = Assert.Throws<Exception>(() => new InnerJoinQueryBuilder(null));

      Assert.AreEqual(expectedExceptionMessage, exception.Message);
    }

    [Test]
    public void BuildsQuery_WithOn() {
      string tableName = "bc_core_doubleclickcampaignmanager_lld.ref_paid_search ps";
      string onStatement = "i.segment_value_1 = ps.legacykeywordid";
      string expectedQuery = $"INNER JOIN {tableName}\r\n\tON {onStatement}\r\n";

      InnerJoinQueryBuilder innerJoinQueryBuilder = new InnerJoinQueryBuilder(tableName);
      innerJoinQueryBuilder.On(onStatement);

      string actualQuery = innerJoinQueryBuilder.Build();

      Assert.AreEqual(expectedQuery, actualQuery);
    }

    [Test]
    public void BuildsQuery_WithAnd() {
      string tableName = "bc_core_doubleclickcampaignmanager_lld.ref_paid_search ps";
      string onStatement = "i.segment_value_1 = ps.legacykeywordid";
      string andStatement = "i.event_type = 'CONVERSION'";
      string expectedQuery = $"INNER JOIN {tableName}\r\n\tON {onStatement}\r\n\tAND {andStatement}\r\n";

      InnerJoinQueryBuilder innerJoinQueryBuilder = new InnerJoinQueryBuilder(tableName);
      innerJoinQueryBuilder.On(onStatement);
      innerJoinQueryBuilder.And(andStatement);

      string actualQuery = innerJoinQueryBuilder.Build();

      Assert.AreEqual(expectedQuery, actualQuery);
    }
  }
}