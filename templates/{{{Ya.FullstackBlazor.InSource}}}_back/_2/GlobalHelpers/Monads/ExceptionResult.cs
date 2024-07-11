// namespace GlobalHelpers.Monads;
//
// public class ExceptionResult<TOk>
// {
//     private readonly Result<TOk, Exception> _innerResult;
//
//     public ExceptionResult() =>
//         throw new ArgumentException("Please provide a value!");
//
//     /// <summary>
//     /// Equivalent to <see cref="Ok{TROk, TRErr}"/>
//     /// </summary>
//     public ExceptionResult(TOk value)
//     {
//         _innerResult = new Result<TOk, Exception>(value);
//     }
//
//     /// <summary>
//     /// Equivalent to <see cref="Err{TROk, TRErr}"/>
//     /// </summary>
//     public ExceptionResult(Exception value)
//     {
//         _innerResult = new Result<TOk, Exception>(value);
//     }
//
//     /// <summary>
//     /// Creates an <see cref="Result{TOk, TErr}"/> that contains a value
//     /// </summary>
//     /// <param name="value">The value contained</param>
//     /// <typeparam name="TROk">The type of the contained value</typeparam>
//     /// <returns>The created result</returns>
//     public static ExceptionResult<TROk> Ok<TROk>(TROk value) =>
//         new(value);
//
//     /// <summary>
//     /// Creates an <see cref="Result{TOk, TErr}"/> of a type that does not contain anything
//     /// </summary>
//     /// <typeparam name="TROk">The type that the result would have contained</typeparam>
//     /// <returns>The created result</returns>
//     public static ExceptionResult<TROk> Err<TROk>(Exception error)
//         => new(error);
//
//     /// <summary>
//     /// Whether the <see cref="Result{TOk, TErr}"/> has a valid value
//     /// </summary>
//     public bool IsOk => _innerResult.IsOk;
//
//     /// <summary>
//     /// Whether the <see cref="Result{TOk, TErr}"/> has an invalid value
//     /// </summary>
//     public bool IsErr => _innerResult.IsErr;
//
//     /// <summary>
//     /// Returns the value of the <see cref="Result{TOk, TErr}"/> or throws an <see cref="AccessViolationException"/>
//     /// if the result does not contain a valid value
//     /// </summary>
//     /// <exception cref="AccessViolationException">Thrown if the result does not contain a valid value</exception>
//     public TOk Unwrap() => _innerResult.Unwrap();
//
//     /// <summary>
//     /// 
//     /// </summary>
//     /// <returns></returns>
//     public Exception UnwrapException() => _innerResult.UnwrapException();
//
//     /// <summary>
//     /// 
//     /// </summary>
//     /// <typeparam name="TErr"></typeparam>
//     /// <returns></returns>
//     public TErr UnwrapException<TErr>() where TErr : Exception => 
//         (TErr) _innerResult.UnwrapException();
//
//     /// <summary>
//     /// Exhaustively switches through all possible states.
//     /// There either is a valid value, in which case <see cref="whenOk"/> is executed,
//     /// or it is not, in which case <see cref="whenErr"/> is executed.
//     /// </summary>
//     /// <param name="whenOk"></param>
//     /// <param name="whenErr"></param>
//     public void Switch(
//         Action<TOk> whenOk,
//         Action<Exception> whenErr)
//     {
//         _innerResult.Switch(whenOk, whenErr);
//     }
//
//     /// <summary>
//     /// Like <see cref="Switch"/>, but async.
//     /// </summary>
//     public async Task SwitchAsync(
//         Func<TOk, Task> whenOk,
//         Func<Exception, Task> whenErr)
//     {
//         await _innerResult.SwitchAsync(whenOk, whenErr);
//     }
//
//     /// <summary>
//     /// Like <see cref="Switch"/>, but returns a value of type.
//     /// Both, <see cref="forOk"/> and <see cref="forErr"/> have to return the same type
//     /// </summary>
//     /// <param name="forOk">Lambda to run for when there is a valid value</param>
//     /// <param name="forErr">Lambda to run for when there is an exception</param>
//     /// <typeparam name="TR">The type returned by <see cref="forOk"/> and <see cref="forErr"/></typeparam>
//     /// <returns>The value returned from either <see cref="forOk"/> or <see cref="forErr"/></returns>
//     public TR Match<TR>(
//         Func<TOk, TR> forOk,
//         Func<Exception, TR> forErr)
//     {
//         return _innerResult.Match(forOk, forErr);
//     }
//
//     /// <summary>
//     /// Like <see cref="Match{TR}"/>, but async.
//     /// </summary>
//     public async Task<TR> MatchAsync<TR>(
//         Func<TOk, ValueTask<TR>> forOk,
//         Func<Exception, ValueTask<TR>> forErr)
//     {
//         return await _innerResult.MatchAsync(forOk, forErr);
//     }
//
//     /// <summary>
//     /// Maps the underlying value and type in a safe way. If the original <see cref="Result{TOk, TErr}"/>
//     /// does not contain a value, the value mapping is skipped but the type will be mapped.
//     /// </summary>
//     /// <param name="mapper">The mapping lambda</param>
//     /// <typeparam name="TROk">The target type</typeparam>
//     /// <returns>The <see cref="Result{T, UR}"/> of type <see cref="TROk"/> and the value provided by
//     /// <see cref="mapper"/></returns>
//     public ExceptionResult<TROk> Map<TROk>(Func<TOk, TROk> mapper)
//     {
//         if (IsOk)
//         {
//             return Ok(mapper(Unwrap()));
//         }
//
//         return Err<TROk>(UnwrapException());
//     }
//
//     /// <summary>
//     /// Like <see cref="Map{TROk}"/>, but async.
//     /// </summary>
//     public async Task<ExceptionResult<TROk>> MapAsync<TROk>(Func<TOk, ValueTask<TROk>> mapper)
//     {
//         if (IsOk)
//         {
//             return Ok(await mapper(Unwrap()));
//         }
//
//         return Err<TROk>(UnwrapException());
//     }
//
//     /// <summary>
//     /// Converts the <see cref="Result{TOk, TErr}"/> to a new one of another type which is provided by
//     /// a lambda (<see cref="binder"/>)
//     /// </summary>
//     /// <param name="binder">The lambda that provides the new <see cref="Result{TOk, TErr}"/></param>
//     /// <typeparam name="TROk">The target type</typeparam>
//     /// <returns>The new <see cref="Result{T, UR}"/></returns>
//     public ExceptionResult<TROk> Bind<TROk>(Func<TOk, ExceptionResult<TROk>> binder)
//     {
//         if (IsOk)
//         {
//             return binder(Unwrap());
//         }
//
//         return Err<TROk>(UnwrapException());
//     }
//
//     /// <summary>
//     /// Like <see cref="Bind{TR}"/>, but async
//     /// </summary>
//     public async Task<ExceptionResult<TROk>> BindAsync<TROk>(
//         Func<TOk, ValueTask<ExceptionResult<TROk>>> binder)
//     {
//         if (IsOk)
//         {
//             return await binder(Unwrap());
//         }
//
//         return Err<TROk>(UnwrapException());
//     }
//
//     /// <summary>
//     /// Allows for implicit conversions between <see cref="TOk"/> and the
//     /// <see cref="Result{TOk, TErr}"/> itself.
//     /// </summary>
//     /// <param name="value">The value to box</param>
//     /// <returns>The resulting <see cref="Result{TOk, TErr}"/></returns>
//     public static implicit operator ExceptionResult<TOk>(TOk value) => Ok(value);
//
//     /// <summary>
//     /// Allows for implicit conversions between <see cref="TErr"/> and the
//     /// <see cref="Result{TOk, TErr}"/> itself.
//     /// </summary>
//     /// <param name="value">The value to box</param>
//     /// <returns>The resulting <see cref="Result{TOk, TErr}"/></returns>
//     public static implicit operator ExceptionResult<TOk>(Exception value) => Err<TOk>(value);
// }