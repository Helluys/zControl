using Moq;

using NUnit.Framework;

using System;

using zControl.Test;

namespace zControl.Core.Wrapped {
	public class WrappedControlledSystemTest {

		private IControlledSystem<TestState> subsystem;
		private IControlledSystem<TestState> system;
		private Func<TestState, TestState> stateMapper;
		private Func<TestState, TestState> stateUnmapper;

		/// <summary>
		/// Set up a wrapped controlled.
		/// </summary>
		[SetUp]
		public void SetUp () {
			subsystem = Mock.Of<IControlledSystem<TestState>>();
			stateMapper = Mock.Of<Func<TestState, TestState>>();
			stateUnmapper = Mock.Of<Func<TestState, TestState>>();
			system = ZControl.WrapControlledSystem(subsystem, stateMapper, stateUnmapper);
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
		}

		/// <summary>
		/// Getting the wrapped system target should return the unmapped internal system target.
		/// Values used in the test:
		/// <code>
		/// 0 ---(unmap)----> 1
		/// </code>
		/// </summary>
		[Test]
		public void GetTarget () {
			Mock.Get(subsystem).SetupGet(sys => sys.Target).Returns(new TestState(0f));
			Mock.Get(stateUnmapper).Setup(unmapper => unmapper.Invoke(It.IsAny<TestState>())).Returns(new TestState(1f));

			Assert.AreEqual(1f, system.Target.Y, float.Epsilon);
		}

		/// <summary>
		/// Setting the wrapped system target should set the internal system target with the mapped value.
		/// Values used in the test:
		/// <code>
		/// 0 ---(map)----> 1
		/// </code>
		/// </summary>
		[Test]
		public void SetTarget () {
			Mock.Get(stateMapper).Setup(mapper => mapper.Invoke(It.IsAny<TestState>())).Returns(new TestState(1f));

			system.Target = new TestState(0f);

			Mock.Get(subsystem).VerifySet(sys => sys.Target = It.Is<TestState>(s => s.IsInRange(1f, float.Epsilon)));
		}

		/// <summary>
		/// Updating the wrapped system should update the subsystem.
		/// Values used in the test:
		/// <code>
		/// 0 ---(unmap)----> 1
		/// </code>
		/// </summary>
		[Test]
		public void Update () {
			system.Update();

			Mock.Get(subsystem).Verify(sys => sys.Update());
		}
	}
}