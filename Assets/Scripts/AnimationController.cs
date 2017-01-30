using UnityEngine;
using System.Collections;
using System;

namespace AgentManagerNamespace
{
	public class AnimationController
	{
		#region PUBLIC_MEMBER_VARIABLES


		#endregion // PUBLIC_MEMBER_VARIABLES



		#region PRIVATE_MEMBER_VARIABLES

		Animator _animator;

		#endregion // PRIVATE_MEMBER_VARIABLES



		#region GETTERS_AND_SETTERS_METHODS



		#endregion // GETTERS_AND_SETTERS_METHODS



		#region PUBLIC_METHODS

		public AnimationController(GameObject character) {
			if (character != null)
				_animator = character.GetComponent<Animator>() as Animator;
		}

		public bool ActivateAnimation(string animation) {
			if (_animator == null)
				return false;
			_animator.SetBool (animation, true);
			return true;
		}

		public bool DeactivateAnimation(string animation) {
			if (_animator == null)
				return false;
			_animator.SetBool (animation, false);
			return true;
		}

		#endregion // PUBLIC_METHODS



		#region PRIVATE_METHODS


		#endregion // PRIVATE_METHODS
	}
}