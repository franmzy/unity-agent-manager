using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgentManagerNamespace
{
	public interface IArchitertureConfiguration {
		void Initialize (Agent agent, GameObject character);

        IPerceptionAction IPerceptionAction { get; }
	}
}