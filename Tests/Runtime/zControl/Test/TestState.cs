using zControl.Math;
using zControl.Math.Types;

namespace zControl.Test {
	public class TestState : IVector<TestState, Float> {
		public float Y { get; set; }

		public TestState Zero => new TestState(0f);
		public Float One => Float.one;

		public TestState (float x) => Y = x;

		public TestState Opposite () => new TestState(-Y);
		public TestState Plus (TestState other) => new TestState(Y + other.Y);
		public TestState Minus (TestState s) => new TestState(Y - s.Y);
		public TestState Multiply (float factor) => new TestState(factor * Y);

		public bool IsInRange (float y, float delta) => System.Math.Abs(Y - y) <= delta;

		public TestState Multiply (Float a) {
			throw new System.NotImplementedException();
		}
	}
}
