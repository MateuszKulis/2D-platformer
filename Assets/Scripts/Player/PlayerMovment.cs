using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    private SpriteRenderer mySprite;
    private HeartsManager heartsManager;

    [Header("Player Values")]
    [SerializeField] private float movmentSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float attackRange;
    [Header("Grounded Checker")]
    [SerializeField] private float distanceToJump;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;

    private bool movmentIsBlock;

    private void OnEnable()
    {
        myRigidbody = this.GetComponent<Rigidbody2D>();
        myAnimator = this.GetComponent<Animator>();
        mySprite = this.GetComponent<SpriteRenderer>();

        heartsManager = FindObjectOfType<HeartsManager>();
    }

    private void Update()
    {
        if (!movmentIsBlock) {
            Move();
            Jump();
            Attack();
        }
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float velocity = x * movmentSpeed;
        myRigidbody.velocity = new Vector2(velocity, myRigidbody.velocity.y);

        FlipSprite(velocity);

        myAnimator.SetFloat("movmentSpeed", Mathf.Abs(velocity));
    }

    private void FlipSprite(float x)
    {
        if (x > 0.1f)
        {
            mySprite.flipX = false;
        }else if (x < -0.1f)
        {
            mySprite.flipX = true;
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpForce);

        myAnimator.SetFloat("jumpSpeed", myRigidbody.velocity.y);
    }

    private bool IsGrounded()
    {
        Collider2D collider = Physics2D.OverlapCircle(groundCheck.position, distanceToJump, groundLayer);

        return collider != null;
    }

    private void Attack()
    {
        if (IsAttack())
        {
            myAnimator.SetBool("attack", true);
            DetectEnemy();
        }
    }

    private bool IsAttack()
    {
        return Input.GetKeyDown(KeyCode.Space) && !myAnimator.GetBool("attack");
    }

    private void DetectEnemy()
    {
        RaycastHit2D hit;
        if (!mySprite.flipX)
            hit = Physics2D.Raycast(transform.position, Vector2.right, attackRange);
        else
            hit = Physics2D.Raycast(transform.position, Vector2.left, attackRange);

        if (hit.collider != null && hit.collider.CompareTag("Mushroom"))
        {
            hit.collider.gameObject.GetComponent<Mushroom>().LostHealthPoints(1);           
        }
    }

    private void EndAttack()
    {
        myAnimator.SetBool("attack", false);
    }

    public void PushBackPlayer(Vector2 direction, int lostHearts)
    {
        StartCoroutine(Wait(0.3f, direction, lostHearts));
    }

    private IEnumerator Wait(float waitTime, Vector2 direction, int lostHearts)
    {
        myRigidbody.AddForce(direction * 150f);

        if (lostHearts > 0 && !movmentIsBlock)
        {
            heartsManager.LostHearts(lostHearts);
            movmentIsBlock = true;
            if(CheckPlayerAlive())
                myAnimator.SetTrigger("death");
        }
        yield return new WaitForSeconds(waitTime);
        if (!CheckPlayerAlive()) movmentIsBlock = false;
    }

    private bool CheckPlayerAlive()
    {
        return heartsManager.GetHeartsQuantity() < 1f;
    }

    public void EndDeath()
    {
#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

}
