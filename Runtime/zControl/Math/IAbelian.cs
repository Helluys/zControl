namespace zControl.Math {
	/// <summary>
	/// A commutative <see cref="IGroup{T}"/>. It verifies the following properties:
	/// <list type="bullet">
	/// <item><description>All <see cref="IGroup{T}"/> properties</description></item>
	/// <item><description>Commutativity: <c>a + b = b + a</c></description></item>
	/// </list>
	/// </summary>
	/// <typeparam name="T">the element type</typeparam>
	public interface IAbelian<T> : IGroup<T> {
		// Marker interface
	}
}