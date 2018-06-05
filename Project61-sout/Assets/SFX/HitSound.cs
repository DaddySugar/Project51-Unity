using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSound : MonoBehaviour
{
    private float totalTimeBeforeDestroy9;

    // Use this for initialization
    void Start()
    {
        var sound = this.GetComponent<AudioSource>();
        totalTimeBeforeDestroy9 = sound.clip.length;
    }

    // Update is called once per frame
    void Update()
    {
        totalTimeBeforeDestroy9 -= Time.deltaTime;

        if (totalTimeBeforeDestroy9 <= 0f)
        {
            Destroy(this.gameObject);
        }
    }
}
