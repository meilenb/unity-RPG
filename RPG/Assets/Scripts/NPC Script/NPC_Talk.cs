using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Talk : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Animator interactAnim;
    private void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
        anim=GetComponentInChildren<Animator>();
    }
    private void OnEnable()
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        anim.Play("Idle");
        Debug.Log("´ý»ú");
        interactAnim.Play("Open");
     
    }
    private void OnDisable()
    {
        interactAnim.Play("Close");
        rb.isKinematic=false;
    }
    private void Update()
    {
        anim.Play("Idle");
    }
}
