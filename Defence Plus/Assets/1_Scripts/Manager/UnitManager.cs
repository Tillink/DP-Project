using System.Collections.Generic;
using UnityEngine;

// 유닛을 생성, 관리하는 매니저
public class UnitManager : SingletonMonobehaviour<UnitManager>
{
    public List<UnitCallButton> UnitCallButtons;    // 유닛을 호출하기 위한 버튼들
    public List<UnitPrefab> Units;                  // 유닛으로 사용할 Prefab들
    // 유닛 종류 별 스탯 클래스들
    private Dictionary<UnitKind, UnitStatus> unitStatuses = new Dictionary<UnitKind, UnitStatus>();
    // 유닛 종류 별 ObjectPool class
    public Dictionary<UnitKind, ObjectPoolStack<Unit>> units = new Dictionary<UnitKind, ObjectPoolStack<Unit>>();

    public Transform UnitZone;                      // 유닛 스폰 위치

    private int sortingOrder = 1001;                // 유닛의 Sprite Layer 우선순위

    public int HeroCallCount { get; set; } = 1;     // 히어로 호출 횟수


    void Start ()
    {
        InitStatusSet();
        AddEventToButtons();
        CheckUnitCallConditions();
        ObjectPoolSetting();
    }

    // 유닛 생성, 삭제 방식을 Object Pool로 변경
    private void ObjectPoolSetting()
    {
        for (int i = 0; i < Units.Count; i++)
        {
            UnitKind unitKind = Units[i].UnitType;
            GameObject unitObj = Units[i].Object;

            units.Add((UnitKind)i, new ObjectPoolStack<Unit>(5, () =>
            {
                GameObject obj = Instantiate(unitObj, UnitZone);
                obj.SetActive(false);

                Unit unit = obj.GetComponent<Unit>();
                unit.Status = unitStatuses[unitKind];

                return unit;
            }));
        }
    }

    // Dictionary에 유닛 종류 별 넣어줄 스탯 클래스를 미리 등록
    private void InitStatusSet()
    {
        unitStatuses.Add(UnitKind.Knight, new KnightStatus());
        unitStatuses.Add(UnitKind.Archer, new ArcherStatus());
        unitStatuses.Add(UnitKind.Hero, new HeroStatus());
    }

    // UnitCallButton(UnitKind, Button) List의 원소마다 unitStatuses의 Key(유닛 종류)에 일치하는 스탯 클래스의 호출Method 연결
    private void AddEventToButtons()
    {
        foreach (var unitCallButton in UnitCallButtons)
        {
            unitCallButton.Button.onClick.AddListener(unitStatuses[unitCallButton.UnitType].CallMethod);
        }
    }
    
    // 모든 유닛 호출 버튼으로부터 호출 가능 여부를 체크
    public void CheckUnitCallConditions()
    {
        for (int i = 0; i < UnitCallButtons.Count; i++)
        {
            UnitCallButtons[i].Button.interactable = unitStatuses[UnitCallButtons[i].UnitType].CallCondition();
        }
    }

    // 매개변수로 받아온 스탯 클래스를 기반으로 Unit을 생성(활성화)
    public void CreateUnit(UnitKind unitKind)
    {
        Unit unit = units[unitKind].GetObject();

        SpriteRenderer sr = unit.gameObject.GetComponent<SpriteRenderer>();
        sr.sortingOrder = sortingOrder++;

        unit.gameObject.SetActive(true);        
    }

    // 유닛을 죽은 것으로 처리하고 비활성화
    public void RemoveUnit(Unit unit)
    {
        units[unit.Status.UnitKind].ReturnObject(unit);
        unit.gameObject.SetActive(false);
    }
}
