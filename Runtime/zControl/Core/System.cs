namespace zControl.Core {
	/// <summary>
	/// A <see cref="ISystem{S, U}"/> built from a <see cref="ZControl.Simulate{S, U}"/> function and an initial state.
	/// </summary>
	/// <typeparam name="S">the type of the system state</typeparam>
	/// <typeparam name="U">the type of the system input</typeparam>
	class System<S, U> : ISystem<S, U> {
		/// <inheritdoc/>
		public S State { get; private set; }

		/// <inheritdoc/>
		public U Input { get; private set; }

		/// <summary>
		/// The simulation function.
		/// </summary>
		private readonly ZControl.Simulate<S, U> simulate;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="initialState">the sisytem initial state</param>
		/// <param name="simulate">the system simulation function</param>
		public System (S initialState, ZControl.Simulate<S, U> simulate) {
			State = initialState;
			this.simulate = simulate;
		}

		/// <inheritdoc/>
		public void Update (U input) {
			Input = input;
			State = simulate(State,  input);
		}
	}
}
