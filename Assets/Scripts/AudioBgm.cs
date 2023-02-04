using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 맨 처음 bgm 재생 스크립트
public class AudioBgm : MonoBehaviour
{
    AudioSource audioSource;
    //public AudioClip audiobgm;
    // 재생 여부
    bool isPlay;

    void Start()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
        // 게임 시작하자마자 재생
        audioSource.Play();
        // 재생 여부 true
        isPlay = true;
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        // 아래 세 씬이 아니면
        if(SceneManager.GetActiveScene().name != "Start" && SceneManager.GetActiveScene().name != "Talk1" && SceneManager.GetActiveScene().name != "Planets")
        {
            // bgm 멈춤
            audioSource.Stop();
            // 재생 여부는 false
            isPlay = false;
        }
        // 아래 세 씬이면
        if(SceneManager.GetActiveScene().name == "Start" || SceneManager.GetActiveScene().name != "Talk1" || SceneManager.GetActiveScene().name != "Planets")
        {
            // 재생 여부가 false면
            if(isPlay == false)
            {
                // bgm 재생
                audioSource.Play();
                // 재생 여부는 true
                isPlay = true;
            }
        }
    }
}
