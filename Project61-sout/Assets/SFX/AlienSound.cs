using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienSound : MonoBehaviour
{
    private float totalTimeBeforeDestroy6;

    // Use this for initialization
    void Start()
    {
        var sound = this.GetComponent<AudioSource>();
        totalTimeBeforeDestroy6 = sound.clip.length;
    }

    // Update is called once per frame
    void Update()
    {
        totalTimeBeforeDestroy6 -= Time.deltaTime;

        if (totalTimeBeforeDestroy6 <= 0f)
        {
            Destroy(this.gameObject);
        }
    }
}
