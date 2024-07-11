namespace GlobalHelpers.Monads;

public readonly struct Result<TOk, TErr>
{
    private readonly object _innerObject;
    private readonly bool _isOk;

    public Result() =>
        throw new ArgumentException("Please provide a value!");

    /// <summary>
    /// Equivalent to <see cref="Ok{TROk, TRErr}"/>
    /// </summary>
    public Result(TOk value)
    {
        if (value is not { } obj)
        {
            throw new NullReferenceException("`null` is not a valid value!");
        }

        _innerObject = obj;
        _isOk = true;
    }

    /// <summary>
    /// Equivalent to <see cref="Err{TROk, TRErr}"/>
    /// </summary>
    public Result(TErr value)
    {
        if (value is not { } obj)
        {
            throw new NullReferenceException("`null` is not a valid value!");
        }

        _innerObject = obj;
        _isOk = false;
    }

    /// <summary>
    /// Creates an <see cref="Result{TOk, TErr}"/> that contains a value
    /// </summary>
    /// <param name="value">The value contained</param>
    /// <typeparam name="TROk">The type of the contained value</typeparam>
    /// <typeparam name="TRErr">The type of the exception problem</typeparam>
    /// <returns>The created result</returns>
    public static Result<TOk, TErr> Ok(TOk value) =>
        new(value);

    /// <summary>
    /// Creates an <see cref="Result{TOk, TErr}"/> of a type that does not contain anything
    /// </summary>
    /// <typeparam name="TROk">The type that the result would have contained</typeparam>
    /// <typeparam name="TRErr">The type of the exception problem</typeparam>
    /// <returns>The created result</returns>
    public static Result<TOk, TErr> Err(TErr error)
        => new(error);

    /// <summary>
    /// Whether the <see cref="Result{TOk, TErr}"/> has a valid value
    /// </summary>
    public bool IsOk => _isOk;

    /// <summary>
    /// Whether the <see cref="Result{TOk, TErr}"/> has an invalid value
    /// </summary>
    public bool IsErr => !_isOk;

    /// <summary>
    /// Returns the value of the <see cref="Result{TOk, TErr}"/> or throws an <see cref="AccessViolationException"/>
    /// if the result does not contain a valid value
    /// </summary>
    /// <exception cref="AccessViolationException">Thrown if the result does not contain a valid value</exception>
    public TOk Unwrap() =>
        _isOk && _innerObject is TOk value
            ? value
            : throw new AccessViolationException($"Value is invalid! Error: {_innerObject}");

    public TErr UnwrapException() =>
        !_isOk && _innerObject is TErr exception
            ? exception
            : throw new AccessViolationException("The result is valid!");

    /// <summary>
    /// Exhaustively switches through all possible states.
    /// There either is a valid value, in which case <see cref="whenOk"/> is executed,
    /// or it is not, in which case <see cref="whenErr"/> is executed.
    /// </summary>
    /// <param name="whenOk"></param>
    /// <param name="whenErr"></param>
    public void Switch(
        Action<TOk> whenOk,
        Action<TErr> whenErr)
    {
        switch (IsOk)
        {
            case true:
                whenOk(Unwrap());
                break;
            case false:
                whenErr(UnwrapException());
                break;
        }
    }

    /// <summary>
    /// Like <see cref="Switch"/>, but async.
    /// </summary>
    public async Task SwitchAsync(
        Func<TOk, Task> whenOk,
        Func<TErr, Task> whenErr)
    {
        var task = IsOk switch
        {
            true => whenOk(Unwrap()),
            false => whenErr(UnwrapException()),
        };
        await task;
    }

    /// <summary>
    /// Like <see cref="Switch"/>, but returns a value of type.
    /// Both, <see cref="forOk"/> and <see cref="forErr"/> have to return the same type
    /// </summary>
    /// <param name="forOk">Lambda to run for when there is a valid value</param>
    /// <param name="forErr">Lambda to run for when there is an exception</param>
    /// <typeparam name="TR">The type returned by <see cref="forOk"/> and <see cref="forErr"/></typeparam>
    /// <returns>The value returned from either <see cref="forOk"/> or <see cref="forErr"/></returns>
    public TR Match<TR>(
        Func<TOk, TR> forOk,
        Func<TErr, TR> forErr)
    {
        return IsOk switch
        {
            true => forOk(Unwrap()),
            false => forErr(UnwrapException()),
        };
    }

    /// <summary>
    /// Like <see cref="Match{TR}"/>, but async.
    /// </summary>
    public async Task<TR> MatchAsync<TR>(
        Func<TOk, ValueTask<TR>> forOk,
        Func<TErr, ValueTask<TR>> forErr)
    {
        var task = IsOk switch
        {
            true => forOk(Unwrap()),
            false => forErr(UnwrapException()),
        };

        return await task;
    }

    /// <summary>
    /// Maps the underlying value and type in a safe way. If the original <see cref="Result{TOk, TErr}"/>
    /// does not contain a value, the value mapping is skipped but the type will be mapped.
    /// </summary>
    /// <param name="mapper">The mapping lambda</param>
    /// <typeparam name="TROk">The target type</typeparam>
    /// <returns>The <see cref="Result{T, UR}"/> of type <see cref="TROk"/> and the value provided by
    /// <see cref="mapper"/></returns>
    public Result<TROk, TErr> Map<TROk>(Func<TOk, TROk> mapper)
    {
        if (IsOk)
        {
            return Result<TROk, TErr>.Ok(mapper(Unwrap()));
        }

        return Result<TROk, TErr>.Err(UnwrapException());
    }

    /// <summary>
    /// Like <see cref="Map{TROk}"/>, but async.
    /// </summary>
    public async Task<Result<TROk, TErr>> MapAsync<TROk>(Func<TOk, ValueTask<TROk>> mapper)
    {
        if (IsOk)
        {
            return Result<TROk, TErr>.Ok(await mapper(Unwrap()));
        }

        return Result<TROk, TErr>.Err(UnwrapException());
    }

    /// <summary>
    /// Converts the <see cref="Result{TOk, TErr}"/> to a new one of another type which is provided by
    /// a lambda (<see cref="binder"/>)
    /// </summary>
    /// <param name="binder">The lambda that provides the new <see cref="Result{TOk, TErr}"/></param>
    /// <typeparam name="TROk">The target type</typeparam>
    /// <returns>The new <see cref="Result{T, UR}"/></returns>
    public Result<TROk, TErr> Bind<TROk>(Func<TOk, Result<TROk, TErr>> binder)
    {
        if (IsOk)
        {
            return binder(Unwrap());
        }

        return Result<TROk, TErr>.Err(UnwrapException());
    }

    /// <summary>
    /// Like <see cref="Bind{TR}"/>, but async
    /// </summary>
    public async Task<Result<TROk, TErr>> BindAsync<TROk>(Func<TOk, ValueTask<Result<TROk, TErr>>> binder)
    {
        if (IsOk)
        {
            return await binder(Unwrap());
        }

        return Result<TROk, TErr>.Err(UnwrapException());
    }

    /// <summary>
    /// Allows for implicit conversions between <see cref="TOk"/> and the
    /// <see cref="Result{TOk, TErr}"/> itself.
    /// </summary>
    /// <param name="value">The value to box</param>
    /// <returns>The resulting <see cref="Result{TOk, TErr}"/></returns>
    public static implicit operator Result<TOk, TErr>(TOk value) =>
        Ok(value);

    /// <summary>
    /// Allows for implicit conversions between <see cref="TErr"/> and the
    /// <see cref="Result{TOk, TErr}"/> itself.
    /// </summary>
    /// <param name="value">The value to box</param>
    /// <returns>The resulting <see cref="Result{TOk, TErr}"/></returns>
    public static implicit operator Result<TOk, TErr>(TErr value) =>
        Err(value);
}