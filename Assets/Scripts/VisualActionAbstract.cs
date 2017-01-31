using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgentManagerNamespace
{
	public class VisualActionAbstract : IVisualAction{

		#region PUBLIC_MEMBER_VARIABLES


		#endregion // PUBLIC_MEMBER_VARIABLES



		#region PRIVATE_MEMBER_VARIABLES

		private GameObject _character;
		private int _masked;

		private bool _activated;

		#endregion // PRIVATE_MEMBER_VARIABLES



		#region GETTERS_AND_SETTERS_METHODS

		protected GameObject Character { get { return _character; } }

		#endregion // GETTERS_AND_SETTERS_METHODS



		#region PUBLIC_METHODS

		public void Initialize(GameObject character) {
			_character = character;
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

		#endregion // PUBLIC_METHODS



		#region PRIVATE_METHODS

		protected virtual void OnActivate() {}

		protected virtual void OnDeactivate() {}

		protected virtual void OnUpdate() {}

		#endregion // PRIVATE_METHODS
	}
}