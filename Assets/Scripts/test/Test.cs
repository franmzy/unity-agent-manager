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
		orc.AddState("idle", typeof(IdleScript), "Idle");
		orc.SetInitialState ("idle");

		// Adding Run State
		orc.AddState("run", typeof(RunScript), "Run");

		// Adding Idle => Run transition
		orc.AddTransition("idle", "run", new TransitionTrigger ( () => Input.GetKeyDown (KeyCode.DownArrow)));

		// Adding Run => Idle transition (Different way to do it)
		orc.AddTransition("run", "idle", new TransitionTrigger ( () => Input.GetKeyDown (KeyCode.DownArrow)));

		// Activating Agent Manager
		AgentManager.instance.ManagerEnabled = true;
	}
}
