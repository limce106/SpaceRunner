using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 에너미 스크립트
public class EnemyFire : MonoBehaviour
{
    // 별을 생산할 공장
    public GameObject starFactory;
    // 발사 위치
    public GameObject firePosition; 

    // 현재시간
    float currentTime;
    // 일정시간
    float createTime = 2f;

    void Start() 
    {
        
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        // 현재시간이 일정시간이 되면
        if (currentTime > createTime)
        {
            // 공장에서 별을 만든다.
            GameObject star = Instantiate(starFactory);
            // 별을 발사
            star.transform.position = firePosition.transform.position;
            // 현재시간을 0으로 초기화
            currentTime = 0;
        }
    }
}
