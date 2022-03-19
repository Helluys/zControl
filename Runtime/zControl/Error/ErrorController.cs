using zControl.Core;
using zControl.Math;

namespace zControl.Error {
	/// <summary>
	/// A <see cref="IController{S, I}"/> that computes system input based on the tracking error,
	/// that is the difference between the target and the state.
	/// </summary>
	/// <typeparam name="S">type of the system state, must be subtractable with itself</typeparam>
	/// <typeparam name="U">type of the system input</typeparam>
	class ErrorController<S, U> : IErrorController<S, U> where S : IGroup<S> {
		/// <inheritdoc/>
		public S Error { get; private set; }

		/// <summary>
		/// The feedback function.
		/// </summary>
		private readonly ZControl.Feedback<S, U> feedback;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="feedback">function that computes controller output from the tracking error</param>
		public ErrorController (ZControl.Feedback<S, U> feedback) {
			this.feedback = feedback;
		}

		/// <inheritdoc/>
		public U Control (S state, S target) {
			Error = target.Minus(state);
			return feedback(Error);
		}
	}
}