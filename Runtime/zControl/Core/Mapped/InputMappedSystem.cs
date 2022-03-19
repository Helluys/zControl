using System;

namespace zControl.Core.Mapped {
	/// <summary>
	/// A <see cref="ISystem{S, I}"/> that maps its input to another subsystem.
	/// </summary>
	/// <typeparam name="S">type of the system state</typeparam>
	/// <typeparam name="UO">type of the returned system input</typeparam>
	/// <typeparam name="UR">type of the original system input</typeparam>
	class InputMappedSystem<S, UR, UO> : ISystem<S, UR> {
		/// <summary>
		/// The original system.
		/// </summary>
		private readonly ISystem<S, UO> originalSystem;

		/// <summary>
		/// The mapping function from returned system input to original system input.
		/// </summary>
		private readonly Func<S, UR, UO> inputMapper;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="inputMapper">the mapping function from returned system input to original system input</param>
		/// <returns>a <see cref="ISystem{S, UR}"/> thats wraps the given system</returns>
		public InputMappedSystem (ISystem<S, UO> subsystem, Func<S, UR, UO> inputMapper) {
			this.originalSystem = subsystem;
			this.inputMapper = inputMapper;
		}

		/// <inheritdoc/>
		public S State => originalSystem.State;

		/// <inheritdoc/>
		public UR Input { get; private set; }

		/// <inheritdoc/>
		public void Update (UR input) {
			Input = input;
			originalSystem.Update(inputMapper(State, input));
		}
	}
}