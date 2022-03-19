using System.Collections.Generic;

namespace zControl.Core.Trajectory {
	/// <summary>
	/// A trajectory consisting in a sequence of states.
	/// </summary>
	/// <typeparam name="S">the type of the trajectory Points (system state)</typeparam>
	public class Trajectory<S> {
		/// <summary>
		/// The trajectory points.
		/// </summary>
		public IReadOnlyList<S> Points => points.AsReadOnly();

		/// <summary>
		/// The trajectory points.
		/// </summary>
		private readonly List<S> points;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="points">the state Points that make up the trajectory</param>
		public Trajectory (IEnumerable<S> points) {
			this.points = new List<S>(points);

			if (this.points.Count == 0)
				throw new System.ArgumentException("Cannot create a trajectory with no Points", "Points");

		}
	}
}