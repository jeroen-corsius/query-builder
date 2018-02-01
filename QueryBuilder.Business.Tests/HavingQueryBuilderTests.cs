using System;
using NUnit.Framework;
using QueryBuilder.Business.Interface.Operators;

namespace QueryBuilder.Business.Tests {
  [TestFixture]
  public class HavingQueryBuilderTests {
    [Test]
    public void ThrowsException_When_Empty() {
      string expectedExceptionMessage = "Unable to instantiate HavingQueryBuilder without statement(s)";

      Exception exception = Assert.Throws<Exception>(() => new HavingQueryBuilder(null));

      Assert.AreEqual(expectedExceptionMessage, exception.Message);
    }

    [Test]
    public void BuildsQuery_WithOn() {
      string onStatement = "SUM(Column) > 100";
      string expectedQuery = $"HAVING {onStatement}\r\n";

      HavingQueryBuilder havingQueryBuilder = new HavingQueryBuilder(onStatement);
      string actualQuery = havingQueryBuilder.Build();

      Assert.AreEqual(expectedQuery, actualQuery);
    }

    [Test]
    public void BuildsQuery_MultipleConditionsWithAndOperator() {
      // Arrange
      string firstCondition = "i.segment_value_1 = ps.legacykeywordid";
      string secondCondition = "i.segment_value_2 = ps.legacykeywordidd";
      string expectedQuery = $"HAVING {firstCondition}\r\n\tAND {secondCondition}\r\n";

      // Act
      HavingQueryBuilder havingQueryBuilder = new HavingQueryBuilder(LogicalOperator.AND, firstCondition, secondCondition);

      string actualQuery = havingQueryBuilder.Build();

      // Assert
      Assert.AreEqual(expectedQuery, actualQuery);
    }

    [Test]
    public void BuildsQuery_MultipleConditionsWithOrOperator() {
      // Arrange
      string firstCondition = "i.segment_value_1 = ps.legacykeywordid";
      string secondCondition = "i.segment_value_2 = ps.legacykeywordidd";
      string expectedQuery = $"HAVING {firstCondition}\r\n\tOR {secondCondition}\r\n";

      // Act
      HavingQueryBuilder havingQueryBuilder = new HavingQueryBuilder(LogicalOperator.OR, firstCondition, secondCondition);

      string actualQuery = havingQueryBuilder.Build();

      // Assert
      Assert.AreEqual(expectedQuery, actualQuery);
    }
  }
}