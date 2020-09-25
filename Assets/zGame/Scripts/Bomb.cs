using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float time = 2f;
    private float timer = 0f;
    public GameObject BoomEffectPrefab;
    private bool isFire = true;
    private Animator ani;

    private void Awake()
    {
        ani = transform.Find("Sprite").GetComponent<Animator>();
    }

    public void putOut()
    {
        this.isFire = false;
        ani.Play("off");
    }

    // Update is called once per frame
    void Update()
    {
        if (this.isFire)
        {
            timer += Time.deltaTime;
            if (timer > time)
            {
                var go = Instantiate(BoomEffectPrefab);
                go.transform.position = transform.position;
                Destroy(gameObject);
            }
        }
        
    }
}
