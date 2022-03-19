using System;

namespace zControl.Core.Mapped {
	/// <summary>
	/// A <see cref="IController{S, I}"/> that maps the output of another controller.
	/// </summary>
	/// <typeparam name="S">type of the controller input</typeparam>
	/// <typeparam name="UR">type of the returned controller output</typeparam>
	/// <typeparam name="UO">type of the original controller output</typeparam>
	class OutputMappedController<S, UR, UO> : IController<S, UR> {
		/// <summary>
		/// The original controller.
		/// </summary>
		private readonly IController<S, UO> originalController;

		/// <summary>
		/// The mapping function from original controller output to returned controller output.
		/// </summary>
		private readonly Func<S, UO, UR> inputUnmapper;

		/// <summary>
		/// Contructor.
		/// </summary>
		/// <param name="subcontroller">the original controller</param>
		/// <param name="inputMapper">the mapping function from returned controller input to original controller input</param>
		/// <param name="outputUnmapper">the mapping function from original controller output to returned controller output</param>
		public OutputMappedController (IController<S, UO> subcontroller, Func<S, UO, UR> inputUnmapper) {
			this.originalController = subcontroller;
			this.inputUnmapper = inputUnmapper;
		}

		/// <inheritdoc/>
		public UR Control (S state, S target) {
			UO sysInputInt = originalController.Control(state, target);
			return inputUnmapper(state, sysInputInt);
		}
	}
}