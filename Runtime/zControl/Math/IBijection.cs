namespace zControl.Math {
	/// <summary>
	/// A bijective operation with a single parameter, which is the aggregation of the operation and its inverse.
	/// </summary>
	/// <typeparam name="T">the parameter type</typeparam>
	/// <typeparam name="TResult">the result type</typeparam>
	public interface IBijection<T, TResult> {
		TResult Direct (T x);
		T Inverse (TResult y);
	}

	/// <summary>
	/// A bijective operation with two parameters, which is the aggregation of the operation and its inverse.
	/// </summary>
	/// <typeparam name="T1">the first parameter type</typeparam>
	/// <typeparam name="T2">the second parameter type</typeparam>
	/// <typeparam name="TResult">the result type</typeparam>
	public interface IBijection<T1, T2, TResult> {
		TResult Direct (T1 xx, T2 x2);
		(T1, T2) Inverse (TResult y);
	}
}
