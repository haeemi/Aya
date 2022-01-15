using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEnding : MonoBehaviour
{
    //제일 마지막에 포인트 출력 뒷배경으로 쓰일 검정 패널
    public GameObject blackScript;  
    //포인트 누계 알려줄 텍스트
    public Text ScriptText;    
    //점점 검정패널로 뒤덮기
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public CanvasGroup exitBackgroundImageCanvasGroup;
    bool m_IsPlayerAtExit;
    float m_Timer;
    //마지막 스코어 테스트 부분
    public Text EndText;
    int loadCount;

    //플레이어
    public GameObject player;
    //포션을 받고 바뀔 엔딩 결과모음
    public GameObject[] player2;
    //랜덤 엔딩결과 번호
    public int number;
    //엔딩 결과 텍스트 부분
    public Text m_TypingText;
    public string m_Message;
    public float m_Speed = 0.05f;

    public AudioClip audioPotion;
    public AudioClip BGM;
    AudioSource audioSource;

    private void Awake()
    {
        //저장되어있던 토탈 포인트 가져오기
        loadCount = PlayerPrefs.GetInt("totalPoint", 0);
        //점수 미리 지정하기
        EndText.text = "Score: " + loadCount;

        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //포션 아이템과 플레이어와 닿았다면
        if (other.gameObject == player)
        {
            //랜덤으로 난수 받기
            number = Random.Range(0, 6);
            //난수에 맞는 캐릭터를 플레이어 현재 위치에 옮겨서 대치
            player2[number].gameObject.transform.position = other.gameObject.transform.position;
            //원래 캐릭터 숨기기
            other.gameObject.SetActive(false);
            //변화한 캐릭터 보이기
            player2[number].gameObject.SetActive(true);
            //대사 출력
            Invoke("printScript", 0.5f);
            //대사 없애기
            Invoke("OffScript", 4.0f);
            //플레이어 퇴장을 true로 지정
            m_IsPlayerAtExit = true;

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            //포션 먹었을때의 소리 출력
            audioSource.clip = audioPotion;
            audioSource.Play();

            //엔딩 타이머 지정
            m_Timer = -5f;
            //Ending 신호가 왔는지와 점수 확인
            Debug.Log("Ending");    
            Debug.Log(loadCount);
        }
    }

    void Update()
    {
        //플레이어 퇴장 신호가 왔다면
        if (m_IsPlayerAtExit)
        {
            //엔딩
            EndLevel();
        }
    }

    void printScript()
    {
        blackScript.gameObject.SetActive(true);
        switch (number)
        {
            case 0:     //파란머리 캐릭터 엔딩 대사 출력
                m_Message = @"아야는 치어리더가 되었다 !";
                StartCoroutine(Typing(m_TypingText, m_Message, m_Speed));

                break;
            case 1:       //노란머리 캐릭터 엔딩 대사 출력 
                m_Message = @"아야는 원더우먼이 되었다 !";
                StartCoroutine(Typing(m_TypingText, m_Message, m_Speed));
                break;
            case 2:       //분홍머리 캐릭터 엔딩 대사 출력 
                m_Message = @"아야는 배달원이 되었다 !";
                StartCoroutine(Typing(m_TypingText, m_Message, m_Speed));
                break;
            case 3:     //빨간머리 캐릭터 엔딩 대사 출력 
                m_Message = @"아야는 수녀가 되었다 !";
                StartCoroutine(Typing(m_TypingText, m_Message, m_Speed));
                break;
            case 4:     //회색머리 캐릭터 엔딩 대사 출력 
                m_Message = @"아야는 마법소녀가 되었다 !";
                StartCoroutine(Typing(m_TypingText, m_Message, m_Speed));
                break;
            case 5:      //보라머리 캐릭터 엔딩 대사 출력 
                m_Message = @"아야는 견습 마녀가 되었다 !";
                StartCoroutine(Typing(m_TypingText, m_Message, m_Speed));
                break;
        }
    }

    //스크립트 종료
    void OffScript()
    {
        blackScript.gameObject.SetActive(false);
    }

    //Fadeout
    void EndLevel()
    {
        m_Timer += Time.deltaTime;

        exitBackgroundImageCanvasGroup.alpha = m_Timer / fadeDuration;
        //일정시간에 도달하면
        if (m_Timer > fadeDuration + displayImageDuration + 2.0f)
        {
            //메뉴 씬으로 돌아가기
            SceneManager.LoadScene("MenuScene");
        }
    }
    
    //텍스트 타이핑 효과
    IEnumerator Typing(Text typingText, string message, float speed)
    {
        for (int i = 0; i < message.Length; i++)
        {
            typingText.text = message.Substring(0, i + 1);
            yield return new WaitForSeconds(speed);
        }
    }

}