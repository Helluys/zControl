using System;

namespace zControl.Core {
	/// <summary>
	/// Factory for <see cref="Core"/> objects.
	/// </summary>
	public static class ZControl {
		/// <summary>
		/// Function representation of a <see cref="IController{S, U}"/>.
		/// </summary>
		/// <typeparam name="S">type of the controller input (system state)</typeparam>
		/// <typeparam name="U">type of the controller output (system input)</typeparam>
		/// <param name="state">the current state of the system</param>
		/// <param name="target">the desired state of the system</param>
		/// <returns>the output to feed the system</returns>
		public delegate U Control<S, U> (S state, S target);

		/// <summary>
		/// Function representation of a <see cref="ISystem{S, U}"/>.
		/// </summary>
		/// <typeparam name="S">type of the system state</typeparam>
		/// <typeparam name="U">type of the system input</typeparam>
		/// <param name="previousState">the previous state of the system</param>
		/// <param name="input">the input of the system</param>
		/// <returns>the new state of the system</returns>
		public delegate S Simulate<S, U> (S previousState, U input);

		/// <summary>
		/// Construct a <see cref="IController{S, U}"/> from a <see cref="Control{S, U}"/> function.
		/// </summary>
		/// <typeparam name="S">type of the controller input (system state)</typeparam>
		/// <typeparam name="U">type of the controller output (system input)</typeparam>
		/// <param name="control">the control function</param>
		/// <returns>a new controller</returns>
		public static IController<S, U> Controller<S, U> (Control<S, U> control) {
			return new Controller<S, U>(control);
		}

		/// <summary>
		/// Constructs a <see cref="ISystem{S, U}"/> from a <see cref="Simulate{S, U}"/> function.
		/// </summary>
		/// <typeparam name="S">type of the system state</typeparam>
		/// <typeparam name="U">type of the system input</typeparam>
		/// <param name="initialState">the initial state of the system</param>
		/// <param name="simulate">the simulation function</param>
		/// <returns>a new system</returns>
		public static ISystem<S, U> System<S, U> (S initialState, Simulate<S, U> simulate) {
			return new System<S, U>(initialState, simulate);
		}

		/// <summary>
		/// Constructs a <see cref="IControlledSystem{S}"/> from a <see cref="IController{S, U}"/> and a <see cref="ISystem{S, U}"/>.
		/// </summary>
		/// <typeparam name="S">type of the system state</typeparam>
		/// <typeparam name="U">type of the system input</typeparam>
		/// <param name="system">the controlled system</param>
		/// <param name="controller">the system controller</param>
		/// <returns>a new controlled system</returns>
		public static IControlledSystem<S> ControlledSystem<S, U> (IController<S, U> controller, ISystem<S, U> system) {
			return new ControlledSystem<S, U>(controller, system);
		}

		/// <summary>
		/// Maps a <see cref="IController{SO, U}"/> to a <see cref="IController{SR, U}"/> using a mapping function.<br />
		/// The returned controller computes its output by providing state and target to the given <paramref name="controller"/>
		/// mapped with <paramref name="stateMapper"/> and returning the result.
		/// </summary>
		/// <typeparam name="SO">type of the original controller input (system state)</typeparam>
		/// <typeparam name="SR">type of the returned controller input (system state)</typeparam>
		/// <typeparam name="U">type of the controller output (system input)</typeparam>
		/// <param name="controller">the original controller</param>
		/// <param name="outputUnmapper">the mapping function from returned controller state to original controller state</param>
		/// <returns>a <see cref="IController{SR, U}"/></returns>
		public static IController<SR, U> MapController<SR, U, SO> (IController<SO, U> controller, Func<SR, SO> stateMapper) {
			return new Mapped.InputMappedController<SR, U, SO>(controller, stateMapper);
		}

		/// <summary>
		/// Maps a <see cref="ISystem{S, UO}"/> to a <see cref="ISystem{S, UR}"/> using a mapping function.<br />
		/// The returned system applies its input mapped with <paramref name="inputMapper"/> to the given <paramref name="system"/>,
		/// and provides a <see cref="IControlledSystem{S}.State"/> that is the <see cref="IControlledSystem{S}.State"/> of the given
		/// <paramref name="system"/>.
		/// </summary>
		/// <typeparam name="S">type of the system state</typeparam>
		/// <typeparam name="UO">type of the original system input</typeparam>
		/// <typeparam name="UR">type of the returned system input</typeparam>
		/// <param name="system">the original system</param>
		/// <param name="inputMapper">the mapping function from returned system input to original system input</param>
		/// <returns>a <see cref="ISystem{SB, IB}"/> thats wraps the given system</returns>
		public static ISystem<S, UR> MapSystem<S, UR, UO> (ISystem<S, UO> system, Func<S, UR, UO> inputMapper) {
			return new Mapped.InputMappedSystem<S, UR, UO>(system, inputMapper);
		}

