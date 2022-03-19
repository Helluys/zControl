namespace zControl.Util {
	/// <summary>
	/// A stepped clock.
	/// </summary>
	public interface IClock {
		/// <summary>
		/// The current wall clock time (seconds).
		/// </summary>
		public float Time { get; }

		/// <summary>
		/// The elapsed time since last simulation step.
		/// </summary>
		public float DeltaTime { get; }
	}
}
