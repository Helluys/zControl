using System;

namespace zControl.Core.Mapped {
	/// <summary>
	/// A <see cref="IController{S, I}"/> that maps its input to another controller.
	/// </summary>
	/// <typeparam name="SO">type of the original controller input (system state)</typeparam>
	/// <typeparam name="SR">type of the returned controller input (system state)</typeparam>
	/// <typeparam name="U">type of the controller output (system input)</typeparam>
	class InputMappedController<SR, U, SO> : IController<SR, U> {
		/// <summary>
		/// The original controller.
		/// </summary>
		private readonly IController<SO, U> originalController;

		/// <summary>
		/// The mapping function from returned system state to original system state.
		/// </summary>
		private readonly Func<SR, SO> stateMapper;

		/// <summary>
		/// Contructor.
		/// </summary>
		/// <param name="controller">the original controller</param>
		/// <param name="outputUnmapper">the mapping function from returned system state to original system state</param>
		public InputMappedController (IController<SO, U> subcontroller, Func<SR, SO> stateMapper) {
			this.originalController = subcontroller;
			this.stateMapper = stateMapper;
		}

		/// <inheritdoc/>
		public U Control (SR state, SR target) {
			SO mappedState = stateMapper(state);
			SO mappedTarget = stateMapper(target);
			return originalController.Control(mappedState, mappedTarget);
		}
	}
}