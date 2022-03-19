using UnityEngine;

namespace zControl.Unity.Debug {
	public class QuaternionComposition : MonoBehaviour {

		public Transform A, B;

		void Update () {
			transform.rotation = B.rotation * Quaternion.Inverse(A.rotation);
		}
	}
}