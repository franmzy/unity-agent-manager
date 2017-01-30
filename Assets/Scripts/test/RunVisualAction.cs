using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgentManagerNamespace;

public class RunVisualAction : VisualActionAbstract {
	public void Move(Vector3 direction, float speed) {
		Character.transform.Translate (direction.normalized * speed * Time.deltaTime);
	}
}
