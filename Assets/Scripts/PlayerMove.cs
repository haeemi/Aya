using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;

    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioDie;
    public AudioClip audioFinish;
    public AudioClip audioPotion;

    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider;
    Animator anim;
    AudioSource audioSource;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    //�÷��̾� ���� �Լ�
    void PlaySound(string action)
    {
        switch (action)//�׼ǿ� ���� ��� �Ҹ��� ������� ����
        {
            case "JUMP":
                audioSource.clip = audioJump;
                break;
            case "ATTACK":
                audioSource.clip = audioAttack;
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged;
                break;
            case "ITEM":
                audioSource.clip = audioItem;
                break;
            case "DIE":
                audioSource.clip = audioDie;
                break;
            case "FINISH":
                audioSource.clip = audioFinish;
                break;
            case "Potion":
                audioSource.clip = audioPotion;
                break;
        }
        audioSource.Play(); //���� ���
    }


    void Update()
    {

        //����
        if (Input.GetButtonDown("Jump")&&!anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse); //jumpPower ��� ����
            anim.SetBool("isJumping", true); //������ �ִϸ��̼� ����
            PlaySound("JUMP"); //���� ���� ���
        }

        //�ӵ� ����
        if (Input.GetButtonUp("Horizontal"))
        {
            //y�� �ӵ��� �״�� �ΰ� x�� �ӵ��� ����
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //��������Ʈ�� ����
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        //�ȱ� �ִϸ��̼��� �ڿ������� ���缼��� �ִϸ��̼� ����
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);
    }
    private void FixedUpdate()
    {
        //Move Speed
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
        if(rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed*(-1))
            rigid.velocity = new Vector2(maxSpeed*(-1), rigid.velocity.y);

        //���� ����
        if (rigid.velocity.y < 0)
        {
            //�÷��� ���̾ ��Ҵ��� rayHit�� ���� Ȯ��
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                //�÷��� ���̾ ��� rayHit�� �ߵ��ϸ� ���� ��� ����
                if (rayHit.distance < 0.5f)
                    anim.SetBool("isJumping", false);
            }
        }
    }

    //���� ���ݰ� ����
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //�÷��̾� ĳ���Ͱ� Enemy���� �� ������ �������� ���� ���� ���� �����Ѵ�
            if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);
            }
            else//���� �ʰ� �ε����� ��� �������� �Դ´�
            {
                OnDamaged(collision.transform.position);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //�ε��� ���ӿ�����Ʈ�� �±װ� Item�� ��� ����Ʈ�� �ջ�
        if (collision.gameObject.tag == "Item")
        {
            //�����, �ǹ�, ����� �Ǵ�
            bool isBronze = collision.gameObject.name.Contains("Bronze");
            bool isSilver = collision.gameObject.name.Contains("Silver");
            bool isGold = collision.gameObject.name.Contains("Gold");
            //�Ǵܿ� ���� ���� ����Ʈ �ջ�
            if (isBronze)
                gameManager.stagePoint += 50;
            else if (isSilver)
                gameManager.stagePoint += 100;
            else if (isGold)
                gameManager.stagePoint += 300;
            //���� �������� ������� �ϱ�
            collision.gameObject.SetActive(false);
            //������ �Ծ������� ���� ���
            PlaySound("ITEM");
        }
        else if (collision.gameObject.tag == "Finish")
        {
            //�ε��� ���ӿ�����Ʈ�� �±װ� Finish(����)�� ��� �������������� �̵�
            gameManager.NextStage();
            //���� ���������� �̵��������� ���� ���
            PlaySound("FINISH");
        }

        //�̱����κ�
        else if (collision.gameObject.tag == "Potion")
        {
            //Potion
            bool isPotionRed = collision.gameObject.name.Contains("PotionRed");
            bool isPotionBlue = collision.gameObject.name.Contains("PotionBlue");
            bool isPotionPurple = collision.gameObject.name.Contains("PotionPurple");
            bool isPotionGreen = collision.gameObject.name.Contains("PotionGreen");
            bool isPotionWhite = collision.gameObject.name.Contains("PotionWhite");
            if (isPotionRed)
                gameManager.stagePoint += 50;
            else if (isPotionBlue)
                gameManager.stagePoint += 50;
            else if (isPotionWhite)
                gameManager.stagePoint += 50;
            else if (isPotionPurple)
                gameManager.stagePoint += 50;
            else if (isPotionGreen)
                gameManager.stagePoint += 50;
            //���̻� �������� �ʰ��ϱ�
            collision.gameObject.SetActive(false);
            PlaySound("Potion");
        }
    }

    //���� ��ġ������
    void OnAttack(Transform enemy)
    {
        //�������� ����Ʈ�� 100�� �߰�
        gameManager.stagePoint += 100;
        //������� �÷��̾� ĳ���Ͱ� ���� �߰� �ϱ�
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        //���� ó��
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        //���� �������� �ִٸ�
        if (enemyMove != null)
        {
            enemyMove.OnDamaged();
            PlaySound("ATTACK");
        }
    }

    void OnDamaged(Vector2 targetPos)
    {
        //ü�� ���
        gameManager.HealthDown();
        //�������¸� ���� ���Ϳ� ���� �� ���� 11�� ���̾�� �̵�
        gameObject.layer = 11;
        //�÷��̾� ������ ��� ���ϰ� �����
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        //Reaction Force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1)*7, ForceMode2D.Impulse);
        //������ �Ծ������� �̹����� ����
        anim.SetTrigger("doDamaged");
        //3���Ŀ� �������� ����
        Invoke("OffDamaged", 3);
        PlaySound("DAMAGED");
    }

    void OffDamaged()
    {
        //���Ϳ� ���� �� �ִ� 10��° ���̾�� �̵�
        gameObject.layer = 10;
        //�ٽ� ���� �������� ������
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    //�÷��̾ �׾��� ��
    public void OnDie()
    {
        //��� �������ϰ� ����� 
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        //��������Ʈ �ִϸ��̼��� �������� ���·� �����
        spriteRenderer.flipY = true;
        //�ݸ��� ����
        capsuleCollider.enabled = false;
        //�ױ����� ��¦ �� �߰�(����) �ϱ�
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        //���̻� �������� �ʰ��ϱ�
        Invoke("DeActive", 5);
        PlaySound("DIE");
    }

    //�ӵ��� 0���� �����
    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
}
