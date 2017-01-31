using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgentManagerNamespace;

public class OrcPerception : PerceptionAbstract{
	public Vector3 InputDirection() {
		Vector3 auxVector = Vector3.zero;

		if (Input.GetKey (KeyCode.RightArrow)) {
			auxVector.x++;
		}
		if (Input.GetKey (KeyCode.LeftArrow)) {
			auxVector.x--;
		}
		if (Input.GetKey (KeyCode.UpArrow)) {
			auxVector.z++;
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			auxVector.z--;
		}
		return auxVector;
	}
}
