namespace zControl.Math.Types {
	public struct Vector2 : IInnerProductSpace<Vector2>, IField<Vector2> {
		public readonly Float x, y;

		public Vector2 (Float x, Float y) {
			this.x = x;
			this.y = y;
		}

		#region conversion
		public static implicit operator UnityEngine.Vector2 (Vector2 v) => new UnityEngine.Vector2(v.x, v.y);
		public static implicit operator Vector2 (UnityEngine.Vector2 v) => new Vector2(v.x, v.y);
		public static implicit operator System.Numerics.Vector2 (Vector2 v) => new System.Numerics.Vector2(v.x, v.y);
		public static implicit operator Vector2 (System.Numerics.Vector2 v) => new Vector2(v.X, v.Y);
		#endregion

		#region additive group
		public Vector2 Zero => zero;
		public readonly Vector2 Opposite () => -this;
		public readonly Vector2 Plus (Vector2 other) => this + other;
		public readonly Vector2 Minus (Vector2 other) => this - other;

		public static readonly Vector2 zero = UnityEngine.Vector2.zero;
		public static Vector2 operator + (Vector2 a) => a;
		public static Vector2 operator + (Vector2 a, Vector2 b) => new Vector2(a.x + b.x, a.y + b.y);
		public static Vector2 operator - (Vector2 a) => new Vector2(-a.x, -a.y);
		public static Vector2 operator - (Vector2 a, Vector2 b) => a + (-b);
		#endregion

		#region field
		Vector2 IField<Vector2>.One => one;
		public readonly Vector2 Inverse () => new Vector2(1f / x, 1f / y);
		public readonly Vector2 Multiply (Vector2 other) => this * other;

		public static readonly Vector2 one = UnityEngine.Vector2.one;
		public static Vector2 operator * (Vector2 a, Vector2 b) => new Vector2(a.x * b.x, a.y * b.y);
		#endregion

		#region inner product space on reals
		public Float One => Float.one;
		public readonly Vector2 Multiply (Float a) => a * this;
		public readonly Float Dot (Vector2 v) => x * v.x + y * v.y;
		public readonly Float Magnitude => (Float) System.Math.Sqrt(SqrMagnitude);
		public readonly Float SqrMagnitude => Dot(this);
		public readonly Vector2 Normalized => System.Math.Abs(Magnitude) < Float.Epsilon ? zero : this / Magnitude;
		
		public static Vector2 operator * (Float a, Vector2 b) => new Vector2(a * b.x, a * b.y);
		public static Vector2 operator * (Vector2 a, Float b) => b * a;
		public static Vector2 operator / (Vector2 a, Float b) => (1f / b) * a;
		#endregion

		public readonly Float Cross (Vector2 v) => x * v.y - y * v.x;
	}
}
