using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgentManagerNamespace
{
	public interface IVisualAction {
		void Activate ();

		void Deactivate ();

		void Update ();

		void Mask ();

		void Unmask ();
	}
}