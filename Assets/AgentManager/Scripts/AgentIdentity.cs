using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgentManagerNamespace
{
	public class AgentIdentity : MonoBehaviour {
		public string _id;
		private bool _initialized;

		public string Id { get { return _id; } }

		public void Initilize (string id) {
			// Making sure that this function only works once 
			if (!_initialized) {
				_id = id;
				_initialized = true;
			}
			else {
				Debug.LogWarningFormat ("The agent id {0} has been already initialized", _id);
			}
		}
	}
}