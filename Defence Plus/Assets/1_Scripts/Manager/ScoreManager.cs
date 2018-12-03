using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : SingletonMonobehaviour<ScoreManager>
{
    private int initPuzzleCoin = 1000;  // 초기에 보너스로 주는 퍼즐코인
    private int puzzleReward = 100;     // 퍼즐을 맞췄을 경우의 보상

    private int questionCost = 50;      // 퍼즐 문제를 초기화 할 경우의 코스트

    private int monsterKillPoint = 100; // 몬스터를 죽였을 경우 받게 되는 점수

    public Text ScoreText; // 게임 점수를 출력하는 텍스트
    private int score = 0; // 게임 중 획득한 점수
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            ScoreText.text = "점수 : " + score.ToString();
        }
    }

    private int totalPuzzleCoin = 0;
    public int TotalPuzzleCoin
    {
        get { return totalPuzzleCoin; }
        set
        {
            totalPuzzleCoin = value;
        }
    }

    public Text PuzzleCoinText; // 퍼즐로 얻은 재화량을 출력하는 텍스트
    private int puzzleCoin = 0; // 퍼즐 재화량
    public int PuzzleCoin
    {
        get { return puzzleCoin; }
        set
        {
            puzzleCoin = value;
            PuzzleCoinText.text = puzzleCoin.ToString();
            UnitManager.Instance.CheckUnitCallConditions();
        }
    }

    // 초기에 보너스로 퍼즐 코인을 줌
    public void GivePuzzleCoinBonus()
    {
        PuzzleCoin += initPuzzleCoin;
    }

    // 퍼즐을 풀었을 때 퍼즐 코인 추가
    public void GivePuzzleCoin()
    {
        TotalPuzzleCoin += puzzleReward;
        PuzzleCoin += puzzleReward;
    }

    // 유닛을 구매할 경우 비용만큼 퍼즐 코인 감소
    public void BuyUnit(int cost)
    {
        PuzzleCoin -= cost;
    }

    // 비용을 지불하고 퍼즐의 문제를 Reset
    public void BuyQuestion()
    {
        if (PuzzleCoin - questionCost >= 0)
        {
            PuzzleCoin -= questionCost;
            PuzzleManager.Instance.GiveNewQuestion();
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    // 현재 유저가 보유한 퍼즐 코인으로 유닛의 구매가 가능한 지의 여부 반환
    public bool CanBuyUnit(int cost)
    {
        return PuzzleCoin - cost >= 0 ? true : false;
    }

    // 몬스터가 죽을 경우 스코어 추가
    public void GetMonsterKillPoint()
    {
        Score += monsterKillPoint;
    }
}
