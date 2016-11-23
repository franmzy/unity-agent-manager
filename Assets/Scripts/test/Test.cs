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
		State idle = orc.AddState ("idle");
		idle.AddComponent (typeof(IdleScript));
		idle.AddAnimation ("Idle");

		orc.SetInitialState ("idle");

		// Adding Run State
		State run = orc.AddState ("run");
		run.AddComponent (typeof(RunScript));
		run.AddAnimation ("Run");

		// Adding Idle => Run transition
		TransitionTrigger trigger = new TransitionTrigger ( () => Input.GetKeyDown (KeyCode.DownArrow));
		idle.AddTransition( new Transition(run, trigger));

		// Adding Run => Idle transition (Different way to do it)
		trigger = new TransitionTrigger (() => Input.GetKeyDown (KeyCode.UpArrow));
		orc.AddTransition ("run", "idle", trigger);

		// Activating Agent Manager
		AgentManager.instance.ManagerEnabled = true;
	}
}
