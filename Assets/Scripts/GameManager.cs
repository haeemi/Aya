using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //��濡 ���� ��
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
    //�ؽ�Ʈ�κ�
    public Text m_TypingText;
    public string m_Message;
    public float m_Speed = 0.05f;
    public GameObject startScript;
    //pause��ư �г�
    public GameObject pausePanel;


    //���� �ؽ�Ʈ ���
    private void Start()
    {
        m_Message = @"�ȳ� ���� �ƾ�!                  " + "\n" + "������ ���ֿ� �ɷ��� �� ���� �̷��� �Ǿ���Ⱦ�Ф�";
        StartCoroutine(Typing(m_TypingText, m_Message, m_Speed));
        //�����Ⱓ ���� ���� �ؽ�Ʈ ���
        Invoke("scriptText2", 5f);
    }

    //�ι�° �ؽ�Ʈ ���
    void scriptText2()
    {
        m_Message = @"���� ������ ã�Ƽ�" + "\n" + "���ָ� Ǯ����!";
        StartCoroutine(Typing(m_TypingText, m_Message, m_Speed));
        //���� �Ⱓ ���� �ؽ�Ʈ ���� �����
        Invoke("offText", 3f);
    }

    //�ؽ�Ʈ ���� �����
    void offText()
    {
        startScript.gameObject.SetActive(false);
    }


    //�������� ����
    public void NextStage()
    {
        //���� �������� ����Ʈ�� �ջ��ϱ�
        totalPoint += stagePoint;
        stagePoint = 0;
        //�������� 1, 2, 3, 4�� ���
        if (stageIndex < Stages.Length - 1)
        {
            Stages[stageIndex].SetActive(false);
            //�޹�� �����
            Backgrounds[stageIndex].SetActive(false);
            //3 �������� ���� ���� ������
            if (stageIndex >= 2)
            {
                Lights[stageIndex - 2].SetActive(false);
            }
            //�������� �ε��� ����
            stageIndex++;
            //���� �������� Ȱ��ȭ
            Stages[stageIndex].SetActive(true);
            Backgrounds[stageIndex].SetActive(true);
            //�÷��̾� ��ġ �ʱ�ȭ
            PlayerReposition();
            //��������1, 2�� ��� ���� �Ⱥ��̰� �ϱ�
            if (stageIndex < 2)
            {
                moon.gameObject.SetActive(false);
            }
            else
            //��������1, 2�� �ƴ� ��� �� ���̰� �ϰ� ���� ���̰��ϱ�
            {
                moon.gameObject.SetActive(true);
                Lights[stageIndex - 2].SetActive(true);
            }
            //��ܿ� �������� TEXT ǥ��
            UIStage.text = "STAGE " + (stageIndex + 1);
        }
        else //�������� 5(����)�� ���
        {
            //���� ���� �ѱ�� ���� ��Ż ����Ʈ �����ϱ�
            PlayerPrefs.SetInt("totalPoint", totalPoint);
            //Ŭ���� �� �θ���
            SceneManager.LoadScene("ClearScen");
        }
    }

    public void HealthDown()
    {
        //ü���� 1�� �̻��̸� 
        if (health > 1)
        {
            //��� ����
            health--;
            //��Ʈ �Ѱ� ���߱�
            UIHealth[health].color = new Color(1, 0, 0, 0.4f);

        }
        //ü���� �� ������ ���(0��)
        else
        {
            //��Ʈ ���߱�
            UIHealth[0].color = new Color(1, 0, 0, 0.4f);
            //�÷��̾� ���� �Լ� ȣ��
            player.OnDie();
            //����� ��ư ���̱�
            ResetButton.SetActive(true);
        }
    }

    //�̱���
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
        //�÷��̾� �±װ� ��輱�� ������
        if (collision.gameObject.tag == "Player")
        {
            //ó�� ��ҷ� ��ġ �ű��
            if (health > 1)
                PlayerReposition();
            //ü�� ���� �Լ� ȣ��
            HealthDown();
        }
    }

    void PlayerReposition()
    {
        //��ġ�� �°� �̵�
        player.transform.position = new Vector3(0, -1.5f, 0);
        //�������� �ʰ� �ӵ� 0���� �����
        player.VelocityZero();
    }
    public void Restart()
    {
        //�����
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    //Pause��ư ����
    private void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();
        //Cancel(ESC) ��ư ������
        if (Input.GetButtonDown("Cancel"))
        {
            //Pause��ư ����
            PauseAction();
        }
    }


    public void PauseAction()
    {
        //����
        Time.timeScale = 0;
        //pause�г� ���̰��ϱ�
        pausePanel.SetActive(true);
        print("PauseAction");
    }

    //����ϱ�
    public void ResumeAction()
    {
        //���� ����
        Time.timeScale = 1;
        //�г� ���߱�
        pausePanel.SetActive(false);
        print("ResumeAction");
    }

    //������(���θ޴��� ���ư���)
    public void MainMenuAction()
    {
        //���� ����
        Time.timeScale = 1;
        //�г� ���߱�
        pausePanel.SetActive(false);
        print("MainMenuAction");
        //���θ޴� ������ �̵�
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