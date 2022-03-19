using System;
using System.Collections.Generic;

using zControl.Core.Trajectory;
using zControl.Math;

namespace zControl.Error.Trajectory {
	/// <summary>
	/// Controls a system to follow a trajectory by reducing an <see cref="Error"/> to <see cref="IGroup{T}.Zero"/>.
	/// </summary>
	/// <typeparam name="S">type of the controller input (system state)</typeparam>
	/// <typeparam name="U">type of the controller output (system input)</typeparam>
	public class ErrorTrajectoryTracker<S, U> : IErrorTrajectoryTracker<S, U> where S : IGroup<S> {
		/// <inheritdoc/>
		public event ITrajectoryTracker<S, U>.PointEvent OnPointReached;

		/// <inheritdoc/>
		public TrackedTrajectory<S> Trajectory => coreTracker.Trajectory;

		/// <inheritdoc/>
		public S Error => error();

		/// <summary>
		/// Underlying trajectory tracker.
		/// </summary>
		private readonly TrajectoryTracker<S, U> coreTracker;

		/// <summary>
		/// Function that provides <see cref="Error"/> (needed as contained trajectory tracker interface does provide it)
		/// </summary>
		private readonly Func<S> error;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="controller">the error controller</param>
		/// <param name="trajectory">the tracked trajectory</param>
		public ErrorTrajectoryTracker (IErrorController<S, U> controller, TrackedTrajectory<S> trajectory) {
			coreTracker = new TrajectoryTracker<S, U>(controller, trajectory);
			error = () => controller.Error;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="controller">the error controller</param>
		/// <param name="points">the state Points that make up the trajectory</param>
		/// <param name="pointMatcher">the function that computes whether the next point is reached</param>
		/// <param name="loop">whether the trajectory is a loop</param>
		/// <exception cref="ArgumentException">thrown if <paramref name="points"/> is empty</exception>
		public ErrorTrajectoryTracker (IErrorController<S, U> controller, IEnumerable<S> points, TrackedTrajectory<S>.PointMatcher pointMatcher, bool loop = false) {
			coreTracker = new TrajectoryTracker<S, U>(controller, points, pointMatcher, loop);
			error = () => controller.Error;

			coreTracker.OnPointReached += OnPointReached;
		}

		/// <inheritdoc/>
		public U Control (S state) => coreTracker.Control(state);
	}
}
