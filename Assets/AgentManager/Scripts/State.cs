using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//using UnityEditor;
using System.Linq;
using System.Reflection;
using System;

namespace AgentManagerNamespace
{
	public class State
	{
		#region PUBLIC_MEMBER_VARIABLES


		#endregion // PUBLIC_MEMBER_VARIABLES



		#region PRIVATE_MEMBER_VARIABLES

		// ID name for the state
		private string _name;
		// Possible transitions for this state
		private List<Transition> _transitions = new List<Transition> ();
		// Animations actived in this state
		private string _animationName;
		private bool _hasAnimation;
		// Action actived in this state
		private HashSet<System.Type> _logicActionTypes = new HashSet<System.Type> ();
		// Activated state
		private bool _activated = false;

		private GameObject _character;

		// VisualAction object
		List<ILogicAction> _logicActions = new List<ILogicAction> ();
		List<IVisualAction> _visualActions = new List<IVisualAction> ();

		// Fix Duration
		private float _fixedDuration;
		private bool _hasFixedDuration;
		private float _timeFixed;
		private bool _lockState;

		#endregion // PRIVATE_MEMBER_VARIABLES



		#region GETTERS_AND_SETTERS_METHODS

		/// The Id name that identifies the Agent.
		public string Name { get { return _name; } }

		/// Animatión names actived in this state.
		// public HashSet<string> Animations { get { return _animations; } }

		/// Layer that contains this state.
		public Layer Layer { get; set; }

		public int Bitmask { get; set; }

		public bool Actived { get { return _activated; } }

		#endregion // GETTERS_AND_SETTERS_METHODS



		#region UNTIY_MONOBEHAVIOUR_METHODS


		#endregion // UNTIY_MONOBEHAVIOUR_METHODS



		#region PUBLIC_METHODS

		/** \brief Creates an Agent object.
		 * 
		 * @param agentName Id name for the agent.
		 */
		public State (string stateName, GameObject character, int bitmask = 0, float fixedDuration = 0)
		{
			_name = stateName;
			_character = character;
			if (fixedDuration > 0) {
				_hasFixedDuration = true;
				_fixedDuration = fixedDuration;
			}
			Bitmask = bitmask;
		}


		/** \brief Equality between State and Object.
		 * 
		 * @param obj Object to compare with.
		 */
		public override bool Equals (object obj)
		{
			if (obj == null)
				return false;
			State state = obj as State;
			if (state == null)
				return false;
			else
				return Equals (state);
		}


		/** \brief Equality between Agents.
		 * 
		 * @param agent Agent to compare with.
		 */
		public bool Equals (State state)
		{
			if (state == null)
				return false;
			return (this.Name.Equals (state.Name));
		}


		/** @brief Add a transition to the state.
		 * 
		 * @param targetState The State to transitate to.
		 * @param trigget Trigger that determinates when to transitate.
		 * @param priotity The prority to be actived.
		 * @return A reference to the transition created.
		 */
		public Transition AddTransition (State targetState, TransitionTrigger trigger, int priority = 0)
		{
			// Check if the target state belong to the same layer than this.
			if (!targetState.Layer.Equals (this.Layer)) {
				Debug.LogWarningFormat ("The state {0} does not belong to the layer {1}.", targetState.Name, this.Layer.Id);
				return null;
			}
				
			Transition newTransition = new Transition (targetState, trigger, priority);
			// Check if this transition alreary exists.
			if (_transitions.Contains (newTransition)) {
				Debug.LogWarningFormat ("This transition {0}-{1} does already exist.", this.Name, newTransition.TargetState.Name);
				return null;
			}
			// Adding the transtion.
			_transitions.Add (newTransition);
			// Sorting transtions by prority.
			_transitions.Sort ();
			return newTransition;
		}


		/** @brief Add a transition to the state.
		 * 
		 * @param transition Transition object to be added.
		 * @return Returns true if the addition has been successfully done.
		 */
		public bool AddTransition (Transition transition)
		{
			// Check if the target state belong to the same agent than this.
			if (!transition.TargetState.Layer.Equals (this.Layer)) {
				Debug.LogWarningFormat ("The state {0} does not belong to the layer {1}.", transition.TargetState.Name, this.Layer.Id);
				return false;
			}
			// Check if this transition alreary exists.
			if (_transitions.Contains (transition)) {
				Debug.LogWarningFormat ("This transition {0}-{1} does already exist.", this.Name, transition.TargetState.Name);
				return false;
			}
			_transitions.Add (transition);
			return true;
		}


		/** @brief Add a animation name to the actived animations in this State.
		 * 
		 * @param animationName The name of the animation to be actived.
		 * @return Return true if the animation has been added.
		 */
		public void SetAnimation (string animationName)
		{
			_animationName = animationName;
			_hasAnimation = true;
		}



		/** @brief Add a Action to be enabled in this State.
		 * 
		 * If the component is already created in the game object it returns it.
		 * @return Return true if the action has been added.
		 */
		public bool AddAction<L> ()
			where L:ILogicAction, new()
		{
			// Check if the component already exists
			if (_logicActionTypes.Contains (typeof(L))) {
				Debug.LogWarningFormat ("The State {0} already has the logic action {1}.", Name, typeof(L).Name);
				return false;
			}

			ILogicAction logicAction = new L ();
			logicAction.Initialize (Layer.Agent, this, _character);
			_logicActions.Add (logicAction);
			_logicActionTypes.Add (typeof(L));

			_visualActions.Add (logicAction.IVisualAction);

			// Calling start Events -> Llamamos aqui a awake, y start se debería llamar cuando se habilite el agent manager
			logicAction.Awake ();
			logicAction.IVisualAction.CustomAwake ();
			logicAction.IPerceptionAction.CustomAwake ();

			return true;
		}


