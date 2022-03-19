using System;

namespace zControl.Test {
	public class TestInput {
		public float X { get; set; }
		public TestInput (float x) => X = x;

		public bool IsInRange(float x, float delta) {
			return System.Math.Abs(X - x) <= delta;
		}
	}
}
