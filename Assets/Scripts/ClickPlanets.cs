using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

// Planets 씬에서 클릭한 행성을 기억하고 행성 맵 잠금 해제를 일부 담당하는 스크립트
public class ClickPlanets : MonoBehaviour
{
    // Planets 씬에서 클릭한 행성 버튼의 이름
    public string PlanetName;
    // 한 행성을 클리어하면 다음 행성(맵)을 열기 위한 변수
    public bool isUranus = false;
    public bool isNeptune = false;
    public bool isSaturn = false;
    public bool isJupiter = false;
    public bool isMars = false;
    void Start()
    {
        // GameManager가 없어지는 것을 방지
        DontDestroyOnLoad(this.gameObject);
    }
    void Update() 
    {
        // Planets 씬일 때
        if(SceneManager.GetActiveScene().name == "Planets")
        {
            // 게임 오브젝트를 눌렀고 누른 게임 오브젝트가 null이나 BackButton이 아닐 때
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                if(EventSystem.current.currentSelectedGameObject.name != "BackButton")
                {
                    clickPlanet();
                }
            }
        }
    }

    void clickPlanet()
    {
        // 변수 PlanetName의 값은 플레이어가 누른 게임오브젝트의 이름
        PlanetName = EventSystem.current.currentSelectedGameObject.name;
        // Roulette 씬으로 이동
        SceneManager.LoadScene("Roulette");
    }
}
