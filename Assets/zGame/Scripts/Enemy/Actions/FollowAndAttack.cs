using UnityEngine;
using UnityEditor;

public class FollowAndAttack 
{
    Enemy enemy;

    string state = ""; //attack follow special
    public FollowAndAttack(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void onEnter()
    {
        this.state = "follow";
    }

    float attackTimer = 0;
    float attackTime = 1f;
    void startAttack()
    {
        this.state = "attack";
        this.attackTimer = 0f;
        
        enemy.ani.Play("attack",0,0);
    }
    void startSpecial()
    {
        this.state = "special";
        this.attackTimer = 0;
        enemy.ani.Play("special",0,0);
    }

    public void onFixedUpdate()
    {
        ConsoleProDebug.Watch("followAndAttackState", state);
        
        if (this.state == "follow")
        {
            if (enemy.seeList.Count == 0)
            {
                //切换
                enemy.startPatrol();
            }
            else
            {


                Transform t = getTarget();
                if (Mathf.Abs(t.position.x - enemy.transform.position.x) < 1f)
                {
                    if (t.CompareTag("Player"))
                    {
                        this.startAttack();
                    }
                    else
                    {
                        this.startSpecial();
                    }

                }
                else
                {
                    int dir = enemy.transform.position.x - t.position.x < 0 ? 1 : -1;
                    enemy.rb.velocity = new Vector2(enemy.speed * dir, enemy.rb.velocity.y);
                    enemy.setDirection(dir);
                    enemy.ani.Play("run");
                }   
            }
                
        }
        else if(this.state=="attack")
        {
            enemy.rb.velocity = new Vector2(0, enemy.rb.velocity.y);
            this.attackTimer += Time.fixedDeltaTime;
            if(attackTimer>=0.25f && attackTimer - Time.fixedDeltaTime < 0.25f)
            {
                //打击点
                enemy.hit();
            }

            if (this.attackTimer > this.attackTime)
            {
                this.state = "follow";
            }
        }else if (this.state == "special")
        {
            enemy.rb.velocity = new Vector2(0, enemy.rb.velocity.y);
            this.attackTimer += Time.fixedDeltaTime;
            if(attackTimer>=0.4f && attackTimer-Time.fixedDeltaTime<0.4f)
            {
                enemy.special();
            }
            if (this.attackTimer > this.attackTime)
            {
                this.state = "follow";
            }
        }
    }

    Transform getTarget()  
    {
        Transform t = enemy.seeList[0];
        for(int i = 1; i < enemy.seeList.Count; i++)
        {
            Transform tNew = enemy.seeList[i];
            if (Mathf.Abs(tNew.position.x - enemy.transform.position.x) < Mathf.Abs(t.position.x - enemy.transform.position.x)){
                t = tNew;
            }
        }
        return t;
    }

    

}