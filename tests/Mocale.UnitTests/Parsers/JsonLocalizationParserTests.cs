using Mocale.Abstractions;
using Mocale.Parsers;

namespace Mocale.UnitTests.Parsers;

public class JsonLocalizationParserTests : FixtureBase
{
    #region Setup

    public override object CreateSystemUnderTest()
    {
        return new JsonLocalizationParser();
    }

    #endregion Setup

    #region Tests

    [Fact]
    public void ParseLocalizationStream_WhenStreamIsNotJson_ShouldReturnNull()
    {
        // Arrange

        // Act

        // Assert
        throw new NotImplementedException();
    }

    [Fact]
    public void ParseLocalizationStream_WhenStreamIsJsonButNotLocalizations_ShouldReturnNull()
    {
        // Arrange

        // Act

        // Assert
        throw new NotImplementedException();
    }

    [Fact]
    public void ParseLocalizationStream_WhenStreamIsLocalizationJson_ShouldReturnExpectedResponse()
    {
        // Arrange

        // Act

        // Assert
        throw new NotImplementedException();
    }

    #endregion Tests
}
