using UnityEngine;
using System.Collections.Generic;

namespace Summoner.Animation {
	[RequireComponent( typeof(Animator) )]
	public class AnimParamController : MonoBehaviour {
		[SerializeField] private int parameter = 0;
		private Animator _anim;
		private Animator anim {
			get {
				if ( _anim == null ) {
					_anim = GetComponent<Animator>();
				}

				return _anim;
			}
		}

		public void SetBool( bool value ) {
			anim.SetBool( parameter, value );
		}

		public void SetFloat( float value ) {
			anim.SetFloat( parameter, value );
		}

		public void SetInteger( int value ) {
			anim.SetInteger( parameter, value );
		}

		public void SetTrigger() {
			anim.SetTrigger( parameter );
		}

		public void ResetTrigger() {
			anim.ResetTrigger( parameter );
		}
	}
}