using Xunit;

namespace Vexit.DataAccess.Tests;

public class DummyTest
{
    [Fact]
    public void Should_Pass()
    {
        // Arrange
        var expected = true;

        // Act
        var actual = true;

        // Assert
        Assert.Equal(expected, actual);
    }
}