		/** @brief Returns the active transition of the State.
		 * 
		 * @return The active transition of the State, null if there is not actived transtion.
		 */
		public Transition GetActivedTransition ()
		{
			// Giving the active transition by priority
			foreach (Transition transition in _transitions) {
				if (transition.Actived ())
					return transition;
			}
			return null;
		}

		//! Activate the State.
		public void ActivateState ()
		{
			if (!_activated) {
				// Active animations
				if (_hasAnimation)
					Layer.Agent.AnimationController.ActivateAnimation (_animationName);

				// Active Logic Actions
				foreach (var loginAction in _logicActions) {
					loginAction.Activate ();
				}

				// Active Visual Actions
				foreach (VisualActionAbstract visualAction in _visualActions) {
					visualAction.Activate ();
				}

				// Control Mask
				int layerId = Layer.Id - 1;
				for (int i = 1 << Layer.Id - 1; i > 0; i >>= 1) {
					if ((i & Bitmask) > 0) {
						Layer auxLayer = Layer.Agent.FindLayer (layerId);
						if (auxLayer != null)
							auxLayer.Mask ();
					}
					layerId--;
				}

				_activated = true;
				if (_hasFixedDuration) {
					_lockState = true;
					_timeFixed = Time.time;
					Layer.LockedState = true;
				}
				//Debug.LogFormat ("Agent {0} actived state {1} in layer {2}", Layer.Agent.Name, Name, Layer.Id);
			}
		}


		/** @brief Deactivate the State.
		 * 
		 * Avoid using it if your intention is to transitate to other estate.
		 * Deactiving the state you are disabling every animation and component
		 * associated to this state.
		 */
		public void DeactivateState ()
		{
			if (_activated) {
				// Deactive animations
				if (_hasAnimation)
					Layer.Agent.AnimationController.DeactivateAnimation (_animationName);

				// Deactive Logic Actions
				foreach (var logicAction in _logicActions) {
					logicAction.Deactivate ();
				}

				// Deactive Visual Actions
				foreach (VisualActionAbstract visualAction in _visualActions) {
					visualAction.Deactivate ();
				}


				// Control Mask
				int layerId = Layer.Id - 1;
				for (int i = 1 << Layer.Id - 1; i > 0; i >>= 1) {
					if ((i & Bitmask) > 0) {
						Layer auxLayer = Layer.Agent.FindLayer (layerId);
						if (auxLayer != null)
							auxLayer.Unmask ();
					}
					layerId--;
				}

				_activated = false;
				//Debug.LogFormat ("Agent {0} Deactivated state {1} in layer {2}", Layer.Agent.Name, Name, Layer.Id);
			}
		}


		public void EndAction ()
		{
			if (_hasFixedDuration && _lockState) {
				_lockState = false;
				Layer.LockedState = false;
			}
		}


		public void SendInterruptingMsg (object value, Agent sender = null)
		{
			// Send interrupting messages to logic actions
			foreach (ILogicAction logicAction in _logicActions) {
				logicAction.ReceiveInterruptingMessage (value, sender);
			}
		}

		public void SendStandardMsg (object value, Agent sender = null)
		{
			// Send interrupting messages to logic actions
			foreach (ILogicAction logicAction in _logicActions) {
				logicAction.ReceiveStandarMessage (value, sender);
			}
		}

		public void Mask ()
		{
			// Deactive Logic Actions
			foreach (ILogicAction logicAction in _logicActions) {
				logicAction.Mask ();
			}

			// Deactive Visual Actions
			foreach (IVisualAction visualAction in _visualActions) {
				visualAction.Mask ();
			}
		}

		public void Unmask ()
		{
			// Deactive Logic Actions
			foreach (ILogicAction logicAction in _logicActions) {
				logicAction.Unmask ();
			}

			// Deactive Visual Actions
			foreach (IVisualAction visualAction in _visualActions) {
				visualAction.Unmask ();
			}
		}

		public void Update ()
		{
			// Check for another iteration of fixed duration
			if (_hasFixedDuration && _lockState && Time.time - _timeFixed > _fixedDuration) {
				_lockState = false;
				Layer.LockedState = false;
			}
			else if (_hasFixedDuration && !_lockState) {
				_lockState = true;
				_timeFixed = Time.time;
				Layer.LockedState = true;
			}

			// Deactive Logic Actions
			foreach (ILogicAction il in _logicActions) {
				il.Update ();
			}

			// Deactive Visual Actions
			foreach (IVisualAction visualAction in _visualActions) {
				visualAction.CustomUpdate ();
			}
		}

		public void Awake ()
		{
		}

		public void Start ()
		{
			foreach (ILogicAction il in _logicActions) {
				il.Start ();
				il.IPerceptionAction.CustomStart ();
			}
			foreach (IVisualAction iv in _visualActions) {
				iv.CustomStart ();
			}
		}

		#endregion // PUBLIC_METHODS



		#region PRIVATE_METHODS


		#endregion // PRIVATE_METHODS
	}
}