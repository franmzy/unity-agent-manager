using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace AgentManagerNamespace
{
	public class Layer: IComparable<Layer>
	{
		#region PUBLIC_MEMBER_VARIABLES


		#endregion // PUBLIC_MEMBER_VARIABLES



		#region PRIVATE_MEMBER_VARIABLES

		// Layer Id
		private int _id;

		private List<State> _states = new List<State> ();
		private State _currentState;

		private int _masked;

		private bool _enabled = false;

		private State _initialState;

		#endregion // PRIVATE_MEMBER_VARIABLES



		#region GETTERS_AND_SETTERS_METHODS

		//! Layer Id
		public int Id {	get { return _id; } }

		//! Initial state in this Layer
		public State InitialState { 
			get { return _initialState; } 
			set {
				_initialState = value;
			}
		}

		/// Enables the Layer for working.
		public bool Enabled {
			get { return _enabled; }
			set { 
				if (!_enabled && value) {
					CurrentState = InitialState;
				}
				if (_enabled && !value) {
					CurrentState = null;
				}
				_enabled = value;
			}
		}

		/// Agent that contains this state.
		public Agent Agent { get; set; }

		/// Current state
		public State CurrentState {
			get { return _currentState; }
			set {
				if (_currentState != value) {
					if (_currentState != null)
						_currentState.DeactivateState ();
					_currentState = value;
					if (_currentState != null)
						_currentState.ActivateState ();
				}
			}
		}

		#endregion // GETTERS_AND_SETTERS_METHODS



		#region UNTIY_MONOBEHAVIOUR_METHODS


		#endregion // UNTIY_MONOBEHAVIOUR_METHODS



		#region PUBLIC_METHODS


		/** \brief Creates a Layer object.
		 * 
		 * @param layerId Id of the Layer.
		 */
		public Layer (int layerId)
		{
			_id = layerId;
		}


		/** \brief Equality between Layer and Object.
		* 
		* @param obj Object to compare with.
		*/
		public override bool Equals (object obj)
		{
			if (obj == null)
				return false;
			Layer layer = obj as Layer;
			if (layer == null)
				return false;
			else
				return Equals (layer);
		}


		/** \brief Equality between Layers.
		 * 
		 * @param layer Layer to compare with.
		 */
		public bool Equals (Layer layer)
		{
			if (layer == null)
				return false;
			return (this.Id.Equals (layer.Id));
		}


		/** \brief Comparation between Layers, sorted in decreassing order.
		 * 
		 * @param layer Layer to compare with.
		 */
		public int CompareTo (Layer compareLayer)
		{
			// A null value means that this object is greater.
			if (compareLayer == null)
				return 1;
			else
				return compareLayer._id.CompareTo (this._id);
		}


		/** @brief Add an State to the Layer.
		 * 
		 * @param state State object to be added.
		 * @return Returns true if the addition has been successfully done.
		 */
		public bool AddState (State state)
		{
			// Check if this state alreary exists.
			if (_states.Contains (state)) {
				Debug.LogWarningFormat ("The state {0} does already exist in the layer {1}.", state.Name, _id);
				return false;
			}

			state.Layer = this;
			_states.Add (state);

			// By default the first state is the initial state.
			if (_states.Count == 1) {
				InitialState = state;
			}

			return true;
		}


		/** @brief Remove an State from the Layer.
		 * 
		 * @param state State object to be removed.
		 * @return Returns true if the state has been successfully removed.
		 */
		public bool RemoveState (State state)
		{
			if (state == InitialState) {
				if (_states.Count > 0) {
					InitialState = _states [0];
				}
				else {
					InitialState = null;
				}
			}
			return _states.Remove (state);
		}


		/** @brief Find an State by name
		 * 
		 * @param stateName Name of the State to be found
		 * @return The Agent found of null if this Layer has not the agent searched
		 */
		public State FindState (string stateName)
		{
			return _states.Find (state => state.Name.Equals (stateName));
		}


		/** @brief Check if this Layer contains an State by name
		 * 
		 * @param stateName Name of the State to be found.
		 * @return True if this Layer contains this State.
		 */
		public bool ContainsState (string stateName)
		{
			return FindState (stateName) != null;
		}


		/** @brief Add a transition between two states.
		 * 
		 * @param originStateName The name of the State to transitate from.
		 * @param targetStateName The name of the State to transitate to.
		 * @param trigget Trigger that determinates when to transitate.
		 * @param priotity The prority to be actived.
		 * @return A reference to the transition created.
		 */
		public Transition AddTransition (string originStateName, string targetStateName, TransitionTrigger trigger, int priority = 0)
		{
			// Check if the states exist.
			State originState = FindState (originStateName);
			if (originState == null) {
				Debug.LogWarningFormat ("The state {0} does not exist in layer.", originStateName, _id);
				return null;
			}

			State targetState = FindState (targetStateName);
			if (targetState == null) {
				Debug.LogWarningFormat ("The state {0} does not exist in layer.", targetStateName, _id);
				return null;
			}

			return originState.AddTransition (targetState, trigger, priority);
		}


		public void ActiveInterruptingState (State interruptingState, object value, Action UpdateContex)
		{
			if (Enabled) {
				CurrentState = interruptingState;

				// ActionActivate before OnActionInterrupting
				UpdateContex ();

				// Calling onInterruptingAction
				if (value != null) {
					foreach (System.Type componentType in interruptingState.ComponentTypes) {
						Component component = Agent.Character.GetComponent (componentType);
						MethodInfo method = component.GetType ().GetMethod ("OnActionInterrupting", new Type[] { typeof(object) });
						if (method != null) {
							object result = method.Invoke (component, new object[] { value });
						}
					}
				}
				else {
					foreach (System.Type componentType in interruptingState.ComponentTypes) {
						Component component = Agent.Character.GetComponent (componentType);
						MethodInfo method = component.GetType ().GetMethod ("OnActionInterrupting");
						if (method != null) {
							object result = method.Invoke (component, new object[0]);
						}
					}
				}
			}
		}


		public void SendStandarMessage (State state, object value)
		{
			// Calling onInterruptingAction
			if (value != null) {
				foreach (System.Type componentType in state.ComponentTypes) {
					Component component = Agent.Character.GetComponent (componentType);
					MethodInfo method = component.GetType ().GetMethod ("OnReceiveMessage", new Type[] { typeof(object) });
					if (method != null) {
						object result = method.Invoke (component, new object[] { value });
					}
				}
			}
			else {
				foreach (System.Type componentType in state.ComponentTypes) {
					Component component = Agent.Character.GetComponent (componentType);
					MethodInfo method = component.GetType ().GetMethod ("OnReceiveMessage");
					if (method != null) {
						object result = method.Invoke (component, new object[0]);
					}
				}
			}
		}

		public void Mask ()
		{
			if (_masked == 0) {
                foreach (System.Type componentType in CurrentState.ComponentTypes) {
					Component component = Agent.Character.GetComponent (componentType);
					MethodInfo method = component.GetType ().GetMethod ("OnActionMasked");
					if (method != null) {
						object result = method.Invoke (component, new object[0]);
					}
				}
                Enabled = false;
            }
            _masked++;
		}


		public void Unmask ()
		{
			if (_masked == 1) {
                Enabled = true;
                foreach (System.Type componentType in CurrentState.ComponentTypes) {
					Component component = Agent.Character.GetComponent (componentType);
					MethodInfo method = component.GetType ().GetMethod ("OnActionUnmasked");
					if (method != null) {
						object result = method.Invoke (component, new object[0]);
					}
				}
			}
			_masked--;
		}

		//! Updates the State of the Layer
		public void Update ()
		{
			if (Enabled) {
				Transition activedTransition = CurrentState.GetActivedTransition ();
				if (activedTransition != null) {
					CurrentState = activedTransition.TargetState;
				}
			}
		}

		#endregion // PUBLIC_METHODS



		#region PRIVATE_METHODS


		#endregion // PRIVATE_METHODS
	}
}