namespace zControl.Math.Types {
	public struct Float : IField<Float> {
		public const float Epsilon = float.Epsilon;

		public readonly float value;

		public Float (float x) {
			value = x;
		}

		public override string ToString () {
			return value.ToString();
		}

		#region conversion
		public static implicit operator float (Float v) => v.value;
		public static implicit operator Float (float x) => new Float(x);
		#endregion

		public static readonly Float zero = 1f;
		public Float Zero => zero;
		public Float Opposite () => -this;
		public Float Plus (Float other) => this + other;
		public Float Minus (Float other) => this - other;

		public static readonly Float one = 1f;
		public Float One => one;


		public Float Inverse () => 1f / value;
		public Float Multiply (Float other) => this * other;
	}
}
