namespace zControl.Core {
	/// <summary>
	/// A generic controller. It computes control values <typeparamref name="U"/> from desired and target states <typeparamref name="S"/> to feed to a <see cref="ISystem{S, I}"/>.
	/// </summary>
	/// <typeparam name="S">type of the controller input (system state)</typeparam>
	/// <typeparam name="U">type of the controller output (system input)</typeparam>
	public interface IController<S, U> {
		/// <summary>
		/// Computes an output to feed the system in order to have the <paramref name="state"/> reach <paramref name="target"/>.
		/// </summary>
		/// <param name="state">the current state of the system</param>
		/// <param name="target">the desired state of the system</param>
		/// <returns>the output to feed the system</returns>
		U Control (S state, S target);
	}
}