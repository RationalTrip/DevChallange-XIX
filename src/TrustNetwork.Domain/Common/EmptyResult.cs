using System.Diagnostics.Contracts;

namespace TrustNetwork.Domain.Common
{
#nullable disable
    public readonly struct EmptyResult
    {
        private readonly ResultState State { get; } = ResultState.Success;
        public Exception Exception { get; }

        /// <summary>
        /// Constructor of a concrete value
        /// </summary>
        /// <param name="value"></param>
        public EmptyResult()
        {
            State = ResultState.Success;
            Exception = null;
        }

        /// <summary>
        /// Constructor of an error value
        /// </summary>
        /// <param name="e"></param>
        public EmptyResult(Exception e)
        {
            State = ResultState.Faulted;
            Exception = e;
        }

        public static EmptyResult SuccessResult => new();

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
                : "(Success Result)";

        [Pure]
        public R Match<R>(Func<R> Succ, Func<Exception, R> Fail) =>
            IsFaulted
                ? Fail(Exception)
                : Succ();

        private enum ResultState : byte
        {
            Faulted,
            Success
        }
    }
}
