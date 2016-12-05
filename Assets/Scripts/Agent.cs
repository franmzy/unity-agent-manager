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
		// Last value of possible animations in the Agent.
		private Dictionary<string, int> _lastAnimationNames = new Dictionary<string, int> ();
		// Possible components in the Agent.
		private Dictionary<System.Type, int> _componentTypes = new Dictionary<System.Type, int> ();
		// Last value of possible component types in the Agent.
		private Dictionary<System.Type, int> _lastComponentTypes = new Dictionary<System.Type, int> ();

		// Layers
		private List<Layer> _layers = new List<Layer> ();

		private bool _enabled;

		#endregion // PRIVATE_MEMBER_VARIABLES



		#region GETTERS_AND_SETTERS_METHODS

		/// Enables the Agent for working.
		public bool Enabled {
			get { return _enabled; }
			set { 
				if (!_enabled && value) {
					foreach (var layer in _layers) {
						layer.Enabled = true;
					}
				}
				if (_enabled && !value) {
					foreach (var layer in _layers) {
						layer.Enabled = false;
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
			private set { 
				_character = value; 
				// If the character has already an animator, we use it as the agent animator.
				Animator animatorAux = _character.GetComponent<Animator> ();
				if (animatorAux == null) {
					animatorAux = _character.AddComponent<Animator> ();
				}
				_animator = animatorAux;
			} 
		}


		//! A reference to the Animator component that controls the animations of this Agent.
		private Animator Animator { 
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
			Character = character;
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
		 * @param layerId Adds the state in the state layer passed.
		 * @param bitmask Hide lower layers when it bit is 0
		 * @return A refence to de state created, null if it has not been created.
		 */
		public bool AddState (string stateName, int layerId = 0, int bitmask = int.MaxValue)
		{
			State newState = new State (stateName, bitmask);
			if (AddState (newState, layerId))
				return true;
			return false;
		}

		/** \brief Add an State to this Agent.
		 * 
		 * @param stateName Id name for the state.
		 * @param layerId Adds the state in the state layer passed.
		 * @param componentType The type component associated with this state
		 * @param animationName The animation name associated with this state
		 * @param bitmask Hide lower layers when it bit is 0
		 * @return A refence to de state created, null if it has not been created.
		 */
		public bool AddState (string stateName, System.Type componentType, string animationName, int layerId = 0, int bitmask = int.MaxValue)
		{
			State newState = new State (stateName, bitmask);
			if (AddState (newState, layerId)) {
				if (newState.AddComponent (componentType) == null) {
					RemoveState (newState);
					return false;
				}
				if (!newState.AddAnimation (animationName)) {
					RemoveState (newState);
					return false;
				}
				return true;
			}
			return false;
		}

		/** \brief Add an State to this Agent.
		 * 
		 * @param stateName Id name for the state.
		 * @param layerId Adds the state in the state layer passed.
		 * @param componentType The type component associated with this state
		 * @param bitmask Hide lower layers when it bit is 0
		 * @return A refence to de state created, null if it has not been created.
		 */
		public bool AddState (string stateName, System.Type componentType, int layerId = 0, int bitmask = int.MaxValue)
		{
			State newState = new State (stateName, bitmask);
			if (AddState (newState, layerId)) {
				if (newState.AddComponent (componentType) == null) {
					RemoveState (newState);
					return false;
				}
				return true;
			}
			return false;
		}

		/** \brief Add an State to this Agent.
		 * 
		 * @param stateName Id name for the state.
		 * @param layerId Adds the state in the state layer passed.
		 * @param animationName The animation name associated with this state
		 * @param bitmask Hide lower layers when it bit is 0
		 * @return A refence to de state created, null if it has not been created.
		 */
		public bool AddState (string stateName, string animationName, int layerId = 0, int bitmask = int.MaxValue)
		{
			State newState = new State (stateName, bitmask);
			if (AddState (newState, layerId)) {
				if (!newState.AddAnimation (animationName)) {
					RemoveState (newState);
					return false;
				}
				return true;
			}
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
		public bool AddTransition (string originStateName, string targetStateName, TransitionTrigger trigger, int priority = 0, int layerId = 0)
		{
			Layer layerAux = FindLayer (layerId);
			if (layerAux == null) {
				Debug.LogWarningFormat ("The transition {0}-{1} has not been added, no exists state {2} in layer {3}.",
					originStateName, targetStateName, originStateName, layerId);
				return false;
			}
			Transition newTransition = layerAux.AddTransition (originStateName, targetStateName, trigger, priority);
			if (newTransition == null)
				Debug.LogWarningFormat ("The transition {0}-{1} has not been added to the state {2} in layer {3}.",
					originStateName, targetStateName, originStateName, layerId);
			return true;
		}



		/** @brief Add a Component(Script) to be enabled in a State.
		 * 
		 * If the component is already created in the game object it returns it.
		 * @param typeComponent Script component to be enabled.
		 * @param stateName State to add the component.
		 * @param layerId Layer where the state is.
		 * @param reset Force to reenable a script if it where
		 * @return Return true if the animation has been added.
		 */
		public bool AddComponent (System.Type typeComponent, string stateName, int layerId = 0,  bool reset = false)
		{
			State auxState = FindState (stateName, layerId);
			if (auxState == null) {
				Debug.LogWarningFormat ("The state {0} could not be found in layer {1}.", stateName, layerId);
				return false;
			}

			if (auxState.AddComponent (typeComponent, reset) != null)
				return true;
			return false;
		}


		/** @brief Add a animation name to the actived animations in a State.
		 * 
		 * @param animationName The name of the animation to be actived.
		 * @param stateName State to add the animation.
		 * @param layerId Layer where the state is.
		 * @return Return true if the animation has been added.
		 */
		public bool AddAnimation (string animationName, string stateName, int layerId = 0,  bool reset = false)
		{
			State auxState = FindState (stateName, layerId);
			if (auxState == null) {
				Debug.LogWarningFormat ("The state {0} could not be found in layer {1}.", stateName, layerId);
				return false;
			}

			if (auxState.AddAnimation (animationName))
				return true;
			return false;
		}


		/** @brief Check if an state is active.
		 * 
		 * @param stateName State to check.
		 * @param layerId Layer where the state is.
		 * @return Return true if the state is actived.
		 */
		public bool IsActivedState (string stateName, int layerId = 0,  bool reset = false)
		{
			State state = FindState (stateName, layerId);
			if (state == null) {
				Debug.LogWarningFormat ("The state {0} could not be found in layer {1}.", stateName, layerId);
				return false;
			}

			return state.Actived;
		}



		//! Updates the state of the agent
		public void Update ()
		{
			if (Enabled) {
				// It is needed to update changes made by the messages before checking transitions
				UpdateContext ();
				foreach (Layer layerI in _layers) {
					layerI.Update ();
				}
				UpdateContext ();
			}
		}


		public bool SetInitialState (string stateName, int layerId = 0)
		{
			State stateAux = FindState (stateName, layerId);
			if (stateAux == null) {
				Debug.LogWarningFormat ("The state {0} could not be found in layer {1}.", stateName, layerId);
				return false;
			}
			FindLayer (layerId).InitialState = stateAux;
			return true;
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


		/** @brief Sends diffents messages types to state components
		 * 
		 * @param agentName The agent to send the message
		 * @param msgType The type of message
		 * @param content The content of the message
		 * @return True if the message has been successfully sent
		 */
		public bool SendMessage (AgentManager.MsgType typeMsg, string content)
		{
			if (typeMsg == AgentManager.MsgType.INTERRUPTING_MSG) {
				List<State> states = FindState (content);
				if (states.Count == 0) {
					Debug.LogWarningFormat ("The state {0} was not found to be interrupting.", content);
					return false;
				}
				states [0].Interrupt ();
			}
			return true;
		}


		#endregion // PUBLIC_METHODS



		#region PRIVATE_METHODS

		/** @brief Add an State to the Agent.
		 * 
		 * @param state State object to be added.
		 * @param layerId Adds the state in the state layer passed.
		 * @return Returns true if the addition has been successfully done.
		 */
		private bool AddState (State state, int layerId = 0)
		{
			Layer layerAux = FindLayer (layerId);
			if (layerAux == null)
				layerAux = AddLayer (layerId);

			if (layerAux.AddState (state))
				return true;
			Debug.LogWarningFormat ("The state {0} has not been added to layer {1}.", state.Name, layerId);
			return false;
		}


		/** @brief Remove an State of the Agent.
		 * 
		 * @param state State to remove
		 * @param layerId Layer to remove from.
		 * @return Returns true if the addition has been successfully done.
		 */
		private bool RemoveState (State state, int layerId = 0)
		{
			Layer layerAux = FindLayer (layerId);
			if (layerAux == null)
				return false;

			return layerAux.RemoveState (state);
		}


		/** @brief Find a State in a Layer by name.
		 * 
		 * @param stateName The name of the state to be found.
		 * @param layerId Layer to search in.
		 * @return The State searched or null if it does not exist.
		 */
		private State FindState (string stateName, int layerId)
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
		private List<State> FindState (string stateName)
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

		/** @brief Add layer to this Agent
		 * 
		 * @param layerId The id of the new Layer
		 */
		private Layer AddLayer (int layerId)
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


		//! Update State of components and animators
		private void UpdateContext ()
		{
			// Update state of animations and components
			foreach (var item in _animationNames) {
				// I need to save the last animation state because I there are trigger animations variables
				if (!_lastAnimationNames.ContainsKey (item.Key))
					_lastAnimationNames [item.Key] = 0;

				if (_lastAnimationNames [item.Key] == 0 && item.Value > 0)
					_animator.SetBool (item.Key, true);
				if (_lastAnimationNames [item.Key] > 0 && item.Value == 0)
					_animator.SetBool (item.Key, false);

				//_animator.SetBool (item.Key, item.Value > 0);
				_lastAnimationNames [item.Key] = item.Value;
			}

			foreach (var item in _componentTypes) {
				Component component = _character.GetComponent (item.Key);

				if (!_lastComponentTypes.ContainsKey (item.Key))
					_lastComponentTypes [item.Key] = 0;

				if (_lastComponentTypes [item.Key] == 0 && item.Value > 0) {
					// Check if it is enabled already
					if (!(component as MonoBehaviour).enabled) {
						(component as MonoBehaviour).enabled = true;
					}
				}
				if (_lastComponentTypes [item.Key] > 0 && item.Value == 0) {
					if ((component as MonoBehaviour).enabled) {
						(component as MonoBehaviour).enabled = false;
					}
				}

				_lastComponentTypes [item.Key] = item.Value;
			}
		}

		#endregion // PRIVATE_METHODS
	}
}