using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 버튼 스크립트
public class Buttons : MonoBehaviour
{
    ClickPlanets cp;
    // 돌리기 버튼
    Button spin;
    // 아이템 이미지를 위한 게임 오브젝트
	GameObject item;
    // Planets 씬에 있는 행성 버튼들
    Button uranus;
    Button neptune;
    Button saturn;
    Button jupiter;
    Button mars;
    // Planets 씬에 있는 자물쇠들
    GameObject locks;
    // Pluto 씬에 있는 조작키 설명 패널
    GameObject functionp;
    // Pluto 씬에 있는 조작키 설명 텍스트
    GameObject functiont;
    // 확인 버튼
    GameObject okButton;
    // 맵에 있는 멈춤 버튼
    GameObject pauseButton;
    // 시작 버튼
    Button startB;
    // Talk1, Talk2 씬에 있는 스킵 버튼
    Button skipB;
    // 뒤로 가기 버튼
    Button backB1;
    Button backB2;
    // 맵 이동 버튼
    Button goB;
    // 행성 목록으로 이동하는 버튼
    Button planetsB;
    // 나가기 버튼
    Button quitB;

    // 오디오
    GameObject audio;
    AudioSource bgm;

    void Awake()
    {
        cp = GameObject.Find("GameManager").GetComponent<ClickPlanets>();
        item = GameObject.Find("ItemImg");

        // Planets 행성 목록 씬에서
        if(SceneManager.GetActiveScene().name == "Planets")
        {
            uranus = GameObject.Find("Uranus").GetComponent<Button>();
            neptune = GameObject.Find("Neptune").GetComponent<Button>();
            saturn = GameObject.Find("Saturn").GetComponent<Button>();
            jupiter = GameObject.Find("Jupiter").GetComponent<Button>();
            mars = GameObject.Find("Mars").GetComponent<Button>();
            locks = GameObject.Find("Locks");

            // 처음에는 Pluto를 제외한 다른 행성(맵)에는 들어가지 못함
            if(cp.isUranus == false)
            {
                uranus.interactable = false;
                locks.transform.GetChild(0).gameObject.SetActive(true);
            }
            if(cp.isNeptune == false)
            {
                neptune.interactable = false;
                locks.transform.GetChild(1).gameObject.SetActive(true);
            }
            if(cp.isSaturn == false)
            {
                saturn.interactable = false;
                locks.transform.GetChild(2).gameObject.SetActive(true);
            }
            if(cp.isJupiter == false)
            {
                jupiter.interactable = false;
                locks.transform.GetChild(3).gameObject.SetActive(true);
            }
            if(cp.isMars == false)
            {
                mars.interactable = false;
                locks.transform.GetChild(4).gameObject.SetActive(true);
            }
        }

        // Pluto 씬일 때
        if(SceneManager.GetActiveScene().name == "Pluto")
        {
            // 시간 멈춤
            Time.timeScale = 0;
            // 조작키 설명 패널, 텍스트, 확인 버튼
            functionp = GameObject.Find("FunctionPanel");
            functiont = GameObject.Find("FunctionText");
            okButton = GameObject.Find("OkButton");
        }

        if(SceneManager.GetActiveScene().name == cp.PlanetName)
        {
            pauseButton = GameObject.Find("PauseButton");
            audio = GameObject.Find("AudioManager");
            bgm = audio.GetComponent<AudioSource>();
        }
    }

    void Update() 
    {
        UnlockStage();
    }

    // 행성 맵 잠금 해제 함수
    private void UnlockStage()
    {
        // Planets 씬 일 때
        if(SceneManager.GetActiveScene().name == "Planets")
        {
            uranus = GameObject.Find("Uranus").GetComponent<Button>();
            neptune = GameObject.Find("Neptune").GetComponent<Button>();
            saturn = GameObject.Find("Saturn").GetComponent<Button>();
            jupiter = GameObject.Find("Jupiter").GetComponent<Button>();
            mars = GameObject.Find("Mars").GetComponent<Button>();
            locks = GameObject.Find("Locks");

            // is(행성명)이 true이면 맵 잠금 해제
            if(cp.isUranus == true)
            {
                uranus.interactable = true;
                locks.transform.GetChild(0).gameObject.SetActive(false);
            }
            if(cp.isNeptune == true)
            {
                neptune.interactable = true;
                locks.transform.GetChild(1).gameObject.SetActive(false);
            }
            if(cp.isSaturn == true)
            {
                saturn.interactable = true;
                locks.transform.GetChild(2).gameObject.SetActive(false);
            }
            if(cp.isJupiter == true)
            {
                jupiter.interactable = true;
                locks.transform.GetChild(3).gameObject.SetActive(false);
            }
            if(cp.isMars == true)
            {
                mars.interactable = true;
                locks.transform.GetChild(4).gameObject.SetActive(false);
            }
        }
    }
   
