using UnityEngine;

public class BossStateAnimation : MonoBehaviour
{
    Animator animator;
    AudioManager audioManager;

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

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

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
                audioManager.BossSFX(audioManager.BossAttack1);
                animator.SetTrigger("attack1");
                break;

            case BossState.attack2:
                audioManager.BossSFX(audioManager.BossAttack2);
                animator.SetTrigger("attack2");
                break;

            case BossState.attack3:
                audioManager.BossSFX(audioManager.BossAttack3);
                animator.SetTrigger("attack3");
                break;

            case BossState.jumpAttack:
                animator.SetTrigger("jumpAttack");
                break;

            case BossState.attack4:
                audioManager.BossSFX(audioManager.BossAttack4);
                animator.SetTrigger("attack4");
                break;

            case BossState.Teleport:
                audioManager.BossSFX(audioManager.BossTeleport);
                animator.SetTrigger("teleport");
                break;

            case BossState.Dead:
                audioManager.BossSFX(audioManager.BossDead);
                animator.SetTrigger("Die");
                break;
        }
    }
}