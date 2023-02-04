using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 코인, 점수를 관리하고 게임 클리어 후를 관리하는 함수
public class RecordManager : MonoBehaviour
{
    // 코인 수를 나타내는 텍스트
    public Text coinText;
    // 점수를 나타내는 텍스트
    public Text scoreText;
    // 게임 클리어씬에서 나타나는 텍스트
    Text Next;
    ClickPlanets cp;
    PlayerController pc;
    // 메달
    GameObject medal;

    void Start()
    {
        cp = GameObject.Find("GameManager").GetComponent<ClickPlanets>();
        pc = GetComponent<PlayerController>();
    }

    void Update()
    {
        // Planets, Roulette, 눌렀던 행성의 맵 씬일 때
        if(SceneManager.GetActiveScene().name == "Planets" || SceneManager.GetActiveScene().name == "Roulette"
            || SceneManager.GetActiveScene().name == cp.PlanetName)
        {
            coinText = GameObject.Find("CoinText").GetComponent<Text>();
            // 코인 수 표시
            coinText.text = "" + DataManager.Instance.coin;
        }
        // GameClear 씬일 때
        if(SceneManager.GetActiveScene().name == "GameClear")
        {
            scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
            Next = GameObject.Find("Next").GetComponent<Text>();
            medal = GameObject.Find("MedalImg");

            // 점수 표시
            scoreText.text = "" + DataManager.Instance.score;

            // 점수가 플레이했던 맵의 최대 점수의 1/3보다 작을 때
            if(DataManager.Instance.score < (int)DataManager.Instance.planets*100/3)
            {
                // 동메달
                medal.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("GUI_6") as Sprite;
            }
            // 점수가 플레이했던 맵의 최대 점수의 1/3보다 크고 2/3보다 작을 때
            else if(DataManager.Instance.score < (int)DataManager.Instance.planets*100/3*2)
            {
                // 은메달
                medal.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("GUI_5") as Sprite;
            }
            // 점수가 플레이했던 맵의 최대 점수의 2/3보다 클 때
            else
            {
                // 금메달
                medal.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("GUI_4") as Sprite;
            }

            // 다음 맵에 갈 수 있도록 다음 행성 잠금 해제
            if(cp.PlanetName == "Pluto")
            {
                cp.isUranus = true;
            }
            else if(cp.PlanetName == "Uranus")
            {
                cp.isNeptune = true;
            }
            else if(cp.PlanetName == "Neptune")
            {
                cp.isSaturn = true;
            }
            else if(cp.PlanetName == "Saturn")
            {
                cp.isJupiter = true;
            }
            else if(cp.PlanetName == "Jupiter")
            {
                cp.isMars = true;
            }

            // 화성을 제외한 나머지 행성들에서 게임 클리어 텍스트가 같음
            if(cp.PlanetName == "Mars")
            {
                Next.text = "야호!\n드디어 지구에 도착했어!!";
            }
            else
            {
                Next.text = "좋았어!지구에 점점 가까워지고 있어\n다음 행성으로 가보자!!";
            }
        }
    }
}
