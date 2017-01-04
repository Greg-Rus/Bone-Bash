using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SkeletonFSM))]
public class SkeletonController : MonoBehaviour {
    private UnityEngine.AI.NavMeshAgent navAgent;
    private Animator animator;
    private SkeletonFSM myFSM;
    public Transform target;
    public Transform lookTarget;
    public int HP = 1;
    public float punchingDistance = 1;
    public bool shatter = false;
    public bool ragdol = false;


    private Vector3 lastPosition;
    private Dictionary<string, int> hitTriggers;
    private Vector3 smoothDeltaPosition = Vector2.zero;
    public bool RootMotionOn = false;

    public float invulnerabilityTime = 0.2f;
    // Use this for initialization
    void Start () {
        lastPosition = transform.position;
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navAgent.updateRotation = false;
        animator = GetComponent<Animator>();
        myFSM = GetComponent<SkeletonFSM>();
        hitTriggers = new Dictionary<string, int>();
    }
	
	// Update is called once per frame
	void Update () {

        //UpdateDestination();
        //UpdateAnimator();

        if (!RootMotionOn)
        {
            FaceTarget();
        }

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

    public void UpdateDestination()
    {
        Vector3 destination = transform.position - target.position;
        destination = destination.normalized * punchingDistance;
        navAgent.SetDestination(destination);
    }

    public void UpdateAnimator()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(navAgent.desiredVelocity);
        //float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        //smoothDeltaPosition = Vector3.Lerp(smoothDeltaPosition, localVelocity, smooth);
        animator.SetFloat("VelX", localVelocity.x);
        animator.SetFloat("VelZ", localVelocity.z);
    }

    private void FaceTarget()
    {
        Vector3 targetPostition = new Vector3(lookTarget.position.x,
                                        this.transform.position.y,
                                        lookTarget.position.z);
        this.transform.LookAt(targetPostition);
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

    public void HitZoneEnter(string TriggerName, string hitSourceName)
    {
        if (!hitTriggers.ContainsKey(hitSourceName))
        {
            hitTriggers.Add(hitSourceName, 1);
            --HP;
            if (HP == 0)
            {
                //ShatterSkeleton();
                RagdolSkeleton();
                myFSM.OnDeath();
            }
            else
            {
                myFSM.OnHit();
                animator.SetTrigger(TriggerName);
            }
        }
        else
        {
            ++hitTriggers[hitSourceName];
        }
        //Debug.Log(hitSourceName);    
    }

    public void HitZoneExit(string TriggerName, string hitSourceName)
    {
        --hitTriggers[hitSourceName];
        //Debug.Log(hitSourceName + " : " + hitTriggers[hitSourceName]);
        if (hitTriggers[hitSourceName] == 0)
        {
            hitTriggers.Remove(hitSourceName);
        }
    }

    public void StartRigidMotionAnimation()
    {
        if (!RootMotionOn)
        {
            RootMotionOn = true;
            navAgent.Stop();
            animator.SetFloat("VelX", 0f);
            animator.SetFloat("VelZ", 0f);
            animator.applyRootMotion = true;
        }
        
    }
    public void FinishRigidMotionAnimation()
    {
        if (RootMotionOn)
        {
            RootMotionOn = false;
            //Debug.Log("RootMotion off");
            animator.applyRootMotion = false;
            //navAgent.nextPosition = transform.position;
            //navAgent.SetDestination(target.position);
            navAgent.Resume();
        }
    }

    public bool IsAtDestination()
    {
        bool atTraget = false;
        if (!navAgent.pathPending)
        {
            if (navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                if (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f)
                {
                    atTraget = true;
                }
            }
        }
        return atTraget;
    }

    public void ExecuteAttack()
    {
        string attack = PickAttack();
        //Debug.Log(attack);
        animator.SetTrigger(attack);
        //EditorApplication.isPaused = true;
    }

    public string PickAttack()
    {
        int attackNumber = UnityEngine.Random.Range(0, 3);
        switch (attackNumber)
        {
            case 0: return "StrLeft";
            case 1: return "StrRight";
            case 2: return "HookLeft";
            default: return "StrLeft";
        }
    }
}
