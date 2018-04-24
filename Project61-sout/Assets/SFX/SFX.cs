using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{

    public AudioSource Beep;

    public void PlayBeep()
    {
        Beep.Play();
    }

}
