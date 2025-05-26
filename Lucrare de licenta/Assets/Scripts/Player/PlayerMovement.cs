using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using System;


public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private bool isFacingRight = true;
    private BoxCollider2D boxCollider;
    private Animator anim;
    public GameObject attackPoint;
    public Vector2 boxSize;
    public LayerMask enemies;
    public float meleeAttackDamage;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] float jumpingPower;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Z) && isGrounded())
        {
            anim.SetTrigger("attack1");
            SoundEffectManager.Play("PlayerAttack1");
        }

        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            anim.SetTrigger("jump");
            SoundEffectManager.Play("Jump");
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);

        }

        Flip();

        anim.SetBool("run", horizontal != 0);
        anim.SetBool("grounded", isGrounded());

    }

    public void endMeleeAttack()
    {
        anim.ResetTrigger("attack1");
    }

    public void meleeAttack()
    {
        Collider2D[] enemy = Physics2D.OverlapBoxAll(attackPoint.transform.position, boxSize, 0, enemies);

        foreach (Collider2D enemyGameObject in enemy)
        {
            if (enemyGameObject.CompareTag("Enemy"))
            {
                Debug.Log("You hit an enemy...");
                enemyGameObject.GetComponent<EnemyHealth>().TakeDamage(1);

            }
            else if (enemyGameObject.CompareTag("Boss"))
            {
                Debug.Log("You hit the boss...");
                enemyGameObject.GetComponent<BossHealth>().TakeDamage(1);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(attackPoint.transform.position, boxSize);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }


    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;



        }
    }

    public bool canAttack()
    {
        return horizontal == 0 && isGrounded();
    }
}
