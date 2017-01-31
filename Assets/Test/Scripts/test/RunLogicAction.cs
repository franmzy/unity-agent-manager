using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgentManagerNamespace;

public class RunLogicAction : LogicActionAbstract<RunVisualAction, OrcPerception> {
	protected override void OnUpdate() {
		VisualAction.Move (Perception.InputDirection (), 0.5f);
	}
}
