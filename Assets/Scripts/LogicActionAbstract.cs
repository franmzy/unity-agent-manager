using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgentManagerNamespace
{
	public abstract class LogicActionAbstract <T,P> 
		: ILogicAction
		where T : VisualActionAbstract, IVisualAction, new()
		where P : PerceptionAbstract, IPerceptionAction, new()
	{

		#region PUBLIC_MEMBER_VARIABLES


		#endregion // PUBLIC_MEMBER_VARIABLES



		#region PRIVATE_MEMBER_VARIABLES

		private Agent _agent;
		private int _masked;
		private T _visualAction;
		private P _perception;

		private bool _activated;

		#endregion // PRIVATE_MEMBER_VARIABLES



		#region GETTERS_AND_SETTERS_METHODS

		protected Agent Agent { get { return _agent; } }

		protected T VisualAction { get { return _visualAction; } }
		protected P Perception { get { return _perception; } }

		public IVisualAction IVisualAction { get { return _visualAction; } }
		 
		#endregion // GETTERS_AND_SETTERS_METHODS



		#region PUBLIC_METHODS

		public void Initialize(Agent agent, GameObject character) {
			_agent = agent;
			_visualAction = new T ();
			_visualAction.Initialize (character);
			_perception = new P ();
			_perception.Initialize (character);
		}

		public void Activate() {
			// Call the children event
			_activated = true;
			if(_masked == 0)
				OnActivate();
		}

		public void Deactivate() {
			// Call the children event
			_activated = false;
			if(_masked == 0)
				OnDeactivate();
		}

		public void Update() {
			if (_masked == 0)
				OnUpdate ();
		}

		public void Mask() {
			if (++_masked == 1) {
				if (_activated)
					OnDeactivate ();
			}
		}

		public void Unmask() {
			if (--_masked == 0) {
				if (_activated)
					OnActivate ();
			}
			if (_masked < 0)
				_masked = 0;
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