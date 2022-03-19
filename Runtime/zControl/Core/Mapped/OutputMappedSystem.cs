using System;

namespace zControl.Core.Mapped {
	/// <summary>
	/// A <see cref="ISystem{SR, U}"/> that maps its state from another subsystem.
	/// </summary>
	/// <typeparam name="SR">type of the returned system state</typeparam>
	/// <typeparam name="SO">type of the original system state</typeparam>
	/// <typeparam name="U">type of the system input</typeparam>
	class OutputMappedSystem<SR, U, SO> : ISystem<SR, U> {
		/// <summary>
		/// The original system.
		/// </summary>
		private readonly ISystem<SO, U> originalSystem;

		/// <summary>
		/// The mapping function from original system state to returned system state.
		/// </summary>
		private readonly Func<SO, SR> stateUnmapper;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="stateUnmapper">the mapping function from original system state to returned system state</param>
		/// <returns>a <see cref="ISystem{S, UR}"/> thats wraps the given system</returns>
		public OutputMappedSystem (ISystem<SO, U> subsystem, Func<SO, SR> stateUnmapper) {
			this.originalSystem = subsystem;
			this.stateUnmapper = stateUnmapper;
		}

		/// <inheritdoc/>
		public SR State => stateUnmapper(originalSystem.State);

		/// <inheritdoc/>
		public U Input => originalSystem.Input;

		/// <inheritdoc/>
		public void Update (U input) => originalSystem.Update(input);
	}
}