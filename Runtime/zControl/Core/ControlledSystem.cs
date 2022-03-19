namespace zControl.Core {
	/// <summary>
	/// A <see cref="IControlledSystem{S}"/> that feeds a system with input computed from the system state.
	/// </summary>
	/// <typeparam name="S">the type of the system state</typeparam>
	/// <typeparam name="U">the type of the system input</typeparam>
	class ControlledSystem<S, U> : IControlledSystem<S> {
		/// <inheritdoc/>
		public S Target { get; set; }

		/// <inheritdoc/>
		public S State => system.State;

		/// <summary>
		/// The underlying controller.
		/// </summary>
		private readonly IController<S, U> controller;

		/// <summary>
		/// The underlying system.
		/// </summary>
		private readonly ISystem<S, U> system;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="system">the controlled system</param>
		/// <param name="controller">the system controller</param>
		public ControlledSystem (IController<S, U> controller, ISystem<S, U> system) {
			this.controller = controller;
			this.system = system;
		}

		/// <inheritdoc/>
		public void Update () {
			system.Update(controller.Control(State, Target));
		}
	}
}
