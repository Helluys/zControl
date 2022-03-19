namespace zControl.Math {
	/// <summary>
	/// A set with an addition operation <see cref="Plus(T)"/> (noted '+') that produces an element within the set. It verifies the following properties:
	/// <list type="bullet">
	/// <item><description>Associativity of <see cref="Plus(T)"/>: <c>a + (b + c) = (a + b) + c</c></description></item>
	/// <item><description>Identity element: <see cref="IGroup{T}.Zero"/></description></item>
	/// <item><description>Inverse element: <see cref="IGroup{T}.Opposite"/></description></item>
	/// </list>
	/// </summary>
	/// <typeparam name="T">the element type</typeparam>
	public interface IGroup<T> {
		/// <summary>
		/// Returns the identity element 0 for <see cref="Plus(T)"/>.<br />
		/// For any element a of <see cref="T"/>, <c>0 + a = a + 0 = a</c>.
		/// </summary>
		/// <returns></returns>
		T Zero { get; }

		/// <summary>
		/// Returns the opposite element if the current element regarding <see cref="Plus(T)"/>.<br />
		/// For any element a of <see cref="T"/>, <c>Opposite(a) + a = a + Opposite(a) = Opposite()</c>.
		/// </summary>
		/// <returns></returns>
		T Opposite ();

		/// <summary>
		/// Computes the result of applying the operation to this value and the <paramref name="other"/>, as per:
		/// <code>result = this + other</code>
		/// </summary>
		/// <param name="other">the other value</param>
		/// <returns>the result of the addition operation</returns>
		T Plus (T other);

		/// <summary>
		/// Shortcut method for <c>this.Plus(other.Opposite())</c>
		/// </summary>
		/// <param name="other">the other value</param>
		/// <returns>the result of the addition operation</returns>
		T Minus (T other);
	}
}