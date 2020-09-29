using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieOnAniComplete : MonoBehaviour
{
    public Animator ani;
    private void Awake()
    {
        if (ani == null)
        {
            ani = GetComponent<Animator>();
        }
    }
    private void Update()
    {
        AnimatorStateInfo info = ani.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 1.0f)
        {
            Destroy(gameObject);
        }
    }
    
}
