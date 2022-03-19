using Moq;

using NUnit.Framework;

using System;

using zControl.Test;

namespace zControl.Core.Wrapped {
	public class WrappedControllerTest {

		private IController<TestState, TestInput> subcontroller;
		private IController<TestState, TestInput> controller;
		private Func<TestState, TestState> inputMapper;
		private Func<TestState, TestInput, TestInput> outputUnmapper;

		/// <summary>
		/// Set up a wrapped controller.
		/// </summary>
		[SetUp]
		public void SetUp () {
			subcontroller = Mock.Of<IController<TestState, TestInput>>();
			inputMapper = Mock.Of<Func<TestState, TestState>>();
			outputUnmapper = Mock.Of<Func<TestState, TestInput, TestInput>>();
			controller = ZControl.WrapController(subcontroller, inputMapper, outputUnmapper);
		}

		/// <summary>
		/// Calling control on the wrapped controller should call control on the internal controller with mapped input and output.
		/// Values used in the test:
		/// <code>
		/// (0, 1) ---(map)----> (2, 3) -(control)--> 4 | (2, 4) --(unmap)---> 5
		/// </code>
		/// </summary>
		[Test]
		public void Control () {
			Mock.Get(inputMapper).Setup(mapper => mapper.Invoke(It.IsAny<TestState>())).Returns<TestState>(s => new TestState(s.Y + 2f));
			Mock.Get(subcontroller).Setup(ctrl => ctrl.Control(It.IsAny<TestState>(), It.IsAny<TestState>())).Returns(new TestInput(4f));
			Mock.Get(outputUnmapper).Setup(unmapper => unmapper.Invoke(It.IsAny<TestState>(), It.IsAny<TestInput>())).Returns(new TestInput(5f));

			Assert.AreEqual(5f, controller.Control(new TestState(0f), new TestState(1f)).X);

			Mock.Get(inputMapper).Verify(mapper => mapper.Invoke(It.Is<TestState>(i => i.IsInRange(0f, float.Epsilon))));
			Mock.Get(inputMapper).Verify(mapper => mapper.Invoke(It.Is<TestState>(i => i.IsInRange(1f, float.Epsilon))));
			Mock.Get(subcontroller).Verify(ctrl => ctrl.Control(It.Is<TestState>(i => i.IsInRange(2f, float.Epsilon)), It.Is<TestState>(i => i.IsInRange(3f, float.Epsilon))));
			Mock.Get(outputUnmapper).Verify(unmapper => unmapper.Invoke(It.Is<TestState>(i => i.IsInRange(2f, float.Epsilon)), It.Is<TestInput>(i => i.IsInRange(4f, float.Epsilon))));
		}
	}
}