using UnityEngine;

using zControl.Math;

namespace zControl.Unity.Debug {
	public class AxisAngleComposition : MonoBehaviour {

		public Transform A, B;

		void Update () {
			Vector3 boa = AxisAngle.ComposeAxisAngle(-A.rotation.ToAngleAxis(), B.rotation.ToAngleAxis());
			transform.rotation = Quaternion.AngleAxis(boa.magnitude, boa.normalized);
		}
	}
}