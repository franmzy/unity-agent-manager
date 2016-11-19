using UnityEngine;
using System.Collections;
using AgentManagerNamespace;

public class Loader : MonoBehaviour
{
	public GameObject agentManager;
	//AgentManager prefab to instantiate.

	void Awake ()
	{
		//Check if a GameManager has already been assigned to static variable GameManager.instance or if it's still null
		if (AgentManager.instance == null) {
			//Instantiate gameManager prefab
			Instantiate (agentManager);
		}
	}

}