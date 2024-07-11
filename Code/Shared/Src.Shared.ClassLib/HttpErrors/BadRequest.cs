using YaMonads;

namespace Src.Shared.ClassLib.HttpErrors;

public readonly struct BadRequest
{
    private readonly bool _wasMessageProvided;
    private readonly string _message;
    
    public BadRequest(string message)
    {
        _message = message;
        _wasMessageProvided = true;
    }

    public BadRequest()
    {
        _message = ErrorConstants.NoErrorMessage;
        _wasMessageProvided = false;
    }
    
    public Option<string> Message => 
        _wasMessageProvided
        ? _message
        : Option<string>.NoneValue;
}