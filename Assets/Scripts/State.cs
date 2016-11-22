using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

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
		private HashSet<string> _animations = new HashSet<string> ();
		private Dictionary<string, bool> _resetAnimation = new Dictionary<string, bool> ();
		// Animations actived in this state
		private HashSet<Component> _components = new HashSet<Component> ();
		private Dictionary<Component, bool> _resetComponent = new Dictionary<Component, bool> ();

		#endregion // PRIVATE_MEMBER_VARIABLES



		#region GETTERS_AND_SETTERS_METHODS

		/// The Id name that identifies the Agent.
		public string Name { get { return _name; } }

		/// Animatión names actived in this state.
		// public HashSet<string> Animations { get { return _animations; } }

		/// Agent that contains this state.
		public Agent Agent { get; set; }

		#endregion // GETTERS_AND_SETTERS_METHODS



		#region UNTIY_MONOBEHAVIOUR_METHODS


		#endregion // UNTIY_MONOBEHAVIOUR_METHODS



		#region PUBLIC_METHODS

		/** \brief Creates an Agent object.
		 * 
		 * @param agentName Id name for the agent.
		 */
		public State (string stateName)
		{
			_name = stateName;
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
			// Check if the target state belong to the same agent than this.
			if (!targetState.Agent.Equals (this.Agent)) {
				Debug.LogWarningFormat ("The state {0} does not belong to the agent {1}.", targetState.Name, this.Agent.Name);
				return null;
			}
				
			Transition newTransition = new Transition (targetState, trigger, priority);
			// Index the transition with this state.
			newTransition.State = this;
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
			if (!transition.TargetState.Agent.Equals (this.Agent)) {
				Debug.LogWarningFormat ("The state {0} does not belong to the agent {1}.", transition.TargetState.Name, this.Agent.Name);
				return false;
			}
			// Check if this transition alreary exists.
			if (_transitions.Contains (transition)) {
				Debug.LogWarningFormat ("This transition {0}-{1} does already exist.", this.Name, transition.TargetState.Name);
				return false;
			}
			transition.State = this;
			_transitions.Add (transition);
			return true;
		}


		/** @brief Add a animation name to the actived animations in this State.
		 * 
		 * @param animationName The name of the animation to be actived.
		 * @param reset Force to turn down and turn on an animation.
		 * @return Return true if the animation has been added.
		 */
		public bool AddAnimation (string animationName, bool reset = false)
		{
			if (_animations.Contains (animationName)) {
				Debug.LogWarningFormat ("The Agent {0} already contain the animation {1}.", this.Agent.Name, animationName);
				return false;
			}
			_animations.Add (animationName);
			_resetAnimation [animationName] = reset;
			Agent.AnimationNames.Add (animationName);
			return true;
		}


		/** @brief Add a Component(Script) to be enabled in this State.
		 * 
		 * @param typeComponent Script component to be enabled.
		 * @param reset Force to reenable a script if it where
		 * @return Return true if the animation has been added.
		 */
		public Component AddComponent (System.Type typeComponent, bool reset = false)
		{
			// Check if the component already exists
			if (Agent.Character.GetComponent (typeComponent) != null) {
				Debug.LogWarningFormat ("The Agent {0} already has the component {1}.", Agent.Name, typeComponent.Name);
				return null;
			}

			// Adding the component
			Component newComponent = Agent.Character.AddComponent (typeComponent);

			// Check if the component is MonoBehaviour
			if (!(newComponent is MonoBehaviour)) {
				Debug.LogWarningFormat ("The componente {0} is not MonoBehaviour type.", typeComponent.Name);
				// Removing the component if it is no monobehaviour.
				GameObject.Destroy (newComponent);
				return null;
			}

			_components.Add (newComponent);
			_resetComponent [newComponent] = reset;
			Agent.Components.Add (newComponent);
			return newComponent;
		}


		/** @brief Returns the active transition of the State.
		 * 
		 * @return The active transition of the State, null if there is not actived transtion.
		 */
		public Transition GetActivedTransition ()
		{
			foreach (Transition transition in _transitions) {
				if (transition.Actived ())
					return transition;
			}
			return null;
		}

		//! Activate the State.
		public void ActivateState ()
		{
			// Active animations
			foreach (string animationName in Agent.AnimationNames) {
				Agent.Animator.SetBool (animationName, false);
			}
			foreach (string animationName in _animations) {
				Agent.Animator.SetBool (animationName, true);
			}

			// Active Components
			foreach (Component component in Agent.Components) {
				if (!_components.Contains (component)) {
					((MonoBehaviour)component).enabled = false;
				}
				else {
					// If this component is found as reset 
					// it is reseted to active de onEnable event.
					if (_resetComponent [component])
						((MonoBehaviour)component).enabled = false;
					((MonoBehaviour)component).enabled = true;
				}

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
			// Deactive animations
			foreach (string animationName in _animations) {
				Agent.Animator.SetBool (animationName, true);
			}

			// Deactive Components
			foreach (Component component in _components) {
				((MonoBehaviour)component).enabled = false;
			}
		}

		#endregion // PUBLIC_METHODS



		#region PRIVATE_METHODS


		#endregion // PRIVATE_METHODS
	}
}