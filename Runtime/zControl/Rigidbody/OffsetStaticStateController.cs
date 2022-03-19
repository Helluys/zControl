using zControl.Error;
using zControl.Math.Types;
using zControl.Util;

using zControl.Rigidbody.Model;

namespace zControl.Rigidbody {
	/// <summary>
	/// Control a rigidbody <see cref="StaticState"/> using PID feedback to compute a <see cref="DynamicInput"/> from an offset handle.
	/// </summary>
	public class OffsetStaticStateController : IErrorController<StaticState, DynamicInput> {
		/// <summary>
		/// Point at which the control force is applied.
		/// </summary>
		public Vector3 HandlePoint { get; set; }

		public Vector3 Force { get; private set; }

		public Vector3 Torque { get; private set; }

		/// <inheritdoc/>
		public StaticState Error => errorController.Error;

		private readonly StaticStateController staticStateController;
		private readonly IErrorController<StaticState, DynamicInput> errorController;

		/// <summary>
		/// Position gains (proportional, integrate, derivate).
		/// </summary>
		public Vector3 PositionGains {
			get => staticStateController.PositionGains;
			set => staticStateController.PositionGains = value;
		}

		/// <summary>
		/// Attitude gains (proportional, integrate, derivate).
		/// </summary>
		public Vector3 AttitudeGains {
			get => staticStateController.AttitudeGains;
			set => staticStateController.AttitudeGains = value;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="positionGains">Proportionate, derivate and integrate factors applied to position control</param>
		/// <param name="attitudeGains">Proportionate, derivate and integrate factors applied to attitude control</param>
		public OffsetStaticStateController (IClock timeFunction) {
			staticStateController = new StaticStateController(timeFunction);
			errorController = ZControl.Map(staticStateController, HandleOffset);
		}

		/// <inheritdoc/>
		public DynamicInput Control (StaticState state, StaticState target) => errorController.Control(state, target);

		/// <summary>
		/// Compensate torque induced by offset force in the dynamic input.<br />
		/// </summary>
		/// <param name="input">The input without torque compensation</param>
		/// <returns>The input to feed into the system to reach the target</returns>
		private DynamicInput HandleOffset (StaticState state, DynamicInput input) {
			Force = input.force;
			Torque = input.torque - HandlePoint.Cross(Force);

			return new DynamicInput() {
				force = Force,
				torque = Torque
			};
		}
	}
}
