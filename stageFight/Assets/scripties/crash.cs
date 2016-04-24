using UnityEngine;
using System.Collections;

public class crash : MonoBehaviour
{
    void OnCollisionEnter (Collision col)
    {
        AudioSource audio = GetComponent<AudioSource>();
		audio.Play();
    }
}