using System;
using System.Collections.Generic;

namespace zControl.Core.Trajectory {
	/// <summary>
	/// Controls a system state so that it follows a <see cref="Trajectory"/> by providing a <see cref="IController{S, U}"/> with the <see cref="TrackedTrajectory{S}"/>
	/// </summary>
	/// <typeparam name="S"></typeparam>
	/// <typeparam name="U"></typeparam>
	public class TrajectoryTracker<S, U> : ITrajectoryTracker<S, U> {
		/// <inheritdoc/>
		public event ITrajectoryTracker<S, U>.PointEvent OnPointReached;

		/// <inheritdoc/>
		public event Action OnTrajectoryCompleted;

		/// <inheritdoc/>
		public TrackedTrajectory<S> Trajectory { get; private set; }

		/// <summary>
		/// The underlying controller.
		/// </summary>
		private readonly IController<S, U> controller;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="controller">the controller</param>
		/// <param name="trajectory">the trajectory</param>
		public TrajectoryTracker (IController<S, U> controller, TrackedTrajectory<S> trajectory) {
			this.controller = controller;
			Trajectory = trajectory;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="controller">the controller</param>
		/// <param name="points">the state Points that make up the trajectory</param>
		/// <param name="pointMatcher">the function that computes whether the next point is reached</param>
		/// <param name="loop">whether the trajectory is a loop</param>
		/// <exception cref="ArgumentException">thrown if <paramref name="points"/> is empty</exception>
		public TrajectoryTracker (IController<S, U> controller, IEnumerable<S> points, TrackedTrajectory<S>.PointMatcher pointMatcher, bool loop = false) {
			this.controller = controller;
			Trajectory = new TrackedTrajectory<S>(points, pointMatcher, loop);
		}

		/// <inheritdoc/>
		public U Control (S state) {
			if (Trajectory.Update(state)) {
				OnPointReached?.Invoke(Trajectory.PreviousPoint);
				if (Trajectory.Completed) {
					OnTrajectoryCompleted?.Invoke();
				}
			}

			return controller.Control(state, Trajectory.NextPoint);
		}
	}
}
