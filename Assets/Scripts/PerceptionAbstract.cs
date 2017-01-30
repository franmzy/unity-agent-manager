using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgentManagerNamespace
{
	public class PerceptionAbstract
	{
		#region PUBLIC_MEMBER_VARIABLES


		#endregion // PUBLIC_MEMBER_VARIABLES



		#region PRIVATE_MEMBER_VARIABLES

		private Agent _agent;
		private GameObject _character;

		#endregion // PRIVATE_MEMBER_VARIABLES



		#region GETTERS_AND_SETTERS_METHODS

		protected Agent Agent { get { return _agent; } }
		protected GameObject Character { get { return _character; } }

		#endregion // GETTERS_AND_SETTERS_METHODS



		#region PUBLIC_METHODS

		public void Initialize (Agent agent, GameObject character) {
			_agent = agent;
			_character = character;
		}

		#endregion // PUBLIC_METHODS



		#region PRIVATE_METHODS


		#endregion // PRIVATE_METHODS
	}
}