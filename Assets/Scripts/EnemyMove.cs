using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider;
    public int nextMove;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        Think();
        Invoke("Think", 5);
    }

    void Update()
    {
        //Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        //Platform Check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.3f, rigid.position.y);
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
            turn();
    }

    void Think()
    {
        //다음의 움직임 거리를 랜덤으로 지정
        nextMove = Random.Range(-1, 2);
        //적 캐릭터 애니메이션 지정
        anim.SetInteger("WalkSpeed", nextMove);
        //스프라이트 방향에 따라 에니메이션 뒤집기
        if(nextMove!=0)
            spriteRenderer.flipX = nextMove == 1;
        //재귀함수로 계속 생각하게 하기
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);

    }

    void turn()
    {
        //왔다갔다 움직이게 하기
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke();
        Invoke("Think", 5);
    }

    public void OnDamaged()
    {
        //약간 희게 만들기
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        //떨어지는 동작으로 애니메이션 변경
        spriteRenderer.flipY = true;
        //더이상 바닥과 닿지 않게하기
        capsuleCollider.enabled = false;
        //죽기전에 살짝 붕뜨게(점프) 하기
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        //일정시간 뒤에 없애기
        Invoke("DeActive", 5);
    }

    void DeActive()
    {
        //더이상 동작하지 않게 하기
        gameObject.SetActive(false);
    }
}
