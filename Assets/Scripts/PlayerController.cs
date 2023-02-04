using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 플레이어 스크립트
public class PlayerController : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    // 점프 힘
    public float jumpForce = 600f;
    // 발판에 닿았는지 여부
    bool isGround = false;
    // 코인 하나의 값
    public int coinValue = 10;
    // 행성 오브젝트(PlanetScore) 하나의 값
    public int scoreValue = 100;
    // 장애물에 부딪혔을 때 깎이는 hp 값
    public int obstacleValue = 10;
    // 체력바
    [SerializeField]
    private Slider hpbar;
    // 최대 hp
    private float maxHp = 100;
    // 현재 hp
    public float curHp = 100;
    ClickPlanets cp;
    // 아이템 창
	GameObject item;
    MapMove m;
    // 체력이 깎였을 때 효과
    public GameObject hitEffect;
    // 보호막 가동 여부
    bool isShield = false;
    // 스피드업 아이템 사용 시 스피드
    float upSpeed = 15f;
    // 기존 스피드
    float originalSpeed = 7f;
    // 발판들을 자식 오브젝트로 가지고 있는 게임 오브젝트
    GameObject ground;
    // 코인들을 자식 오브젝트로 가지고 있는 게임 오브젝트
    GameObject coins;
    // 행성들을 자식 오브젝트로 가지고 있는 게임 오브젝트
    GameObject planets;
    // 깃발
    GameObject flag;
    // 장애물들을 자식 오브젝트로 가지고 있는 게임 오브젝트
    GameObject obstacle;
    // 포션(회복약)
    GameObject potion;
    // 점프 횟수
    int jumpCount = 0;

    // 오디오
    public AudioClip audioJump;
    public AudioClip audioSlide;
    public AudioClip audioCoin;
    public AudioClip audioDamaged;
    AudioSource audioSource;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cp = GameObject.Find("GameManager").GetComponent<ClickPlanets>();
        item = GameObject.Find("ItemImg");
        m = GameObject.FindWithTag("Ground").GetComponent<MapMove>();
        ground = GameObject.Find("Ground");
        coins = GameObject.Find("Coins");
        planets = GameObject.Find("Planets");
        flag = GameObject.Find("Flag");
        obstacle = GameObject.Find("Obstacle");
        potion = GameObject.Find("Potion");
        audioSource = this.gameObject.GetComponent<AudioSource>();
        DataManager.Instance.planets = planets.transform.childCount;
        Debug.Log("코인 수: "+coins.transform.childCount);
        Debug.Log("행성 수: "+planets.transform.childCount);
    }

    void Update()
    {
        KeyMove();
        ControlHp();
        UseItem();
        GameOver();
        hpbar.value = (float)curHp/(float) maxHp;
    }

    // 방향키로 조작하는 함수
    void KeyMove()
    {
        // 위 방향키를 누르면
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            // 점프 애니메이션 실행
            anim.SetTrigger("isJump");
            // 플레이어의 크기는 기본
            transform.localScale = new Vector3(-1.7f, 1.7f, 0);

            // jumpCount가 0이고 발판에 닿아있지 않으면
            if(jumpCount == 0 && !isGround)
            {
                return;
            }
            // jumpCount가 2보다 작으면
            else if(jumpCount < 2)
            {
                // 점프 효과음 출력
                audioSource.PlayOneShot(audioJump);
                // 점프
                JumpAddForce();
                jumpCount++;
            }
        }
        // 아래 방향키를 누르고 있으면
        else if(Input.GetKey(KeyCode.DownArrow))
        {
            // 슬라이드 애니메이션 실행
            anim.SetBool("isSliding", true);
            // 달릴 때 플레이어의 크기와 슬라이드 크기가 비슷하여 y 값을 줄임
            transform.localScale = new Vector3(-1.7f, 1.1f, 0);
        }
        // 왼쪽 방향키를 누르면
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
            // 멈추기 애니메이션 실행
            anim.SetBool("isStop", true);
            // 플레이어의 크기는 기본
            transform.localScale = new Vector3(-1.7f, 1.7f, 0);
        }
        else
        {
            // 슬라이드, 멈추기 애니메이션 실행 안 함
            anim.SetBool("isSliding", false);
            anim.SetBool("isStop", false);
            // 플레이어의 크기는 기본
            transform.localScale = new Vector3(-1.7f, 1.7f, 0);
        }

        // 아래 방향키를 누르면
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            // 슬라이드 효과음 출력
            audioSource.PlayOneShot(audioSlide);
        }
    }

    // 점프 함수
    void JumpAddForce()
    {
        // 점프
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce);
    }

    // hp를 조금씩 줄이는 함수
    private void ControlHp()
    {
        // 1초 후에 1초마다 실행
        InvokeRepeating("reduceHp", 1f, 1f);
    }
    // hp를 조금씩 줄이는 함수
    private void reduceHp()
    {
        // hp는 시간이 흐르면서 조금씩 줄어듦
        curHp -= 0.01f * Time.deltaTime;
    }

    // 아이템을 사용하는 함수
    void UseItem()
    {
        // 클릭한 행성의 맵일 때
        if (SceneManager.GetActiveScene().name == cp.PlanetName)
        { 
            // 스페이스바를 누르면
            if(Input.GetKeyDown(KeyCode.Space))
            {
                // 아이템이 포션일 때
                if(item.GetComponent<SpriteRenderer>().sprite.name == "UI_Icon_Potion_2")
                {
                    // 체력 회복
                    curHp += 10;
                    // 아이템 창 비우기
                    item.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Blank") as Sprite;
                }
                // 아이템이 speedUp일 때
                else if(item.GetComponent<SpriteRenderer>().sprite.name == "Running")
                {
                    // speedUp 코루틴 실행
                    StartCoroutine(speedUp());
                    // 아이템 창 비우기
                    item.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Blank") as Sprite;
                }
                // 아이템이 Shield일 때
                else if(item.GetComponent<SpriteRenderer>().sprite.name == "Shield")
                {
                    // Shield 코루틴 실행
                    StartCoroutine(Shield());
                }
            }
        }
    }

    // 게임 오보 함수
    private void GameOver()
    {
        // hp가 0이하거나 플레이어가 떨어지면
        if(curHp <= 0 || this.gameObject.transform.position.y < -10)
        {
            // 점수 초기화
            DataManager.Instance.score = 0;
            // GameOver 씬으로 이동
            SceneManager.LoadScene("GameOver");
            // 아이템 창 비우기
            item.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Blank") as Sprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // 포션(회복약)과 겹치면
        if(other.gameObject.name == "Potion")
        {
            // 체력 회복
            curHp += 15;
            other.gameObject.SetActive(false);
        }
        // 깃발과 겹치면
        if(other.gameObject.name == "Flag")
        {
            // GameClear 씬으로 이동
            SceneManager.LoadScene("GameClear");
            // 아이템 창 비우기
            item.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Blank") as Sprite;
        }
        // 행성(PlanetScore)과 겹치면, tag과 Planet인 오브젝트와 겹치면
        if(other.gameObject.tag == "Planet")
        {
            // 점수 100점 추가
            DataManager.Instance.score += scoreValue;
            Destroy(other.gameObject);
        }
        // 코인과 겹치면, tag가 Coin인 오브젝트와 겹치면
        if(other.gameObject.tag == "Coin")
        {
            // 아이템 창이 비워져있으면
            if(item.GetComponent<SpriteRenderer>().sprite == null || item.GetComponent<SpriteRenderer>().sprite.name == "Blank")
            {
                // 코인 값은 그대로
                coinValue = 10;
            }
            // 아이템이 코인 2배면
            else if(item.GetComponent<SpriteRenderer>().sprite.name == "UI_Icon_Coin")
            {
                // 코인 값은 2배
                coinValue = 20;
            }
            // 아이템이 코인 2배가 아닌 다른 것이면
            else
            {
                // 코인 값은 그대로
                coinValue = 10;
            }
            // 코인 효과음 출력
            audioSource.PlayOneShot(audioCoin);
            // 코인 추가
            DataManager.Instance.coin += coinValue;
            Destroy(other.gameObject);
        }
        // 장애물과 겹치면, tag가 Obstacle인 오브젝트와 겹치면
        if(other.gameObject.tag == "Obstacle")
        {
            // 체력이 0보다 크고 보호막이 가동 중이지 않다면
            if (curHp > 0 && isShield == false)
            {
                // 타격 효과음 출력
                audioSource.PlayOneShot(audioDamaged);
                // PlayHitEffect 코루틴 실행
                StartCoroutine(PlayHitEffect());
                // 체력 10 감소
                curHp -= obstacleValue;
            }
        }
        // 별과 겹치면, tag가 Star인 오브젝트와 겹치면
        if(other.gameObject.tag == "Star")
        {
            // 체력이 0보다 크고 보호막이 가동 중이지 않다면
            if (curHp > 0 && isShield == false)
            {
                // 타격 효과음 출력
                audioSource.PlayOneShot(audioDamaged);
                // PlayHitEffect 코루틴 실행
                StartCoroutine(PlayHitEffect());
                // 체력 15 감소
                curHp -= 15;
            }
        }
        // 에너미와 겹치면, tag가 Enemy인 오브젝트와 겹치면
        if(other.gameObject.tag == "Enemy")
        {
            // 에너미 사라짐
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        // 발판과 닿으면
        if(other.gameObject.tag == "Ground")
        {
            isGround = true;
            jumpCount = 0;
        }
        else
        {
            isGround = false;
        }
    }

    // 보호막 코루틴
    IEnumerator Shield()
    {
        Debug.Log("보호막 가동 중");
        // 보호막 가동 여부 true
        isShield = true;
        yield return new WaitForSeconds(5);
        // 5초 후 보호막 가동 여부 false
        isShield = false;
        // 아이템 창 비우기
        item.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Blank") as Sprite;
        Debug.Log("보호막 해제");
    }

    // 스피드업 코루틴
    IEnumerator speedUp()
    {
        Debug.Log("스피드업");
        // 보호막 가동 여부 true
        isShield = true;

        for(int i = 0; i< ground.transform.childCount; i++)
        {
            // 발판들 스피드업
            ground.transform.GetChild(i).gameObject.GetComponent<MapMove>().mapSpeed = upSpeed;
        }
        for(int i = 0; i< coins.transform.childCount; i++)
        {
            // 코인들 스피드업
            coins.transform.GetChild(i).gameObject.GetComponent<MapMove>().mapSpeed = upSpeed;
        }
        for(int i = 0; i< planets.transform.childCount; i++)
        {
            // 행성들(PlanetScore) 스피드업
            planets.transform.GetChild(i).gameObject.GetComponent<MapMove>().mapSpeed = upSpeed;
        }
        for(int i = 0; i< obstacle.transform.childCount; i++)
        {
            // 장애물들 스피드업
            obstacle.transform.GetChild(i).gameObject.GetComponent<MapMove>().mapSpeed = upSpeed;
        }
        // 깃발 스피드업
        flag.GetComponent<MapMove>().mapSpeed = upSpeed;
        // 포션이 활성화되어있다면(포션을 먹지 않은 상태라면)
        if(potion.gameObject.activeSelf == true)
        {
             // 포션(회복약) 스피드업
            potion.GetComponent<MapMove>().mapSpeed = upSpeed;
        }
        
        yield return new WaitForSeconds(5);

        // 5초 모두 원래 스피드로 돌아옴
        for(int i = 0; i< ground.transform.childCount; i++)
        {
            ground.transform.GetChild(i).gameObject.GetComponent<MapMove>().mapSpeed = originalSpeed;
        }
        for(int i = 0; i< coins.transform.childCount; i++)
        {
            coins.transform.GetChild(i).gameObject.GetComponent<MapMove>().mapSpeed = originalSpeed;
        }
        for(int i = 0; i< planets.transform.childCount; i++)
        {
            planets.transform.GetChild(i).gameObject.GetComponent<MapMove>().mapSpeed = originalSpeed;
        }
        for(int i = 0; i< obstacle.transform.childCount; i++)
        {
            obstacle.transform.GetChild(i).gameObject.GetComponent<MapMove>().mapSpeed = originalSpeed;
        }
        flag.GetComponent<MapMove>().mapSpeed = originalSpeed;
        // 포션이 활성화되어있다면(포션을 먹지 않은 상태라면)
        if(potion.gameObject.activeSelf == true)
        {
            potion.GetComponent<MapMove>().mapSpeed = originalSpeed;
        }

        // 보호막 가동 여부 false
        isShield = false;
        Debug.Log("보호막 해제");
    }

    // 피격 코루틴
    IEnumerator PlayHitEffect()
    {
        // 피격 효과 활성화
        hitEffect.SetActive(true);

        // 0.3초 후
        yield return new WaitForSeconds(0.3f);

        // 피격 효과 비활성화
        hitEffect.SetActive(false);
    }
}
