using System.Collections.Generic;

using UnityEngine;

using zControl.Core;
using zControl.Core.Trajectory;
using zControl.Math;
using zControl.Rigidbody;
using zControl.Rigidbody.Model;
using zControl.Unity.Clock;

using System.Linq;
using System;
using zControl.Unity.Debug;

namespace zControl.Unity {
	/// <summary>
	/// Unity component to bind a <see cref="UnityEngine.Rigidbody"/> to a linear <see cref="IController{Rigidbody.PositionState, Rigidbody.Model.DynamicInput}"/>.
	/// </summary>
	[RequireComponent(typeof(UnityEngine.Rigidbody))]
	public class LinearRigidbodyTrajectoryTracker : MonoBehaviour {
		[Tooltip(tooltip: "The tracked trajectory")]
		public List<TargetKinematicState> trajectory;

		[Tooltip(tooltip: "Whether the trajectory is a loop")]
		public bool loop;

		[Tooltip(tooltip: "The gains of the proportionate, integrate and derivate feedback for position control")]
		public Vector3 PositionGains;

		[Tooltip(tooltip: "The gains of the proportionate, integrate and derivate feedback for attitude control")]
		public Vector3 AttitudeGains;

		[Tooltip(tooltip: "The maximum force the controller can exert on the rigidbody")]
		public float maxForce;

		[Tooltip(tooltip: "The maximum torque the controller can exert on the rigidbody")]
		public float maxTorque;

		private StaticStateTrajectoryTracker controller;
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
		private IEnumerable<StaticState> Trajectory => trajectory.Select(p => new StaticState() {
			position = p.TargetPosition,
			attitude = p.TargetAttitude
		});

		/// <summary>
		/// Initialise components.
		/// </summary>
		void Start () {
			controller = new StaticStateTrajectoryTracker(new TrackedTrajectory<StaticState>(Trajectory, PointMatcher, loop), new UnityFixedClock());
			actuator = new RigidbodyActuator(new RigidbodySystem(GetComponent<UnityEngine.Rigidbody>()));

#if UNITY_EDITOR
			controller.OnPointReached += LogReachedPoint;
#endif

			Update();
		}

		private void LogReachedPoint (StaticState point) {
			UnityEngine.Debug.Log("Reached " + point);
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
			actuator.Update(controller.Control(State));

#if UNITY_EDITOR
			positionErrorTracer.Measure(Time.fixedTime, controller.Error.position);
			attitudeErrorTracer.Measure(Time.fixedTime, controller.Error.attitude);
			forceTracer.Measure(Time.fixedTime, actuator.Input.force);
			torqueTracer.Measure(Time.fixedTime, actuator.Input.torque);
#endif
		}

		private bool PointMatcher (StaticState currentState, StaticState previousPoint, StaticState nextPoint) {
			Vector3 direction = (nextPoint.position.Minus(previousPoint.position)).Normalized;
			return currentState.position.Dot(direction) > nextPoint.position.Dot(direction);
		}
	}
}