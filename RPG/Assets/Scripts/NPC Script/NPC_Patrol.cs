using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Patrol : MonoBehaviour
{
    public Vector2[] patrolPoints;
    public Vector2 target;
    public float speed = 2;
    public float pauseDuration = 1.5f;

    private bool isPaused=false;
    private Rigidbody2D rb;
    private int currentPatrolIndex;
    private Animator anim;
    private Coroutine patrolCoroutine;  // 保存协程引用

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        patrolCoroutine = StartCoroutine(SetPatrolPoint());

    }
    private void OnEnable()
    {
        isPaused = false;

        if (patrolCoroutine != null)
        {
            StopCoroutine(patrolCoroutine);
        }

        patrolCoroutine = StartCoroutine(SetPatrolPoint());
    }
    private void OnDisable()
    {
        Debug.Log("Patrol Disable");

        if (patrolCoroutine != null)
        {
            StopCoroutine(patrolCoroutine);
            patrolCoroutine = null;
        }
    }

    private void Update()
    {
        if (!enabled) return;

        if (isPaused)
        {
            if (rb != null) rb.velocity = Vector2.zero;
            return;
        }

        if (patrolPoints == null || patrolPoints.Length == 0) return;

        Vector2 direction = ((Vector3)target - transform.position).normalized;
        if (direction.x < 0 && transform.localScale.x > 0 || direction.x > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        rb.velocity = direction * speed;

        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            if (patrolCoroutine != null)
                StopCoroutine(patrolCoroutine);
            patrolCoroutine = StartCoroutine(SetPatrolPoint());
        }
    }

    IEnumerator SetPatrolPoint()
    {
        isPaused = true;
        if (anim != null) anim.Play("Idle");
        yield return new WaitForSeconds(pauseDuration);

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        target = patrolPoints[currentPatrolIndex];
        isPaused = false;

        Debug.Log("行走");
        if (anim != null) anim.Play("Walk");
    }
}