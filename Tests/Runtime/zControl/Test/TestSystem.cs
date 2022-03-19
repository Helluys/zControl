using zControl.Core;

namespace zControl.Test {
	public class TestSystem : ISystem<TestState, TestInput> {
		public TestState State { get; private set; }

		public TestInput Input { get; private set; }

		public TestSystem (float initialValue) {
			State = new TestState(initialValue);
		}

		public void Update (TestInput input) {
			Input = input;
			State = new TestState(State.Y + input.X);
		}
	}
}
