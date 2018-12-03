using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Puzzle : MonoBehaviour
{
    const int POS_INTERVAL = 170;                   // 퍼즐 마다의 간격
    const int POS_X = -515;                         // 퍼즐의 초기 X 위치
    const int POS_Y = 505;                          // 퍼즐의 초기 Y 위치

    public int x;                                   // 퍼즐의 x좌표
    public int y;                                   // 퍼즐의 y좌표
    public int number;                              // 퍼즐의 숫자
    public Sprite puzzleImage;                      // 퍼즐 블럭의 이미지
    public Material[] materials = new Material[3];  // 퍼즐 블럭에 사용할 Material들

    public PuzzleState _state = PuzzleState.Normal;

    private Text text;
    private Image image;


    void Awake()
    {
        text = GetComponentInChildren<Text>();
        image = GetComponent<Image>();

        InitTriggerEvent();
    }

    // When Active true
    void OnEnable()
    {
        SetPuzzle(PuzzleState.Normal);
        SetPuzzleUI();
        SetPositionByXY();
    }

    // PuzzleManager로부터 받은 속성들을 UI에 적용
    private void SetPuzzleUI()
    {
        text.text = number.ToString();
        image.sprite = puzzleImage;
    }

    // Puzzl class의 x,y값에 따라 좌표값을 결정
    public void SetPositionByXY()
    {
        float pos_x = POS_X + (POS_INTERVAL * x);
        float pos_y = POS_Y - (POS_INTERVAL * y);
        transform.localPosition = new Vector3(pos_x, pos_y, 0);
    }

    #region TriggerEvent
    private void InitTriggerEvent()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();

        AddTriggerEvent(trigger, EventTriggerType.PointerEnter, PointerEnterEvent);
        AddTriggerEvent(trigger, EventTriggerType.PointerDown, PointerDownEvent);
        AddTriggerEvent(trigger, EventTriggerType.PointerUp, PointerUpEvent);
    }

    // 퍼즐에 트리거 이벤트 추가
    private void AddTriggerEvent(EventTrigger trigger, EventTriggerType eventType, UnityAction<BaseEventData> action)
    {
        EventTrigger.Entry e1 = new EventTrigger.Entry();
        e1.eventID = eventType;
        e1.callback.AddListener(action);
        trigger.triggers.Add(e1);
    }

    // PointerUp State(손가락을 뗀 상태)일 경우 그동안 선택된 퍼즐들을 전부 리셋
    private void PointerUpEvent(BaseEventData arg0)
    {
        if (PuzzleManager.Instance._state != PuzzleManagerState.BoardIsChanging
            && PuzzleManager.Instance.IsSelected(this))
        {
            PuzzleManager.Instance.SetState(PuzzleManagerState.Waiting);
            PuzzleManager.Instance.CancleAnswer();
        }
    }

    // PointerDown State(손가락으로 누른 상태)일 경우 퍼즐을 풀기 시작하는 상태로 인식시킴
    private void PointerDownEvent(BaseEventData arg0)
    {
        if (PuzzleManager.Instance._state == PuzzleManagerState.Waiting)
        {
            PuzzleManager.Instance.SetState(PuzzleManagerState.Answering);
            SetPuzzle(PuzzleState.Pressed);
            PuzzleManager.Instance.AddToAnswer(this);
        }
    }

    // PointerEnter State(손가락이 들어온 상태)일 경우 퍼즐을 검사해서 결과에 따라 사용자가 선택한 퍼즐들에 추가
    private void PointerEnterEvent(BaseEventData arg0)
    {
        if (_state == PuzzleState.Pressed)
        {
            PuzzleManager.Instance.CancleAnswer();
            return;
        }

        if(PuzzleManager.Instance._state == PuzzleManagerState.Answering)
        {
            PuzzleManager.Instance.AddToAnswer(this);
            if (PuzzleManager.Instance.IsSelected(this))
                SetPuzzle(PuzzleState.Pressed);
        }
    }
    #endregion
    
    // 퍼즐의 상태 변화
    public void SetPuzzle(PuzzleState state)
    {
        _state = state;
        image.material = materials[(int)state];
    }
}