		/// <summary>
		/// Maps a <see cref="IController{S, UI}"/> to a <see cref="IController{SE, UE}"/> using a mapping function.<br />
		/// The returned controller computes its output by providing state and target to the given <paramref name="controller"/>
		/// and returning the result mapped with <paramref name="inputUnmapper"/>.
		/// </summary>
		/// <typeparam name="S">type of the controller input (system state)</typeparam>
		/// <typeparam name="UR">type of the returned controller output (system input)</typeparam>
		/// <typeparam name="UO">type of the original controller output (system input)</typeparam>
		/// <param name="controller">the original controller</param>
		/// <param name="inputUnmapper">the mapping function from original controller output to returned controller output</param>
		/// <returns></returns>
		public static IController<S, UR> MapController<S, UR, UO> (IController<S, UO> controller, Func<S, UO, UR> inputUnmapper) {
			return new Mapped.OutputMappedController<S, UR, UO>(controller, inputUnmapper);
		}

		/// <summary>
		/// Maps a <see cref="ISystem{SO, U}"/> to a <see cref="ISystem{SR, U}"/> using a mapping function.<br />
		/// The returned system applies its input to the given <paramref name="system"/>,
		/// and provides a <see cref="IControlledSystem{SR}.State"/> that is the <see cref="IControlledSystem{SO}.State"/> of the given
		/// <paramref name="system"/> mapped with <paramref name="stateUnmapper"/>.
		/// </summary>
		/// <typeparam name="S">type of the system state</typeparam>
		/// <typeparam name="UI">type of the original system input</typeparam>
		/// <typeparam name="UE">type of the returned system input</typeparam>
		/// <param name="system">the original system</param>
		/// <param name="stateUnmapper">the mapping function from original system state to returned system state</param>
		/// <returns>a <see cref="ISystem{SR, U}"/></returns>
		public static ISystem<SR, U> MapSystem<SR, U, SO> (ISystem<SO, U> system, Func<SO, SR> stateUnmapper) {
			return new Mapped.OutputMappedSystem<SR, U, SO>(system, stateUnmapper);
		}

		/// <summary>
		/// Wraps a <see cref="IControlledSystem{SO}"/> in a <see cref="IControlledSystem{SR}"/> using mapping functions.<br />
		/// The returned system tracks its <see cref="IControlledSystem{S}.Target"/> mapped with <paramref name="stateMapper"/>
		/// to the given <paramref name="system"/>, and provides a <see cref="IControlledSystem{S}.State"/> that is the
		/// <see cref="IControlledSystem{S}.State"/> of the given <paramref name="system"/> mapped with <paramref name="stateUnmapper"/>.
		/// Given mapping functions <b>must</b> be exact inverse of each other.
		/// </summary>
		/// <typeparam name="SI">type of the original system state</typeparam>
		/// <typeparam name="SE">type of the external system state</typeparam>
		/// <param name="system">the original system</param>
		/// <param name="stateMapper">the mapping function from returned system state to original system state</param>
		/// <param name="stateUnmapper">the mapping function from original system state to returned system state</param>
		/// <returns></returns>
		public static IControlledSystem<SR> WrapControlledSystem<SR, SO> (IControlledSystem<SO> system, Func<SR, SO> stateMapper, Func<SO, SR> stateUnmapper) {
			return new Wrapped.WrappedControlledSystem<SR, SO>(system, stateMapper, stateUnmapper);
		}

		/// <summary>
		/// Wraps a <see cref="IController{SO, UO}"/> in a <see cref="IController{SR, UR}"/> using mapping functions.<br />
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
		public static IController<SR, UR> WrapController<SR, UR, SO, UO> (IController<SO, UO> controller, Func<SR, SO> stateMapper, Func<SO, UO, UR> outputUnmapper) {
			return MapController(MapController(controller, outputUnmapper), stateMapper);
		}

		/// <summary>
		/// Wraps a <see cref="ISystem{SO, IO}"/> in a <see cref="ISystem{SR, IR}"/> using mapping functions.<br />
		/// The returned system applies its input mapped with <paramref name="inputMapper"/> to the given <paramref name="system"/>,
		/// and provides a <see cref="IControlledSystem{S}.State"/> that is the <see cref="IControlledSystem{S}.State"/> of the given
		/// <paramref name="system"/> mapped with <paramref name="stateUnmapper"/>.
		/// </summary>
		/// <typeparam name="SO">type of the original system state</typeparam>
		/// <typeparam name="SO">type of the original system input</typeparam>
		/// <typeparam name="SR">type of the returned system state</typeparam>
		/// <typeparam name="UR">type of the returned system input</typeparam>
		/// <param name="system">the original system</param>
		/// <param name="inputMapper">the mapping function from returned system input to original system input</param>
		/// <param name="stateUnmapper">the mapping function from original system state to returned system state</param>
		/// <returns>a <see cref="ISystem{SB, IB}"/> thats wraps the given system</returns>
		public static ISystem<SR, UR> WrapSystem<SR, UR, SO, UO> (ISystem<SO, UO> system, Func<SR, UR, UO> inputMapper, Func<SO, SR> stateUnmapper) {
			return MapSystem(MapSystem(system, stateUnmapper), inputMapper);
		}
	}
}
