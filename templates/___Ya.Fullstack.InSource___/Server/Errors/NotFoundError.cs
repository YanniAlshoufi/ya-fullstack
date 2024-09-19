using LanguageExt;

namespace Server.Errors;

public record struct NotFoundError(
    Option<string> Message = default);