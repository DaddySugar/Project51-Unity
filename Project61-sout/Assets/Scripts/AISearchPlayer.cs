using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
//using System.Collections;
using UnityEngine.AI;
using UnityEngine.Networking;

public class AISearchPlayer : NetworkBehaviour {

    private NavMeshAgent agent;
    private Transform myTransform;
    public Transform targetTransform;
    private LayerMask raycastLayer;
    private float radius = 100;

    
    void Start () 
    {
        agent = GetComponent<NavMeshAgent>();
        myTransform = transform;
        raycastLayer = 1<<LayerMask.NameToLayer("Player");

        if(isServer)
        {
            StartCoroutine(DoCheck());
        }
    }

    
    void FixedUpdate () 
    {
        //SearchForTarget();
        //MoveToTarget();
    }

    void SearchForTarget()
    {
        if(!isServer)
        {
            return;
        }

        if(targetTransform == null)
        {
            Collider[] hitColliders = Physics.OverlapSphere(myTransform.position, radius, raycastLayer);

            if(hitColliders.Length>0)
            {
                int randomint = Random.Range(0, hitColliders.Length);
                targetTransform = hitColliders[randomint].transform;
            }
        }

        if(targetTransform != null && targetTransform.GetComponent<BoxCollider>().enabled == false)
        {
            targetTransform = null;
        }
    }

    void MoveToTarget()
    {
        if(targetTransform != null && isServer)
        {
            SetNavDestination(targetTransform);
            Debug.Log("1");
        }
    }

    void SetNavDestination(Transform dest)
    {
        agent.SetDestination(dest.position);
        Debug.Log(agent.name + dest.name    );
    }

    IEnumerator DoCheck()
    {
        for(;;)
        {
            SearchForTarget();
            MoveToTarget();
            yield return new WaitForSeconds(0.2f);
        }
    }
}
