using Moq;

using NUnit.Framework;

using System;

using zControl.Test;

namespace zControl.Core.Wrapped {
	public class WrappedSystemTest {

		private ISystem<TestState, TestInput> subsystem;
		private ISystem<TestState, TestInput> system;
		private Func<TestState, TestInput, TestInput> inputMapper;
		private Func<TestState, TestState> stateUnmapper;

		/// <summary>
		/// Set up a wrapped system.
		/// </summary>
		[SetUp]
		public void SetUp () {
			subsystem = Mock.Of<ISystem<TestState, TestInput>>();
			inputMapper = Mock.Of<Func<TestState, TestInput, TestInput>>();
			stateUnmapper = Mock.Of<Func<TestState, TestState>>();
			system = ZControl.WrapSystem(subsystem, inputMapper, stateUnmapper);
		}

		/// <summary>
		/// Getting the wrapped system state should return the unmapped internal system state.
		/// Values used in the test:
		/// <code>
		/// 0 ---(unmap)----> 1
		/// </code>
		/// </summary>
		[Test]
		public void GetState () {
			Mock.Get(subsystem).SetupGet(sys => sys.State).Returns(new TestState(0f));
			Mock.Get(stateUnmapper).Setup(unmapper => unmapper.Invoke(It.IsAny<TestState>())).Returns(new TestState(1f));

			Assert.AreEqual(1f, system.State.Y, float.Epsilon);

			Mock.Get(stateUnmapper).Verify(unmapper => unmapper.Invoke(It.Is<TestState>(i => i.IsInRange(0f, float.Epsilon))));
		}

		/// <summary>
		/// Applying input to the wrapped system should apply the mapped input to the subsystem and return the unmapped internal state.
		/// Values used in the test:
		/// <code>
		/// 0 ----------------------(map)----> 3 --(Update)-> void
		/// 1 ---(unmap)--> 2 --/
		/// </code>
		/// </summary>
		[Test]
		public void Update () {
			Mock.Get(subsystem).SetupGet(sys => sys.State).Returns(new TestState(1f));
			Mock.Get(stateUnmapper).Setup(unmapper => unmapper.Invoke(It.IsAny<TestState>())).Returns(new TestState(2f));
			Mock.Get(inputMapper).Setup(mapper => mapper.Invoke(It.IsAny<TestState>(), It.IsAny<TestInput>())).Returns(new TestInput(3f));

			system.Update(new TestInput(0f));

			Mock.Get(inputMapper).Verify(mapper => mapper.Invoke(It.Is<TestState>(s => s.IsInRange(2f, float.Epsilon)), It.Is<TestInput>(i => i.IsInRange(0f, float.Epsilon))));
			Mock.Get(subsystem).Verify(sys => sys.Update(It.Is<TestInput>(i => i.IsInRange(3f, float.Epsilon))));
		}
	}
}