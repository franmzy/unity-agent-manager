using UnityEngine;
using System.Collections;
using AgentManagerNamespace;
using UnityEditor;

public class Test : MonoBehaviour {

	public GameObject _character;

	void Start() {
		// Creanting an Agent Orc
		Agent orc = AgentManager.instance.CreateAgent ("orc", _character);

		// Adding Idle State
		orc.AddState("idle", "Idle");

		// Adding Run State
		orc.AddState("run", "Run");

		orc.AddAction<RunLogicAction, RunVisualAction, OrcPerception> ("run");

		// Adding Idle => Run transition
		//orc.AddTransition("idle", "run", new TransitionTrigger ( () => ((OrcPerception) orc.Perception).InputDirection != Vector3.zero));

		// Adding Run => Idle transition (Different way to do it)
		//orc.AddTransition("run", "idle", new TransitionTrigger ( () => ((OrcPerception) orc.Perception).InputDirection == Vector3.zero));



		// Activating Agent Manager
		AgentManager.instance.ManagerEnabled = true;

	}
}
