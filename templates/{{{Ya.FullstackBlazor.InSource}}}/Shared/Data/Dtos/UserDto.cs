namespace Shared.Data.Dtos;

public record UserDto(
    string? FirstName,
    string? LastName)
{
    public string? TrimmedFirstName => FirstName?.Trim();
    public string? TrimmedLastName => LastName?.Trim();
};
