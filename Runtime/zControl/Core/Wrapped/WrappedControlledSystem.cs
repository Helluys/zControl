using System;

namespace zControl.Core.Wrapped {
	/// <summary>
	/// A <see cref="IControlledSystem{S}"/> that maps its input and output to and from another controlled system.
	/// </summary>
	/// <typeparam name="SR">type of the returned system state</typeparam>
	/// <typeparam name="SO">type of the original system state</typeparam>
	class WrappedControlledSystem<SR, SO> : IControlledSystem<SR> {
		/// <summary>
		/// The original system.
		/// </summary>
		private readonly IControlledSystem<SO> subsystem;

		/// <summary>
		/// The mapping function from returned system state to original system state.
		/// </summary>
		private readonly Func<SR, SO> mapper;

		/// <summary>
		/// The mapping function from original system state to returned system state.
		/// </summary>
		private readonly Func<SO, SR> unmapper;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="system">the original system</param>
		/// <param name="stateMapper">the mapping function from returned system state to original system state</param>
		/// <param name="stateUnmapper">the mapping function from original system state to returned system state</param>
		public WrappedControlledSystem (IControlledSystem<SO> subsystem, Func<SR, SO> stateMapper, Func<SO, SR> stateUnmapper) {
			this.subsystem = subsystem;
			this.mapper = stateMapper;
			this.unmapper = stateUnmapper;
		}

		/// <inheritdoc/>
		public SR State => unmapper(subsystem.State);

		/// <inheritdoc/>
		public SR Target {
			get { return unmapper(subsystem.Target); }
			set { subsystem.Target = mapper(value); }
		}

		/// <inheritdoc/>
		public void Update () => subsystem.Update();
	}
}