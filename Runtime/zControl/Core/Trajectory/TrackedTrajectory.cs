using System.Collections.Generic;

namespace zControl.Core.Trajectory {
	/// <summary>
	/// A <see cref="Trajectory"/> being tracked using a <see cref="PointMatcher"/>.
	/// </summary>
	/// <typeparam name="S">the type of the trajectory Points (system state)</typeparam>
	public class TrackedTrajectory<S> : Trajectory<S> {
		/// <summary>
		/// Should return true if the current state has reached the next point. The previous trajectory point is also provided.
		/// The point that precedes the first point of the trajectory is the first state given to <see cref="Update(S)"/>.
		/// </summary>
		/// <param name="currentState">the current state</param>
		/// <param name="previousPoint">the previous trajectory point</param>
		/// <param name="nextPoint">the next trajectory point</param>
		/// <returns></returns>
		public delegate bool PointMatcher (S currentState, S previousPoint, S nextPoint);

		/// <summary>
		/// The previous trajectory point.<br />
		/// The point that precedes the first point of the trajectory is the first state given to <see cref="Update(S)"/>, or <c>null</c> prior to any call.
		/// </summary>
		public S PreviousPoint { get; private set; }

		/// <summary>
		/// The index of the next trajctory point in the sequence.<br/>
		/// If the trajectory is <see cref="Completed"/>, the index of the last trajectory point is returned.
		/// </summary>
		public int NextPointIndex { get; private set; }

		/// <summary>
		/// The next trajectory point.<br/>
		/// If the trajectory is <see cref="Completed"/>, the last trajectory point is returned.
		/// </summary>
		public S NextPoint => Completed ? Points[Points.Count - 1] : Points[NextPointIndex];

		/// <summary>
		/// <c>true</c> when the last point of the trajectory have been reached at least once.<br />
		/// Can never be <c>true</c> for a <see cref="Loop"/>.
		/// </summary>
		public bool Completed { get; private set; }

		/// <summary>
		/// If the trajectory is a loop, the <see cref="NextPoint"/> goes back to the first of the trajectory instead of becoming <see cref="Completed"/>.
		/// </summary>
		public bool Loop { get; private set; }

		/// <summary>
		/// Function that determine if the next point was reached or not.
		/// </summary>
		private readonly PointMatcher pointMatcher;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="points">the state Points that make up the trajectory</param>
		/// <param name="pointMatcher">the function that computes whether the next point is reached</param>
		/// <param name="loop">whether the trajectory is a loop</param>
		/// <exception cref="System.ArgumentException">thrown if <paramref name="points"/> is empty</exception>
		public TrackedTrajectory (IEnumerable<S> points, PointMatcher pointMatcher, bool loop = false) : base(points) {
			this.pointMatcher = pointMatcher;
			NextPointIndex = 0;
			Loop = loop;
			Completed = false;
		}

		/// <summary>
		/// Update the <see cref="PreviousPoint"/> and <see cref="NextPoint"/> of the trajectory using the <see cref="PointMatcher"/> function.<br />
		/// The current point is advanced to the next one if all of the following are true:
		/// <list type="bullet">
		/// <item><description>the trajectory is not <see cref="Completed"/></description></item>
		/// <item><description>the <c>pointMatcher</c> function returns <c>true</c> with the current state and point and the current point as arguments</description></item>
		/// </list>
		/// The <see cref="PointMatcher"/> is not executed if the trajectory <see cref="Completed"/>.
		/// </summary>
		/// <param name="currentState">the current state</param>
		/// <returns><c>true</c> if the current point advanced to the next</returns>
		public bool Update (S currentState) {
			if (PreviousPoint == null) {
				// Initialise previous point to inital state on first invocation
				PreviousPoint = currentState;
			}

			if (Completed) {
				return false;
			} else if (pointMatcher(currentState, PreviousPoint, NextPoint)) {
				// Point matched, update completion status, previous and next points
				Completed = !Loop && (NextPointIndex == Points.Count - 1);
				PreviousPoint = NextPoint;
				NextPointIndex = Loop ? (NextPointIndex + 1) % Points.Count : System.Math.Min(NextPointIndex + 1, Points.Count - 1);
				return true;
			} else {
				return false;
			}
		}
	}
}
