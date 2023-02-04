using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Talk1, Talk2에서 대사를 출력하기 위한 스크립트
public class TalkManager : MonoBehaviour
{
    // Talk1 씬 대사
    List<string> talk1 = new List<string>();
    // Talk2 씬 대사
    List<string> talk2 = new List<string>();
    // 대화창에 띄울 텍스트
    public Text talkText;
    // 대화창
    public GameObject talkPanel;
    // 대사 인덱스
    int index = 0;
    void Start()
    {
        // Talk1 씬 대사 추가
        talk1.Add("..........");
        talk1.Add("어디까지 와버린거지?");
        talk1.Add("저건 명왕성이잖아?");
        talk1.Add("..........");
        talk1.Add("음...한 행성씩 거쳐서 지구로 돌아가는 방법 밖엔 없는건가..?");
        talk1.Add("어쩔 수 없지. 어서 지구로 돌아가자!");
        talk1.Add("내가 지구로 돌아갈 수 있게 너도 도와줘!!");

        // Talk2 씬 대사 추가
        talk2.Add("야호! 드디어 지구에 도착했어!");
        talk2.Add("지구에 도착할 수 있게 도와줘서 고마워.");
        talk2.Add("지금까지 거쳐왔던 행성들은 다시 돌아갈 수 있어.");
        talk2.Add("원하는 행성들에 다시 가 봐.");
        talk2.Add("물론 나처럼 조난당하지 않게 조심하고!!");
        talk2.Add("그럼 안녕~!");

        // 대화창, 텍스트 활성화
        talkPanel.SetActive(true);
        talkText.gameObject.SetActive(true);
    }

    void Update()
    {
        // Talk1 씬일 때
        if(SceneManager.GetActiveScene().name == "Talk1")
        {
            Talk1();
        }
        // Talk2 씬일 때
        else if(SceneManager.GetActiveScene().name == "Talk2")
        {
            Talk2();
        }
    }

    // Talk1 씬 대사 출력 함수
    void Talk1()
    {
        // 텍스트는 Talk1 씬 대사
        talkText.text = talk1[index];

        // 스페이스바를 누르면
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // 대사가 넘어감
            index++;

            // 마지막 대사까지 출력된 후 스페이스바를 한 번 더 누르면
            if (index == 7 && Input.GetKeyDown(KeyCode.Space))
            {
                // Planets 씬으로 이동
                SceneManager.LoadScene("Planets");
                // 대사 인덱스 초기화
                index = 0;
            }
        }
    }

    // Talk2 씬 대사 출력 함수
    void Talk2()
    {
        // 텍스트는 Talk2 씬 대사
        talkText.text = talk2[index];

        // 스페이스바를 누르면
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // 대사가 넘어감
            index++;

            // 마지막 대사까지 출력된 후 스페이스바를 한 번 더 누르면
            if (index == 6 && Input.GetKeyDown(KeyCode.Space))
            {
                // Start 씬으로 이동
                SceneManager.LoadScene("Start");
                // 대사 인덱스 초기화
                index = 0;
            }
        }
    }
}
