using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgentManagerNamespace
{
	public  class VisualActionAbstract {

		#region PUBLIC_MEMBER_VARIABLES


		#endregion // PUBLIC_MEMBER_VARIABLES



		#region PRIVATE_MEMBER_VARIABLES

		private Agent _agent;
		private GameObject _character;

		private bool _masked;

		#endregion // PRIVATE_MEMBER_VARIABLES



		#region GETTERS_AND_SETTERS_METHODS

		protected GameObject Character { get { return _character; } }
		protected Agent Agent { get { return _agent; } }

		#endregion // GETTERS_AND_SETTERS_METHODS



		#region PUBLIC_METHODS

		public void Initialize(Agent agent, GameObject character) {
			_agent = agent;
			_character = character;
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

		#endregion // PUBLIC_METHODS



		#region PRIVATE_METHODS

		protected virtual void OnActivate() {}

		protected virtual void OnDeactivate() {}

		protected virtual void OnUpdate() {}

		#endregion // PRIVATE_METHODS
	}
}