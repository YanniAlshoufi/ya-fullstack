using YaMonads;

namespace Src.Shared.ClassLib.HttpErrors;

public readonly struct NotFound
{
    private readonly bool _wasMessageProvided;
    private readonly string _message;
    
    public NotFound(string message)
    {
        _message = message;
        _wasMessageProvided = true;
    }

    public NotFound()
    {
        _message = ErrorConstants.NoErrorMessage;
        _wasMessageProvided = false;
    }
    
    public Option<string> Message => 
        _wasMessageProvided
        ? _message
        : Option<string>.NoneValue;
}
