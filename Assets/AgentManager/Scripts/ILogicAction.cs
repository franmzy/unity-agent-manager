using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgentManagerNamespace
{
	public interface ILogicAction {
		void Initialize (Agent agent, State state, GameObject character);

		void Activate ();

		void Deactivate ();

		void Awake ();

		void Start ();

		void Update ();

		void Mask ();

		void Unmask ();

		void ReceiveInterruptingMessage (object value, Agent sender = null);

		void ReceiveStandarMessage (object value, Agent sender = null);

		IVisualAction IVisualAction { get; }

		IPerceptionAction IPerceptionAction { get; }
	}
}