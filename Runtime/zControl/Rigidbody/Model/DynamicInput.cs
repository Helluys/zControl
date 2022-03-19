using zControl.Math;
using zControl.Math.Types;

namespace zControl.Rigidbody.Model {
	/// <summary>
	/// Aggregation of a force and a torque that makes up the input to a rigidbody system.
	/// </summary>
	[System.Serializable]
	public class DynamicInput : IVector<DynamicInput, Vector2> {
		public Vector3 force;
		public Vector3 torque;

		public Vector2 One => Vector2.one;

		public DynamicInput Zero =>
			new DynamicInput() {
				force = Vector3.zero,
				torque = Vector3.zero
			};

		public DynamicInput DivideBy (Vector2 diviser) =>
			new DynamicInput {
				force = force / diviser.x,
				torque = torque / diviser.y
			};

		public DynamicInput Minus (DynamicInput other) => new DynamicInput {
			force = force - other.force,
			torque = torque - other.torque
		};

		public DynamicInput Multiply (Vector2 factor) =>
			new DynamicInput {
				force = force * factor.x,
				torque = torque * factor.y
			};

		public DynamicInput Opposite () =>
			new DynamicInput {
				force = -force,
				torque = -torque
			};

		public DynamicInput Plus (DynamicInput other) => new DynamicInput {
			force = force + other.force,
			torque = torque + other.torque
		};

		public override string ToString () {
			return force.ToString() + "; " + torque.ToString();
		}

	}
}