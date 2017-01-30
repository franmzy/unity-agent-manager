using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgentManagerNamespace
{
	public class LogicActionAbstract <T,P> 
		where T : VisualActionAbstract, new()
		where P : PerceptionAbstract, new()
	{

		#region PUBLIC_MEMBER_VARIABLES


		#endregion // PUBLIC_MEMBER_VARIABLES



		#region PRIVATE_MEMBER_VARIABLES

		private Agent _agent;
		private GameObject _character;
		private bool _masked;
		private T _visualAction;
		private P _perception;

		#endregion // PRIVATE_MEMBER_VARIABLES



		#region GETTERS_AND_SETTERS_METHODS

		protected GameObject Character { get { return _character; } }
		protected Agent Agent { get { return _agent; } }

		public T VisualAction { get { return _visualAction; } }
		public P Perception { get { return _perception; } }
		 
		#endregion // GETTERS_AND_SETTERS_METHODS



		#region PUBLIC_METHODS

		public void Initialize(Agent agent, GameObject character) {
			_agent = agent;
			_character = character;
			_visualAction = new T ();
			_visualAction.Initialize (agent, character);
		}

		public void Activate() {
			// Call the children event
			OnActivate();
		}

		public void Deactivate() {
			// Call the children event
			OnDeactivate();
		}

		public void Update() {
			if (!_masked)
				OnUpdate ();
		}

		public void Mask() {
			_masked = true;
		}

		public void Unmask() {
			_masked = false;
		}

		public void ReceiveInterruptingMessage(object value, Agent sender = null) {
			OnInterruptingMessageReceived (value, sender);
		}

		public void ReceiveStandarMessage(object value, Agent sender = null) {
			OnMessageReceived (value, sender);
		}

		#endregion // PUBLIC_METHODS



		#region PRIVATE_METHODS

		protected virtual void OnActivate() {}

		protected virtual void OnDeactivate() {}

		protected virtual void OnUpdate() {}

		protected virtual void OnMessageReceived(object value, Agent sender = null) {}

		protected virtual void OnInterruptingMessageReceived(object value, Agent sender = null) {}

		#endregion // PRIVATE_METHODS
	}
}