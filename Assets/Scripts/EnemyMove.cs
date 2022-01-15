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
        //������ ������ �Ÿ��� �������� ����
        nextMove = Random.Range(-1, 2);
        //�� ĳ���� �ִϸ��̼� ����
        anim.SetInteger("WalkSpeed", nextMove);
        //��������Ʈ ���⿡ ���� ���ϸ��̼� ������
        if(nextMove!=0)
            spriteRenderer.flipX = nextMove == 1;
        //����Լ��� ��� �����ϰ� �ϱ�
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);

    }

    void turn()
    {
        //�Դٰ��� �����̰� �ϱ�
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke();
        Invoke("Think", 5);
    }

    public void OnDamaged()
    {
        //�ణ ��� �����
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        //�������� �������� �ִϸ��̼� ����
        spriteRenderer.flipY = true;
        //���̻� �ٴڰ� ���� �ʰ��ϱ�
        capsuleCollider.enabled = false;
        //�ױ����� ��¦ �ض߰�(����) �ϱ�
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        //�����ð� �ڿ� ���ֱ�
        Invoke("DeActive", 5);
    }

    void DeActive()
    {
        //���̻� �������� �ʰ� �ϱ�
        gameObject.SetActive(false);
    }
}
