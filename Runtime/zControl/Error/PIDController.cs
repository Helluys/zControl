using zControl.Math;
using zControl.Math.Types;
using zControl.Util;

namespace zControl.Error {
	/// <summary>
	/// Control a rigidbody using proportionate, integrate and derivate error feedback.
	/// </summary>
	/// <typeparam name="S">the controller input type (system state)</typeparam>
	/// <typeparam name="U">the controller output type (system input)</typeparam>
	/// <typeparam name="G">the gains type</typeparam>
	class PIDController<S, U, G> : IPIDController<S, U, G> where S : IVector<S, G>, IVector<S, Float> where U : IVector<U, G> where G : IField<G> {
		/// <inheritdoc/>
		public G KP { get; set; }

		/// <inheritdoc/>
		public G KI { get; set; }

		/// <inheritdoc/>
		public G KD { get; set; }

		/// <inheritdoc/>
		public S Error => errorController.Error;

		/// <inheritdoc/>
		public S PError { get; private set; }

		/// <inheritdoc/>
		public S IError { get; private set; }

		/// <inheritdoc/>
		public S DError { get; private set; }

		/// <summary>
		/// The internal error controller.
		/// </summary>
		private readonly IErrorController<S, U> errorController;

		/// <summary>
		/// The clock.
		/// </summary>
		private readonly IClock clock;

		/// <summary>
		/// The previous error for derivate error calculation. Is <code>null</code> before first <see cref="PIDFeedback(ZControl.Feedback{S, U})"/>.
		/// </summary>
		private S previousError;

		/// <summary>
		/// The cumulated error for integrate error calculation. Is <code>null</code> before first <see cref="PIDFeedback(ZControl.Feedback{S, U})"/>.
		/// </summary>
		private S integratedError;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="feedback">Feedback function to convert <typeparamref name="S"/> into <typeparamref name="U"/></param>
		/// <param name="clock">Clock</param>
		public PIDController (ZControl.Feedback<S, U> feedback, IClock clock) {
			errorController = ZControl.ErrorController(PIDFeedback(feedback));
			this.clock = clock;
		}

		/// <inheritdoc/>
		public U Control (S state, S target) {
			return errorController.Control(state, target);
		}

		/// <summary>
		/// Provides a PID <see cref="ZControl.Feedback{S, U}"/> function from a regular <see cref="ZControl.Feedback{S, U}"/> function.
		/// </summary>
		/// <param name="feedback">Feedback function to convert <typeparamref name="S"/> into <typeparamref name="U"/></param>
		/// <returns>A feedback function using error mapping</returns>
		private ZControl.Feedback<S, U> PIDFeedback (ZControl.Feedback<S, U> feedback) {
			return e => {
				S p = ProportionateError(e);
				S i = IntegrateError(e);
				S d = DerivateError(e);

				// Terms addition is performed in system input space, which is more reliable for orientations (vector versus quaternion)
				U pFeedback = feedback(p).Multiply(KP);
				U iFeedback = feedback(i).Multiply(KI);
				U dFeedback = feedback(d).Multiply(KD);
				return pFeedback.Plus(iFeedback).Plus(dFeedback);
			};
		}

		/// <summary>
		/// Proportionate error part.
		/// </summary>
		/// <param name="error">the original error</param>
		/// <returns>the propertionate error</returns>
		private S ProportionateError (S error) {
			PError = error;
			return PError;
		}

		/// <summary>
		/// Intergate error part.
		/// </summary>
		/// <param name="error">the original error</param>
		/// <returns>the integrate error</returns>
		private S IntegrateError (S error) {
			if (integratedError == null) {
				integratedError = error.Zero;
			}

			integratedError = integratedError.Plus(error.Multiply(clock.DeltaTime));
			IError = integratedError;
			return IError;
		}

		/// <summary>
		/// Derivate error part.
		/// </summary>
		/// <param name="error">the original error</param>
		/// <returns>the derivate error, or the original on first call</returns>
		private S DerivateError (S error) {
			if (previousError == null) {
				previousError = error.Zero;
			}

			S derivate = error.Minus(previousError).Multiply(1f / clock.DeltaTime);
			previousError = error;
			DError = derivate;
			return DError;
		}
	}
}
