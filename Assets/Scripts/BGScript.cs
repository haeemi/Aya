using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScript : MonoBehaviour
{
    //��� �帣�� �ӵ� ����
    public float speed = 0.3f;
    SpriteRenderer spr;
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //�ӵ��� ���� �������� ������ �̵��ϵ��� �ϱ�
        transform.position += Vector3.left * Time.deltaTime * speed;
        Vector3 pos = transform.position;
        //���� �� ��ġ�� �ٴٶ��� ���
        if (pos.x + spr.bounds.size.x / 2 < -8)
        {
            //�� ��ġ�� �̵��Ͽ� �ٽ� �ڷ� �帣�� �����
            float size = spr.bounds.size.x * 2;
            pos.x += size;
            transform.position = pos;
        }
    }
}
