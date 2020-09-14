using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectForce : MonoBehaviour
{

    public float radius = 1f;
    public float power = 10;
    public LayerMask layer;

    private void Start()
    {
        Collider2D[] res=Physics2D.OverlapCircleAll(transform.position, radius,layer);
        for(int i = 0; i < res.Length; i++)
        {
            Collider2D c = res[i];
            Rigidbody2D rb = c.GetComponent<Rigidbody2D>();
            if (rb)
            {
                print("addForce");
                var force = (c.transform.position - transform.position).normalized * power;
                print(force);
                rb.AddForce(force,ForceMode2D.Impulse);
            }
        }
        

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }



}
