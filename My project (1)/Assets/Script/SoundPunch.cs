using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPunch : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // ดึง AudioSource จาก GameObject ที่สคริปต์นี้ถูกแนบอยู่
        audioSource = GetComponent<AudioSource>();
    }

    public void pucnhsound()
    {
        audioSource.Play();
    }
}
