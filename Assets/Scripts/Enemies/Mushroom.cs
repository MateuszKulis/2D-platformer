using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [SerializeField] private int healthPoints = 2;
    [SerializeField] private float speed;
    [SerializeField] private Transform[] patrolPoints;
    private int currentPatrolPoint;

    private Animator myAnimator;
    private SpriteRenderer mySprite;
    private PlayerMovment player;

    private void OnEnable()
    {
        player = FindObjectOfType<PlayerMovment>();
        myAnimator = this.GetComponent<Animator>();
        mySprite = this.GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        if (CanPatrol())
            Move();
        else
            myAnimator.SetBool("idle", true);
    }

    private bool CanPatrol()
    {
        return patrolPoints != null && patrolPoints.Length > 0 && !myAnimator.GetBool("crush");
    }

    private void Move()
    {
        var targetPosition = new Vector2(patrolPoints[currentPatrolPoint].position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (transform.position.x == patrolPoints[currentPatrolPoint].position.x)
        {
            currentPatrolPoint++;
        }

        if (currentPatrolPoint >= patrolPoints.Length)
        {
            currentPatrolPoint = 0;
        }

        FlipSprite(targetPosition);

        myAnimator.SetBool("idle", false);
    }

    private void FlipSprite(Vector2 targetPosition)
    {
        if (transform.position.x > targetPosition.x)
        {
            mySprite.flipX = true;
        }
        else
        {
            mySprite.flipX = false;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            foreach (ContactPoint2D hitPos in collision.contacts)
            {
                if (hitPos.normal.x == 1)
                {
                    player.PushBackPlayer(Vector2.left, 1);
                }
                else if (hitPos.normal.x == -1)
                {
                    player.PushBackPlayer(Vector2.right, 1);
                }
                else if (hitPos.normal.y == -1)
                {
                    player.PushBackPlayer(Vector2.up, 0);
                    myAnimator.SetBool("crush", true);
                }
            }
        }
    }

    public void EndCrushed()
    {
        myAnimator.SetBool("crush", false);
    }

    public void LostHealthPoints(int value)
    {
        healthPoints -= value;
        myAnimator.SetTrigger("hit");

        if (healthPoints < 1)
            Death();
    }

    private void Death()
    {
        myAnimator.SetTrigger("die");
    }

    public void EndDeath()
    {
        Destroy(gameObject);
    }
}
