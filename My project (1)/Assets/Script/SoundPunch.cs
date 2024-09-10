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

    void Update()
    {
        // ตรวจสอบเมื่อผู้เล่นคลิกซ้าย (Mouse Button 0 คือคลิกซ้าย)
        if (Input.GetMouseButtonDown(0))
        {
            // เล่นเสียง
            audioSource.Play();
        }
    }
}
