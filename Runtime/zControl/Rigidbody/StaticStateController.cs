using zControl.Error;
using zControl.Math.Types;

using zControl.Rigidbody.Model;
using zControl.Util;

namespace zControl.Rigidbody {
	/// <summary>
	/// Control a rigidbody <see cref="StaticState"/> using PID feedback to compute a <see cref="DynamicInput"/>.
	/// </summary>
	public class StaticStateController : IErrorController<StaticState, DynamicInput> {
		/// <summary>
		/// Underlying PID controller.
		/// </summary>
		private IPIDController<StaticState, DynamicInput, Vector2> controller;

		/// <inheritdoc/>
		public StaticState Error => controller.Error;

		/// <inheritdoc/>
		public Vector3 Force { get; private set; }

		/// <inheritdoc/>
		public Vector3 Torque { get; private set; }

		/// <summary>
		/// Position gains (proportional, integrate, derivate).
		/// </summary>
		public Vector3 PositionGains {
			get {
				return new Vector3(controller.KP.x, controller.KI.x, controller.KD.x);
			}
			set {
				controller.KP = new Vector2(value.x, controller.KP.y);
				controller.KI = new Vector2(value.y, controller.KI.y);
				controller.KD = new Vector2(value.z, controller.KD.y);
			}
		}

		/// <summary>
		/// Attitude gains (proportional, integrate, derivate).
		/// </summary>
		public Vector3 AttitudeGains {
			get {
				return new Vector3(controller.KP.y, controller.KI.y, controller.KD.y);
			}
			set {
				controller.KP = new Vector2(controller.KP.x, value.x);
				controller.KI = new Vector2(controller.KI.x, value.y);
				controller.KD = new Vector2(controller.KD.x, value.z);
			}
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="type">Controller type</param>
		/// <param name="positionGains">Proportionate, derivate and integrate factors applied to position control</param>
		/// <param name="attitudeGains">Proportionate, derivate and integrate factors applied to attitude control</param>
		public StaticStateController (IClock clock) {
			controller = ZControl.PIDController<StaticState, DynamicInput, Vector2>(Feedback, clock);
		}

		/// <inheritdoc/>
		public DynamicInput Control (StaticState state, StaticState target) {
			return controller.Control(state, target);
		}

		/// <summary>
		/// Feedback computation method.<br />
		/// Uses implemented error mapping function prior to converting it into an <see cref="DynamicInput"/>.
		/// </summary>
		/// <param name="error">The error, that is <code>target - state</code></param>
		/// <returns>The input to feed into the system to reach the target</returns>
		private DynamicInput Feedback (StaticState error) {
			Force = error.position;
			Torque = error.attitude;

			return new DynamicInput() {
				force = Force,
				torque = Torque
			};
		}
	}
}
