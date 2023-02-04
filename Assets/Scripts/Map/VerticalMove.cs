using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 수직으로 왕복하는 발판 스크립트
public class VerticalMove : MonoBehaviour
{
    // 최대 위로 이동 위치
    float upMax = 3.0f;
    // 최소 아래롤 이동 위치
    float downMax = -6.0f;
    // 현재 y의 위치
    float curPos;
    // 이동 속도
    float moveSpeed = 4.0f;

    void Start()
    {
        curPos = transform.position.y;
    }

    void Update()
    {
        curPos += Time.deltaTime * moveSpeed;

        // 현재 y의 위치가 최대 위치보다 크거나 같으면
        if(curPos >= upMax)
        {
            // 이동 방향 전환
            moveSpeed *= -1;
            curPos = upMax;
        }
        // 현재 y의 위치가 최소 위치보다 작거나 같으면
        else if(curPos <= downMax)
        {
            // 이동 방향 전환
            moveSpeed *= -1;
            curPos = downMax;
        }

        // x 위치는 그대로, y 위치만 바뀜
        transform.position = new Vector3(transform.position.x, curPos, 0);
    }
}
