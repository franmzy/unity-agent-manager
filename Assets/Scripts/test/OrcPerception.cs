using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgentManagerNamespace;

public class OrcPerception : PerceptionAbstract{
	public Vector3 InputDirection() {
		Vector3 auxVector = Vector3.zero;

		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			auxVector.x++;
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			auxVector.x--;
		}
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			auxVector.z++;
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			auxVector.z--;
		}
		return auxVector;
	}
}
