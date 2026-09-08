using ErrorsFlow.Errors;
using ErrorsFlow.Models;
using Xunit;

namespace ErrorsFlow.Tests;

public class ErrorTests
{
    [Fact]
    public void Create_CopiesMetadataAndExposesItAsReadOnly()
    {
        var metadata = new Dictionary<string, string?>
        {
            ["email"] = "client@example.com"
        };

        var error = ErrorFactory.Create(
            "users.email.already.exists",
            "A user with this email already exists.",
            ErrorType.Conflict,
            "email",
            metadata);

        metadata["email"] = "changed@example.com";

        Assert.Equal("users.email.already.exists", error.Code);
        Assert.Equal(ErrorType.Conflict, error.Type);
        Assert.Equal("email", error.Target);
        Assert.Equal("client@example.com", error.Metadata["email"]);
    }

    [Fact]
    public void AuthErrors_RoleIsInvalid_UsesStableCodeAndMetadata()
    {
        var error = AuthErrors.RoleIsInvalid("Master");

        Assert.Equal(AuthErrorCodes.RoleInvalid, error.Code);
        Assert.Equal(ErrorType.Validation, error.Type);
        Assert.Equal("role", error.Target);
        Assert.Equal("Master", error.Metadata["expectedRole"]);
    }
}
