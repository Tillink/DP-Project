using UnityEngine;

// DI(의존성 주입)을 위한 인터페이스 정의 부분

// 유닛 스탯
public interface UnitStatus
{
    UnitKind UnitKind { get; }

    int MaxHP { get; set; }
    int HP { get; set; }
    int ATK { get; }
    float Speed { get; }

    Vector2 UnitSpawnPos { get; }
    float SearchDistance { get; }
    Vector2 UnitGoalPos { get; }

    int UnitCost { get; }

    void CallMethod();
    bool CallCondition();
}

// 몬스터 스탯
public interface MonsterStatus
{
    MonsterKind MonsterKind { get; }

    int MaxHP { get; set; }
    int HP { get; set; }
    int ATK { get; }
    float Speed { get; }

    Vector2 MonsterSpawnPos { get; }
    float SearchDistance { get; }

    int KillPoint { get; }
}
