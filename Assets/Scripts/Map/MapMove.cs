using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 맵 이동 스크립트
public class MapMove : MonoBehaviour
{
    // 플레이어가 직접 이동하는 것이 아닌 맵이 움직여 이동하도록 하기 위한 스크립트
    public float mapSpeed = 7;
    void Start()
    {
        
    }

    void Update()
    {
        // 왼쪽  화살표를 누르고 있으면 안 움직임
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            return;
        }
        else
        {
            // 맵이 왼쪽으로 이동
            transform.Translate(-mapSpeed * Time.deltaTime, 0, 0);
        }
    }
}
