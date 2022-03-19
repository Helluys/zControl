using NUnit.Framework;

using zControl.Core;
using zControl.Test;

namespace zControl.Error {
	public class ErrorFeedbackControllerTest {

		private readonly int ITERATIONS = 10;

		private IController<TestState, TestInput> controller;
		private ISystem<TestState, TestInput> system;
		private TestState target;

		[SetUp]
		public void SetUp () {
			controller = ZControl.ErrorController<TestState, TestInput>(e => new TestInput(0.5f * e.Y));
			system = new TestSystem(0f);
			target = new TestState(10f);
		}

		/// <summary>
		/// A proportional feedback should reduce error of a system with no dynamics.
		/// </summary>
		[Test]
		public void Control () {
			float lastError = System.Math.Abs(target.Y - system.State.Y);
			for (int i = 0; i < ITERATIONS; i++) {
				system.Update(controller.Control(system.State, target));
				float error = System.Math.Abs(target.Y - system.State.Y);
				Assert.Less(error, lastError);
				lastError = error;
			}
		}
	}
}