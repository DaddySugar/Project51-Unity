	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.AI;
	using UnityEngine.Networking;
	
	public class ChaseAlien : NetworkBehaviour
	{
		private GameObject goal; 
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
			
			//agent.destination =  NetworkManager.singleton.client.connection.playerControllers[0].gameObject.transform.position;
		}
		
		void Update () 
		{
			//SearchForTarget();
			
			agent.destination = targetTransform.transform.position;
			if (NetworkManager.singleton.client.connection.playerControllers.Count != 0)
			{
				agent.destination =  NetworkManager.singleton.client.connection.playerControllers[0].gameObject.transform.position;

			}
			Debug.Log(NetworkManager.singleton.client.connection.playerControllers.Count);

			if ((AlienPosition.position - targetTransform.transform.position).magnitude < 3)
			{
				anim.SetBool("isAttacking", true);
			}
			else
			{
				anim.SetBool("isAttacking", false);
			}
	
			
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
