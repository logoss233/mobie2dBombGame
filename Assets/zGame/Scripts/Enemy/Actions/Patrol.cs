using UnityEngine;
using UnityEditor;

public class Patrol {
    
    public Enemy enemy;


    Transform targetPoint;
    string state = ""; // run idle
    
    public Patrol(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void onEnter()
    {
        startRun();
    }

    void startRun()
    {
        this.state = "run";
        SwitchPoint();

    }

    float idleTime=2f;
    float idleTimer = 0f;
    void startIdle()
    {
        this.state = "idle";
        idleTimer = 0f;
    }


    public void onUpdate()
    {
        
    }

    public void onFixedUpdate()
    {
        ConsoleProDebug.Watch("patrolState", state);

        if (enemy.seeList.Count > 0)
        {
            enemy.startFollowAndAttack();
        }

        if (this.state == "run")
        {
            enemy.ani.Play("run");
        
            if (Mathf.Abs(enemy.transform.position.x - targetPoint.position.x) < enemy.speed*Time.fixedDeltaTime)
            {
                startIdle();
            }
            else
            {
                int dir = enemy.transform.position.x - targetPoint.position.x < 0 ? 1 : -1;
                enemy.rb.velocity = new Vector2(enemy.speed * dir, enemy.rb.velocity.y);
                enemy.setDirection(dir);
            } 
        }else if (this.state == "idle")
        {
            enemy.ani.Play("idle");
            idleTimer += Time.fixedDeltaTime;
            enemy.rb.velocity = new Vector2(0, enemy.rb.velocity.y);
            if (idleTimer > idleTime)
            {
                startRun();
            }
        }

        
        
    }

    void SwitchPoint()
    {
        if (Mathf.Abs(enemy.pointA.position.x - enemy.transform.position.x) > Mathf.Abs(enemy.pointB.position.x - enemy.transform.position.x))
        {
            targetPoint = enemy.pointA;
        }
        else
        {
            targetPoint = enemy.pointB;
        }
    }
}