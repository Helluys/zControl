namespace zControl.Math.Types {
	public struct Vector3 : IInnerProductSpace<Vector3>, IField<Vector3> {
		public readonly Float x, y, z;

		public Vector3 (Float x, Float y, Float z) {
			this.x = x;
			this.y = y;
			this.z = z;
		}
		
		public override string ToString () {
			return "(" + x + "; " + y + "; " + z + ")";
		}

		#region conversion
		public static implicit operator UnityEngine.Vector3 (Vector3 v) => new UnityEngine.Vector3(v.x, v.y, v.z);
		public static implicit operator Vector3 (UnityEngine.Vector3 v) => new Vector3(v.x, v.y, v.z);
		public static implicit operator System.Numerics.Vector3 (Vector3 v) => new System.Numerics.Vector3(v.x, v.y, v.z);
		public static implicit operator Vector3 (System.Numerics.Vector3 v) => new Vector3(v.X, v.Y, v.Z);
		#endregion

		#region additive group
		public Vector3 Zero => zero;
		public readonly Vector3 Opposite () => -this;
		public readonly Vector3 Plus (Vector3 other) => this + other;
		public readonly Vector3 Minus (Vector3 other) => this - other;

		public static readonly Vector3 zero = UnityEngine.Vector3.zero;
		public static Vector3 operator + (Vector3 a) => a;
		public static Vector3 operator + (Vector3 a, Vector3 b) => new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
		public static Vector3 operator - (Vector3 a) => new Vector3(-a.x, -a.y, -a.z);
		public static Vector3 operator - (Vector3 a, Vector3 b) => a + (-b);
		#endregion

		#region field
		Vector3 IField<Vector3>.One => one;
		public readonly Vector3 Inverse () => new Vector3(1f / x, 1f / y, 1f / z);
		public readonly Vector3 Multiply (Vector3 other) => this * other;

		public static readonly Vector3 one = new Vector3(1f, 1f, 1f);
		public static Vector3 operator * (Vector3 a, Vector3 b) => new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
		#endregion

		#region inner product space on reals
		public Float One => Float.one;
		public readonly Vector3 Multiply (Float a) => a * this;
		public readonly Float Dot (Vector3 v) => x * v.x + y * v.y + z * v.z;
		public readonly Float Magnitude => (Float) System.Math.Sqrt(SqrMagnitude);
		public readonly Float SqrMagnitude => Dot(this);
		public readonly Vector3 Normalized => System.Math.Abs(Magnitude) < Float.Epsilon ? zero : this / Magnitude;

		public static Vector3 operator * (Float a, Vector3 b) => new Vector3(a * b.x, a * b.y, a * b.z);
		public static Vector3 operator * (Vector3 a, Float b) => b * a;
		public static Vector3 operator / (Vector3 a, Float b) => (1f / b) * a;
		#endregion

		public readonly Vector3 Cross (Vector3 v) => new Vector3(y * v.z - z * v.y, z * v.x - x * v.z, x * v.y - y * v.x);
	}
}
