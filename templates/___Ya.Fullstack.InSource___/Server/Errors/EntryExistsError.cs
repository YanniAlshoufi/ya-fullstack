using LanguageExt;

namespace Server.Errors;

public record struct EntryExistsError(
    Option<string> Message = default);
