using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D myRigidBody;
    [SerializeField] float moveSpeed = 1f;
    BoxCollider2D myTriggerCollider;
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myTriggerCollider = GetComponent<BoxCollider2D>();
        myRigidBody.velocity = new Vector2(moveSpeed, 0f);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void TurnAround()
    {
        myRigidBody.velocity = new Vector2((myRigidBody.velocity.x * -moveSpeed), 0f);
        transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), transform.localScale.y);
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        TurnAround();
    }
}
