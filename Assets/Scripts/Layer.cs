using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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

		#endregion // PRIVATE_MEMBER_VARIABLES



		#region GETTERS_AND_SETTERS_METHODS

		//! Layer Id
		public int Id {	get { return _id; } }

		//! Initial state in this Layer
		public State InitialState { get; set; }

		/// Enables the Layer for working.
		public bool Enabled {
			get { return _enabled; }
			set { 
				if (!_enabled && value) {
					_currentState = InitialState;
					_currentState.ActivateState ();
				}
				if (_enabled && !value) {
					_currentState.DeactivateState ();
				}
				_enabled = value;
			}
		}

		/// Agent that contains this state.
		public Agent Agent { get; set; }

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


		/** \brief Comparation between Layers.
		 * 
		 * @param layer Layer to compare with.
		 */
		public int CompareTo (Layer compareLayer)
		{
			// A null value means that this object is greater.
			if (compareLayer == null)
				return 1;
			else
				return this._id.CompareTo (compareLayer._id);
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
			// By default the first state is the initial state.
			if (_states.Count == 0) {
				InitialState = state;
			}
			state.Layer = this;
			_states.Add (state);
			return true;
		}


		/** @brief Remove an State from the Layer.
		 * 
		 * @param state State object to be removed.
		 * @return Returns true if the state has been successfully removed.
		 */
		public bool RemoveState (State state)
		{
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


		public void ActiveInterruptingState (State interruptingState)
		{
			if (Enabled) {
				State lastState = _currentState;
				_currentState = interruptingState;

				lastState.DeactivateState ();
				_currentState.ActivateState ();
			}
		}

		public void Mask(){
			_masked++;
			if (_masked == 1) {
				Enabled = false;
			}
		}


		public void Unmask(){
			_masked--;
			if (_masked == 0) {
				Enabled = true;
			}
		}

		//! Updates the State of the Layer
		public void Update ()
		{
			if (Enabled) {
				Transition activedTransition = _currentState.GetActivedTransition ();
				if (activedTransition != null) {
					State lastState = _currentState;
					_currentState = activedTransition.TargetState;
					// Update states
					lastState.DeactivateState ();
					_currentState.ActivateState ();

				}
			}
		}

		#endregion // PUBLIC_METHODS



		#region PRIVATE_METHODS


		#endregion // PRIVATE_METHODS
	}
}