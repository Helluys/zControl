using Vector3 = UnityEngine.Vector3;

using zControl.Core;
using zControl.Math;
using zControl.Rigidbody.Model;

namespace zControl.Unity {
	public class RigidbodySystem : ISystem<StaticState, DynamicInput> {

		/// <inheritdoc/>
		public StaticState State => new StaticState() {
			position = rigidbody.transform.position,
			attitude = rigidbody.transform.rotation.ToAngleAxis()
		};

		/// <inheritdoc/>
		public DynamicInput Input { get; private set; }

		public Vector3 ActionPoint { get; set; }

		private readonly UnityEngine.Rigidbody rigidbody;

		public RigidbodySystem(UnityEngine.Rigidbody rigidbody) {
			this.rigidbody = rigidbody;
		}

		public void Update (DynamicInput input) {
			Input = input;
			rigidbody.AddForceAtPosition(input.force, ActionPoint);
			rigidbody.AddTorque(input.torque);
		}
	}
}
