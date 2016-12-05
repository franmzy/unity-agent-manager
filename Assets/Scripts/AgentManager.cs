using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AgentManagerNamespace
{
	/** @brief Main class of the Agent Manager.
	 * 
	 * This class manages the actived agents at that current moment.
	 */
	public class AgentManager : MonoBehaviour
	{
		#region PUBLIC_MEMBER_VARIABLES

		///Static instance of AgentManager which allows it to be accessed by any other script.
		public static AgentManager instance = null;


		//! Different types of messages
		public enum MsgType
		{
			INTERRUPTING_MSG
		}
			
		#endregion // PUBLIC_MEMBER_VARIABLES



		#region PRIVATE_MEMBER_VARIABLES

		// Current list of agents
		private List<Agent> _agents;

		// Manager enabled
		private bool _managerEnabled;

		#endregion // PRIVATE_MEMBER_VARIABLES



		#region GETTERS_AND_SETTERS_METHODS

		//! Controls whether the agent manager is enabled.
		public bool ManagerEnabled { 
			get { return _managerEnabled; }
			set { 
				if (!_managerEnabled && value) {
					foreach (var agent in _agents) {
						agent.Enabled = true;
					}
				}
				if (_managerEnabled && !value) {
					foreach (var agent in _agents) {
						agent.Enabled = false;
					}
				}
				_managerEnabled = value;
			}
		}

		#endregion // GETTERS_AND_SETTERS_METHODS



		#region UNTIY_MONOBEHAVIOUR_METHODS

		void Awake ()
		{
			// Check if instance already exists
			if (instance == null)
				// if not, set instance to this
				instance = this;

			// If instance already exists and it's not this:
			else if (instance != this)
				// Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
				Destroy (gameObject);    

			// Sets this to not be destroyed when reloading scene
			DontDestroyOnLoad (gameObject);

			_agents = new List<Agent> ();
		}

		void Update ()
		{
			if (ManagerEnabled) {
				// Call each agent update
				foreach (Agent agent in _agents) {
					agent.Update ();
				}
			}
		}

		#endregion // UNTIY_MONOBEHAVIOUR_METHODS



		#region PUBLIC_METHODS

		/** @brief Search Agent by name.
		 * 
		 * If no one of the current agents has this name it returns null.
		 * @param agentName Name of the agent to search
		 * @return The agent found with the name passed or null if it does not exist.
		 */
		public Agent GetAgent (string agentName)
		{
			foreach (Agent agent in _agents) {
				if (agent.Name.Equals (agentName)) {
					return agent;
				}
			}
			return null;
		}


		/** @brief Creates a new Agent in the system.
		 * 
		 * @param agentName The ID name of the Agent.
		 * @param character A GameObject with the model character and its Animator component.
		 * @return A reference to the agent created in the system.
		 */
		public Agent CreateAgent (string agentName, GameObject character)
		{
			// Check if agent already exists
			if (_agents.Find (agent => agent.Name.Equals (agentName)) != null) {
				Debug.LogWarningFormat ("The agent {0} already exists", agentName);
				return null;
			}
			if (character == null) {
				Debug.LogWarningFormat ("The character associated with the agent {0} can't be null", agentName);
				return null;
			}
			Agent newAgent = new Agent (agentName, character);
			_agents.Add (newAgent);
			return newAgent;
		}


		/** @brief Remove from the system the Agent that match agentName.
		 * 
		 * @param agentName The ID name of the Agent to be removed.
		 * @return True if the Agent has been found and removed, False in any other case.
		 */
		public bool RemoveAgent (string agentName)
		{
			// The Equals method only checks name for equality.
			return _agents.Remove (new Agent (agentName));
		}


		/** @brief Sends diffents messages types to state components
		 * 
		 * @param agentName The agent to send the message
		 * @param msgType The type of message
		 * @param content The content of the message
		 * @return True if the message has been successfully sent
		 */
		public bool SendMessage(string agentName, MsgType msgType, string content) {
			Agent agent = GetAgent (agentName);
			if (agent == null) {
				Debug.LogWarningFormat ("The agent {0} does not exists", agentName);
				return false;
			}
			return agent.SendMessage (msgType, content);
		}

		#endregion // PUBLIC_METHODS



		#region PRIVATE_METHODS


		#endregion // PRIVATE_METHODS
	}
}
