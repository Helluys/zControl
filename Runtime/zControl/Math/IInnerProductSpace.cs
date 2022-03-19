using zControl.Math.Types;

namespace zControl.Math {
	/// <summary>
	/// A mathematical inner product space over the <see cref="IField{T}"/> of real numbers <see cref="Float"/>, which is a <see cref="IVector{V, Float}"/>
	/// together with an inner product <see cref="Dot(V)"/> (noted '.') that satisfies the following properties:
	/// <list type="bullet">
	///		<item><description>Symmetry: <c>a.b = b.a</c></description></item>
	///		<item><description>Linearity in the first argument: <c>(a*x + b*y).z = a*(x.z) + b*(y.z)</c></description></item>
	///		<item><description>Positive-definiteness: for any x except <see cref="IGroup{T}.Zero"/>: <c>x.x > 0</c></description></item>
	/// </list>
	/// </summary>
	/// <typeparam name="V">the vector type</typeparam>
	public interface IInnerProductSpace<V> : IVector<V, Float> {
		/// <summary>
		/// Inner product operation. It has the following properties:
		/// <list type="bullet">
		///		<item><description>Symmetry: <c>a.b = b.a</c></description></item>
		///		<item><description>Linearity in the first argument: <c>(a*x + b*y).z = a*(x.z) + b*(y.z)</c></description></item>
		///		<item><description>Positive-definiteness: for any x except <see cref="IGroup{T}.Zero"/>: <c>x.x > 0</c></description></item>
		/// </list>
		/// </summary>
		/// <param name="other">the other vector</param>
		/// <returns>the result of the inner product operation</returns>
		Float Dot (V other);

		/// <summary>
		/// Magnitude of the vector. It is defined as:
		/// <code><see cref="Magnitude"/> = Sqrt(this.<see cref="Dot(this)"/>)</code>
		/// </summary>
		Float Magnitude { get; }

		/// <summary>
		/// Sqruare of the magnitude of the vector. It is defined as:
		/// <code><see cref="Magnitude"/> = this.<see cref="Dot(this)"/></code>
		/// </summary>
		Float SqrMagnitude { get; }

		/// <summary>
		/// This vector divided by its <see cref="Magnitude"/>:
		/// <code><see cref="Normalized"/> = this * (1 / this.Magnitude)</code>
		/// </summary>
		V Normalized { get; }
	}
}
