using System;

using UnityEngine;

using zControl.Core;
using zControl.Rigidbody.Model;

namespace zControl.Unity {
	public class RigidbodyActuator : ISystem<StaticState, DynamicInput> {
		public Vector3 ActionPoint { get {
				return rigidbodySystem.ActionPoint;
			}
			set {
				rigidbodySystem.ActionPoint = value;
			}
		}
		public float MaxForce { get; set; }
		public float MaxTorque { get; set; }

		public StaticState State => system.State;

		public DynamicInput Input { get; private set; }

		private readonly ISystem<StaticState, DynamicInput> system;
		private readonly RigidbodySystem rigidbodySystem;

		public RigidbodyActuator (RigidbodySystem originalSystem) {
			rigidbodySystem = originalSystem;
			system = ZControl.MapSystem(originalSystem, Limiter());
		}

		private Func<StaticState, DynamicInput, DynamicInput> Limiter () =>
			(state, input) => new DynamicInput {
				force = (input.force.SqrMagnitude > MaxForce * MaxForce) ? MaxForce * input.force.Normalized : input.force,
				torque = (input.torque.SqrMagnitude > MaxTorque * MaxTorque) ? MaxTorque * input.torque.Normalized : input.torque
			};

		public void Update (DynamicInput input) {
			Input = input;
			system.Update(input);
		}
	}
}
