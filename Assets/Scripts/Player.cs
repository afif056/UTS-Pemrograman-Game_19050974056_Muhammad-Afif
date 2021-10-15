using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    bool facingRight = true;
    float velX, speed = 2f;
    //baru
    public float jumpValue;
    int health = 3;
    bool isHurt, isDead;
    //baru

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKey (KeyCode.LeftShift))
            speed = 5f;
        else
            speed = 2f;

        //baru
        if (Input.GetButtonDown("Jump") && rb.velocity.y == 0)
            rb.AddForce (Vector2.up * jumpValue);
        //baru

        AnimationState();

        //baru
        if(!isDead)
        //baru
        velX = Input.GetAxisRaw ("Horizontal") * speed;
    }

    void FixedUpdate() 
    {
        //baru
        if (!isHurt)
        //baru
        rb.velocity = new Vector2(velX, rb.velocity.y);
    }

    void LateUpdate()
    {
        CheckWhereToFace();
    }

    void CheckWhereToFace() 
    {
        Vector3 localScale = transform.localScale;
        if (velX > 0) {
            facingRight = true;
        }
        else if (velX < 0) {
            facingRight = false;
        }
        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0))) 
        {
            localScale.x *= -1;
        }
        transform.localScale = localScale;
    }
    void AnimationState()
    {
        if (velX == 0)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", false);
        }
        //baru
        if (rb.velocity.y == 0)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);
        }
        if (Mathf.Abs(velX) == 2 && rb.velocity.y == 0)
            anim.SetBool("isWalking", true);
        if (Mathf.Abs(velX) == 5 && rb.velocity.y == 0)
            anim.SetBool("isRunning", true);
        else
            anim.SetBool("isRunning", false);

        if (Input.GetKey(KeyCode.DownArrow) && Mathf.Abs(velX) == 5)
            anim.SetBool("isSliding", true);
        else
            anim.SetBool("isSliding", false);

        if (rb.velocity.y > 0)
            anim.SetBool("isJumping", true);

        if (rb.velocity.y < 0)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", true);
        }
        //baru
    }
    //baru
    void OnTriggerEnter2D (Collider2D col)
  
    {
        if (col.gameObject.name.Equals("Enemy"))
        {
            health -= 1;
        }

        if (col.gameObject.name.Equals("Enemy") && health > 0)
        {
            anim.SetTrigger("isHurt");
            StartCoroutine ("Hurt");
        }else {
            velX = 0;
            isDead = true;
            anim.SetTrigger ("isDead");
        }    
    }

    IEnumerator Hurt()
    {
        isHurt = true;
        rb.velocity = Vector2.zero;

        if (facingRight)
            rb.AddForce (new Vector2(-300f, 100f));
        else
            rb.AddForce (new Vector2(100f, 100f));

        yield return new WaitForSeconds(0.5f);

        isHurt = false;
    }
    //baru
}

    


