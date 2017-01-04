using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonFSM : MonoBehaviour {
    public enum SkeletonState {Idle, Walking, Attacking, BeingHit, Waiting, Dead }
    public SkeletonState state = SkeletonState.Walking;
    public float attackCooldownTime = 1f;
    private float attackCooldownTimeOut = 0f;
    public float despawnTime = 2f;
    private float despawnTimeOut = 0f;
    private SkeletonController myController;
    private Animator myAnimator;
    // Use this for initialization
    void Start () {
        myController = GetComponent<SkeletonController>();
        myAnimator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        switch (state)
        {
            case SkeletonState.Idle: break;
            case SkeletonState.Walking: UpdateWalking(); break;
            case SkeletonState.Attacking: UpdateAttacking(); break;
            case SkeletonState.Waiting: UpdateWaiting(); break;
            case SkeletonState.BeingHit: UpdateBeingHit(); break;
            case SkeletonState.Dead: UpdateBeingDead(); break;
        }
	}

    private void TransitionToWalking()
    {
        myController.FinishRigidMotionAnimation();
        state = SkeletonState.Walking;
    }

    private void UpdateWalking()
    {
        myController.UpdateDestination();
        myController.UpdateAnimator();

        if (myController.IsAtDestination())
        {
            TransitionToAttacking();
        }
    }

    private void TransitionToAttacking()
    {
        state = SkeletonState.Attacking;
        myController.StartRigidMotionAnimation();
        myController.ExecuteAttack();
    }

    private void UpdateAttacking()
    {
        //if (myAnimator.IsInTransition(0) && myAnimator.GetNextAnimatorStateInfo(0).IsName("Idle"))
        //{
        if(!myAnimator.IsInTransition(0) && myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            TransitionToWalking();
        }
    }

    private void TransitionToWaiting()
    {
        attackCooldownTimeOut = 0f;
        state = SkeletonState.Waiting;
    }

    private void UpdateWaiting()
    {
        attackCooldownTimeOut += Time.deltaTime;
        if (attackCooldownTimeOut >= attackCooldownTime)
        {
            TransitionToAttacking();
        }
    }

    private void TransitionToBingHit()
    {
        myController.StartRigidMotionAnimation();
        state = SkeletonState.BeingHit;
    }

    private void UpdateBeingHit()
    {
        if (!myAnimator.IsInTransition(0) && myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            TransitionToWalking();
        }
    }

    private void TransitionToBeingDead()
    {
        state = SkeletonState.Dead;
        despawnTimeOut = 0;
    }

    private void UpdateBeingDead()
    {
        despawnTimeOut += Time.deltaTime;
        if (despawnTimeOut >= despawnTime)
        {
            Destroy(this.gameObject);
        }
    }

    public void OnHit()
    {
        TransitionToBingHit();
    }

    public void OnDeath()
    {
        TransitionToBeingDead();
    }
}
