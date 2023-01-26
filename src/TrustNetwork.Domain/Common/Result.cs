using System.Diagnostics.Contracts;

namespace TrustNetwork.Domain.Common
{
#nullable disable
    public readonly struct Result<A>
    {
        private readonly ResultState State { get; }
        public A Value { get; }
        public Exception Exception { get; }

        /// <summary>
        /// Constructor of a concrete value
        /// </summary>
        /// <param name="value"></param>
        public Result(A value)
        {
            State = ResultState.Success;
            Value = value;
            Exception = null;
        }

        /// <summary>
        /// Constructor of an error value
        /// </summary>
        /// <param name="e"></param>
        public Result(Exception e)
        {
            State = ResultState.Faulted;
            Exception = e;
            Value = default;
        }

        /// <summary>
        /// Implicit conversion operator from A to Result<A>
        /// </summary>
        /// <param name="value">Value</param>
        [Pure]
        public static implicit operator Result<A>(A value) =>
            new Result<A>(value);

        /// <summary>
        /// True if the result is faulted
        /// </summary>
        [Pure]
        public bool IsFaulted =>
            State == ResultState.Faulted;

        /// <summary>
        /// True if the struct is in an invalid state
        /// </summary>
        [Pure]
        public bool IsBottom =>
            State == ResultState.Faulted && Exception == null;

        /// <summary>
        /// True if the struct is in an success
        /// </summary>
        [Pure]
        public bool IsSuccess =>
            State == ResultState.Success;

        /// <summary>
        /// Convert the value to a showable string
        /// </summary>
        [Pure]
        public override string ToString() =>
            IsFaulted
                ? Exception?.ToString() ?? "(Bottom)"
                : Value?.ToString() ?? "(null)";

        [Pure]
        public R Match<R>(Func<A, R> Succ, Func<Exception, R> Fail) =>
            IsFaulted
                ? Fail(Exception)
                : Succ(Value);

        [Pure]
        public Result<B> Map<B>(Func<A, B> f) =>
            IsFaulted
                ? new Result<B>(Exception)
                : new Result<B>(f(Value));

        [Pure]
        public async Task<Result<B>> MapAsync<B>(Func<A, Task<B>> f) =>
            IsFaulted
                ? new Result<B>(Exception)
                : new Result<B>(await f(Value));

        private enum ResultState : byte
        {
            Faulted,
            Success
        }
    }
}
