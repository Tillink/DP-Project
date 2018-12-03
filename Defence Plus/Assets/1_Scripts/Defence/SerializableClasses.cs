using System;
using UnityEngine;
using UnityEngine.UI;

// 직렬화를 통해 인스펙터에서 제어 가능하도록 하기 위한 클래스들 (UnitManager에서 사용)


// 어떤 유닛을 위한 호출 버튼인 지 할당해주기 위해 만든 클래스
[Serializable]
public class UnitCallButton
{
    public UnitKind UnitType;
    public Button Button;
}

// 어떤 유닛의 Prefab인 지 할당해주기 위한 클래스
[Serializable]
public class UnitPrefab
{
    public UnitKind UnitType;
    public GameObject Object;
}
