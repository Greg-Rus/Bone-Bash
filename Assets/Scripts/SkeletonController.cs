using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class SkeletonController : MonoBehaviour {
    private UnityEngine.AI.NavMeshAgent navAgent;
    private Animator animator;
    public Transform target;
    public bool shatter = false;
    public bool ragdol = false;
    // Use this for initialization
    void Start () {
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
        navAgent.SetDestination(target.position);       
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 localVelocity = transform.InverseTransformDirection(navAgent.velocity);
        animator.SetFloat("VelX", localVelocity.x);
        animator.SetFloat("VelZ", localVelocity.z);
        if (shatter)
        {
            shatter = false;
            ShatterSkeleton();
        }
        if (ragdol)
        {
            ragdol = false;
            RagdolSkeleton();
        }
    }

    private void ShatterSkeleton()
    {
        animator.Stop();
        navAgent.Stop();
        Rigidbody[] rigidBodies = GetComponentsInChildren<Rigidbody>();
        SetKinematicInChildren(false);
        SetJointBreakForce(0);
    }

    private void RagdolSkeleton()
    {
        animator.Stop();
        navAgent.Stop();
        SetKinematicInChildren(false);
    }

    private void SetKinematicInChildren(bool isKinematic)
    {
        Rigidbody[] rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidbody in rigidBodies)
        {
            rigidbody.isKinematic = isKinematic;
        }
    }

    private void SetJointBreakForce(float breakForce)
    {
        CharacterJoint[] joints = GetComponentsInChildren<CharacterJoint>();
        foreach(var joint in joints)
        {
            joint.breakForce = breakForce;
        }
    }
}
