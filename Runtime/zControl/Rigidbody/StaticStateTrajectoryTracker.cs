using UnityEngine;
using zControl.Error.Trajectory;
using zControl.Util;
using zControl.Core.Trajectory;
using zControl.Rigidbody.Model;

namespace zControl.Rigidbody {
	/// <summary>
	/// Control a rigidbody static state (position and attitude) along a <see cref="Trajectory{S}"/> using error feedback and dynamic input (force and torque).
	/// </summary>
	public class StaticStateTrajectoryTracker : IErrorTrajectoryTracker<StaticState, DynamicInput> {
		/// <summary>
		/// Underlying controller.
		/// </summary>
		private readonly StaticStateController controller;

		/// <summary>
		/// Underlying trajectory tracker.
		/// </summary>
		private readonly ErrorTrajectoryTracker<StaticState, DynamicInput> tracker;

		/// <inheritdoc/>
		public event ITrajectoryTracker<StaticState, DynamicInput>.PointEvent OnPointReached;

		/// <inheritdoc/>
		public StaticState Error => tracker.Error;

		/// <inheritdoc/>
		public Vector3 Force => controller.Force;

		/// <inheritdoc/>
		public Vector3 Torque => controller.Torque;

		/// <inheritdoc/>
		public TrackedTrajectory<StaticState> Trajectory => tracker.Trajectory;

		/// <summary>
		/// Position gains (proportional, integrate, derivate).
		/// </summary>
		public Vector3 PositionGains {
			get => controller.PositionGains;
			set { controller.PositionGains = value; }
		}

		/// <summary>
		/// Attitude gains (proportional, integrate, derivate).
		/// </summary>
		public Vector3 AttitudeGains {
			get => controller.AttitudeGains;
			set { controller.AttitudeGains = value; }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="trajectory">the trajectory</param>
		/// <param name="clock">the clock</param>
		public StaticStateTrajectoryTracker (TrackedTrajectory<StaticState> trajectory, IClock clock) {
			controller = new StaticStateController(clock);
			tracker = new ErrorTrajectoryTracker<StaticState, DynamicInput>(controller, trajectory);

			tracker.OnPointReached += OnPointReached;
		}

		/// <inheritdoc/>
		public DynamicInput Control (StaticState state) {
			return tracker.Control(state);
		}
	}
}
