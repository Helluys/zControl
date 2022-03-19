using zControl.Math;

namespace zControl.Error {
	public interface IPIDController<S, U, G> : IErrorController<S, U> where S : IVector<S, G> where U : IVector<U, G> where G : IField<G> {
		/// <summary>
		/// The proportionate gain.
		/// </summary>
		public G KP { get; set; }

		/// <summary>
		/// The integral gain.
		/// </summary>
		public G KI { get; set; }

		/// <summary>
		/// The derivate gain.
		/// </summary>
		public G KD { get; set; }

		/// <summary>
		/// The proportionate error.
		/// </summary>
		public S PError { get; }

		/// <summary>
		/// The integral error.
		/// </summary>
		public S IError { get; }

		/// <summary>
		/// The derivate error.
		/// </summary>
		public S DError { get; }

	}
}