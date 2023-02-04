using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 룰렛 칸 스크립트
public class RoulettePiece : MonoBehaviour
{
    // 아이템 이미지
    [SerializeField]
    private Image imageIcon;
    // 아이템 설명
    [SerializeField]
    private Text textDesciption;

    // 룰렛 아이템 이미지, 설명 설정 함수
    public void Setup(RoulettePieceData pieceData)
    {
        imageIcon.sprite = pieceData.icon;
        textDesciption.text = pieceData.description;
    }
}
