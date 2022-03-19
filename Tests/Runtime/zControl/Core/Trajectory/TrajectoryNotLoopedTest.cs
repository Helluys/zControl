namespace zControl.Core.Trajectory {
	using Moq;

	using NUnit.Framework;

	using System.Collections.Generic;

	/// <summary>
	/// Test cases for <see cref="Trajectory"/> configured as an open trajectory (not looped).
	/// </summary>
	public class TrajectoryNotLoopedTest {
		/// <summary>
		/// Initial state.
		/// </summary>
		private const string O = "O";

		/// <summary>
		/// Additional state.
		/// </summary>
		private const string D = "D";

		/// <summary>
		/// Trajectory individual points.
		/// </summary>
		private const string A = "A", B = "B", C = "C";

		/// <summary>
		/// Trajectory points.
		/// </summary>
		private IList<string> points;

		/// <summary>
		/// Point matcher mock function.
		/// </summary>
		private TrackedTrajectory<string>.PointMatcher pointMatcher;

		/// <summary>
		/// Unit under test.
		/// </summary>
		private TrackedTrajectory<string> trajectory;

		/// <summary>
		/// Set up a mocked trajectory.
		/// </summary>
		[SetUp]
		public void SetUp () {
			points = new List<string>(new string[] { A, B, C });
			pointMatcher = Mock.Of<TrackedTrajectory<string>.PointMatcher>();
			trajectory = new TrackedTrajectory<string>(points, pointMatcher);

			Mock.Get(pointMatcher).Setup(f => f(A, It.IsAny<string>(), A)).Returns(true);
			Mock.Get(pointMatcher).Setup(f => f(B, A, B)).Returns(true);
			Mock.Get(pointMatcher).Setup(f => f(C, B, C)).Returns(true);
			Mock.Get(pointMatcher).Setup(f => f(D, C, D)).Returns(true);
		}

		/// <summary>
		/// Creating a trajectory with no point should raise an argument exception.
		/// </summary>
		[Test]
		public void NoPoint () {
			Assert.Throws<System.ArgumentException>(() => new TrackedTrajectory<string>(new string[0], pointMatcher, true));
		}

		/// <summary>
		/// Retrieving trajectory points should yield the same list of points as given on creation.
		/// </summary>
		[Test]
		public void Points () {
			CollectionAssert.AreEquivalent(points, trajectory.Points);
		}

		/// <summary>
		/// Retrieving trajectory points should yield the same list of points as given on creation, even when original collection is modified.
		/// </summary>
		[Test]
		public void PointsImmutable () {
			points.RemoveAt(0);
			points.Add(D);

			CollectionAssert.AreEquivalent(new string[] { A, B, C }, trajectory.Points);
		}

		/// <summary>
		/// Next point index should be initialised to 0.
		/// </summary>
		[Test]
		public void NextPointIndexInitialValue () {
			Assert.AreEqual(0, trajectory.NextPointIndex);
		}

		/// <summary>
		/// Next point index should be incremented on update when point matches.
		/// </summary>
		[Test]
		public void NextPointIndexIncrement () {
			Assert.AreEqual(0, trajectory.NextPointIndex);

			trajectory.Update(A);
			Assert.AreEqual(1, trajectory.NextPointIndex);

			trajectory.Update(B);
			Assert.AreEqual(2, trajectory.NextPointIndex);
		}

		/// <summary>
		/// Next point index should not be incremented on update when point does not match.
		/// </summary>
		[Test]
		public void NextPointIndexNoIncrement () {
			trajectory.Update(B);
			Assert.AreEqual(0, trajectory.NextPointIndex);

			trajectory.Update(C);
			Assert.AreEqual(0, trajectory.NextPointIndex);

			trajectory.Update(D);
			Assert.AreEqual(0, trajectory.NextPointIndex);
		}

		/// <summary>
		/// Next point index should not be incremented on update past the end of the trajectory.
		/// </summary>
		[Test]
		public void NextPointIndexLastPoint () {
			ReachLastPoint();
			Assert.AreEqual(2, trajectory.NextPointIndex);

			trajectory.Update(A);
			Assert.AreEqual(2, trajectory.NextPointIndex);

			trajectory.Update(B);
			Assert.AreEqual(2, trajectory.NextPointIndex);

			trajectory.Update(C);
			Assert.AreEqual(2, trajectory.NextPointIndex);
		}

		/// <summary>
		/// Next point should be initialised to the first point of the trajectory.
		/// </summary>
		[Test]
		public void NextPointInitialValue () {
			Assert.AreSame(A, trajectory.NextPoint);
		}

		/// <summary>
		/// Next point should be advanced on update when point matches.
		/// </summary>
		[Test]
		public void NextPointAdvance () {
			trajectory.Update(A);
			Assert.AreSame(B, trajectory.NextPoint);

			trajectory.Update(B);
			Assert.AreSame(C, trajectory.NextPoint);
		}

		/// <summary>
		/// Next point should not be advanced on update when end of the trajectory is reached.
		/// </summary>
		[Test]
		public void NextPointLastPoint () {
			ReachLastPoint();

			trajectory.Update(A);
			Assert.AreSame(C, trajectory.NextPoint);

			trajectory.Update(B);
			Assert.AreSame(C, trajectory.NextPoint);

			trajectory.Update(C);
			Assert.AreSame(C, trajectory.NextPoint);
		}

		/// <summary>
		/// Next point should not be advanced on update when point does not match.
		/// </summary>
		[Test]
		public void NextPointNoAdvance () {
			Assert.AreEqual(A, trajectory.NextPoint);
			trajectory.Update(B);
			trajectory.Update(C);
			trajectory.Update(D);
			Assert.AreEqual(A, trajectory.NextPoint);
		}

		/// <summary>
		/// Previous point should be initialised to null, and then the first given state.
		/// </summary>
		[Test]
		public void PreviousPointInitialValue () {
			Assert.IsNull(trajectory.PreviousPoint);

			trajectory.Update(O);
			Assert.AreSame(O, trajectory.PreviousPoint);
		}

		/// <summary>
		/// Previous point should be advanced on update when point matches.
		/// </summary>
		[Test]
		public void PreviousPointAdvance () {
			trajectory.Update(A);
			Assert.AreSame(A, trajectory.PreviousPoint);

			trajectory.Update(B);
			Assert.AreSame(B, trajectory.PreviousPoint);

			trajectory.Update(C);
			Assert.AreSame(C, trajectory.PreviousPoint);
		}

		/// <summary>
		/// Previous point should not be advanced on update when end of the trajectory is reached.
		/// </summary>
		[Test]
		public void PreviousPointLastPoint () {
			ReachLastPoint();

			trajectory.Update(A);
			Assert.AreSame(C, trajectory.PreviousPoint);

			trajectory.Update(B);
			Assert.AreSame(C, trajectory.PreviousPoint);

			trajectory.Update(C);
			Assert.AreSame(C, trajectory.PreviousPoint);
		}

		/// <summary>
		/// Previous point should not be advanced on update when point does not match.
		/// </summary>
		[Test]
		public void PreviousPointNoAdvance () {
			trajectory.Update(O);

			trajectory.Update(B);
			trajectory.Update(C);
			trajectory.Update(D);
			Assert.AreEqual(O, trajectory.PreviousPoint);
		}

		/// <summary>
		/// Update should return <c>false</c> when point does not match.
		/// </summary>
		[Test]
		public void UpdateFalse () {
			Assert.IsFalse(trajectory.Update(O));
			Assert.IsFalse(trajectory.Update(B));
			Assert.IsFalse(trajectory.Update(C));
			Assert.IsFalse(trajectory.Update(D));
		}

		/// <summary>
		/// Update should return <c>true</c> when point matches.
		/// </summary>
		[Test]
		public void UpdateAdvance () {
			Assert.IsTrue(trajectory.Update(A));
			Assert.IsTrue(trajectory.Update(B));
			Assert.IsTrue(trajectory.Update(C));
		}

		/// <summary>
		/// Update should return <c>false</c> when last point was reached.
		/// </summary>
		[Test]
		public void UpdateLoop () {
			ReachLastPoint();
			Assert.IsFalse(trajectory.Update(C));
			Assert.IsFalse(trajectory.Update(D));
		}

		/// <summary>
		/// Point matcher function should be called with current state, previous and next points as arguments.
		/// </summary>
		[Test]
		public void PointMatcherCalls () {
			Mock<TrackedTrajectory<string>.PointMatcher> pointMatcherMock = Mock.Get(pointMatcher);

			trajectory.Update(O);
			pointMatcherMock.Verify(p => p(O, O, A));

			trajectory.Update(A);
			pointMatcherMock.Verify(p => p(A, O, A));

			trajectory.Update(A);
			pointMatcherMock.Verify(p => p(A, A, B));

			trajectory.Update(B);
			pointMatcherMock.Verify(p => p(B, A, B));

			trajectory.Update(C);
			pointMatcherMock.Verify(p => p(C,B,C));
		}

		/// <summary>
		/// Point matcher function should not be called when the trajectory is completed.
		/// </summary>
		[Test]
		public void PointMatcherNotCalled () {
			Mock<TrackedTrajectory<string>.PointMatcher> pointMatcherMock = Mock.Get(pointMatcher);
			ReachLastPoint();
			pointMatcherMock.Invocations.Clear();

			trajectory.Update(D);
			pointMatcherMock.VerifyNoOtherCalls();
		}

		/// <summary>
		/// Last point should be true when current point is the last point.
		/// </summary>
		[Test]
		public void CompletedTrue () {
			ReachLastPoint();

			Assert.AreEqual(true, trajectory.Completed);
		}

		/// <summary>
		/// Last point should be false when current point is not the last point.
		/// </summary>
		[Test]
		public void CompletedFalse () {
			Assert.AreEqual(false, trajectory.Completed);

			trajectory.Update(A);
			Assert.AreEqual(false, trajectory.Completed);

			trajectory.Update(B);
			Assert.AreEqual(false, trajectory.Completed);
		}

		/// <summary>
		/// Update each point in order so that trajectory last point is reached.
		/// </summary>
		private void ReachLastPoint () {
			trajectory.Update(A);
			trajectory.Update(B);
			trajectory.Update(C);
		}
	}
}