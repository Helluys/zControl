using System;

using UnityEngine;

namespace zControl.Unity.Debug {
	[Serializable]
	public class Vector3Tracer {
		public AnimationCurve X;
		public AnimationCurve Y;
		public AnimationCurve Z;

		[SerializeField] private Tracer magnitudeTracer;

		public Vector3Tracer () {
			X = new AnimationCurve();
			Y = new AnimationCurve();
			Z = new AnimationCurve();
			magnitudeTracer = new Tracer();
		}

		public void Measure (float time, Vector3 value) {
			X.AddKey(new Keyframe(time, value.x));
			Y.AddKey(new Keyframe(time, value.y));
			Z.AddKey(new Keyframe(time, value.z));
			magnitudeTracer.Measure(time, value.magnitude);
		}
	}
}