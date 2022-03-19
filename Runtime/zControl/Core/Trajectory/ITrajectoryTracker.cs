namespace zControl.Core.Trajectory {
	/// <summary>
	/// Controls a system to follow a trajectory.
	/// </summary>
	/// <typeparam name="S">type of the controller input (system state)</typeparam>
	/// <typeparam name="U">type of the controller output (system input)</typeparam>
	public interface ITrajectoryTracker<S, U> {
		/// <summary>
		/// Point related event delegate.
		/// </summary>
		/// <param name="point">the point</param>
		public delegate void PointEvent (S point);

		/// <summary>
		/// The tracked trajectory.
		/// </summary>
		public TrackedTrajectory<S> Trajectory { get; }

		/// <summary>
		/// Event triggered when a point of the trajectory has been reached.
		/// </summary>
		event PointEvent OnPointReached;

		/// <summary>
		/// Updates trajectory Computes 
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		U Control (S state);
	}
}