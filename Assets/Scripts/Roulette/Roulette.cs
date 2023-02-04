using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 룰렛 스크립트
public class Roulette : MonoBehaviour
{
	[SerializeField]
    // 룰렛에 표시되는 정보 프리팹
	private	Transform piecePrefab;
	[SerializeField]
    // 정보들을 구분하는 선 프리팹
	private	Transform linePrefab;
	[SerializeField]
    // 정보들이 배치되는 부모 Transform
	private	Transform pieceParent;
	[SerializeField]
    // 선들이 배치되는 부모 Transform
	private	Transform lineParent;
	[SerializeField]
    // 룰렛에 표시되는 정보 배열
	public	RoulettePieceData[]	roulettePieceData;
	[SerializeField]
	// 회전 시간
	private	int spinDuration;
	[SerializeField]
	// 실제 회전하는 회전판 Transform
	private	Transform spinningRoulette;
	[SerializeField]
	// 회전 속도 제어를 위한 그래프
	private	AnimationCurve spinningCurve;
    // 정보 하나가 배치되는 각도
	private	float pieceAngle;
    // 정보 하나가 배치되는 각도의 절반 크기
	private	float halfPieceAngle;
	// 선의 굵기를 고려한 Padding이 포함된 절반 크기
	private	float halfPieceAngleWithPaddings;

	// 가중치 계산을 위한 변수
	private	int accumulatedWeight;
	// 회전 여부
	private	bool isSpinning = false;
	// 룰렛에서 선택된 아이템
	public int selectedIndex = 0;
	// 아이템 이미지를 위한 게임 오브젝트
	GameObject item;
	ClickPlanets cp;

	private void Awake()
	{
		cp = GameObject.Find("GameManager").GetComponent<ClickPlanets>();
		item = GameObject.Find("ItemImg");
		// 룰렛 조각 하나의 각도를 구함
		pieceAngle = 360 / roulettePieceData.Length;
		// 룰렛 조각 하나의 절반 각도를 구함
		halfPieceAngle = pieceAngle * 0.5f;
		halfPieceAngleWithPaddings = halfPieceAngle - (halfPieceAngle * 0.25f);

		SpawnPiecesAndLines();
		CalculateWeightsAndIndices();
	}

	// 룰렛 조각과 선 오브젝트 생성 함수
	private void SpawnPiecesAndLines()
	{
		for ( int i = 0; i < roulettePieceData.Length; ++ i )
		{
			// 룰렛 조각 오브젝트 생성
			Transform piece = Instantiate(piecePrefab, pieceParent.position, Quaternion.identity, pieceParent);
			// 생성한 룰렛 조각의 정보 설정(아이콘, 설명)
			piece.GetComponent<RoulettePiece>().Setup(roulettePieceData[i]);
			// 생성한 룰렛 조각 회전
			piece.RotateAround(pieceParent.position, Vector3.back, (pieceAngle * i));
			// 룰렛 선 오브젝트 생성
			Transform line = Instantiate(linePrefab, lineParent.position, Quaternion.identity, lineParent);
			// 생성한 선 회전 (룰렛 조각 사이를 구분하는 용도)
			line.RotateAround(lineParent.position, Vector3.back, (pieceAngle * i) + halfPieceAngle);
		}
	}

	// 임의의 아이템 선택을 위한 아이템 인데스 가중치를 계산하는 함수
	private void CalculateWeightsAndIndices()
	{
		for ( int i = 0; i < roulettePieceData.Length; ++ i )
		{
			// 현재 배치되는 룰렛의 조각의 i번째 데이터 인덱스를 i로 설정
			roulettePieceData[i].index = i;

			// 예외 처리, chance 값이 0 이하라면 1로 처리(chance 값이 0이하이면 아이템이 선택되지 않기 때문)
			if ( roulettePieceData[i].chance <= 0 )
			{
				roulettePieceData[i].chance = 1;
			}

			// i번째 룰렛 조각 데이터의 chance(등장확률) 값을 accumulatedWeight에 더함
			// i번째 룰렛 조각 데이터의 weight(가중치)에 accumulatedWeight 값을 저장
			accumulatedWeight += roulettePieceData[i].chance;
			roulettePieceData[i].weight = accumulatedWeight;

			// Debug.Log($"({roulettePieceData[i].index}){roulettePieceData[i].description}:{roulettePieceData[i].weight}");
		}
	}

