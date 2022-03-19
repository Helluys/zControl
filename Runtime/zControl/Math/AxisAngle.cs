using Mathf = UnityEngine.Mathf;

using Vector3 = zControl.Math.Types.Vector3;

namespace zControl.Math {
	/// <summary>
	/// Helper class for computing rotations in axis-angle representation.
	/// </summary>
	public static class AxisAngle {
		/// <summary>
		/// Compose two rotations, <c>a</c> <b>then</b> <c>b</c>, in axis-angle representation. The angles of both input and output vectors are in degrees,
		/// and the resulting angle is always in <c>[-180; 180]</c>.<br />
		/// Implemented using <see href="https://math.stackexchange.com/a/2627462">Gibbs composition formula</see>.
		/// </summary>
		/// <param name="a">the first rotation as an axis-angle vector (in degrees)</param>
		/// <param name="b">the second rotation as an axis-angle vector (in degrees)</param>
		/// <returns>the composed <c>b o a</c> rotation as an axis-angle vector (in degrees)</returns>
		public static Vector3 ComposeAxisAngle (Vector3 a, Vector3 b) {
			// Gibbs vectors
			Vector3 ga = Mathf.Tan(Mathf.Deg2Rad * a.Magnitude / 2f) * a.Normalized;
			Vector3 gb = Mathf.Tan(Mathf.Deg2Rad * b.Magnitude / 2f) * b.Normalized;

			// Composition Gibbs vector
			Vector3 gboa = (ga + gb - ga.Cross(gb)) / (1 - ga.Dot(gb));

			// Back to degrees axis-angle
			return 2f * Mathf.Rad2Deg * Mathf.Atan(gboa.Magnitude) * gboa.Normalized;
		}
	}
}
