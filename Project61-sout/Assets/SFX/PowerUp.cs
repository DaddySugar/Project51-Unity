using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private float totalTimeBeforeDestroy5;

    // Use this for initialization
    void Start()
    {
        var sound = this.GetComponent<AudioSource>();
        totalTimeBeforeDestroy5 = sound.clip.length;
    }

    // Update is called once per frame
    void Update()
    {
        totalTimeBeforeDestroy5 -= Time.deltaTime;

        if (totalTimeBeforeDestroy5 <= 0f)
        {
            Destroy(this.gameObject);
        }
    }
}
