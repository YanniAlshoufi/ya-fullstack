namespace GlobalHelpers.Monads;

public readonly struct Option<T>
{
    public static Option<T> NoneValue => new();
    
    private readonly T _value = default!; // Always using _isSome.
    private readonly bool _isSome = false;

    /// <summary>
    /// Equivalent to <see cref="None{TR}"/>
    /// </summary>
    public Option()
    {
    }

    /// <summary>
    /// Equivalent to <see cref="Some{TR}"/>
    /// </summary>
    public Option(T value)
    {
        _value = value;
        _isSome = true;
    }

    /// <summary>
    /// Creates an <see cref="Option{T}"/> that contains a value
    /// </summary>
    /// <param name="value">The value contained</param>
    /// <typeparam name="TR">The type of the contained value</typeparam>
    /// <returns>The created option</returns>
    public static Option<TR> Some<TR>(TR value) => new(value);

    /// <summary>
    /// Creates an <see cref="Option{T}"/> of a type that does not contain anything
    /// </summary>
    /// <typeparam name="TR">The type that the option would have contained</typeparam>
    /// <returns>The created option</returns>
    public static Option<TR> None<TR>() => Option<TR>.NoneValue;

    /// <summary>
    /// Whether the <see cref="Option{T}"/> has a value
    /// </summary>
    public bool IsSome => _isSome;

    /// <summary>
    /// Whether the <see cref="Option{T}"/> does NOT have a value
    /// </summary>
    public bool IsNone => !_isSome;

    /// <summary>
    /// Returns the value of the <see cref="Option{T}"/> or throws an <see cref="AccessViolationException"/>
    /// if the option does not contain a value
    /// </summary>
    /// <exception cref="AccessViolationException">Thrown if the option does not contain a value</exception>
    public T Unwrap() =>
        _isSome
            ? _value
            : throw new AccessViolationException("Value is not defined!");

    /// <summary>
    /// Exhaustively switches through all possible states.
    /// There either is a value, in which case <see cref="whenSome"/> is executed,
    /// or there isn't, in which case <see cref="whenNone"/> is executed.
    /// </summary>
    /// <param name="whenSome"></param>
    /// <param name="whenNone"></param>
    public void Switch(
        Action<T> whenSome,
        Action whenNone)
    {
        switch (_isSome)
        {
            case true:
                whenSome(_value);
                break;
            case false:
                whenNone();
                break;
        }
    }
    
    /// <summary>
    /// Like <see cref="Switch"/>, but async.
    /// </summary>
    public async void SwitchAsync(
        Func<T, Task> whenSome,
        Func<Task> whenNone)
    {
        var task = _isSome switch
        {
            true => whenSome(_value),
            false => whenNone(),
        };
        await task;
    }

    /// <summary>
    /// Like <see cref="Switch"/>, but returns a value of type.
    /// Both, <see cref="forSome"/> and <see cref="forNone"/> have to return the same type.
    /// </summary>
    /// <param name="forSome">Lambda to run for when there is a value.</param>
    /// <param name="forNone">Lambda to run for when there is no value.</param>
    /// <typeparam name="TR">The type returned by <see cref="forSome"/> and <see cref="forNone"/></typeparam>
    /// <returns>The value returned from either <see cref="forSome"/> or <see cref="forNone"/></returns>
    public TR Match<TR>(
        Func<T, TR> forSome,
        Func<TR> forNone)
    {
        return _isSome switch
        {
            true => forSome(_value),
            false => forNone(),
        };
    }

    /// <summary>
    /// Like <see cref="Match{TR}"/>, but async.
    /// </summary>
    public async Task<TR> MatchAsync<TR>(
        Func<T, ValueTask<TR>> forSome,
        Func<ValueTask<TR>> forNone)
    {
        var task = _isSome switch
        {
            true => forSome(_value),
            false => forNone(),
        };

        return await task;
    }

    /// <summary>
    /// Maps the underlying value and type in a safe way. If the original <see cref="Option{T}"/> does not contain
    /// a value, the value mapping is skipped but the type will be mapped.
    /// </summary>
    /// <param name="mapper">The mapping lambda</param>
    /// <typeparam name="TR">The target type</typeparam>
    /// <returns>The <see cref="Option{TR}"/> of type <see cref="TR"/> and the value provided by <see cref="mapper"/></returns>
    public Option<TR> Map<TR>(Func<T, TR> mapper)
    {
        if (_isSome)
        {
            return Some(mapper(_value));
        }

        return None<TR>();
    }

    /// <summary>
    /// Like <see cref="Map{TR}"/>, but async.
    /// </summary>
    public async Task<Option<TR>> MapAsync<TR>(Func<T, ValueTask<TR>> mapper)
    {
        if (_isSome)
        {
            return Some(await mapper(_value));
        }

        return None<TR>();
    }

    /// <summary>
    /// Converts the <see cref="Option{T}"/> to a new one of another type which is provided by
    /// a lambda (<see cref="binder"/>)
    /// </summary>
    /// <param name="binder">The lambda that provides the new <see cref="Option{T}"/></param>
    /// <typeparam name="TR">The target type</typeparam>
    /// <returns>The new <see cref="Option{TR}"/></returns>
    public Option<TR> Bind<TR>(Func<T, Option<TR>> binder)
    {
        if (_isSome)
        {
            return binder(_value);
        }

        return None<TR>();
    }

    /// <summary>
    /// Like <see cref="Bind{TR}"/>, but async
    /// </summary>
    public async Task<Option<TR>> BindAsync<TR>(Func<T, ValueTask<Option<TR>>> mapper)
    {
        if (_isSome)
        {
            return await mapper(_value);
        }

        return None<TR>();
    }

    /// <summary>
    /// Allows for implicit conversions between the type of the <see cref="Option{T}"/> and the
    /// <see cref="Option{T}"/> itself.
    /// If the value is null, the option is stored as an empty option.
    /// </summary>
    /// <param name="nullableValue">The value to box</param>
    /// <returns>The resulting <see cref="Option{T}"/></returns>
    public static implicit operator Option<T>(T? nullableValue) =>
        nullableValue is null
            ? None<T>()
            : Some(nullableValue);
}