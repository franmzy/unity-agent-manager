using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgentManagerNamespace
{
	public interface IPerceptionAction
	{
		void Initialize (GameObject character);

		void CustomAwake ();

		void CustomStart ();
	}
}