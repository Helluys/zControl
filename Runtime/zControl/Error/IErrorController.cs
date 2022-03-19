using zControl.Core;
using zControl.Math;

namespace zControl.Error {
	/// <summary>
	/// A type of <see cref="IController{S, U}"/> that attempts to reduce an <see cref="Error"/> to <see cref="IGroup{T}.Zero"/>.
	/// </summary>
	/// <typeparam name="S">type of the controller input (system state)</typeparam>
	/// <typeparam name="U">type of the controller output (system input)</typeparam>
	public interface IErrorController<S, U> : IController<S, U> where S : IGroup<S> {
		/// <summary>
		/// The difference, as per <see cref="IGroup{T}.Minus(T)"/>, between the system target and the system state.
		/// </summary>
		public S Error { get; }
	}
}
