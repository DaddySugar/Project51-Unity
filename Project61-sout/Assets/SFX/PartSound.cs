using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartSound : MonoBehaviour
{
    private float totalTimeBeforeDestroy4;

    // Use this for initialization
    void Start()
    {
        var sound = this.GetComponent<AudioSource>();
        totalTimeBeforeDestroy4 = sound.clip.length;
    }

    // Update is called once per frame
    void Update()
    {
        totalTimeBeforeDestroy4 -= Time.deltaTime;

        if (totalTimeBeforeDestroy4 <= 0f)
        {
            Destroy(this.gameObject);
        }
    }
}
