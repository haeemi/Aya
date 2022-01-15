using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEnding : MonoBehaviour
{
    //���� �������� ����Ʈ ��� �޹������ ���� ���� �г�
    public GameObject blackScript;  
    //����Ʈ ���� �˷��� �ؽ�Ʈ
    public Text ScriptText;    
    //���� �����гη� �ڵ���
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public CanvasGroup exitBackgroundImageCanvasGroup;
    bool m_IsPlayerAtExit;
    float m_Timer;
    //������ ���ھ� �׽�Ʈ �κ�
    public Text EndText;
    int loadCount;

    //�÷��̾�
    public GameObject player;
    //������ �ް� �ٲ� ���� �������
    public GameObject[] player2;
    //���� ������� ��ȣ
    public int number;
    //���� ��� �ؽ�Ʈ �κ�
    public Text m_TypingText;
    public string m_Message;
    public float m_Speed = 0.05f;

    public AudioClip audioPotion;
    public AudioClip BGM;
    AudioSource audioSource;

    private void Awake()
    {
        //����Ǿ��ִ� ��Ż ����Ʈ ��������
        loadCount = PlayerPrefs.GetInt("totalPoint", 0);
        //���� �̸� �����ϱ�
        EndText.text = "Score: " + loadCount;

        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //���� �����۰� �÷��̾�� ��Ҵٸ�
        if (other.gameObject == player)
        {
            //�������� ���� �ޱ�
            number = Random.Range(0, 6);
            //������ �´� ĳ���͸� �÷��̾� ���� ��ġ�� �Űܼ� ��ġ
            player2[number].gameObject.transform.position = other.gameObject.transform.position;
            //���� ĳ���� �����
            other.gameObject.SetActive(false);
            //��ȭ�� ĳ���� ���̱�
            player2[number].gameObject.SetActive(true);
            //��� ���
            Invoke("printScript", 0.5f);
            //��� ���ֱ�
            Invoke("OffScript", 4.0f);
            //�÷��̾� ������ true�� ����
            m_IsPlayerAtExit = true;

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            //���� �Ծ������� �Ҹ� ���
            audioSource.clip = audioPotion;
            audioSource.Play();

            //���� Ÿ�̸� ����
            m_Timer = -5f;
            //Ending ��ȣ�� �Դ����� ���� Ȯ��
            Debug.Log("Ending");    
            Debug.Log(loadCount);
        }
    }

    void Update()
    {
        //�÷��̾� ���� ��ȣ�� �Դٸ�
        if (m_IsPlayerAtExit)
        {
            //����
            EndLevel();
        }
    }

    void printScript()
    {
        blackScript.gameObject.SetActive(true);
        switch (number)
        {
            case 0:     //�Ķ��Ӹ� ĳ���� ���� ��� ���
                m_Message = @"�ƾߴ� ġ����� �Ǿ��� !";
                StartCoroutine(Typing(m_TypingText, m_Message, m_Speed));

                break;
            case 1:       //����Ӹ� ĳ���� ���� ��� ��� 
                m_Message = @"�ƾߴ� ��������� �Ǿ��� !";
                StartCoroutine(Typing(m_TypingText, m_Message, m_Speed));
                break;
            case 2:       //��ȫ�Ӹ� ĳ���� ���� ��� ��� 
                m_Message = @"�ƾߴ� ��޿��� �Ǿ��� !";
                StartCoroutine(Typing(m_TypingText, m_Message, m_Speed));
                break;
            case 3:     //�����Ӹ� ĳ���� ���� ��� ��� 
                m_Message = @"�ƾߴ� ���డ �Ǿ��� !";
                StartCoroutine(Typing(m_TypingText, m_Message, m_Speed));
                break;
            case 4:     //ȸ���Ӹ� ĳ���� ���� ��� ��� 
                m_Message = @"�ƾߴ� �����ҳడ �Ǿ��� !";
                StartCoroutine(Typing(m_TypingText, m_Message, m_Speed));
                break;
            case 5:      //����Ӹ� ĳ���� ���� ��� ��� 
                m_Message = @"�ƾߴ� �߽� ���డ �Ǿ��� !";
                StartCoroutine(Typing(m_TypingText, m_Message, m_Speed));
                break;
        }
    }

    //��ũ��Ʈ ����
    void OffScript()
    {
        blackScript.gameObject.SetActive(false);
    }

    //Fadeout
    void EndLevel()
    {
        m_Timer += Time.deltaTime;

        exitBackgroundImageCanvasGroup.alpha = m_Timer / fadeDuration;
        //�����ð��� �����ϸ�
        if (m_Timer > fadeDuration + displayImageDuration + 2.0f)
        {
            //�޴� ������ ���ư���
            SceneManager.LoadScene("MenuScene");
        }
    }
    
    //�ؽ�Ʈ Ÿ���� ȿ��
    IEnumerator Typing(Text typingText, string message, float speed)
    {
        for (int i = 0; i < message.Length; i++)
        {
            typingText.text = message.Substring(0, i + 1);
            yield return new WaitForSeconds(speed);
        }
    }

}