using UnityEngine;

using zControl.Core;
using zControl.Math;
using zControl.Rigidbody;
using zControl.Rigidbody.Model;
using zControl.Unity.Clock;

#if UNITY_EDITOR
using zControl.Unity.Debug;
#endif

namespace zControl.Unity {
	/// <summary>
	/// Unity component to bind a <see cref="UnityEngine.Rigidbody"/> to a linear <see cref="IController{Rigidbody.PositionState, Rigidbody.Model.DynamicInput}"/>.
	/// </summary>
	[RequireComponent(typeof(UnityEngine.Rigidbody))]
	public class LinearRigidbodyController : MonoBehaviour {
		[Tooltip(tooltip: "The controller target")]
		public TargetKinematicState target;

		[Tooltip(tooltip: "The gains of the proportionate, integrate and derivate feedback for position control")]
		public Vector3 PositionGains;

		[Tooltip(tooltip: "The gains of the proportionate, integrate and derivate feedback for attitude control")]
		public Vector3 AttitudeGains;

		[Tooltip(tooltip: "The maximum force the controller can exert on the rigidbody")]
		public float maxForce;

		[Tooltip(tooltip: "The maximum torque the controller can exert on the rigidbody")]
		public float maxTorque;

		private StaticStateController controller;
		private RigidbodyActuator actuator;

#if UNITY_EDITOR
		[SerializeField] private Vector3Tracer positionErrorTracer = new Vector3Tracer();
		[SerializeField] private Vector3Tracer attitudeErrorTracer = new Vector3Tracer();
		[SerializeField] private Vector3Tracer forceTracer = new Vector3Tracer();
		[SerializeField] private Vector3Tracer torqueTracer = new Vector3Tracer();
#endif

		/// <summary>
		/// Business representation of current state.
		/// </summary>
		private StaticState State => new StaticState() {
			position = transform.position,
			attitude = transform.rotation.ToAngleAxis()
		};

		/// <summary>
		/// Business representation of target state.
		/// </summary>
		private StaticState Target => new StaticState() {
			position = target.TargetPosition,
			attitude = target.TargetAttitude
		};

		/// <summary>
		/// Initialise components.
		/// </summary>
		void Start () {
			controller = new StaticStateController(new UnityFixedClock());
			actuator = new RigidbodyActuator(new RigidbodySystem(GetComponent<UnityEngine.Rigidbody>()));
			Update();
		}

		/// <summary>
		/// Update gains.
		/// </summary>
		void Update () {
			controller.PositionGains = PositionGains;
			controller.AttitudeGains = AttitudeGains;

			actuator.ActionPoint = transform.position;
			actuator.MaxForce = maxForce;
			actuator.MaxTorque = maxTorque;
		}

		/// <summary>
		/// Compute force and torque to control the rigidbody to the desired state.<br />
		/// This method delegates the computation to the underlying <see cref="StaticStateController"/>.
		/// </summary>
		void FixedUpdate () {
			actuator.Update(controller.Control(State, Target));

#if UNITY_EDITOR
			positionErrorTracer.Measure(Time.fixedTime, controller.Error.position);
			attitudeErrorTracer.Measure(Time.fixedTime, controller.Error.attitude);
			forceTracer.Measure(Time.fixedTime, actuator.Input.force);
			torqueTracer.Measure(Time.fixedTime, actuator.Input.torque);
#endif
		}
	}
}