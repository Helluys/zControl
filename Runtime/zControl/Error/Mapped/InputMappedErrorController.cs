using System;

using zControl.Core;
using zControl.Math;

namespace zControl.Error.Mapped {
	/// <summary>
	/// A <see cref="IErrorController{S, I}"/> that maps its input to another error controller.
	/// </summary>
	/// <typeparam name="SO">type of the original error controller input (system state)</typeparam>
	/// <typeparam name="SR">type of the returned error controller input (system state)</typeparam>
	/// <typeparam name="U">type of the error controller output (system input)</typeparam>
	class InputMappedErrorController<SR, U, SO> : IErrorController<SR, U> where SR : IAbelian<SR> where SO : IAbelian<SO> {
		/// <inheritdoc/>
		public SR Error => error();

		/// <summary>
		/// The original controller.
		/// </summary>
		private readonly IController<SR, U> originalController;

		/// <summary>
		/// Function that provides <see cref="Error"/> (needed as contained controller interface does provide it)
		/// </summary>
		private readonly Func<SR> error;

		/// <summary>
		/// Contructor.
		/// </summary>
		/// <param name="controller">the original controller</param>
		/// <param name="outputUnmapper">the mapping function from original controller output to returned controller output</param>
		public InputMappedErrorController (IErrorController<SO, U> subcontroller, IBijection<SR, SO> stateMapper) {
			originalController = Core.ZControl.MapController<SR, U, SO>(subcontroller, stateMapper.Direct);
			error = () => stateMapper.Inverse(subcontroller.Error);
		}

		/// <inheritdoc/>
		public U Control (SR state, SR target) => originalController.Control(state, target);
	}
}