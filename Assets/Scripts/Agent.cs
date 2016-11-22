using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AgentManagerNamespace
{
	//! Class that identifies each agent that takes part in the agent system.
	public class Agent
	{
		#region PUBLIC_MEMBER_VARIABLES



		#endregion // PUBLIC_MEMBER_VARIABLES



		#region PRIVATE_MEMBER_VARIABLES

		// ID name for the agent
		private string _name;
		// Reference to the character GameObject
		private GameObject _character;
		// Reference to the Animator Component
		private Animator _animator;
		// Possible animations in the Agent.
		private HashSet<string> _animationNames = new HashSet<string> ();
		// Possible components in the Agent.
		private HashSet<Component> _components = new HashSet<Component> ();

		// Transiting states of the agent
		private List<State> _transitingStates = new List<State> ();
		// Interrupting states of the agent
		private List<State> _interruptingStates = new List<State> ();

		private bool _enabled;
		private State _currentState;

		#endregion // PRIVATE_MEMBER_VARIABLES



		#region GETTERS_AND_SETTERS_METHODS

		public bool Enabled {
			get { return _enabled; }
			set { 
				if (!_enabled && value) {
					_currentState = _transitingStates [0];
					_currentState.ActivateState ();
				}
				_enabled = value;
			}
		}

		/// The Id name that identifies the Agent.
		public string Name { get { return _name; } }

		//! A reference to the character GameObjects that controls this Agent.
		public GameObject Character { 
			get { return _character; } 
			set { 
				_character = value; 
				// If the character has already an animator, we use it as the agent animator.
				Animator animatorAux = _character.GetComponent<Animator> ();
				if (animatorAux != null) {
					_animator = animatorAux;
				}
			} 
		}

		//! A reference to the Animator component that controls the animations of this Agent.
		public Animator Animator { 
			get { return _animator; } 
			set { _animator = value; } 
		}

		//! Possible animations in this Agent.
		public HashSet<string> AnimationNames {
			get { return _animationNames; }
		}

		//! Possible components in this Agent.
		public HashSet<Component> Components {
			get { return _components; }
		}

		#endregion // GETTERS_AND_SETTERS_METHODS



		#region UNTIY_MONOBEHAVIOUR_METHODS


		#endregion // UNTIY_MONOBEHAVIOUR_METHODS



		#region PUBLIC_METHODS

		/** \brief Creates an Agent object.
		 * 
		 * @param agentName Id name for the agent.
		 */
		public Agent (string agentName)
		{
			_name = agentName;
		}


		/** \brief Equality between Agent and Object.
		 * 
		 * @param obj Object to compare with.
		 */
		public override bool Equals (object obj)
		{
			if (obj == null)
				return false;
			Agent agent = obj as Agent;
			if (agent == null)
				return false;
			else
				return Equals (agent);
		}


		/** \brief Equality between Agents.
		 * 
		 * @param agent Agent to compare with.
		 */
		public bool Equals (Agent agent)
		{
			if (agent == null)
				return false;
			return (this.Name.Equals (agent.Name));
		}


		/** \brief Add an State to this Agent.
		 * 
		 * @param stateName Id name for the state.
		 * @param isInterrupting True if it will be able to break the normal flow of the state machine.
		 * @return A refence to de state created, null if it has not been created.
		 */
		public State AddState (string stateName, bool isInterrupting = false)
		{
			// Check if this state already exists
			if (FindState (stateName) != null) {
				Debug.LogWarningFormat ("The state {0} already exists.", stateName);
				return null;
			}
			
			State newState = new State (stateName);
			// Index the state with this Agent
			newState.Agent = this;
			if (!isInterrupting)
				_transitingStates.Add (newState);
			else
				_interruptingStates.Add (newState);
			return newState;
		}


		/** @brief Add an State to the Agent.
		 * 
		 * @param state State object to be added.
		 * @param isInterrupting True if it will be able to break the normal flow of the state machine.
		 * @return Returns true if the addition has been successfully done.
		 */
		public bool AddState (State state, bool isInterrupting = false)
		{
			// Check if this transition alreary exists.
			if (_interruptingStates.Contains (state) || _transitingStates.Contains (state)) {
				Debug.LogWarningFormat ("The state {0} does already exist.", state.Name);
				return false;
			}
			state.Agent = this;
			if (!isInterrupting)
				_transitingStates.Add (state);
			else
				_interruptingStates.Add (state);
			return true;
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
				Debug.LogWarningFormat ("The state {0} does not exist.", originStateName);
				return null;
			}

			State targetState = FindState (targetStateName);
			if (targetState == null) {
				Debug.LogWarningFormat ("The state {0} does not exist.", targetStateName);
				return null;
			}

			return originState.AddTransition (targetState, trigger, priority);
		}


		/** @brief Find a State by name.
		 * 
		 * @param stateName The name of the state to be found.
		 * @return The State searched or null if it does not exist.
		 */
		public State FindState (string stateName)
		{
			// Search the state in the possible types
			State foundState = _transitingStates.Find (state => state.Name.Equals (stateName));
			if (foundState != null)
				return foundState;

			foundState = _interruptingStates.Find (state => state.Name.Equals (stateName));	
			if (foundState != null)
				return foundState;

			// Null if it is not found
			return null;
		}


		//! Updates the state of the agent
		public void Update ()
		{
			if (Enabled) {
				Transition activedTransition = _currentState.GetActivedTransition ();
				if (activedTransition != null) {
					_currentState = activedTransition.TargetState;
					_currentState.ActivateState ();
				}
			}
		}


		#endregion // PUBLIC_METHODS



		#region PRIVATE_METHODS



		#endregion // PRIVATE_METHODS
	}
}