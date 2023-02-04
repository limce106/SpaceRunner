using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// �÷��̾� ��ũ��Ʈ
public class PlayerController : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    // ���� ��
    public float jumpForce = 600f;
    // ���ǿ� ��Ҵ��� ����
    bool isGround = false;
    // ���� �ϳ��� ��
    public int coinValue = 10;
    // �༺ ������Ʈ(PlanetScore) �ϳ��� ��
    public int scoreValue = 100;
    // ��ֹ��� �ε����� �� ���̴� hp ��
    public int obstacleValue = 10;
    // ü�¹�
    [SerializeField]
    private Slider hpbar;
    // �ִ� hp
    private float maxHp = 100;
    // ���� hp
    public float curHp = 100;
    ClickPlanets cp;
    // ������ â
	GameObject item;
    MapMove m;
    // ü���� ���� �� ȿ��
    public GameObject hitEffect;
    // ��ȣ�� ���� ����
    bool isShield = false;
    // ���ǵ�� ������ ��� �� ���ǵ�
    float upSpeed = 15f;
    // ���� ���ǵ�
    float originalSpeed = 7f;
    // ���ǵ��� �ڽ� ������Ʈ�� ������ �ִ� ���� ������Ʈ
    GameObject ground;
    // ���ε��� �ڽ� ������Ʈ�� ������ �ִ� ���� ������Ʈ
    GameObject coins;
    // �༺���� �ڽ� ������Ʈ�� ������ �ִ� ���� ������Ʈ
    GameObject planets;
    // ���
    GameObject flag;
    // ��ֹ����� �ڽ� ������Ʈ�� ������ �ִ� ���� ������Ʈ
    GameObject obstacle;
    // ����(ȸ����)
    GameObject potion;
    // ���� Ƚ��
    int jumpCount = 0;

    // �����
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
        Debug.Log("���� ��: "+coins.transform.childCount);
        Debug.Log("�༺ ��: "+planets.transform.childCount);
    }

    void Update()
    {
        KeyMove();
        ControlHp();
        UseItem();
        GameOver();
        hpbar.value = (float)curHp/(float) maxHp;
    }

    // ����Ű�� �����ϴ� �Լ�
    void KeyMove()
    {
        // �� ����Ű�� ������
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            // ���� �ִϸ��̼� ����
            anim.SetTrigger("isJump");
            // �÷��̾��� ũ��� �⺻
            transform.localScale = new Vector3(-1.7f, 1.7f, 0);

            // jumpCount�� 0�̰� ���ǿ� ������� ������
            if(jumpCount == 0 && !isGround)
            {
                return;
            }
            // jumpCount�� 2���� ������
            else if(jumpCount < 2)
            {
                // ���� ȿ���� ���
                audioSource.PlayOneShot(audioJump);
                // ����
                JumpAddForce();
                jumpCount++;
            }
        }
        // �Ʒ� ����Ű�� ������ ������
        else if(Input.GetKey(KeyCode.DownArrow))
        {
            // �����̵� �ִϸ��̼� ����
            anim.SetBool("isSliding", true);
            // �޸� �� �÷��̾��� ũ��� �����̵� ũ�Ⱑ ����Ͽ� y ���� ����
            transform.localScale = new Vector3(-1.7f, 1.1f, 0);
        }
        // ���� ����Ű�� ������
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
            // ���߱� �ִϸ��̼� ����
            anim.SetBool("isStop", true);
            // �÷��̾��� ũ��� �⺻
            transform.localScale = new Vector3(-1.7f, 1.7f, 0);
        }
        else
        {
            // �����̵�, ���߱� �ִϸ��̼� ���� �� ��
            anim.SetBool("isSliding", false);
            anim.SetBool("isStop", false);
            // �÷��̾��� ũ��� �⺻
            transform.localScale = new Vector3(-1.7f, 1.7f, 0);
        }

        // �Ʒ� ����Ű�� ������
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            // �����̵� ȿ���� ���
            audioSource.PlayOneShot(audioSlide);
        }
    }

    // ���� �Լ�
    void JumpAddForce()
    {
        // ����
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce);
    }

    // hp�� ���ݾ� ���̴� �Լ�
    private void ControlHp()
    {
        // 1�� �Ŀ� 1�ʸ��� ����
        InvokeRepeating("reduceHp", 1f, 1f);
    }
    // hp�� ���ݾ� ���̴� �Լ�
    private void reduceHp()
    {
        // hp�� �ð��� �帣�鼭 ���ݾ� �پ��
        curHp -= 0.01f * Time.deltaTime;
    }

    // �������� ����ϴ� �Լ�
    void UseItem()
    {
        // Ŭ���� �༺�� ���� ��
        if (SceneManager.GetActiveScene().name == cp.PlanetName)
        { 
            // �����̽��ٸ� ������
            if(Input.GetKeyDown(KeyCode.Space))
            {
                // �������� ������ ��
                if(item.GetComponent<SpriteRenderer>().sprite.name == "UI_Icon_Potion_2")
                {
                    // ü�� ȸ��
                    curHp += 10;
                    // ������ â ����
                    item.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Blank") as Sprite;
                }
                // �������� speedUp�� ��
                else if(item.GetComponent<SpriteRenderer>().sprite.name == "Running")
                {
                    // speedUp �ڷ�ƾ ����
                    StartCoroutine(speedUp());
                    // ������ â ����
                    item.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Blank") as Sprite;
                }
                // �������� Shield�� ��
                else if(item.GetComponent<SpriteRenderer>().sprite.name == "Shield")
                {
                    // Shield �ڷ�ƾ ����
                    StartCoroutine(Shield());
                }
            }
        }
    }

    // ���� ���� �Լ�
    private void GameOver()
    {
        // hp�� 0���ϰų� �÷��̾ ��������
        if(curHp <= 0 || this.gameObject.transform.position.y < -10)
        {
            // ���� �ʱ�ȭ
            DataManager.Instance.score = 0;
            // GameOver ������ �̵�
            SceneManager.LoadScene("GameOver");
            // ������ â ����
            item.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Blank") as Sprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // ����(ȸ����)�� ��ġ��
        if(other.gameObject.name == "Potion")
        {
            // ü�� ȸ��
            curHp += 15;
            other.gameObject.SetActive(false);
        }
        // ��߰� ��ġ��
        if(other.gameObject.name == "Flag")
        {
            // GameClear ������ �̵�
            SceneManager.LoadScene("GameClear");
            // ������ â ����
            item.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Blank") as Sprite;
        }
        // �༺(PlanetScore)�� ��ġ��, tag�� Planet�� ������Ʈ�� ��ġ��
        if(other.gameObject.tag == "Planet")
        {
            // ���� 100�� �߰�
            DataManager.Instance.score += scoreValue;
            Destroy(other.gameObject);
        }
        // ���ΰ� ��ġ��, tag�� Coin�� ������Ʈ�� ��ġ��
        if(other.gameObject.tag == "Coin")
        {
            // ������ â�� �����������
            if(item.GetComponent<SpriteRenderer>().sprite == null || item.GetComponent<SpriteRenderer>().sprite.name == "Blank")
            {
                // ���� ���� �״��
                coinValue = 10;
            }
            // �������� ���� 2���
            else if(item.GetComponent<SpriteRenderer>().sprite.name == "UI_Icon_Coin")
            {
                // ���� ���� 2��
                coinValue = 20;
            }
            // �������� ���� 2�谡 �ƴ� �ٸ� ���̸�
            else
            {
                // ���� ���� �״��
                coinValue = 10;
            }
            // ���� ȿ���� ���
            audioSource.PlayOneShot(audioCoin);
            // ���� �߰�
            DataManager.Instance.coin += coinValue;
            Destroy(other.gameObject);
        }
        // ��ֹ��� ��ġ��, tag�� Obstacle�� ������Ʈ�� ��ġ��
        if(other.gameObject.tag == "Obstacle")
        {
            // ü���� 0���� ũ�� ��ȣ���� ���� ������ �ʴٸ�
            if (curHp > 0 && isShield == false)
            {
                // Ÿ�� ȿ���� ���
                audioSource.PlayOneShot(audioDamaged);
                // PlayHitEffect �ڷ�ƾ ����
                StartCoroutine(PlayHitEffect());
                // ü�� 10 ����
                curHp -= obstacleValue;
            }
        }
        // ���� ��ġ��, tag�� Star�� ������Ʈ�� ��ġ��
        if(other.gameObject.tag == "Star")
        {
            // ü���� 0���� ũ�� ��ȣ���� ���� ������ �ʴٸ�
            if (curHp > 0 && isShield == false)
            {
                // Ÿ�� ȿ���� ���
                audioSource.PlayOneShot(audioDamaged);
                // PlayHitEffect �ڷ�ƾ ����
                StartCoroutine(PlayHitEffect());
                // ü�� 15 ����
                curHp -= 15;
            }
        }
        // ���ʹ̿� ��ġ��, tag�� Enemy�� ������Ʈ�� ��ġ��
        if(other.gameObject.tag == "Enemy")
        {
            // ���ʹ� �����
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        // ���ǰ� ������
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

    // ��ȣ�� �ڷ�ƾ
    IEnumerator Shield()
    {
        Debug.Log("��ȣ�� ���� ��");
        // ��ȣ�� ���� ���� true
        isShield = true;
        yield return new WaitForSeconds(5);
        // 5�� �� ��ȣ�� ���� ���� false
        isShield = false;
        // ������ â ����
        item.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Blank") as Sprite;
        Debug.Log("��ȣ�� ����");
    }

    // ���ǵ�� �ڷ�ƾ
    IEnumerator speedUp()
    {
        Debug.Log("���ǵ��");
        // ��ȣ�� ���� ���� true
        isShield = true;

        for(int i = 0; i< ground.transform.childCount; i++)
        {
            // ���ǵ� ���ǵ��
            ground.transform.GetChild(i).gameObject.GetComponent<MapMove>().mapSpeed = upSpeed;
        }
        for(int i = 0; i< coins.transform.childCount; i++)
        {
            // ���ε� ���ǵ��
            coins.transform.GetChild(i).gameObject.GetComponent<MapMove>().mapSpeed = upSpeed;
        }
        for(int i = 0; i< planets.transform.childCount; i++)
        {
            // �༺��(PlanetScore) ���ǵ��
            planets.transform.GetChild(i).gameObject.GetComponent<MapMove>().mapSpeed = upSpeed;
        }
        for(int i = 0; i< obstacle.transform.childCount; i++)
        {
            // ��ֹ��� ���ǵ��
            obstacle.transform.GetChild(i).gameObject.GetComponent<MapMove>().mapSpeed = upSpeed;
        }
        // ��� ���ǵ��
        flag.GetComponent<MapMove>().mapSpeed = upSpeed;
        // ������ Ȱ��ȭ�Ǿ��ִٸ�(������ ���� ���� ���¶��)
        if(potion.gameObject.activeSelf == true)
        {
             // ����(ȸ����) ���ǵ��
            potion.GetComponent<MapMove>().mapSpeed = upSpeed;
        }
        
        yield return new WaitForSeconds(5);

        // 5�� ��� ���� ���ǵ�� ���ƿ�
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
        // ������ Ȱ��ȭ�Ǿ��ִٸ�(������ ���� ���� ���¶��)
        if(potion.gameObject.activeSelf == true)
        {
            potion.GetComponent<MapMove>().mapSpeed = originalSpeed;
        }

        // ��ȣ�� ���� ���� false
        isShield = false;
        Debug.Log("��ȣ�� ����");
    }

    // �ǰ� �ڷ�ƾ
    IEnumerator PlayHitEffect()
    {
        // �ǰ� ȿ�� Ȱ��ȭ
        hitEffect.SetActive(true);

        // 0.3�� ��
        yield return new WaitForSeconds(0.3f);

        // �ǰ� ȿ�� ��Ȱ��ȭ
        hitEffect.SetActive(false);
    }
}
