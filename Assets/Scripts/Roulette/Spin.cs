using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 룰렛 돌리기 스크립트
public class Spin : MonoBehaviour
{
	// 룰렛
	[SerializeField]
	private	Roulette roulette;
	// 돌리기 버튼
	[SerializeField]
	public	Button buttonSpin;
	// 맵으로 이동하는 버튼
	[SerializeField]
	private Button GoB;
	// 뒤로 가기 버튼
	[SerializeField]
	private Button BackB;

	// 오디오
    GameObject audio;
    AudioSource bgm;

	private void Awake()
	{
		GoB = GameObject.Find("GoButton").GetComponent<Button>();
		BackB = GameObject.Find("BackButton").GetComponent<Button>();
		audio = GameObject.Find("AudioManager");
        bgm = audio.GetComponent<AudioSource>();

		// 플레이어의 코인 수가 500 이상일 때 돌리기 버튼 활성화
		if(DataManager.Instance.coin >= 500)
		{
			// 돌리기 버튼 활성화
			buttonSpin.interactable = true;
		}
		else
		{
			// 돌리기 버튼 비활성화
			buttonSpin.interactable = false;
		}
		// 돌리기 버튼을 누르면
		buttonSpin.onClick.AddListener(()=>
		{
			// 오디오 재생
			bgm.Play();
			// 돌리기 버튼 비활성화
			buttonSpin.interactable = false;
			// Go! 버튼 비활성화
			GoB.interactable = false;
			// 뒤로가기 버튼 비활성화
			BackB.interactable = false;
			// 코인은 1000 감소
			DataManager.Instance.coin -= 500;
			// 룰렛 돌아가는 것이 멈춤
			roulette.Spin(EndOfSpin);
		});
	}

	private void EndOfSpin(RoulettePieceData selectedData)
	{
		// 오디오 멈춤
		bgm.Stop();

		// 룰렛이 멈추면 Go!, 뒤로가기 버튼 활성화
		GoB.interactable = true;
		BackB.interactable = true;

		Debug.Log($"{selectedData.index}:{selectedData.description}");
	}
}
