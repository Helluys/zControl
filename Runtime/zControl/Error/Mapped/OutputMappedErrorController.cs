using System;

using zControl.Core;
using zControl.Math;

namespace zControl.Error.Mapped {
	/// <summary>
	/// A <see cref="IErrorController{S, I}"/> that maps the output of another controller.
	/// </summary>
	/// <typeparam name="S">type of the controller input</typeparam>
	/// <typeparam name="UR">type of the returned controller output</typeparam>
	/// <typeparam name="UO">type of the original controller output</typeparam>
	class OutputMappedErrorController<S, UR, UO> : IErrorController<S, UR> where S : IAbelian<S> {
		/// <inheritdoc/>
		public S Error => error();

		/// <summary>
		/// The original controller.
		/// </summary>
		private readonly IController<S, UR> originalController;

		/// <summary>
		/// Function that provides <see cref="Error"/> (needed as contained controller interface does provide it)
		/// </summary>
		private readonly Func<S> error;

		/// <summary>
		/// Contructor.
		/// </summary>
		/// <param name="subcontroller">the original controller</param>
		/// <param name="inputMapper">the mapping function from returned controller input to original controller input</param>
		/// <param name="outputUnmapper">the mapping function from original controller output to returned controller output</param>
		public OutputMappedErrorController (IErrorController<S, UO> subcontroller, Func<S, UO, UR> inputUnmapper) {
			originalController = Core.ZControl.MapController(subcontroller, inputUnmapper);
			error = () => subcontroller.Error;
		}

		/// <inheritdoc/>
		public UR Control (S state, S target) => originalController.Control(state, target);
	}
}