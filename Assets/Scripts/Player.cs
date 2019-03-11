using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity {
   
    public int maxJumps = 2;
    int jumpCount = 0;

    public Vector3 resetPoint;

    public LayerMask boxLayer;
    protected override void Start()
    {
        base.Start();
        resetPoint = transform.position;
    }

    // Update is called once per frame
    void Update ()
    {
        Move();
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            Jump();
        }
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
        
        anim.SetBool("isGrounded", isGrounded);
    }

    public override void Move()
    {
        horizontal = Input.GetAxis("Horizontal");
        base.Move();
        anim.SetBool("isMoving", Mathf.Abs(horizontal) > 0);
    }

    public override void Jump()
    {
        base.Jump();
        jumpCount++;       
    }

    public override void OnGroundHit()
    {
        base.OnGroundHit();
        jumpCount = 0;
    }

    public override void Death()
    {
        base.Death();
        transform.position = resetPoint;
    }

    public override void OnEnemyHit(GameObject enemy)
    {
        base.OnEnemyHit(enemy);
        BoxCollider2D myCollider = GetComponent<BoxCollider2D>();
        //BoxCollider2D enemyCollider = enemy.GetComponent<BoxCollider2D>();
        //float heightDifference = transform.position.y - enemy.transform.position.y;
        float direction = enemy.transform.position.x - transform.position.x;
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, myCollider.size * 0.9f, 0, new Vector2(direction, 0), 1, boxLayer);
        if(transform.position.y < enemy.GetComponent<Entity>().anim.transform.position.y || (hit && hit.collider.gameObject == enemy))
        {
            Death();
        }
        else
        {
            enemy.GetComponent<Entity>().Death();
        }
    }
}
