using System;

using zControl.Math;
using zControl.Math.Types;
using zControl.Util;

namespace zControl.Error {
	/// <summary>
	/// Factory for <see cref="Error"/> objects.
	/// </summary>
	public static class ZControl {
		/// <summary>
		/// Feedback function that transforms a system state in a system input.
		/// </summary>
		/// <typeparam name="S">type of the system state</typeparam>
		/// <typeparam name="U">type of the system input</typeparam>
		/// <param name="error"></param>
		/// <returns></returns>
		public delegate U Feedback<S, U> (S error);

		/// <summary>
		/// Constructs an <see cref="IErrorController{S, U}"/> from a <see cref="Feedback{S, U}"/> function.
		/// </summary>
		/// <typeparam name="S">type of the system state, must be subtractable with itself</typeparam>
		/// <typeparam name="U">type of the system input</typeparam>
		/// <param name="feedback">the feedback function</param>
		/// <returns>a new error controller</returns>
		public static IErrorController<S, U> ErrorController<S, U> (Feedback<S, U> feedback) where S : IAbelian<S> {
			return new ErrorController<S, U>(feedback);
		}

		/// <summary>
		/// Constructs a <see cref="Error.PIDController{S, G, U}"/> from a <see cref="Feedback{S, U}"/> function.
		/// </summary>
		/// <typeparam name="S">the controller input type (system state)</typeparam>
		/// <typeparam name="U">the controller output type (system input)</typeparam>
		/// <typeparam name="G">the gains type</typeparam>
		/// <param name="feedback">Feedback function to convert <typeparamref name="S"/> into <typeparamref name="U"/></param>
		/// <param name="clock">Clock</param>
		/// <returns>a new PID controller</returns>
		public static IPIDController<S, U, G> PIDController<S, U, G> (Feedback<S, U> feedback, IClock clock)
			where S : IVector<S, G>, IVector<S, Float>
			where U : IVector<U, G>
			where G : IField<G> {
			return new PIDController<S, U, G>(feedback, clock);
		}

		/// <summary>
		/// Maps a <see cref="IErrorController{SO, U}"/> to a <see cref="IErrorController{SR, U}"/> using a mapping bijection.<br />
		/// The returned controller computes its output by providing state and target to the given <paramref name="controller"/>
		/// mapped with <paramref name="stateMapper"/> and returning the result.
		/// </summary>
		/// <typeparam name="SO">type of the original error controller input (system state)</typeparam>
		/// <typeparam name="SR">type of the returned error controller input (system state)</typeparam>
		/// <typeparam name="U">type of the error controller output (system input)</typeparam>
		/// <param name="controller">the original error controller</param>
		/// <param name="outputUnmapper">the mapping function from returned controller state to original controller state</param>
		/// <returns>a <see cref="IController{SR, U}"/></returns>
		public static IErrorController<SR, U> Map<SR, U, SO> (IErrorController<SO, U> controller, IBijection<SR, SO> stateMapper)
			where SR : IAbelian<SR>
			where SO : IAbelian<SO> {
			return new Mapped.InputMappedErrorController<SR, U, SO>(controller, stateMapper);
		}

		/// <summary>
		/// Maps a <see cref="IErrorController{S, UI}"/> to a <see cref="IErrorController{SE, UE}"/> using a mapping function.<br />
		/// The returned controller computes its output by providing state and target to the given <paramref name="controller"/>
		/// and returning the result mapped with <paramref name="inputUnmapper"/>.
		/// </summary>
		/// <typeparam name="S">type of the controller input (system state)</typeparam>
		/// <typeparam name="UE">type of the controller output (system input)</typeparam>
		/// <typeparam name="UI">type of the original controller output (mapped system input)</typeparam>
		/// <param name="controller">the original controller</param>
		/// <param name="outputUnmapper">the mapping function from original controller output to returned controller output</param>
		/// <returns></returns>
		public static IErrorController<S, UR> Map<S, UR, UO> (IErrorController<S, UO> controller, Func<S, UO, UR> inputUnmapper)
			where S : IAbelian<S> {
			return new Mapped.OutputMappedErrorController<S, UR, UO>(controller, inputUnmapper);
		}

		/// <summary>
		/// Wraps a <see cref="IErrorController{SI ,UI}"/> in a <see cref="IErrorController{SE, UE}"/> using mapping functions.<br />
		/// The returned controller computes its output by providing state and target mapped with <paramref name="stateMapper"/>
		/// to the given <paramref name="controller"/> and returning the result mapped with <paramref name="outputUnmapper"/>.
		/// </summary>
		/// <typeparam name="SR">type of the returned controller input (system state)</typeparam>
		/// <typeparam name="UR">type of the returned controller output (system input)</typeparam>
		/// <typeparam name="SO">type of the original controller input (mapped system state)</typeparam>
		/// <typeparam name="UO">type of the original controller output (mapped system input)</typeparam>
		/// <param name="controller">the original controller</param>
		/// <param name="stateMapper">the mapping function from returned controller input to original controller input</param>
		/// <param name="outputUnmapper">the mapping function from original controller output to returned controller output</param>
		/// <returns></returns>
		public static IErrorController<SR, UR> Wrap<SR, UR, SO, UO> (IErrorController<SO, UO> controller, IBijection<SR, SO> stateMapper, Func<SO, UO, UR> outputUnmapper)
			where SR : IAbelian<SR>
			where SO : IAbelian<SO> {
			return Map(Map(controller, outputUnmapper), stateMapper);
		}
	}
}
