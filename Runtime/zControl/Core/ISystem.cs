namespace zControl.Core {
	/// <summary>
	/// A generic controllable system. It has an observable <see cref="State"/> <typeparamref name="S"/> and reacts to some input <typeparamref name="U"/>.
	/// </summary>
	/// <typeparam name="S">type of the system state</typeparam>
	/// <typeparam name="U">type of the system input</typeparam>
	public interface ISystem<S, U> {
		/// <summary>
		/// The internal state of the system.
		/// </summary>
		S State { get; }

		/// <summary>
		/// The last input applied to the system.
		/// </summary>
		U Input { get; }

		/// <summary>
		/// Update the system feeding it the given input
		/// </summary>
		/// <param name="input">the system input</param>
		void Update (U input);
	}
}
