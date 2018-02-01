using NUnit.Framework;
using QueryBuilder.Business.Interface.Operators;

namespace QueryBuilder.Business.Tests {
  [TestFixture]
  public class WhereQueryBuilderTests {
    [Test]
    public void Builds_SingleConditionStatement() {
      // Arrange
      string condition = "i.segment_value_1 = ps.legacykeywordid";
      string expectedQuery = $"WHERE (TRUE\r\n\tAND {condition})\r\n";

      // Act
      WhereQueryBuilder whereQueryBuilder = new WhereQueryBuilder(LogicalOperator.AND, condition);
      
      string actualQuery = whereQueryBuilder.Build();

      // Assert
      Assert.AreEqual(expectedQuery, actualQuery);
    }

    [Test]
    public void Builds_SingleConditionStatementAndAdditionalAndCondition() {
      // Arrange
      string condition = "i.segment_value_1 = ps.legacykeywordid";
      string additionalCondition = "1 = 1";
      string expectedQuery = $"WHERE (TRUE\r\n\tAND {condition})\r\n\tAND {additionalCondition}\r\n";

      // Act
      WhereQueryBuilder whereQueryBuilder = new WhereQueryBuilder(LogicalOperator.AND, condition);
      whereQueryBuilder.And(additionalCondition);

      string actualQuery = whereQueryBuilder.Build();

      // Assert
      Assert.AreEqual(expectedQuery, actualQuery);
    }

    [Test]
    public void Builds_MultipleConditionsWithAndOperator() {
      // Arrange
      string firstCondition = "i.segment_value_1 = ps.legacykeywordid";
      string secondCondition = "i.segment_value_2 = ps.legacykeywordidd";
      string expectedQuery = $"WHERE (TRUE\r\n\tAND {firstCondition}\r\n\tAND {secondCondition})\r\n";

      // Act
      WhereQueryBuilder whereQueryBuilder = new WhereQueryBuilder(LogicalOperator.AND, firstCondition, secondCondition);

      string actualQuery = whereQueryBuilder.Build();

      // Assert
      Assert.AreEqual(expectedQuery, actualQuery);
    }

    [Test]
    public void Builds_MultipleConditionsWithOrOperator() {
      // Arrange
      string firstCondition = "i.segment_value_1 = ps.legacykeywordid";
      string secondCondition = "i.segment_value_2 = ps.legacykeywordidd";
      string expectedQuery = $"WHERE (FALSE\r\n\tOR {firstCondition}\r\n\tOR {secondCondition})\r\n";

      // Act
      WhereQueryBuilder whereQueryBuilder = new WhereQueryBuilder(LogicalOperator.OR, firstCondition, secondCondition);

      string actualQuery = whereQueryBuilder.Build();

      // Assert
      Assert.AreEqual(expectedQuery, actualQuery);
    }

    [Test]
    public void Builds_MultipleConditionsWithAndOperatorAndAdditionalAndCondition() {
      // Arrange
      string firstCondition = "i.segment_value_1 = ps.legacykeywordid";
      string secondCondition = "i.segment_value_2 = ps.legacykeywordidd";
      string additionalCondition = "1 != 2";
      string expectedQuery = $"WHERE (TRUE\r\n\tAND {firstCondition}\r\n\tAND {secondCondition})\r\n\tAND {additionalCondition}\r\n";

      // Act
      WhereQueryBuilder whereQueryBuilder = new WhereQueryBuilder(LogicalOperator.AND, firstCondition, secondCondition);
      whereQueryBuilder.And(additionalCondition);

      string actualQuery = whereQueryBuilder.Build();

      // Assert
      Assert.AreEqual(expectedQuery, actualQuery);
    }

    [Test]
    public void Builds_MultipleConditionsWithOrOperatorAndAdditionalAndCondition() {
      // Arrange
      string firstCondition = "i.segment_value_1 = ps.legacykeywordid";
      string secondCondition = "i.segment_value_2 = ps.legacykeywordidd";
      string additionalCondition = "2 != 1";
      string expectedQuery = $"WHERE (FALSE\r\n\tOR {firstCondition}\r\n\tOR {secondCondition})\r\n\tAND {additionalCondition}\r\n";

      // Act
      WhereQueryBuilder whereQueryBuilder = new WhereQueryBuilder(LogicalOperator.OR, firstCondition, secondCondition);
      whereQueryBuilder.And(additionalCondition);

      string actualQuery = whereQueryBuilder.Build();

      // Assert
      Assert.AreEqual(expectedQuery, actualQuery);
    }
  }
}