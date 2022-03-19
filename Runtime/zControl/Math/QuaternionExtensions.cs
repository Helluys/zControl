using UnityEngine;

namespace zControl.Math {
	public static class QuaternionExtensions {
		public static Vector3 ToAngleAxis(this Quaternion quaternion) {
			quaternion.ToAngleAxis(out float angle, out Vector3 axis);
			return angle * axis;
		}

		public static Quaternion AngleAxis (Vector3 angleAxis) {
			return Quaternion.AngleAxis(angleAxis.magnitude, angleAxis.normalized);
		}
	}
}
