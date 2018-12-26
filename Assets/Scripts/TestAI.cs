using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestAI : MonoBehaviour {

    public Transform target;

    private NavMeshAgent agent;

	void Awake () {
        agent = GetComponent<NavMeshAgent>();
	}
	
	void Update () {
        agent.destination = target.position;
    }
}