    // 시작 버튼 함수
    public void StartButton()
    {
        // Talk1 씬으로 이동
        SceneManager.LoadScene("Talk1");
    }
    // 스킵 버튼 함수
    public void SkipButton()
    {
        // Talk1 씬 일 때
        if(SceneManager.GetActiveScene().name == "Talk1")
        {
            // Planets 씬으로 이동
            SceneManager.LoadScene("Planets");
        }
        // Talk1 씬 일 때
        else if(SceneManager.GetActiveScene().name == "Talk2")
        {
            // Start 씬으로 이동
            SceneManager.LoadScene("Start");
        }
    }
    // 뒤로 가기 버튼
    public void BackButton()
    {
        // Planets 씬이면
        if(SceneManager.GetActiveScene().name == "Planets")
        {
            // Start 씬으로 이동
            SceneManager.LoadScene("Start");
        }
        // Roulette 씬이면
        else if(SceneManager.GetActiveScene().name == "Roulette")
        {
            // Planets 씬으로 이동
            SceneManager.LoadScene("Planets");
            // 아이템 창 비우기
            item.GetComponent<SpriteRenderer>().sprite = null;
        }
    }
    // 맵 이동 버튼 함수
    public void GoButton()
    {
        // 클릭했던 행성 맵으로 이동
        SceneManager.LoadScene(cp.PlanetName);

        // Roulette 씬이면
        if(SceneManager.GetActiveScene().name == "Roulette")
        {
            spin = GameObject.Find("ButtonSpin").GetComponent<Button>();
            // 돌리기 버튼 활성화
            spin.interactable = true;
        }
    }
    // 조작키 설명 확인 버튼 함수
    public void FunctionOk()
    {
        // 조작키 설명 패널, 텍스트, 확인 버튼 비활성화
        functionp.SetActive(false);
        functiont.SetActive(false);
        okButton.SetActive(false);
        // 시간 흐름
        Time.timeScale = 1;
    }
    // 재생/멈추기 버튼 함수
    public void PauseButton()
    {
        // 스프라이트가 일시정지이고 시간이 흐르고 있을 때 버튼을 누르면
        if(pauseButton.GetComponent<Image>().sprite.name == "GUI_34" && Time.timeScale == 1)
        {
            // 멈춤
            Time.timeScale = 0;
            // 스프라이트는 재생으로 변경
            pauseButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("GUI_33") as Sprite;
            // bgm 일시정지
            bgm.Pause();
        }
        // 스프라이트가 재생이고 시간이 멈췄을 때 버튼을 누르면
        else if(pauseButton.GetComponent<Image>().sprite.name == "GUI_33" && Time.timeScale == 0)
        {
            // 시간 흐름
            Time.timeScale = 1;
            // 스프라이트는 일시정지로 변경
            pauseButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("GUI_34") as Sprite;
            // bgm 재생
            bgm.Play();
        }
    }
    // Planets 버튼 함수
    public void PlanetsButton()
    {
        // Planets 씬으로 이동
        SceneManager.LoadScene("Planets");
    }
    // 나가기 버튼 함수
    public void QuitButton()
    {
        // 게임 종료
        Application.Quit();
    }
    // 확인 버튼 함수
    public void OkButton()
    {
        // 점수, 맵마다 있는 행성 오브젝트(PlanetScore) 수 초기화
        DataManager.Instance.score = 0;
        DataManager.Instance.planets = 0;

        // 화성 맵을 GameClear 하면
        if(cp.PlanetName == "Mars" && SceneManager.GetActiveScene().name == "GameClear")
        {
            // Talk2 씬으로 이동
            SceneManager.LoadScene("Talk2");
        }
        else
        {
            // Planets 씬으로 이동
            SceneManager.LoadScene("Planets");
        }
    }
}
