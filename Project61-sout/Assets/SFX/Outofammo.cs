using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outofammo : MonoBehaviour
{
    private float totalTimeBeforeDestroy8;

    // Use this for initialization
    void Start()
    {
        var sound = this.GetComponent<AudioSource>();
        totalTimeBeforeDestroy8 = sound.clip.length;
    }

    // Update is called once per frame
    void Update()
    {
        totalTimeBeforeDestroy8 -= Time.deltaTime;

        if (totalTimeBeforeDestroy8 <= 0f)
        {
            Destroy(this.gameObject);
        }
    }
}
