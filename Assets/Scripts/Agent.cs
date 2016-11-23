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
		private Dictionary<string, int> _animationNames = new Dictionary<string, int> ();
		// Possible components in the Agent.
		private Dictionary<System.Type, int> _componentTypes = new Dictionary<System.Type, int> ();

		// Layers
		private List<Layer> _layers = new List<Layer> ();

		private bool _enabled = false;
		private bool _firstTimeEnabled = false;

		#endregion // PRIVATE_MEMBER_VARIABLES



		#region GETTERS_AND_SETTERS_METHODS

		/// Enables the Agent for working.
		public bool Enabled {
			get { return _enabled; }
			set { 
				if (!_firstTimeEnabled && value) {
					_firstTimeEnabled = true;
					foreach (Layer layer in _layers) {
						layer.Enabled = true;
					}
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
		public Dictionary<string, int>  AnimationNames {
			get { return _animationNames; }
		}


		//! Possible components in this Agent.
		public Dictionary<System.Type, int>  ComponentTypes {
			get { return _componentTypes; }
		}


		/** @brief Get the Initial state of an agent layer.
		 * 
		 * @param layer Agent layer wanted.
		 */ 
		public State GetInitialState (int layer = 0)
		{
			return _layers [0].InitialState;
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


		/** \brief Creates an Agent object.
		 * 
		 * @param agentName Id name for the agent.
		 * @param character GameObject to be associated to the Agent
		 */
		public Agent (string agentName, GameObject character)
		{
			_name = agentName;
			_character = character;
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
		 * @param layer Adds the state in the state layer passed.
		 * @return A refence to de state created, null if it has not been created.
		 */
		public State AddState (string stateName, int layerId = 0)
		{
			State newState = new State (stateName);
			if (AddState (newState, layerId))
				return newState;
			return null;
		}


		/** @brief Add an State to the Agent.
		 * 
		 * @param state State object to be added.
		 * @return Returns true if the addition has been successfully done.
		 */
		public bool AddState (State state, int layerId = 0)
		{
			Layer layerAux = FindLayer (layerId);
			if (layerAux == null)
				layerAux = AddLayer (layerId);
			
			if (layerAux.AddState (state))
				return true;
			Debug.LogWarningFormat ("The state {0} has not been added to layer {1}.", state.Name, layerId);
			return false;
		}


		/** @brief Add a transition between two states.
		 * 
		 * @param originStateName The name of the State to transitate from.
		 * @param targetStateName The name of the State to transitate to.
		 * @param trigget Trigger that determinates when to transitate.
		 * @param layerId The layer in which the states has to be found.
		 * @param priotity The prority to be actived.
		 * @return A reference to the transition created.
		 */
		public Transition AddTransition (string originStateName, string targetStateName, TransitionTrigger trigger, int layerId = 0, int priority = 0)
		{
			Layer layerAux = FindLayer (layerId);
			if (layerAux == null) {
				Debug.LogWarningFormat ("The transition {0}-{1} has not been added, no exists state {2} in layer {3}.",
					originStateName, targetStateName, originStateName, layerId);
				return null;
			}
			Transition newTransition = _layers [layerId].AddTransition (originStateName, targetStateName, trigger, priority);
			if (newTransition == null) 
				Debug.LogWarningFormat ("The transition {0}-{1} has not been added to the state {2} in layer {3}.",
					originStateName, targetStateName, originStateName, layerId);
			return newTransition;
		}


		/** @brief Find a State in a Layer by name.
		 * 
		 * @param stateName The name of the state to be found.
		 * @param layerId Layer to search in.
		 * @return The State searched or null if it does not exist.
		 */
		public State FindState (string stateName, int layerId)
		{
			Layer layerAux = FindLayer (layerId);
			if (layerAux == null)
				return null;
			return layerAux.FindState (stateName);
		}


		/** @brief Find a State in an Agent by name.
		 * 
		 * @param stateName The name of the state to be found.
		 * @return The States found or null if it does not exist.
		 */
		public List<State> FindState (string stateName)
		{
			List<State> result = new List<State> ();
			foreach (Layer layerI in _layers) {
				State stateFound = layerI.FindState (stateName);
				if (stateFound != null) {
					result.Add (stateFound);
				}
			}
			return result;
		}


		/** @brief Find a Layer in an Agent by Id.
		 * 
		 * @param layerId The id of the Layer to be found.
		 * @return The Layer found or null if it does not exist.
		 */
		public Layer FindLayer (int layerId)
		{
			return _layers.Find (layer => layer.Id == layerId);
		}


		//! Updates the state of the agent
		public void Update ()
		{
			if (Enabled) {
				foreach (Layer layerI in _layers) {
					layerI.Update ();
				}

				// Update state of animations and components
				foreach (var item in _animationNames) {
					_animator.SetBool (item.Key, item.Value > 0);
				}

				foreach (var item in _componentTypes) {
					Component component = _character.GetComponent (item.Key);
					if ((component as MonoBehaviour).enabled != (item.Value > 0)) {
						(component as MonoBehaviour).enabled = (item.Value > 0);
					}
				}
			}
		}


		public bool SetInitialState (string stateName, int layerId = 0) {
			State stateAux = FindState (stateName, layerId);
			if (stateAux == null) {
				Debug.LogWarningFormat ("The state {0} could not be found in layer {1}.", stateName, layerId);
				return false;
			}
			FindLayer (layerId).InitialState = stateAux;
			return true;
		}

		#endregion // PUBLIC_METHODS



		#region PRIVATE_METHODS

		/** @brief Add layer to this Agent
		 * 
		 * @param layerId The id of the new Layer
		 */
		public Layer AddLayer (int layerId)
		{
			// Check if this layer alreary exists.
			if (_layers.Find (layer => layer.Id == layerId) != null) {
				Debug.LogWarningFormat ("The layer {0} does already exist in the agent {1}.", layerId, Name);
				return null;
			}
			Layer newLayer = new Layer (layerId);
			newLayer.Agent = this;
			_layers.Add (newLayer);
			_layers.Sort ();
			return newLayer;
		}


		#endregion // PRIVATE_METHODS
	}
}