using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgentManagerNamespace
{
	public class PerceptionAbstract : MonoBehaviour, IPerceptionAction
	{
		#region PUBLIC_MEMBER_VARIABLES


		#endregion // PUBLIC_MEMBER_VARIABLES



		#region PRIVATE_MEMBER_VARIABLES

		private GameObject _character;

		#endregion // PRIVATE_MEMBER_VARIABLES



		#region GETTERS_AND_SETTERS_METHODS

		protected GameObject Character { get { return _character; } }

		public Vector3 Position { get { return Character.transform.position; } }

		public Quaternion Rotation { get { return Character.transform.rotation; } }

		public Vector3 Forward { get { return Character.transform.forward; } }

		public Vector3 Up { get { return Character.transform.up; } }

		public Vector3 Right { get { return Character.transform.right; } }

		[System.Obsolete ("Use Perception methods", true)]
		public new GameObject gameObject { get { return this.gameObject; } }

		#endregion // GETTERS_AND_SETTERS_METHODS



		#region PUBLIC_METHODS

		public void Initialize (GameObject character)
		{
			_character = character;
		}

		public void CustomAwake ()
		{
			OnAwake ();
		}

		public void CustomStart ()
		{
			OnStart ();
		}

		#endregion // PUBLIC_METHODS



		#region PRIVATE_METHODS

		protected virtual void OnAwake ()
		{
		}

		protected virtual void OnStart ()
		{
		}

		#endregion // PRIVATE_METHODS
	}
}