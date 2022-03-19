using zControl.Core.Trajectory;
using zControl.Math;

namespace zControl.Error.Trajectory {
	/// <summary>
	/// A type of <see cref="ITrajectoryTracker{S, U}"/> that attempts to reduce an <see cref="Error"/> to <see cref="IGroup{T}.Zero"/>.
	/// </summary>
	/// <typeparam name="S">type of the controller input (system state)</typeparam>
	/// <typeparam name="U">type of the controller output (system input)</typeparam>
	public interface IErrorTrajectoryTracker<S, U> : ITrajectoryTracker<S, U> where S : IGroup<S> {
		/// <summary>
		/// The difference, as per <see cref="IGroup{T}.Minus(T)"/>, between the system target and the system state.
		/// </summary>
		S Error { get; }
	}
}