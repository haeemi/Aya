using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //배경에 쓰일 달
    public GameObject moon;
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public PlayerMove player;
    public GameObject[] Stages;
    public GameObject[] Backgrounds;
    public GameObject[] Lights;
    public Image[] UIHealth;
    public Text UIPoint;
    public Text UIStage;
    public GameObject ResetButton;
    //텍스트부분
    public Text m_TypingText;
    public string m_Message;
    public float m_Speed = 0.05f;
    public GameObject startScript;
    //pause버튼 패널
    public GameObject pausePanel;


    //시작 텍스트 출력
    private void Start()
    {
        m_Message = @"안녕 나는 아야!                  " + "\n" + "마녀의 저주에 걸려서 내 몸이 이렇게 되어버렸어ㅠㅠ";
        StartCoroutine(Typing(m_TypingText, m_Message, m_Speed));
        //일정기간 이후 다음 텍스트 출력
        Invoke("scriptText2", 5f);
    }

    //두번째 텍스트 출력
    void scriptText2()
    {
        m_Message = @"마법 물약을 찾아서" + "\n" + "저주를 풀어줘!";
        StartCoroutine(Typing(m_TypingText, m_Message, m_Speed));
        //일정 기간 이후 텍스트 상자 숨기기
        Invoke("offText", 3f);
    }

    //텍스트 상자 숨기기
    void offText()
    {
        startScript.gameObject.SetActive(false);
    }


    //스테이지 변경
    public void NextStage()
    {
        //현재 스테이지 포인트를 합산하기
        totalPoint += stagePoint;
        stagePoint = 0;
        //스테이지 1, 2, 3, 4의 경우
        if (stageIndex < Stages.Length - 1)
        {
            Stages[stageIndex].SetActive(false);
            //뒷배경 숨기기
            Backgrounds[stageIndex].SetActive(false);
            //3 스테이지 부터 빛점 가리기
            if (stageIndex >= 2)
            {
                Lights[stageIndex - 2].SetActive(false);
            }
            //스테이지 인덱스 증가
            stageIndex++;
            //다음 스테이지 활성화
            Stages[stageIndex].SetActive(true);
            Backgrounds[stageIndex].SetActive(true);
            //플레이어 위치 초기화
            PlayerReposition();
            //스테이지1, 2일 경우 달은 안보이게 하기
            if (stageIndex < 2)
            {
                moon.gameObject.SetActive(false);
            }
            else
            //스테이지1, 2가 아닐 경우 달 보이게 하고 빛점 보이게하기
            {
                moon.gameObject.SetActive(true);
                Lights[stageIndex - 2].SetActive(true);
            }
            //상단에 스테이지 TEXT 표시
            UIStage.text = "STAGE " + (stageIndex + 1);
        }
        else //스테이지 5(최종)의 경우
        {
            //다음 씬에 넘기기 위해 토탈 포인트 저장하기
            PlayerPrefs.SetInt("totalPoint", totalPoint);
            //클리어 씬 부르기
            SceneManager.LoadScene("ClearScen");
        }
    }

    public void HealthDown()
    {
        //체력이 1개 이상이면 
        if (health > 1)
        {
            //목숨 감소
            health--;
            //하트 한개 감추기
            UIHealth[health].color = new Color(1, 0, 0, 0.4f);

        }
        //체력을 다 소진할 경우(0개)
        else
        {
            //하트 감추기
            UIHealth[0].color = new Color(1, 0, 0, 0.4f);
            //플레이어 죽음 함수 호출
            player.OnDie();
            //재시작 버튼 보이기
            ResetButton.SetActive(true);
        }
    }

    //미구현
    public void HealthUp()
    {
        if (health < 5)
        {
            health++;
            UIHealth[health].color = new Color(1, 0, 0, 0.4f);

        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //플레이어 태그가 경계선에 닿으면
        if (collision.gameObject.tag == "Player")
        {
            //처음 장소로 위치 옮기기
            if (health > 1)
                PlayerReposition();
            //체력 감소 함수 호출
            HealthDown();
        }
    }

    void PlayerReposition()
    {
        //위치에 맞게 이동
        player.transform.position = new Vector3(0, -1.5f, 0);
        //움직이지 않게 속도 0으로 만들기
        player.VelocityZero();
    }
    public void Restart()
    {
        //재시작
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    //Pause버튼 동작
    private void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();
        //Cancel(ESC) 버튼 누르면
        if (Input.GetButtonDown("Cancel"))
        {
            //Pause버튼 동작
            PauseAction();
        }
    }


    public void PauseAction()
    {
        //정지
        Time.timeScale = 0;
        //pause패널 보이게하기
        pausePanel.SetActive(true);
        print("PauseAction");
    }

    //계속하기
    public void ResumeAction()
    {
        //정지 해제
        Time.timeScale = 1;
        //패널 감추기
        pausePanel.SetActive(false);
        print("ResumeAction");
    }

    //나가기(메인메뉴로 돌아가기)
    public void MainMenuAction()
    {
        //정지 해제
        Time.timeScale = 1;
        //패널 감추기
        pausePanel.SetActive(false);
        print("MainMenuAction");
        //메인메뉴 씬으로 이동
        SceneManager.LoadScene("MenuScene");
    }
    IEnumerator Typing(Text typingText, string message, float speed)
    {
        for (int i = 0; i < message.Length; i++)
        {
            typingText.text = message.Substring(0, i + 1);
            yield return new WaitForSeconds(speed);
        }
    }

}