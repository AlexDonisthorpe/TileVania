using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Config
    [Header("Config Variables")]
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(250f, 25f);

    // SFX
    [Header("Sound Effects")]
    [SerializeField] AudioClip jumpSFX;
    [SerializeField] AudioClip deathSFX;

    // State
    bool isAlive = true;
    float baseGravityScale;

    // Cached Component References
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D bodyCollider;
    BoxCollider2D feetCollider;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        baseGravityScale = myRigidBody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) {
            StartCoroutine("StopMoving");
            return; }
        Run();
        Climb();
        Jump();
        FlipSprite();
        Die();
    }

    private void Run()
    {
        float controlThrow = Input.GetAxisRaw("Horizontal"); // from -1 to +1
        Vector2 playerVelocity = new Vector2((controlThrow * runSpeed), myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            myAnimator.SetBool("isRunning", true);
        } else
        {
            myAnimator.SetBool("isRunning", false);
        }
    }

    private void Jump()
    {
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }
        if (Input.GetButtonDown("Jump"))
        {
            AudioSource.PlayClipAtPoint(jumpSFX, Camera.main.transform.position);
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if(playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2 (Mathf.Sign(myRigidBody.velocity.x), transform.localScale.y);
        }
    }

    private void Climb()
    {
        if(!bodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            myAnimator.SetBool("isClimbing", false);
            myRigidBody.gravityScale = baseGravityScale;
            return;
        }
        myRigidBody.gravityScale = 0;
        float controlThrow = Input.GetAxisRaw("Vertical");
        Vector2 climbingVelocity = new Vector2(myRigidBody.velocity.x, (controlThrow * climbSpeed));
        myRigidBody.velocity = climbingVelocity;
        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        if (playerHasVerticalSpeed)
        {
            myAnimator.SetBool("isClimbing", true);
        }
        else
        {
            myAnimator.SetBool("isClimbing", false);
        }
    }

    private void Die()
    {
        if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            myAnimator.SetTrigger("Die");
            myRigidBody.velocity = deathKick;
            isAlive = false;
            AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position);
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    private void Stop()
    {
        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            myRigidBody.velocity = Vector2.zero;
        }
    }

    IEnumerator StopMoving()
    {
        yield return new WaitForSeconds(0.1f);
        Stop();
    }
}

