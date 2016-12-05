using UnityEngine;
using System.Collections;
using System;

namespace AgentManagerNamespace
{
	/** \brief Trigger that determinates when to transitate
	 * 
	 * @return Returns true if the transition can be performed.
	 */
	public delegate bool TransitionTrigger ();

	public class Transition : IComparable<Transition>
	{
		#region PUBLIC_MEMBER_VARIABLES


		#endregion // PUBLIC_MEMBER_VARIABLES



		#region PRIVATE_MEMBER_VARIABLES

		// The state to transitate to.
		private State _targetState;
		// The trigger that determinates when to transitate.
		private TransitionTrigger _trigger;
		// Priority of the transition
		private int _priority;

		#endregion // PRIVATE_MEMBER_VARIABLES



		#region GETTERS_AND_SETTERS_METHODS

		/// A reference to the target state.
		public State TargetState { get { return _targetState; } }

		#endregion // GETTERS_AND_SETTERS_METHODS



		#region UNTIY_MONOBEHAVIOUR_METHODS


		#endregion // UNTIY_MONOBEHAVIOUR_METHODS



		#region PUBLIC_METHODS

		/** \brief Creates a Trasition object.
		 * 
		 * @param targetState The state which this transition transitates to.
		 * @param trigger Trigger that determinates when to transitate.
		 * @param priotity The prority to be actived.
		 */
		public Transition (State targetState, TransitionTrigger trigger, int priority = 0)
		{
			_targetState = targetState;
			_trigger = trigger;
			_priority = priority;
		}

		/** \brief Compares Transitions.
		 * 
		 * It compares transtions if descending order.
		 * @param compareTransition Transition to compare with.
		 */
		public int CompareTo (Transition compareTransition)
		{
			// A null value means that this object is greater.
			if (compareTransition == null)
				return 1;
			else
				return compareTransition._priority.CompareTo (this._priority);
				//return this._priority.CompareTo (compareTransition._priority);
		}

		/** \brief Returns if the transition can be performed.
		 * 
		 * @return True if the transition can be performed.
		 */
		public bool Actived ()
		{
			return _trigger ();
		}

		#endregion // PUBLIC_METHODS



		#region PRIVATE_METHODS


		#endregion // PRIVATE_METHODS
	}
}