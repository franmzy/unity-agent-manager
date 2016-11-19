using System.Collections;
using UnityEngine;

namespace AgentManagerNamespace
{
	public class Agent
	{
		#region PUBLIC_MEMBER_VARIABLES

		private string _name;
		private GameObject _character;

		#endregion // PUBLIC_MEMBER_VARIABLES



		#region PRIVATE_MEMBER_VARIABLES



		#endregion // PRIVATE_MEMBER_VARIABLES



		#region GETTERS_AND_SETTERS_METHODS

		public string Name { get { return _name; } }

		public GameObject Character { 
			get { return _character; } 
			set { _character = value; } 
		}

		#endregion // GETTERS_AND_SETTERS_METHODS



		#region UNTIY_MONOBEHAVIOUR_METHODS


		#endregion // UNTIY_MONOBEHAVIOUR_METHODS



		#region PUBLIC_METHODS

		public Agent ()
		{
		}

		public Agent (string agentName)
		{
			_name = agentName;
		}

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

		public bool Equals (Agent other)
		{
			if (other == null)
				return false;
			return (this.Name.Equals (other.Name));
		}

		#endregion // PUBLIC_METHODS



		#region PRIVATE_METHODS


		#endregion // PRIVATE_METHODS
	}
}