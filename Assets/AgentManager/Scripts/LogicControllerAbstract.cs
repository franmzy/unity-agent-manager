using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgentManagerNamespace
{
	public abstract class LogicControllerAbstract
	{

		#region PUBLIC_MEMBER_VARIABLES


		#endregion // PUBLIC_MEMBER_VARIABLES



		#region PRIVATE_MEMBER_VARIABLES

		private Agent _agent;

		#endregion // PRIVATE_MEMBER_VARIABLES



		#region GETTERS_AND_SETTERS_METHODS

		protected Agent Agent { get { return _agent; } }

		#endregion // GETTERS_AND_SETTERS_METHODS



		#region PUBLIC_METHODS

		public void Initialize (Agent agent)
		{
			_agent = agent;
		}

		public void Awake ()
		{
			OnAwake ();
		}

		public void Start ()
		{
			OnStart ();
		}

		public void Update ()
		{
			OnUpdate ();
		}

		#endregion // PUBLIC_METHODS



		#region PRIVATE_METHODS

		protected virtual void OnAwake ()
		{
		}

		protected virtual void OnStart ()
		{
		}

		protected virtual void OnUpdate ()
		{
		}

		#endregion // PRIVATE_METHODS
	}
}
