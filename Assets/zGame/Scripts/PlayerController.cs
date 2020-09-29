using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum State { 
        normal,
        hit
    }

    private Rigidbody2D rb;
    public float speed = 5;
    public float jumpForce = 10;
    public float cd = 1f;
    public GameObject bombPrefab;

    
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
    private float cdTimer = 0;


    State state = State.normal;
    
    private float hitTimer=0;
    private float hitTime = 0.5f;

    private int face = 1;
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


        if (state==State.normal && Input.GetButtonDown("Fire"))
        {
            if (cdTimer <= 0)
            {
                cdTimer = cd;
                GameObject go = Instantiate(bombPrefab);
                go.transform.position = transform.position;
            }
        }

        if (state == State.hit)
        {
            hitTimer += Time.deltaTime;
            if (hitTimer > hitTime)
            {
                this.state = State.normal;
            }
        }

        //动画
        if (state == State.normal)
        {
            if (isGround)
            {
                if (dir != 0)
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
        

        cdTimer -= Time.deltaTime;
    }
    private void FixedUpdate()
    {
        PhysicsCheck();
        if (state == State.normal)
        {
            Movement();
            Jump();
        }else if (state == State.hit)
        {
            
        }


        //按键结束
        jumpPressed = false;
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
            face = dir > 0 ? 1 : -1;
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
    }

    void PhysicsCheck()
    {
        var preIsGround = isGround;
        isGround = Physics2D.OverlapBox(groundCheck.position,new Vector2(checkWidth,checkHeight), 0, groundLayer);
        if(isGround && !preIsGround)
        {
            //生成落地特效
            GameObject go=Instantiate(fallEffectPrefab);
            go.transform.position = new Vector3(transform.position.x, transform.position.y-0.75f, 0);
        }
    }
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundCheck.position, new Vector3(checkWidth, checkHeight, 1));
    }

    public void beHit()
    {
        print("beHit");
        hitTimer = 0;
        ani.Play("player_hit", 0, 0);
        this.state = State.hit;
        rb.velocity = new Vector2(-3*face , 10);
    }

    private void OnGUI()
    {
        GUI.TextArea(new Rect(0, 0, 200, 100), "playerState:" + state.ToString());
    }
}
