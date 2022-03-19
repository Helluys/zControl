namespace zControl.Unity.Clock {
	/// <summary>
	/// Implementation of <see cref="Util.IClock"/> providing Unity's fixed update time.
	/// </summary>
	class UnityFixedClock : Util.IClock {
		/// <inheritdoc/>
		public float Time => UnityEngine.Time.fixedTime;

		/// <inheritdoc/>
		public float DeltaTime => UnityEngine.Time.fixedDeltaTime;
	}
}
