namespace zControl.Math {
	/// <summary>
	/// A mathematical vector space over a <see cref="IField{T}"/> <typeparamref name="F"/>, which is an <see cref="IAbelian{T}"/> <typeparamref name="V"/>
	/// together with a scalar <see cref="Multiply(F)"/> operation (noted '*') that satisfies the following properties:
	/// <list type="bullet">
	///		<item><description>Compatibility of scalar multiplication with field multiplication: <c>a * (bv) = (ab) * v</c></description></item>
	///		<item><description>Distributivity of scalar multiplication with respect to vector addition: <c>a * (u + v) = a * u + a * v</c></description></item>
	///		<item><description>Distributivity of scalar multiplication with respect to field addition: <c>(a + b) * v = a * v + b * v</c></description></item>
	/// </list>
	/// </summary>
	/// <typeparam name="V">the vector type</typeparam>
	/// <typeparam name="F">the scalar type</typeparam>
	public interface IVector<V, F> : IAbelian<V> where F : IField<F> {
		/// <summary>
		/// Returns the field identity element <see cref="IField{T}.One"/>.
		/// </summary>
		F One { get; }

		/// <summary>
		/// The scalar multiplication operation.
		/// </summary>
		/// <param name="a">the scalar</param>
		/// <returns>the result of the scalara product <code>this * a</code></returns>
		V Multiply (F a);
	}
}
