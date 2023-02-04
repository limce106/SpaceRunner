using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 변수 값을 유지하기 위한 싱글톤 스크립트
public class DataManager : MonoBehaviour
{
    static DataManager instance = null;
    public static DataManager Instance
    {
        get
        {
            if(null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    private void Awake() 
    {
        if(instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 점수
    public int score = 0;
    // 코인
    public int coin = 500;
    // 맵마다 있는 행성 오브젝트(PlanetScore) 수
    public int planets = 0;
}
