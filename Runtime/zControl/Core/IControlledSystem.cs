namespace zControl.Core {
	/// <summary>
	/// A generic controlled system. It has an observable <see cref="State"/> that should move towards its 
	/// <see cref="Target"/> on each <see cref="Update"/>.
	/// </summary>
	/// <typeparam name="S">the type of the system state</typeparam>
	public interface IControlledSystem<S> {
		/// <summary>
		/// The target state of the system.
		/// </summary>
		public S Target { get; set; }

		/// <summary>
		/// The current state of the system.
		/// </summary>
		public S State { get; }

		/// <summary>
		/// Feed the internal system with correct input to control it towards <see cref="Target"/>.
		/// </summary>
		public void Update ();
	}
}
