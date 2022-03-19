namespace zControl.Unity.Clock {
	/// <summary>
	/// Implementation of <see cref="Util.IClock"/> providing Unity's fixed update time.
	/// </summary>
	class UnityClock : Util.IClock {
		/// <inheritdoc/>
		public float Time => UnityEngine.Time.time;

		/// <inheritdoc/>
		public float DeltaTime => UnityEngine.Time.deltaTime;
	}
}
