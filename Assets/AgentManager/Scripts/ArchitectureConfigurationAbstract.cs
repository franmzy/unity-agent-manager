using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgentManagerNamespace
{
	public abstract class ArchitectureConfigurationAbstract <P> 
		: IArchitertureConfiguration
		where P : PerceptionAbstract, new()
	{

		#region PUBLIC_MEMBER_VARIABLES


		#endregion // PUBLIC_MEMBER_VARIABLES



		#region PRIVATE_MEMBER_VARIABLES

		private Agent _agent;
		private P _perception;

		#endregion // PRIVATE_MEMBER_VARIABLES



		#region GETTERS_AND_SETTERS_METHODS


		protected P Perception { get { return _perception; } }

		public IPerceptionAction IPerceptionAction { get { return _perception; } }

		#endregion // GETTERS_AND_SETTERS_METHODS



		#region PUBLIC_METHODS

		public void Initialize (Agent agent, GameObject character)
		{
			_agent = agent;
			_perception = character.AddComponent<P> ();
			OnInitializeConfiguration ();
		}


		protected bool AddState (string stateName, int layerId = 0, int bitmask = 0, float fixedDuration = 0)
		{
			return _agent.AddState (stateName, layerId, bitmask, fixedDuration);
		}

		protected bool AddState<L> (string stateName, string animationName, int layerId = 0, int bitmask = 0, float fixedDuration = 0) 
			where L:ILogicAction, new()
		{
			return _agent.AddState<L> (stateName, animationName, layerId, bitmask, fixedDuration); 
		}

		protected bool AddState<L> (string stateName, int layerId = 0, int bitmask = 0, float fixedDuration = 0)
			where L:ILogicAction, new()
		{
			return _agent.AddState<L> (stateName, layerId, bitmask, fixedDuration); 
		}

		protected bool AddState (string stateName, string animationName, int layerId = 0, int bitmask = 0, float fixedDuration = 0)
		{
			return _agent.AddState (stateName, animationName, layerId, bitmask, fixedDuration);
		}

		protected bool AddTransition (string originStateName, string targetStateName, TransitionTrigger trigger, int layerId = 0, int priority = 0)
		{
			return _agent.AddTransition (originStateName, targetStateName, trigger, layerId, priority);
		}

		protected bool AddAction<L> (string stateName, int layerId = 0) 
			where L:ILogicAction, new()
		{
			return _agent.AddAction<L> (stateName, layerId);
		}

		protected bool SetAnimation (string animationName, string stateName, int layerId = 0)
		{
			return _agent.SetAnimation (animationName, stateName, layerId);  
		}

		protected bool SetInitialState (string stateName, int layerId = 0)
		{
			return _agent.SetInitialState (stateName, layerId);
		}

		#endregion // PUBLIC_METHODS



		#region PRIVATE_METHODS

		protected abstract void OnInitializeConfiguration ();

		#endregion // PRIVATE_METHODS
	}
}