using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseAlien : MonoBehaviour
{

	public GameObject player;
	private NavMeshAgent agent;
	private Animator anim;
	private Transform AlienPosition;
	void Start ()
	{
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		AlienPosition = GetComponent<Transform>();
	}
	
	void Update () 
	{
		agent.destination = player.transform.position;
		if ((AlienPosition.position - player.transform.position).magnitude < 3)
		{
			anim.SetBool("isAttacking", true);
		}
		else
		{
			anim.SetBool("isAttacking", false);
		}

	}
}
