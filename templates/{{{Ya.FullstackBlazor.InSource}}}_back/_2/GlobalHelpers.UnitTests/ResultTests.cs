using System.Globalization;
using FluentAssertions;
using GlobalHelpers.Monads;
using Xunit.Abstractions;

namespace GlobalHelpers.UnitTests;

public class ResultTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private static readonly Random _random = new();

    public ResultTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [MemberData(nameof(GetTestingDecimals))]
    private void Ok_OfDecimalAndString_DecimalsGiven_ResultsWithOkCreated(
        decimal okValue)
    {
        // Arrange
        var result = Result<decimal, string>.Ok(okValue);
        // Act
        // Assert
        AssertResultValidWith(result, okValue);
    }

    [Theory]
    [MemberData(nameof(GetTestingObjects))]
    public void Ok_OfObjectAndString_ObjectGiven_ResultsWithOkCreated(
        object okValue)
    {
        // Arrange
        var result = Result<object, string>.Ok(okValue);

        // Act
        // Assert
        AssertResultValidWith(result, okValue);
    }

    [Fact]
    private void Ok_OfObjectAndString_NullGiven_NullReferenceExceptionThrown()
    {
        // Arrange
        // Act
        Action act = () => Result<object, string>.Ok(null!);

        // Assert
        act.Should().Throw<NullReferenceException>()
            .WithMessage("`null` is not a valid value!");
    }

    [Theory]
    [MemberData(nameof(GetTestingErrorStrings))]
    private void Err_OfDecimalAndString_StringErrorsGiven_ResultsWithSameStringCreated(
        string errValue)
    {
        // Arrange
        var result = Result<decimal, string>.Err(errValue);

        // Act
        // Assert
        AssertResultInvalidWith(result, errValue);
    }

    [Theory]
    [MemberData(nameof(GetTestingErrorExceptions))]
    private void Err_OfDecimalAndException_ExceptionErrorsGiven_ResultsWithSameExceptionCreated(
        Exception errValue)
    {
        // Arrange
        var result = Result<decimal, Exception>.Err(errValue);

        // Act
        // Assert
        AssertResultInvalidWith(result, errValue);
    }

    [Fact]
    private void Err_OfObjectAndException_NullGiven_NullReferenceExceptionThrown()
    {
        // Arrange
        // Act
        Action act = () => Result<object, Exception>.Err(null!);

        // Assert
        act.Should().Throw<NullReferenceException>()
            .WithMessage("`null` is not a valid value!");
    }

    [Theory]
    [MemberData(nameof(GetTestingDecimals))]
    private void Switch_OfDecimalAndString_ValidResultGiven_CorrectActionExecuted(
        decimal okValue)
    {
        // Arrange
        var result = Result<decimal, string>.Ok(okValue);
        var wasCorrectActionExecuted = false;
        decimal? shouldValue = null;

        // Act
        result.Switch(
            whenOk: val =>
            {
                wasCorrectActionExecuted = true;
                shouldValue = val;
            },
            whenErr: err =>
            {
                _testOutputHelper.WriteLine(err);
                wasCorrectActionExecuted = false;
            });

        // Assert
        wasCorrectActionExecuted.Should().BeTrue();
        shouldValue.Should()
            .NotBeNull()
            .And
            .Be(okValue);
    }

    [Theory]
    [MemberData(nameof(GetTestingErrorStrings))]
    private void Switch_OfDecimalAndString_InvalidResultGiven_CorrectActionExecuted(
        string errValue)
    {
        // Arrange
        var result = Result<decimal, string>.Err(errValue);
        var wasCorrectActionExecuted = false;
        string? shouldValue = null;

        // Act
        result.Switch(
            whenOk: actual =>
            {
                _testOutputHelper.WriteLine(actual.ToString(CultureInfo.InvariantCulture));
                wasCorrectActionExecuted = false;
            },
            whenErr:
            val =>
            {
                wasCorrectActionExecuted = true;
                shouldValue = val;
            });

        // Assert
        wasCorrectActionExecuted.Should().BeTrue();
        shouldValue.Should()
            .NotBeNull()
            .And
            .Be(errValue);
    }

    [Theory]
    [MemberData(nameof(GetTestingDecimals))]
    private async Task SwitchAsync_OfDecimalAndString_ValidResultGiven_CorrectActionExecuted(
        decimal okValue)
    {
        // Arrange
        var result = Result<decimal, string>.Ok(okValue);
        var wasCorrectActionExecuted = false;
        decimal? shouldValue = null;

        // Act
        await result.SwitchAsync(
            whenOk: async val =>
            {
                await Task.Delay(_random.Next(100, 400));
                wasCorrectActionExecuted = true;
                shouldValue = val;
            },
            whenErr: async err =>
            {
                await Task.Delay(_random.Next(100, 400));
                _testOutputHelper.WriteLine(err);
                wasCorrectActionExecuted = false;
            });

        // Assert
        wasCorrectActionExecuted.Should().BeTrue();
        shouldValue.Should()
            .NotBeNull()
            .And
            .Be(okValue);
    }

    [Theory]
    [MemberData(nameof(GetTestingErrorStrings))]
    private async Task SwitchAsync_OfDecimalAndString_InvalidResultGiven_CorrectActionExecuted(
        string errValue)
    {
        // Arrange
        var result = Result<decimal, string>.Err(errValue);
        var wasCorrectActionExecuted = false;
        string? shouldValue = null;

        // Act
        await result.SwitchAsync(
            whenOk: async actual =>
            {
                await Task.Delay(_random.Next(100, 400));
                _testOutputHelper.WriteLine(actual.ToString(CultureInfo.InvariantCulture));
                wasCorrectActionExecuted = false;
            },
            whenErr: async val =>
            {
                await Task.Delay(_random.Next(100, 400));
                wasCorrectActionExecuted = true;
                shouldValue = val;
            });

        // Assert
        wasCorrectActionExecuted.Should().BeTrue();
        shouldValue.Should()
            .NotBeNull()
            .And
            .Be(errValue);
    }

    [Theory]
    [MemberData(nameof(GetTestingDecimals))]
    private void Match_OfDecimalAndString_ValidResultGiven_CorrectFuncExecuted(
        decimal okValue)
    {
        // Arrange
        var result = Result<decimal, string>.Ok(okValue);
        decimal? shouldValue = null;

        // Act
        var wasCorrectActionExecuted = result.Match(
            forOk: val =>
            {
                shouldValue = val;
                return true;
            },
            forErr: err =>
            {
                _testOutputHelper.WriteLine(err);
                return false;
            });

        // Assert
        wasCorrectActionExecuted.Should().BeTrue();
        shouldValue.Should()
            .NotBeNull()
            .And
            .Be(okValue);
    }

    [Theory]
    [MemberData(nameof(GetTestingErrorExceptions))]
    private void Match_OfDecimalAndException_InvalidResultGiven_CorrectFuncExecuted(
        Exception errValue)
    {
        // Arrange
        var result = Result<decimal, Exception>.Err(errValue);
        Exception? shouldValue = null;

        // Act
        var wasCorrectActionExecuted = result.Match(
            forOk: actual =>
            {
                _testOutputHelper.WriteLine(actual.ToString(CultureInfo.InvariantCulture));
                return false;
            },
            forErr:
            val =>
            {
                shouldValue = val;
                return true;
            });

        // Assert
        wasCorrectActionExecuted.Should().BeTrue();
        shouldValue.Should()
            .NotBeNull()
            .And
            .Be(errValue);
    }

    [Theory]
    [MemberData(nameof(GetTestingDecimals))]
    private async Task MatchAsync_OfDecimalAndString_ValidResultGiven_CorrectFuncExecuted(
        decimal okValue)
    {
        // Arrange
        var result = Result<decimal, string>.Ok(okValue);
        decimal? shouldValue = null;

        // Act
        var wasCorrectActionExecuted = await result.MatchAsync(
            forOk: async val =>
            {
                await Task.Delay(_random.Next(100, 400));
                shouldValue = val;
                return true;
            },
            forErr: async err =>
            {
                await Task.Delay(_random.Next(100, 400));
                _testOutputHelper.WriteLine(err);
                return false;
            });

        // Assert
        wasCorrectActionExecuted.Should().BeTrue();
        shouldValue.Should()
            .NotBeNull()
            .And
            .Be(okValue);
    }

    [Theory]
    [MemberData(nameof(GetTestingErrorExceptions))]
    private async Task MatchAsync_OfDecimalAndException_InvalidResultGiven_CorrectFuncExecuted(
        Exception errValue)
    {
        // Arrange
        var result = Result<decimal, Exception>.Err(errValue);
        Exception? shouldValue = null;

        // Act
        var wasCorrectActionExecuted = await result.MatchAsync(
            forOk: async actual =>
            {
                await Task.Delay(_random.Next(100, 400));
                _testOutputHelper.WriteLine(actual.ToString(CultureInfo.InvariantCulture));
                return false;
            },
            forErr: async val =>
            {
                await Task.Delay(_random.Next(100, 400));
                shouldValue = val;
                return true;
            });

        // Assert
        wasCorrectActionExecuted.Should().BeTrue();
        shouldValue.Should()
            .NotBeNull()
            .And
            .Be(errValue);
    }

    #region Helpers

    public static IEnumerable<object[]> GetTestingObjects()
    {
        yield return [5];
        yield return [3.141045];
        yield return [Math.PI];
        yield return [0];
        yield return [0.0];
        yield return [4.88f];
        yield return [double.NaN];
        yield return [double.PositiveInfinity];
        yield return [float.PositiveInfinity];
        yield return [double.NegativeInfinity];
        yield return [new Exception()];
        yield return [new NullReferenceException()];
        yield return [new object()];
        yield return [true];
        yield return [false];
        yield return [int.MaxValue];
        yield return [int.MinValue];
    }

    public static IEnumerable<object[]> GetTestingDecimals()
    {
        yield return [5];
        yield return [3.141045];
        yield return [Math.PI];
        yield return [0];
        yield return [0.0];
        yield return [int.MaxValue];
        yield return [int.MinValue];
    }

    public static IEnumerable<object[]> GetTestingErrorStrings()
    {
        yield return ["Nope!"];
        yield return ["Error!"];
        yield return ["Invalid!"];
        yield return ["Not valid!"];
        yield return ["Not good!"];
        yield return ["I am to be very expected! :)"];
        yield return [string.Empty];
    }

    public static IEnumerable<object[]> GetTestingErrorExceptions()
    {
        yield return [new Exception()];
        yield return [new NullReferenceException()];
        yield return [new ArgumentException()];
        yield return [new IOException()];
        yield return [new DllNotFoundException()];
        yield return [new AggregateException()];
        yield return [new ApplicationException()];
    }

    private static void AssertResultValidWith<TOk, TErr>(Result<TOk, TErr> result, TOk expectedOkValue)
    {
        // Act
        Action act = () => result.UnwrapException();

        // Assert
        result.IsOk.Should().BeTrue();
        result.IsErr.Should().BeFalse();
        result.Unwrap().Should().Be(expectedOkValue);

        act.Should().Throw<AccessViolationException>()
            .WithMessage("The result is valid!");
    }

    private static void AssertResultInvalidWith<TOk, TErr>(Result<TOk, TErr> result, TErr expectedErrValue)
    {
        // Act
        var act = result.Unwrap;

        // Assert
        result.IsOk.Should().BeFalse();
        result.IsErr.Should().BeTrue();
        result.UnwrapException().Should().Be(expectedErrValue);
        act.Should().Throw<AccessViolationException>()
            .WithMessage($"Value is invalid! Error: {expectedErrValue}");
    }

    #endregion
}