using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using System.Linq;
using System.Reflection;

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
		private HashSet<string> _animationNames = new HashSet<string> ();
		private Dictionary<string, bool> _resetAnimation = new Dictionary<string, bool> ();
		// Animations actived in this state
		private HashSet<System.Type> _componentTypes = new HashSet<System.Type> ();
		private Dictionary<System.Type, bool> _resetComponent = new Dictionary<System.Type, bool> ();

		private bool _activated = false;

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

		public List<System.Type> ComponentTypes { get { return _componentTypes.ToList (); } }

		#endregion // GETTERS_AND_SETTERS_METHODS



		#region UNTIY_MONOBEHAVIOUR_METHODS


		#endregion // UNTIY_MONOBEHAVIOUR_METHODS



		#region PUBLIC_METHODS

		/** \brief Creates an Agent object.
		 * 
		 * @param agentName Id name for the agent.
		 */
		public State (string stateName, int bitmask = int.MaxValue)
		{
			_name = stateName;
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
		public bool AddAnimation (string animationName, bool reset = false)
		{
			if (_animationNames.Contains (animationName)) {
				Debug.LogWarningFormat ("The State {0} already contain the animation {1}.", Name, animationName);
				return false;
			}
			_animationNames.Add (animationName);
			_resetAnimation [animationName] = reset;
	
			return true;
		}


		/** @brief Add a Component(Script) to be enabled in this State.
		 * 
		 * If the component is already created in the game object it returns it.
		 * @param typeComponent Script component to be enabled.
		 * @param reset Force to reenable a script if it where
		 * @return Return true if the animation has been added.
		 */
		public Component AddComponent (System.Type typeComponent, bool reset = false)
		{
			// Check if the component already exists
			if (_componentTypes.Contains (typeComponent)) {
				Debug.LogWarningFormat ("The State {0} already has the component {1}.", Name, typeComponent.Name);
				return null;
			}

			// Check if the component already exists
			Component newComponent = Layer.Agent.Character.GetComponent (typeComponent);
			// If not add it
			if (newComponent == null) {
				newComponent = Layer.Agent.Character.AddComponent (typeComponent);
				// Only initializing to false when it is new added
				(newComponent as MonoBehaviour).enabled = false;
			}

			// Check if the component is MonoBehaviour
			if (!(newComponent is MonoBehaviour)) {
				Debug.LogWarningFormat ("The componente {0} is not MonoBehaviour type.", typeComponent.Name);
				// Removing the component if it is no monobehaviour.
				GameObject.Destroy (newComponent);
				return null;
			}

			_componentTypes.Add (typeComponent);
			_resetComponent [typeComponent] = reset;

			return newComponent;
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
				foreach (string animationName in _animationNames) {
					if (_resetAnimation [animationName]) {
						Layer.Agent.ResetAnimationNames [animationName] = true;
					}
					Layer.Agent.AnimationNames [animationName]++;
				}

				// Active Components
				foreach (System.Type componentType in _componentTypes) {
					// If this component is found as reset 
					// it is reseted to active de onEnable event.
					// To do it we disable it an it will be enabled in the update period.
					if (_resetComponent [componentType]) {
						Layer.Agent.ResetComponentTypes [componentType] = true;
					}
					Layer.Agent.ComponentTypes [componentType]++;
				}

				// Control Mask
				int layerId = Layer.Id - 1;
				for (int i = 1 << Layer.Id - 1; i > 0; i >>= 1) {
					if ((i & ~Bitmask) > 0) {
						Layer auxLayer = Layer.Agent.FindLayer (layerId);
						if (auxLayer != null)
							auxLayer.Mask ();
					}
					layerId--;
				}

				_activated = true;
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
				foreach (string animationName in _animationNames) {
					Layer.Agent.AnimationNames [animationName]--;
				}

				// Deactive Components
				foreach (System.Type componentType in _componentTypes) {
					Layer.Agent.ComponentTypes [componentType]--;
				}

				// Control Mask
				int layerId = Layer.Id - 1;
				for (int i = 1 << Layer.Id - 1; i > 0; i >>= 1) {
					if ((i & ~Bitmask) > 0) {
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

		#endregion // PUBLIC_METHODS



		#region PRIVATE_METHODS


		#endregion // PRIVATE_METHODS
	}
}