using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    private float totalTimeBeforeDestroy7;

    // Use this for initialization
    void Start()
    {
        var sound = this.GetComponent<AudioSource>();
        totalTimeBeforeDestroy7 = sound.clip.length;
    }

    // Update is called once per frame
    void Update()
    {
        totalTimeBeforeDestroy7 -= Time.deltaTime;

        if (totalTimeBeforeDestroy7 <= 0f)
        {
            Destroy(this.gameObject);
        }
    }
}
