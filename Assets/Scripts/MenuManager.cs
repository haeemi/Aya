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

    //���� ���۹�ư ����
    public void GoGameScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    //������ ��ư ����
    public void Quit()
    {
        Application.Quit();
    }

    //��ư Ŭ�� �Ҹ� ���
    public void OnPointerClick()
    {
        audioSource.clip = audioClick;
        audioSource.Play();
    }

    //��ư ���� ���콺�����Ͱ� �ö󰡸� �Ҹ� ���
    public void OnPointerEnter()
    {
        audioSource.clip = audioPoint;
        audioSource.Play();
    }
}
