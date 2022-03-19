namespace zControl.Math {
	/// <summary>
	/// A mathematical field, which is a set with an addition operation <see cref="IGroup{T}.Plus"/> (noted '+') and a multiplication operation <see cref="Multiply(T)"/> (noted '*'), and the following properties:
	/// <list type="bullet">
	/// <item><description>Associativity of <see cref="IGroup{T}.Plus(T)"/> and <see cref="Multiply(T)"/>: <c>a + (b + c) = (a + b) + c, and a * (b * c) = (a * b) * c</c></description></item>
	/// <item><description>Commutativity of <see cref="IGroup{T}.Plus(T)"/> and <see cref="Multiply(T)"/>: <c>a + b = b + a, and a * b = b * a</c></description></item>
	/// <item><description><see cref="IGroup{T}.Plus(T)"/> and <see cref="Multiply(T)"/> identity: there exist two different elements <see cref="IGroup{T}.Zero"/> and <see cref="One"/> such that <c>a + 0 = a</c> and <c>a * 1 = a</c></description></item>
	/// <item><description><see cref="IGroup{T}.Plus(T)"/> inverses: for every a in F, there exists an element in F, denoted −a, called the additive inverse of a, such that a + (−a) = 0</description></item>
	/// <item><description><see cref="Multiply(T)"/> inverses: for every a except <see cref="IGroup{T}.Zero"/> in F, there exists an element in F, denoted by 1/a, called the multiplicative inverse of a, such that a / a = 1</description></item>
	/// <item><description>Distributivity of <see cref="Multiply(T)"/> over <see cref="IGroup{T}.Plus(T)"/>: a * (b + c) = (a * b) + (a * c)</description></item>
	/// </list>
	/// </summary>
	/// <typeparam name="T">the set element type</typeparam>
	public interface IField<T> : IAbelian<T> {
		/// <summary>
		/// Returns the identity element 1 for <see cref="Multiply(T)"/>.<br />
		/// For any element a of <see cref="T"/>, <c>e * a = a * e = a</c>.
		/// </summary>
		/// <returns>the identity element for <see cref="Multiply(T)"/></returns>
		T One { get; }

		/// <summary>
		/// Returns the inverse element of the current element regarding <see cref="Multiply(T)"/> .<br />
		/// For any element a of <see cref="T"/>, <c>Inverse(a) * a = a * Inverse(a) = Identity()</c>.
		/// </summary>
		/// <returns>the inverse element of the current element</returns>
		T Inverse ();

		/// <summary>
		/// Computes the result of applying the multiplication operation to this value and the <paramref name="other"/>, as per:
		/// <code>result = this * other</code>
		/// </summary>
		/// <param name="other">the other value</param>
		/// <returns>the product of this value and the <paramref name="other"/></returns>
		T Multiply (T other);
	}
}
