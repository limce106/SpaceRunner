using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoulettePieceData : MonoBehaviour
{
    // 아이콘 이미지
    public Sprite icon;
    // 이름, 속성, 능력치 등의 정보
    public string description;
    // 등장확률의 합은 200
    [Range(1, 100)]
    // 등장확률
    public int chance = 100;
    [HideInInspector]
    // 아이템 순번
    public int index;
    [HideInInspector]
    // 가중치
    public int weight;
}
