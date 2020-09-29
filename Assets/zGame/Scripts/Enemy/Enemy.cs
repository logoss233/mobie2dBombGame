using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("MoveMent")]
    public float speed=2;
    public Transform pointA, pointB;
    public Transform targetPoint;

    public List<Transform> seeList = new List<Transform>(); //视野中的物体
    public List<Transform> targetList = new List<Transform>();
    
    [HideInInspector]
    public Animator ani;
    [HideInInspector]
    public Rigidbody2D rb;

    Patrol patrol;
    FollowAndAttack followAndAttack;

    private int face=1;

    string state = ""; //patrol 

    [Header("hit")]
    public Vector2 hitPos = new Vector2(0, 0);
    public Vector2 hitSize = new Vector2(1, 1);

    private void Awake()
    {
        ani = transform.Find("Sprite").GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        patrol = new Patrol(this);
        followAndAttack = new FollowAndAttack(this);

        this.startPatrol();
    }

    void Start()
    {
       
    }

    public void startPatrol()
    {
        this.state = "patrol";
        patrol.onEnter();
    }
    public void startFollowAndAttack()
    {
        this.state = "followAndAttack";
        followAndAttack.onEnter();
    }

    void Update()
    {
        updateSeeList();

        if (this.state == "patrol")
        {
            patrol.onUpdate();
        }
        
    }
    private void FixedUpdate()
    {
        if (this.state == "patrol")
        {
            patrol.onFixedUpdate();
        }else if (this.state == "followAndAttack")
        {
            followAndAttack.onFixedUpdate();
        }
       
       
    }


    public virtual void AttackAction() //攻击玩家
    {

    }
    public virtual void SkillAction() //对炸弹使用技能
    {

    }
    

    public void setDirection(int dir)
    {
        face = dir;
        if (dir > 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Bomb" || collision.tag == "Player")
        {
            this.seeList.Add(collision.transform);
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        this.seeList.Remove(collision.transform);

    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + new Vector3(hitPos.x, hitPos.y, 0), new Vector3(hitSize.x, hitSize.y, 1));
    }
    public void hit()
    {
        
        Collider2D[] res=Physics2D.OverlapAreaAll(new Vector2(transform.position.x+hitPos.x*face - hitSize.x/2, transform.position.y+hitPos.y - hitSize.y/2),
            new Vector2(transform.position.x+hitPos.x*face + hitSize.x/2, transform.position.y+hitPos.y + hitSize.y/2), LayerMask.GetMask("Player"));
        if (res.Length > 0)
        {
            res[0].SendMessage("beHit");
        }
    }
    public void special()
    {
        Collider2D[] res = Physics2D.OverlapAreaAll(new Vector2(transform.position.x + hitPos.x*face - hitSize.x/2, transform.position.y + hitPos.y - hitSize.y/2),
            new Vector2(transform.position.x + hitPos.x*face + hitSize.x/2, transform.position.y + hitPos.y + hitSize.y/2), LayerMask.GetMask("Bomb"));
        if (res.Length > 0)
        {
            foreach(Collider2D c in res)
            {
                c.SendMessage("putOut");
            }
        }
    }

    private void updateSeeList()
    {
        for(int i = seeList.Count - 1; i >= 0; i--)
        {
            if (seeList[i].tag == "BombOff")
            {
                seeList.RemoveAt(i);
            }
        }
    }
   
}
