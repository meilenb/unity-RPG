using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Chasing,
    Attacking,
    KnockBack // 你加了这个
}

public class Enemy_Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform player;
    private int facingDirection = 1;
    public Animator anim;
    private EnemyState enemyState;

    [Header("数值配置")]
    public float attackRange = 1.5f;
    public float chaseRange = 5f;
    public float attackCd = 1f;
    public float speed = 3f;

    private bool isAttackingCd;
    private bool isAttackLock;

    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ChangeState(EnemyState.Idle);
    }

    public void ChangeState(EnemyState newState)
    {
        // 关闭全部
        anim.SetBool("isIdle", false);
        anim.SetBool("isChasing", false);
        anim.SetBool("isAttacking", false);
        //anim.SetBool("isKnockBack", false);

        enemyState = newState;

        switch (enemyState)
        {
            case EnemyState.Idle:
                anim.SetBool("isIdle", true);
                break;
            case EnemyState.Chasing:
                anim.SetBool("isChasing", true);
                break;
            case EnemyState.Attacking:
                anim.SetBool("isAttacking", true);
                break;
            //case EnemyState.KnockBack:
            //    anim.SetBool("isKnockBack", true);
            //    break;
        }
    }

    void FixedUpdate()
    {
        // --------------------------
        // 核心：击退状态 → 完全不运行逻辑
        // --------------------------
        if (enemyState == EnemyState.KnockBack)
        {
            return;
        }

        if (isAttackLock)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (player == null)
        {
            ChangeState(EnemyState.Idle);
            rb.velocity = Vector2.zero;
            return;
        }

        float dis = Vector2.Distance(transform.position, player.position);

        if (dis <= attackRange && !isAttackingCd)
        {
            StartAttack();
            return;
        }

        if (dis > attackRange && dis <= chaseRange)
        {
            if (enemyState != EnemyState.Chasing)
                ChangeState(EnemyState.Chasing);

            ChaseMove();
        }

        if (dis > chaseRange)
        {
            player = null;
            ChangeState(EnemyState.Idle);
            rb.velocity = Vector2.zero;
        }
    }

    void StartAttack()
    {
        isAttackLock = true;
        ChangeState(EnemyState.Attacking);
        rb.velocity = Vector2.zero;
        Attack();
    }

    void ChaseMove()
    {
        float xDiff = player.position.x - transform.position.x;
        if (Mathf.Abs(xDiff) > 0.15f)
        {
            if ((xDiff > 0 && facingDirection == -1) || (xDiff < 0 && facingDirection == 1))
            {
                Flip();
            }
        }

        Vector2 dir = (player.position - transform.position).normalized;
        rb.velocity = dir * speed;
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(facingDirection, transform.localScale.y, transform.localScale.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (enemyState == EnemyState.KnockBack) return;

        if (collision.CompareTag("Player"))
        {
            player = null;
            ChangeState(EnemyState.Idle);
            rb.velocity = Vector2.zero;
        }
    }

    public void Attack()
    {
        Debug.Log("攻击");
        StartCoroutine(AttackCD());
    }

    IEnumerator AttackCD()
    {
        isAttackingCd = true;
        yield return new WaitForSeconds(attackCd);
        isAttackingCd = false;
        isAttackLock = false;

        if (player != null && Vector2.Distance(transform.position, player.position) <= chaseRange)
            ChangeState(EnemyState.Chasing);
        else
            ChangeState(EnemyState.Idle);
    }
}