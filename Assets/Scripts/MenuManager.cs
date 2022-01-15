using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour{
    public AudioClip audioPoint;
    public AudioClip audioClick;
    AudioSource audioSource;

   void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //게임 시작버튼 동작
    public void GoGameScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    //나가기 버튼 동작
    public void Quit()
    {
        Application.Quit();
    }

    //버튼 클릭 소리 재생
    public void OnPointerClick()
    {
        audioSource.clip = audioClick;
        audioSource.Play();
    }

    //버튼 위에 마우스포인터가 올라가면 소리 재생
    public void OnPointerEnter()
    {
        audioSource.clip = audioPoint;
        audioSource.Play();
    }
}
