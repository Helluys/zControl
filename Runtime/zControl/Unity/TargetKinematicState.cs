using UnityEngine;

using zControl.Math;

namespace zControl.Unity {
	public class TargetKinematicState : MonoBehaviour {
		public Vector3 TargetPosition => transform.position;
		public Vector3 TargetAttitude => transform.rotation.ToAngleAxis();
	}
}