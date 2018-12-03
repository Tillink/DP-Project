using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


public class KnightStatus : UnitStatus
{
    public UnitKind UnitKind { get; } = UnitKind.Knight;
    public int MaxHP { get; set; } = 15;
    public int HP { get; set; } = 15;
    public int ATK { get; } = 3;
    public float Speed { get; } = 1;
    public Vector2 UnitSpawnPos { get; } = new Vector2(-2.55f, 3.07f);
    public float SearchDistance { get; } = 0.5f;
    public Vector2 UnitGoalPos { get; } = new Vector2(-0.03f, 3.05f);
    public int UnitCost { get; } = 350;

    public void CallMethod()
    {
        ScoreManager.Instance.BuyUnit(UnitCost);
        UnitManager.Instance.CreateUnit(UnitKind);
        SoundManager.Instance.PlaySound(SFXType.Sold);
    }

    public bool CallCondition()
    {
        return ScoreManager.Instance.CanBuyUnit(UnitCost);
    }
}

public class ArcherStatus : UnitStatus
{
    public UnitKind UnitKind { get; } = UnitKind.Archer;
    public int MaxHP { get; set; } = 5;
    public int HP { get; set; } = 5;
    public int ATK { get; } = 2;
    public float Speed { get; } = 1;
    public Vector2 UnitSpawnPos { get; } = new Vector2(-2.55f, 3.07f);
    public float SearchDistance { get; } = 2.5f;
    public Vector2 UnitGoalPos { get; } = new Vector2(-2f, 3.07f);
    public int UnitCost { get; } = 500;

    public void CallMethod()
    {
        ScoreManager.Instance.BuyUnit(UnitCost);
        UnitManager.Instance.CreateUnit(UnitKind);
        SoundManager.Instance.PlaySound(SFXType.Sold);
    }

    public bool CallCondition()
    {
        return ScoreManager.Instance.CanBuyUnit(UnitCost);
    }
}

public class HeroStatus : UnitStatus
{
    public UnitKind UnitKind { get; } = UnitKind.Hero;
    public int MaxHP { get; set; } = 30;
    public int HP { get; set; } = 30;
    public int ATK { get; } = 7;
    public float Speed { get; } = 1;
    public Vector2 UnitSpawnPos { get; } = new Vector2(-2.55f, 3.60f);
    public float SearchDistance { get; } = 0.7f;
    public Vector2 UnitGoalPos { get; } = new Vector2(-0.03f, 3.60f);
    public int UnitCost { get; } = 1500;

    public void CallMethod()
    {
        UnitManager.Instance.CreateUnit(UnitKind);
        UnitManager.Instance.HeroCallCount++;
        UnitManager.Instance.CheckUnitCallConditions();
        SoundManager.Instance.PlaySound(SFXType.Sold);
    }

    private int possibleCallCount = 1;
    public bool CallCondition()
    {
        if (ScoreManager.Instance.TotalPuzzleCoin != 0 
            && ScoreManager.Instance.TotalPuzzleCoin % (UnitCost * possibleCallCount) == 0)
        {
            possibleCallCount++;
        }

        return possibleCallCount - UnitManager.Instance.HeroCallCount > 0 ? true : false;
    }
}

public class TrollStatus : MonsterStatus
{
    public MonsterKind MonsterKind { get; } = MonsterKind.Troll;
    public int MaxHP { get; set; } = 50;
    public int HP { get; set; } = 50;
    public int ATK { get; } = 5;
    public float Speed { get; } = 1;
    public Vector2 MonsterSpawnPos { get; } = new Vector2(2.5f, 3.1f);
    public float SearchDistance { get; } = 0.2f;
    public int KillPoint { get; } = 100;
}

