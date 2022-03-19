namespace zControl.Core {
	/// <summary>
	/// A <see cref="IController{S, U}"/> built from a <see cref="ZControl.Control{S, U}"/> function.
	/// </summary>
	/// <typeparam name="S">the type of the system state</typeparam>
	/// <typeparam name="U">the type of the system input</typeparam>
	class Controller<S, U> : IController<S, U> {
		/// <summary>
		/// The control function.
		/// </summary>
		private readonly ZControl.Control<S, U> control;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="control">the control computation function</param>
		public Controller (ZControl.Control<S, U> control) {
			this.control = control;
		}

		/// <inheritdoc/>
		public U Control (S state, S target) {
			return control(state, target);
		}
	}
}
