using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class BossDecision : MonoBehaviour
{
    public BossStateAnimation stateAnimation;
    public BossHealth bossHealth;
    Animator animator;
    public CapsuleCollider2D capsuleCollider;

    public float attackDelay = 2f;
    private bool isAttacking;
    private bool isActioningAttack;
    private float FaceDir;
    private bool isPauseAnim;
    Vector3 ls;
    Vector3 centerPoint;

    #region For combat
    public Rigidbody2D BossRB;
    public Transform BossTransform;
    public Transform PlayerTransform;
    public Transform BehindPlayerTransform;

    public Transform leftTeleport;
    public Transform rightTeleport;
    public Transform centerPointAttack;

    [Header("Skill1")]
    public float maxCooldownSkill1 = 20f;
    private float CooldownSkill1;
    public float dashSpeed = 80f;

    [Header("Skill2")]
    public float maxCooldownSkill2 = 10f;
    private float CooldownSkill2;

    [Header("Skill3")]
    public GameObject bulletBossPrefab;
    public LayerMask groundLayer;
    public Vector2 jumpForce = new Vector2(8f, 8f);
    public float bulletSpeed = 10f;
    public float maxCooldownSkill3 = 12f;
    private float CooldownSkill3;

    [Header("Skill4")]
    public float chaseSpeed = 5f;
    public bool isChasing;
    public int maxAmountThunder = 6;
    public int countThunder;
    public float freqThunder = 3f;
    public float maxCooldownSkill4 = 30f;
    private float CooldownSkill4;

    #endregion

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(AILoop());
        isActioningAttack = false;
        isChasing = false;
        centerPoint = (rightTeleport.position - leftTeleport.position) / 2;
    }

    void Update()
    {
        if(isActioningAttack == false)
        {

            FaceDir = Mathf.Sign(PlayerTransform.position.x - BossTransform.position.x);
            ls = BossTransform.localScale;
            Flip();

            CoolingDown();
        }

        if (isPauseAnim)
        {
            if (IsGrounded())
            {
                animator.speed = 1f;
                isPauseAnim = false;
            }
        }

        if (isChasing)
        {
            ChasePlayer();
        }
    }

    bool IsGrounded()
    {
        return Physics2D.IsTouchingLayers(capsuleCollider, groundLayer);
    }

    IEnumerator AILoop()
    {
        while (!bossHealth.isDie)
        {
            yield return new WaitForSeconds(attackDelay);

            if (!isAttacking)
            {
                ChooseAttack();
            }
        }
    }
    void Flip()
    {
        if(FaceDir < 0 )
        {
            ls.x = 1;
            BossTransform.localScale = ls;
        }
        else
        {
            ls.x = -1;
            BossTransform.localScale = ls;
        }
    }

    void CoolingDown()
    {
        if(CooldownSkill1 <= maxCooldownSkill1 && CooldownSkill1 > 0) CooldownSkill1 -= Time.deltaTime;

        if(CooldownSkill2 <= maxCooldownSkill2 && CooldownSkill2 > 0) CooldownSkill2 -= Time.deltaTime;

        if(CooldownSkill3 <= maxCooldownSkill3 && CooldownSkill3 > 0) CooldownSkill3 -= Time.deltaTime;

        if(CooldownSkill4 <= maxCooldownSkill4 && CooldownSkill4 > 0) CooldownSkill4 -= Time.deltaTime;

    }

    void ChooseAttack()
    {
        int rand = Random.Range(0, 4);

        switch (rand)
        {
            case 0:
                StartCoroutine(Skill1());
                break;

            case 1:
                StartCoroutine(Skill2());
                break;

            case 2:
                StartCoroutine(Skill3());
                break;

            case 3:
                StartCoroutine(Skill4());
                break;
        }
    }

    #region Skill action
    IEnumerator Skill1()
    {
        if(CooldownSkill1 <=  0)
        {
            isAttacking = true;

            bool isLeft;
            Vector3 targetPos;

            int rand = Random.Range(0, 2);
            if (rand == 0)
            {
                targetPos = leftTeleport.position;
                isLeft = true;
            }
            else
            {
                targetPos = rightTeleport.position;
                isLeft = false;
            }

            stateAnimation.ChangeState(BossStateAnimation.BossState.Teleport);

            yield return null;

            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).IsName("boss_teleport") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f
            );

            teleport(targetPos);

            yield return null;

            stateAnimation.ChangeState(BossStateAnimation.BossState.Idle);

            yield return new WaitForSeconds(1.5f);

            targetPos = isLeft ? rightTeleport.position : leftTeleport.position;

            stateAnimation.ChangeState(BossStateAnimation.BossState.attack1);

            yield return null;

            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).IsName("boss_attack1") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f
            );

            BossRB.linearVelocity = new Vector2(0f, BossRB.linearVelocityY);
            isAttacking=false;
            isActioningAttack=false;
            CooldownSkill1 = maxCooldownSkill1;
        }
        else
        {
            yield return null;
        }
    }

    IEnumerator Skill2()
    {
        if(CooldownSkill2 <= 0 && isRangeSkill2())
        {
            isAttacking = true;

            Vector3 targetPos = BehindPlayerTransform.position;
            stateAnimation.ChangeState(BossStateAnimation.BossState.Teleport);

            yield return null;

            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).IsName("boss_teleport") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f
            );

            teleport(targetPos);

            yield return null;

            stateAnimation.ChangeState(BossStateAnimation.BossState.Idle);

            yield return new WaitForSeconds(1);

            stateAnimation.ChangeState(BossStateAnimation.BossState.attack2);

            yield return null;

            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).IsName("boss_attack2") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f
            );

            isAttacking = false;
            isActioningAttack = false;
            CooldownSkill2 = maxCooldownSkill2;
        }
        else
        {
            yield return null;
        }
        
    }

    IEnumerator Skill3()
    {
        if(CooldownSkill3 <= 0)
        {
            isAttacking = true;

            Vector3 targetPos = PlayerTransform.position.x > centerPoint.x ? leftTeleport.position : rightTeleport.position;
            stateAnimation.ChangeState(BossStateAnimation.BossState.Teleport);

            yield return null;

            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).IsName("boss_teleport") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f
            );

            teleport(targetPos);

            yield return null;

            stateAnimation.ChangeState(BossStateAnimation.BossState.Idle);

            yield return new WaitForSeconds(0.5f);

            stateAnimation.ChangeState(BossStateAnimation.BossState.attack3);

            yield return null;

            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).IsName("boss_attack3") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f
            );

            yield return null;

            stateAnimation.ChangeState(BossStateAnimation.BossState.jumpAttack);

            yield return null;

            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).IsName("boss_jumpAttack") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f
            );

            isAttacking = false;
            isActioningAttack = false;
            CooldownSkill3 = maxCooldownSkill3;
        }
        else
        {
            yield return null;
        }
    }

    IEnumerator Skill4()
    {
        if(CooldownSkill4 <= 0)
        {
            isAttacking = true;

            float currentChaseSpeed = chaseSpeed;
            BossRB.gravityScale = 0f;
            Vector3 targetPos = centerPointAttack.position;
            stateAnimation.ChangeState(BossStateAnimation.BossState.Teleport);

            yield return null;

            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).IsName("boss_teleport") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f
            );

            teleport(targetPos);
            animator.SetBool("isThunderMode", true);

            yield return null;
            isChasing = true;

            while(countThunder < maxAmountThunder)
            {
                yield return new WaitForSeconds(freqThunder - 1);

                chaseSpeed = 0f;

                yield return new WaitForSeconds(freqThunder - 2);

                stateAnimation.ChangeState(BossStateAnimation.BossState.attack4);

                yield return null;

                yield return new WaitUntil(() =>
                    animator.GetCurrentAnimatorStateInfo(0).IsName("boss_attack4") &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f
                );

                chaseSpeed = currentChaseSpeed;
                countThunder++;
            }

            isChasing = false;
            Attack4();
            yield return new WaitForSeconds(freqThunder);

            stateAnimation.ChangeState(BossStateAnimation.BossState.attack4);

            yield return null;

            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).IsName("boss_attack4") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f
            );

            BossRB.gravityScale = 1f;

            yield return new WaitForSeconds(freqThunder - 2);

            isAttacking = false;
            CooldownSkill4 = maxCooldownSkill4;
            animator.SetBool("isThunderMode", false);
            countThunder = 0;
        }
        else
        {
            yield return null;
        }
    }

    #endregion

    private bool isRangeSkill2()
    {
        return PlayerTransform.position.x > leftTeleport.position.x && PlayerTransform.position.x < rightTeleport.position.x;
    }

    #region Combat Method

    public void Attack1()
    {
        isActioningAttack = true;
        BossRB.linearVelocity = new Vector2(FaceDir * dashSpeed, BossRB.linearVelocityY);
        Debug.Log("attack1 is active");
    }
    public void Attack2()
    {
        isActioningAttack = true;
        Debug.Log("attack2 is active");
    }
    public void Attack3()
    {
        float shootDirectionX = Mathf.Sign(PlayerTransform.position.x - BossTransform.position.x);
        isActioningAttack = true;

        GameObject bullet = Instantiate(bulletBossPrefab, new Vector3(BossTransform.position.x, PlayerTransform.position.y, BossTransform.position.z), Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(shootDirectionX, 0) * bulletSpeed;
        Debug.Log("attack3 is active");
    }
    public void pauseAnimation()
    {
        isPauseAnim = true;
        animator.speed = 0;
    }
    public void JumpAttack()
    {
        isActioningAttack = true;
        BossRB.linearVelocity = new Vector2(FaceDir * jumpForce.x, jumpForce.y);

        if(Physics2D.Raycast(BossTransform.position, Vector2.down, 1f, groundLayer))
        {
            animator.speed = 1f;
        }
        
        Debug.Log("jumpattack is active");
    }
    public void Attack4()
    {
        BossTransform.position = centerPointAttack.position;
        Debug.Log("attack4 is active");
    }
    public void ResetSkill4()
    {

    }
    public void ChasePlayer()
    {
        BossRB.linearVelocityX = FaceDir * chaseSpeed;
    }
    public void teleport(Vector2 targetPoint)
    {
        BossTransform.position = targetPoint;
    }
    #endregion
}