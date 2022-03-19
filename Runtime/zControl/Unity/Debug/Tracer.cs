using System;

using UnityEngine;

namespace zControl.Unity.Debug {
	[Serializable]
	public class Tracer {
		public AnimationCurve curve;

		public Tracer () {
			curve = new AnimationCurve();
		}

		public void Measure (float time, float value) {
			curve.AddKey(new Keyframe(time, value));
		}
	}
}