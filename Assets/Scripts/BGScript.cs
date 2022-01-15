using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScript : MonoBehaviour
{
    //배경 흐르는 속도 조절
    public float speed = 0.3f;
    SpriteRenderer spr;
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //속도에 맞춰 왼쪽으로 서서히 이동하도록 하기
        transform.position += Vector3.left * Time.deltaTime * speed;
        Vector3 pos = transform.position;
        //일정 뒷 위치로 다다랐을 경우
        if (pos.x + spr.bounds.size.x / 2 < -8)
        {
            //앞 위치로 이동하여 다시 뒤로 흐르게 만들기
            float size = spr.bounds.size.x * 2;
            pos.x += size;
            transform.position = pos;
        }
    }
}
