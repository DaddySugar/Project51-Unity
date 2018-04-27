﻿	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.AI;
	using UnityEngine.Networking;
	using Random = UnityEngine.Random;

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
		private Vector3 pos1;
		private Vector3 pos2;

		
		
		void Start ()
		{
			agent = GetComponent<NavMeshAgent>();
			anim = GetComponent<Animator>();
			AlienPosition = GetComponent<Transform>();
			raycastLayer = 2 << LayerMask.NameToLayer("LocalPlayer");
		
		}
		
		void Update () 
		{
			
			agent.destination = targetTransform.transform.position;
			if (NetworkManager.singleton.client.connection.playerControllers.Count == 1)
			{
				agent.destination =  NetworkManager.singleton.client.connection.playerControllers[0].gameObject.transform.position;

			}
			else if (NetworkManager.singleton.client.connection.playerControllers.Count == 1)
			{
				pos1 = NetworkManager.singleton.client.connection.playerControllers[0].gameObject.transform.position;
				pos2 = NetworkManager.singleton.client.connection.playerControllers[1].gameObject.transform.position;

				if ((AlienPosition.position - pos1).magnitude < (AlienPosition.position - pos2).magnitude)
				{
					agent.destination = pos1;
				}
				else
				{
					agent.destination = pos2;
				}
			}

			if ((AlienPosition.position - agent.destination).magnitude < 3)
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