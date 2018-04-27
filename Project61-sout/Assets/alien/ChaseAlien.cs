	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.AI;
	using UnityEngine.Networking;
	
	public class ChaseAlien : NetworkBehaviour	
	{
	
		//public GameObject player;
		public Transform targetTransform; //the enemy's target
		private NavMeshAgent agent;
		private Animator anim;
		private Transform AlienPosition;
		private float radius = 100;
		private LayerMask raycastLayer;
		
		
		void Start ()
		{
			agent = GetComponent<NavMeshAgent>();
			anim = GetComponent<Animator>();
			AlienPosition = GetComponent<Transform>();
			raycastLayer = 2 << LayerMask.NameToLayer("LocalPlayer");
		}
		
		void Update () 
		{
			SearchForTarget();
			/*
			agent.destination = player.transform.position;
			if ((AlienPosition.position - player.transform.position).magnitude < 3)
			{
				anim.SetBool("isAttacking", true);
			}
			else
			{
				anim.SetBool("isAttacking", false);
			}*/
	
			
		}
	
		void SearchForTarget()
		{
			if(!isServer)	
				return;
	
			if (targetTransform == null)
			{
				Collider[] hitColliders = Physics.OverlapSphere(AlienPosition.position, radius, raycastLayer);
	
				if (hitColliders.Length > 0)
				{
					int randomint = Random.Range(0, hitColliders.Length);
					targetTransform = hitColliders[randomint].transform; 
				}
			}
			
		}
		
	}
