using zControl.Math;
using zControl.Math.Types;

namespace zControl.Rigidbody.Model {
	/// <summary>
	/// Position and attitude state. Supports algebraic operations.
	/// </summary>
	[System.Serializable]
	public class StaticState : IVector<StaticState, Vector2>, IVector<StaticState, Float> {
		/// <summary>
		/// Position.
		/// </summary>
		public Vector3 position;

		/// <summary>
		/// Attitude, using angle-axis representation, in radians.
		/// </summary>
		public Vector3 attitude;

		public StaticState Zero => new StaticState() {
			position = Vector3.zero,
			attitude = Vector3.zero
		};

		public Vector2 One => Vector2.one;
		Float IVector<StaticState, Float>.One => Float.one;

		public static StaticState operator + (StaticState a) => a;
		public static StaticState operator - (StaticState a) => new StaticState() {
			position = -a.position,
			attitude = -a.attitude
		};

		// Warning: not commutative, breaks IAbelian which is required from IVector
		public static StaticState operator + (StaticState a, StaticState b) {
			return new StaticState() {
				position = a.position + b.position,
				attitude = AxisAngle.ComposeAxisAngle(b.attitude, a.attitude)
			};
		}
		public static StaticState operator - (StaticState a, StaticState b) => a + -b;

		public static StaticState operator * (StaticState s, float factor) => factor * Vector2.one * s;
		public static StaticState operator * (Vector2 factor, StaticState s) =>
			new StaticState() {
				position = factor.x * s.position,
				attitude = factor.y * s.attitude
			};
		public static StaticState operator / (StaticState s, Vector2 diviser) => new Vector2(1f / diviser.x, 1f / diviser.y) * s;

		public StaticState Opposite () => -this;
		public StaticState Minus (StaticState s) => this - s;
		public StaticState Plus (StaticState other) => this + other;
		public StaticState Multiply (Vector2 factor) => factor * this;
		public StaticState DivideBy (Vector2 diviser) => this / diviser;
		public StaticState Multiply (Float factor) => this * factor;

		public override string ToString () {
			return "(" + position + "; " + attitude + ")";
		}
	}
}