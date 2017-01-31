using UnityEngine;
using System.Collections;
using AgentManagerNamespace;
using UnityEditor;

public class Test : MonoBehaviour {

	public GameObject _character;

	void Start() {
		// Creanting an Agent Orc
		Agent orc = AgentManager.instance.CreateAgent<OrcLogicController> ("orc", _character);

		orc.AddArchitecture<OrcArchitectureConfiguration> ();
		// Activating Agent Manager
		AgentManager.instance.ManagerEnabled = true;
	}
}