	private int GetRandomIndex()
	{
		// 0 ~ accumulatedWeight-1 중 임의의 숫자를 뽑아 weight에 저장
		int weight = Random.Range(0, accumulatedWeight);
		int i = 0;
		// for ( int i = 0; i < roulettePieceData.Length; ++ i )
		// {
		// 	// 방금 뽑은 데이터 값이 룰렛 조각 데이터 i번째의 weight보다 작으면
		// 	if ( roulettePieceData[i].weight > weight )
		// 	{
		// 		// 뽑힌 아이템의 순번 반환
		// 		return i;
		// 	}
		// }
		if(cp.PlanetName == "Pluto")
		{
			i = 2;
		}
		else if(cp.PlanetName == "Uranus")
		{
			i = 1;
		}
		else if(cp.PlanetName == "Neptune")
		{
			i = 0;
		}
		else if(cp.PlanetName == "Saturn")
		{
			i = 3;
		}
		else if(cp.PlanetName == "Jupiter")
		{
			i = 4;
		}
		else if(cp.PlanetName == "Mars")
		{
			i = 0;
		}
		return i;
	}

	public void Spin(UnityAction<RoulettePieceData> action=null)
	{
		if ( isSpinning == true ) return;

		// 룰렛의 결과 값 선택
		selectedIndex = GetRandomIndex();
		// 선택된 결과의 중심 각도
		float angle	= pieceAngle * selectedIndex;
		// 정확히 중심이 아닌 결과 값 범위 안의 임의의 각도 선택
		float leftOffset = (angle - halfPieceAngleWithPaddings) % 360;
		float rightOffset = (angle + halfPieceAngleWithPaddings) % 360;
		float randomAngle = Random.Range(leftOffset, rightOffset);

		// 목표 각도(targetAngle) = 결과 각도 + 360 * 회전 시간 * 회전 속도
		int	  rotateSpeed = 2;
		float targetAngle = (randomAngle + 360 * spinDuration * rotateSpeed);

		// Debug.Log($"SelectedIndex:{selectedIndex}, Angle:{angle}");
		// Debug.Log($"left/right/random:{leftOffset}/{rightOffset}/{randomAngle}");
		// Debug.Log($"targetAngle:{targetAngle}");

		// 룰렛 회전 중
		isSpinning = true;
		// targetAngle 값까지 회전하도록 OnSpin 코루틴 실행
		StartCoroutine(OnSpin(targetAngle, action));
	}

	// 룰렛을 돌리는 함수
	private IEnumerator OnSpin(float end, UnityAction<RoulettePieceData> action)
	{
		// 현재 시간
		float current = 0;
		float percent = 0;

		while ( percent < 1 )
		{
			// 시간 흐름
			current += Time.deltaTime;
			// percent = 현재 시간 / 회전 시간, 현재 시간과 회전 시간이 일치하면 회전 멈춤
			percent = current / spinDuration;

			// 시작 각도인 0도부터 end 각도까지 z축 회전
			float z = Mathf.Lerp(0, end, spinningCurve.Evaluate(percent));
			spinningRoulette.rotation = Quaternion.Euler(0, 0, z);

			yield return null;
		}

		// 회전하고 있지 않음
		isSpinning = false;

		// action이 null이 아니면
		if ( action != null )
		{
			// 룰렛의 회전이 종료되었을 때 호출하도록 등록해둔 메소드 호출
			// 매개변수로 선택된 룰렛 조각 정보 전달
			action.Invoke(roulettePieceData[selectedIndex]);

			// 선택된 아이템이 폭탄이 아니라면
			if(selectedIndex != 4)
			{
				// 선택된 아이템 이미지를 띄움
				item.GetComponent<SpriteRenderer>().sprite = roulettePieceData[selectedIndex].icon;
				//item.SetActive(true);
			}
			else
			{
				// 아이템 창 비우기
				item.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Blank") as Sprite;
			}
		}
	}
}
