using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgentManagerNamespace;

public class OrcArchitectureConfiguration : ArchitectureConfigurationAbstract<OrcPerception> {
	protected override void OnInitializeConfiguration() {
		// Adding Idle State
		AddState("idle", "Idle");

		// Adding Run State
		AddState("run", "Run");

		AddAction<RunLogicAction>("run");

		// Adding Idle => Run transition
		AddTransition("idle", "run", new TransitionTrigger ( () => (Perception.InputDirection() != Vector3.zero)));

		// Adding Run => Idle transition (Different way to do it)
		AddTransition("run", "idle", new TransitionTrigger ( () => (Perception.InputDirection() == Vector3.zero)));
	}
}
