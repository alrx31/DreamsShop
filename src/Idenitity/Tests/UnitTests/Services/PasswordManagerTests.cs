using FluentAssertions;
using Infrastructure.Services;

namespace Tests.UnitTests.Services;

public class PasswordManagerTests
{
    private readonly PasswordManager _sut = new();

    [Fact]
    public void HashPassword_ShouldReturnDeterministicHash()
    {
        const string password = "StrongP@ssw0rd!";

        var hash1 = _sut.HashPassword(password);
        var hash2 = _sut.HashPassword(password);

        hash1.Should().NotBeNullOrWhiteSpace();
        hash1.Should().Be(hash2);
    }

    [Fact]
    public void Verify_ShouldReturnTrue_ForMatchingHash()
    {
        const string password = "AnotherP@ssw0rd!";
        var hash = _sut.HashPassword(password);

        var result = _sut.Verify(hash, password);

        result.Should().BeTrue();
    }

    [Fact]
    public void CheckPassword_ShouldReturnMaxLevel()
    {
        var result = _sut.CheckPassword("any");

        result.Should().Be(255);
    }
}
