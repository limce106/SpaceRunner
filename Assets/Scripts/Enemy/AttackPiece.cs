using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 별 스크립트
public class AttackPiece : MonoBehaviour
{
    // 이동 속도
    public float speed = 4;
    // 조각의 공격력
    public int attackPower = 15;
    PlayerController pc;

    void Start()
    {
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        // 조각은 3초뒤에 없어짐
        Invoke("DestroyPiece", 3);
    }

    void Update()
    {
        // 별은 왼쪽으로 쏜다
        Vector3 dir = Vector3.left;
        transform.position += dir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어와 충돌했을 때 조각 사라짐
        if (other.gameObject.name == "Player")
        {
            Destroy(this.gameObject);
        }
    }

    void DestroyPiece()
    {
        Destroy(gameObject);
    }
}
