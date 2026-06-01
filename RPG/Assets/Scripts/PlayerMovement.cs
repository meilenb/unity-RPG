using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    public Rigidbody2D rb;
    public Animator animator;
    public int facingDirection = 1;
    private bool isKnockBack = false;
    
    public Player_Combat player_Combat;


    private void Update()
    {
        if (Input.GetButtonDown("Slash"))
        {
            player_Combat.Attack();
        }
    }



    void FixedUpdate()
    {
        if (isKnockBack == false)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            rb.velocity = new Vector2(horizontal, vertical) * speed;
            animator.SetFloat("horizontal", Mathf.Abs(horizontal));
            animator.SetFloat("vertical", Mathf.Abs(vertical));
            if (horizontal > 0 && transform.localScale.x < 0 || horizontal < 0 && transform.localScale.x > 0)
            {
                Flip();
            }
        }
    }

     void Flip()
    {
        facingDirection *= -1;
        transform.localScale=new Vector3(transform.localScale.x*-1,transform.localScale.y,transform.localScale.z);
    }
    public void Knockback(Transform enemy,float force,float stunTime)
    {
        isKnockBack = true;
        Vector2 direction = (transform.position - enemy.position).normalized;
        rb.velocity = direction * force;
        StartCoroutine(KnockbackCounter(stunTime));
    }
    IEnumerator KnockbackCounter(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        isKnockBack=false;
        rb.velocity= Vector2.zero;
    }
}
