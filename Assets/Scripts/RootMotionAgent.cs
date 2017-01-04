using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionAgent : MonoBehaviour {
    Animator animator;
    UnityEngine.AI.NavMeshAgent agent;
    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = true;
    }
	
	// Update is called once per frame
	void Update () {
        //agent.nextPosition = transform.position + anim.deltaPosition;
        float dx = Vector3.Dot(transform.right, agent.desiredVelocity);
        float dz = Vector3.Dot(transform.forward, agent.desiredVelocity);
        //Vector2 deltaPosition = new Vector2(dx, dy);
        //animator.SetFloat("VelX", agent.desiredVelocity.x);
        //animator.SetFloat("VelZ", agent.desiredVelocity.z);

        animator.SetFloat("VelX", dx);
        animator.SetFloat("VelZ", dz);
    }

    void OnAnimatorMove()
    {
        //agent.velocity = animator.deltaPosition / Time.deltaTime;
        agent.nextPosition = transform.position + animator.deltaPosition;
        transform.position = animator.rootPosition;
        
        //transform.rotation = animator.rootRotation;
    }
}
