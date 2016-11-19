using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AgentManagerNamespace
{
	/** @brief Main class of the Agent Manager.
	 * It is the main class of the Agent Manager,
	 * it manages the actived agents at the current moment.
	 */
	public class AgentManager : MonoBehaviour
	{
		#region PUBLIC_MEMBER_VARIABLES

		///Static instance of AgentManager which allows it to be accessed by any other script.
		public static AgentManager instance = null;

		#endregion // PUBLIC_MEMBER_VARIABLES



		#region PRIVATE_MEMBER_VARIABLES

		// Current list of agents
		private List<Agent> _agents;

		#endregion // PRIVATE_MEMBER_VARIABLES



		#region GETTERS_AND_SETTERS_METHODS


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

		#endregion // UNTIY_MONOBEHAVIOUR_METHODS



		#region PUBLIC_METHODS

		/** Returns the agent with the named passed,
		 * if no one of the current agents has this name it returns null.
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

		public Agent CreateAgent (string agentName)
		{
			Agent newAgent = new Agent (agentName);
			_agents.Add (newAgent);
			return newAgent;
		}

		public Agent CreateAgent (string agentName, GameObject character)
		{
			Agent newAgent = CreateAgent (agentName);
			newAgent.Character = character;
			return newAgent;
		}

		public bool RemoveAgent (string agentName)
		{
			// The Equals method only checks name for equality.
			return _agents.Remove (new Agent (agentName));
		}

		#endregion // PUBLIC_METHODS



		#region PRIVATE_METHODS


		#endregion // PRIVATE_METHODS
	}
}
