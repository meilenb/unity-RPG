using System.Collections;
using UnityEngine;

public class Enemy_KnockBack : MonoBehaviour
{
    private Rigidbody2D rb;
    private Enemy_Movement enemy_Movement;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy_Movement = GetComponent<Enemy_Movement>();
    }

    public void KnockBack(Transform playerTransform, float knockbackForce, float knockbackTime, float stunTime)
    {
        // Á¢¿ÌÇÐ»÷ÍË×´Ì¬
        enemy_Movement.ChangeState(EnemyState.KnockBack);

        Vector2 direction = (transform.position - playerTransform.position).normalized;
        rb.velocity = direction * knockbackForce;

        StartCoroutine(StunTimer(knockbackTime, stunTime));
    }

    IEnumerator StunTimer(float knockbackTime, float stunTime)
    {
        // µÈ´ý»÷ÍËÊ±¼ä
        yield return new WaitForSeconds(knockbackTime);

        // Í£Ö¹Î»ÒÆ
        rb.velocity = Vector2.zero;

        // ¼ÌÐøÓ²Ö±
        yield return new WaitForSeconds(stunTime);

        // »Ö¸´×´Ì¬
        enemy_Movement.ChangeState(EnemyState.Idle);
    }
}