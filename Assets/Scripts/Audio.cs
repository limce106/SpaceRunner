using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 오디오 스크립트
public class Audio : MonoBehaviour
{
    GameObject audio;
    AudioSource bgm;
    void Start()
    {
        audio = GameObject.Find("AudioManager");
        bgm = audio.GetComponent<AudioSource>();
        // 오디오 재생
        bgm.Play();
    }

    void Update()
    {

    }
}
