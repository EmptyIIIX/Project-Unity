using UnityEngine;

public class BossStateAnimation : MonoBehaviour
{
    Animator animator;

    public enum BossState
    {
        Idle,
        attack1,
        attack2,
        attack3,
        jumpAttack,
        attack4,
        Teleport,
        Dead
    }

    public BossState currentState;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentState = BossState.Idle;
    }

    public void ChangeState(BossState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case BossState.Idle:
                break;

            case BossState.attack1:
                animator.SetTrigger("attack1");
                break;

            case BossState.attack2:
                animator.SetTrigger("attack2");
                break;

            case BossState.attack3:
                animator.SetTrigger("attack3");
                break;

            case BossState.jumpAttack:
                animator.SetTrigger("jumpAttack");
                break;

            case BossState.attack4:
                animator.SetTrigger("attack4");
                break;

            case BossState.Teleport:
                animator.SetTrigger("teleport");
                break;

            case BossState.Dead:
                animator.SetTrigger("Die");
                break;
        }
    }
}