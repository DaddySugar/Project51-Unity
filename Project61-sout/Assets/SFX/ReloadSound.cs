using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadSounds : MonoBehaviour
{

    private float totalTimeBeforeDestroy2;

    // Use this for initialization
    void Start()
    {
        var sound = this.GetComponent<AudioSource>();
        totalTimeBeforeDestroy2 = sound.clip.length;
    } 

    // Update is called once per frame
    void Update()
    {
        totalTimeBeforeDestroy2 -= Time.deltaTime;

        if (totalTimeBeforeDestroy2 <= 0f)
        {
            Destroy(this.gameObject);
        }
    }
}
