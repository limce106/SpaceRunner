using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어가 접촉 후 떨어지는 발판 스크립트
public class FallMap : MonoBehaviour
{
    // 플레이어가 접촉 후 떨어질 때까지의 시간 간격
    float fallSec = 0.2f;
    // 사라지는 시간
    float destroySec = 2f;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        // 플레이어와 접촉하면
        if(other.gameObject.name == "Player")
        {
            // 0.2초 후에 떨어짐
            Invoke("Fall", fallSec);
            // 2초 후에 사라짐
            Destroy(gameObject, destroySec);
        }
    }
    void Fall()
    {
        rb.isKinematic = false;
    }
}
