using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float time = 2f;
    private float timer = 0f;
    public GameObject BoomEffectPrefab;


    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > time)
        {
            var go=Instantiate(BoomEffectPrefab);
            go.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}
