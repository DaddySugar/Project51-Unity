using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationWeapon : MonoBehaviour
{

	private Animation _animation;
	void Start ()
	{
		_animation = GetComponent<Animation>();
	}
	
	void Update ()
	{
		if (Input.GetKey(KeyCode.Mouse0))
		{
			_animation.Play("fire");

		}
	}

	
}
