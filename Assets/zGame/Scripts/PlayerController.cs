using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 5;
    public float jumpForce = 10;


    
    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkWidth;
    public float checkHeight;
    public LayerMask groundLayer;

    [Header("States Check")]
    public bool isGround;

    [Header("Prefabs")]
    public GameObject jumpEffectPrefab;
    public GameObject fallEffectPrefab;

    private bool jumpPressed = false;
    private float dir = 0;
    private Animator ani;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ani =transform.Find("Sprite").GetComponent<Animator>();

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();


        //d动画
        if (isGround)
        {
            if (dir!=0)
            {
                ani.Play("player_run");
            }
            else
            {
                ani.Play("player_idle");
            }
        }
        else
        {
            if (rb.velocity.y > 0)
            {
                ani.Play("player_jump");
            }
            else
            {
                ani.Play("player_fall");
            }
        }
    }
    private void FixedUpdate()
    {
        PhysicsCheck();
        Movement();
        Jump();
    }

    void checkInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
        }
    }

    void Movement()
    {
        dir = Input.GetAxisRaw("Horizontal");
       
        rb.velocity = new Vector2(dir * speed, rb.velocity.y);

        if (dir != 0)
        {
            transform.localScale = new Vector3(dir, 1, 1);
        }
    }
    void Jump()
    {
        if (jumpPressed && isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            GameObject go=Instantiate(jumpEffectPrefab);
            go.transform.position = new Vector3(transform.position.x, transform.position.y-0.475f, 0);

        }
        jumpPressed = false;
    }

    void PhysicsCheck()
    {
        
        isGround = Physics2D.OverlapBox(groundCheck.position,new Vector2(checkWidth,checkHeight), 0, groundLayer);
    }
    public void OnDrawGizmos()
    {
        
        Gizmos.DrawWireCube(groundCheck.position, new Vector3(checkWidth, checkHeight, 1));
    }
}
