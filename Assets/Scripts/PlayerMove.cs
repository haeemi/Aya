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

    //플레이어 사운드 함수
    void PlaySound(string action)
    {
        switch (action)//액션에 따라 어느 소리를 재생할지 선택
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
        audioSource.Play(); //음량 재생
    }


    void Update()
    {

        //점프
        if (Input.GetButtonDown("Jump")&&!anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse); //jumpPower 대로 점프
            anim.SetBool("isJumping", true); //점프중 애니메이션 동작
            PlaySound("JUMP"); //점프 사운드 재생
        }

        //속도 제한
        if (Input.GetButtonUp("Horizontal"))
        {
            //y축 속도는 그대로 두고 x축 속도만 제한
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //스프라이트의 방향
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        //걷기 애니메이션을 자연스럽게 멈춰세우는 애니메이션 설정
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

        //착지 구현
        if (rigid.velocity.y < 0)
        {
            //플랫폼 레이어에 닿았는지 rayHit를 통해 확인
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                //플랫폼 레이어에 닿아 rayHit가 발동하면 점핑 모션 끄기
                if (rayHit.distance < 0.5f)
                    anim.SetBool("isJumping", false);
            }
        }
    }

    //적의 공격과 피해
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //플레이어 캐릭터가 Enemy보다 더 위에서 수직으로 밟을 경후 적을 공격한다
            if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);
            }
            else//밟지 않고 부딪혔을 경우 데미지를 입는다
            {
                OnDamaged(collision.transform.position);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //부딪힌 게임오브젝트의 태그가 Item일 경우 포인트를 합산
        if (collision.gameObject.tag == "Item")
        {
            //브론즈, 실버, 골드의 판단
            bool isBronze = collision.gameObject.name.Contains("Bronze");
            bool isSilver = collision.gameObject.name.Contains("Silver");
            bool isGold = collision.gameObject.name.Contains("Gold");
            //판단에 따라 현재 포인트 합산
            if (isBronze)
                gameManager.stagePoint += 50;
            else if (isSilver)
                gameManager.stagePoint += 100;
            else if (isGold)
                gameManager.stagePoint += 300;
            //먹은 아이템은 사라지게 하기
            collision.gameObject.SetActive(false);
            //아이템 먹었을때의 사운드 출력
            PlaySound("ITEM");
        }
        else if (collision.gameObject.tag == "Finish")
        {
            //부딪힌 게임오브젝트의 태그가 Finish(골인)일 경우 다음스테이지로 이동
            gameManager.NextStage();
            //다음 스테이지로 이동했을때의 사운드 출력
            PlaySound("FINISH");
        }

        //미구현부분
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
            //더이상 동작하지 않게하기
            collision.gameObject.SetActive(false);
            PlaySound("Potion");
        }
    }

    //적을 해치웠을때
    void OnAttack(Transform enemy)
    {
        //스테이지 포인트를 100점 추가
        gameManager.stagePoint += 100;
        //밟았을때 플레이어 캐릭터가 위로 뜨게 하기
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        //적의 처리
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        //적의 움직임이 있다면
        if (enemyMove != null)
        {
            enemyMove.OnDamaged();
            PlaySound("ATTACK");
        }
    }

    void OnDamaged(Vector2 targetPos)
    {
        //체력 깎기
        gameManager.HealthDown();
        //무적상태를 위해 몬스터와 닿을 수 없는 11번 레이어로 이동
        gameObject.layer = 11;
        //플레이어 색상을 희고 연하게 만들기
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        //Reaction Force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1)*7, ForceMode2D.Impulse);
        //데미지 입었을때의 이미지로 조정
        anim.SetTrigger("doDamaged");
        //3초후에 무적상태 끄기
        Invoke("OffDamaged", 3);
        PlaySound("DAMAGED");
    }

    void OffDamaged()
    {
        //몬스터와 닿을 수 있는 10번째 레이어로 이동
        gameObject.layer = 10;
        //다시 원래 색상으로 돌리기
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    //플레이어가 죽었을 때
    public void OnDie()
    {
        //희고 반투명하게 만들기 
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        //스프라이트 애니메이션을 떨어지는 형태로 만들기
        spriteRenderer.flipY = true;
        //콜리더 끄기
        capsuleCollider.enabled = false;
        //죽기전에 살짝 붕 뜨게(점프) 하기
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        //더이상 동작하지 않게하기
        Invoke("DeActive", 5);
        PlaySound("DIE");
    }

    //속도를 0으로 만들기
    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
}
