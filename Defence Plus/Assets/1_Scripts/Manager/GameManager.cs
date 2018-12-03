using UnityEngine;
using UnityEngine.UI;

// 게임 전반적인 부분의 field, method 관리
public class GameManager : SingletonMonobehaviour<GameManager>
{
    private int initLife = 10;  // 초기로 주는 라이프

    public Text LifeText;       // 남은 라이프를 출력하는 텍스트
    private int life = 0;
    public int Life
    {
        get { return life; }
        set
        {
            life = value;
            LifeText.text = "Life : " + life;
        }
    }

    void Start()
    {
        InitLife();
    }

    // 라이프 초기화
    public void InitLife()
    {
        Life = initLife;
    }

    // 게임 중지
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    // 게임 재시작
    public void ContinueGame()
    {
        Time.timeScale = 1f;
    }

    // life 감소
    public void TakeLife()
    {
        Life--;
        if (Life == 0)
            GoToResult(GameState.GameOver);
    }

    // 게임 결과 띄우기
    private void GoToResult(GameState state)
    {
        PauseGame();
        GameUIController.Instance.OpenResultPanel(state.ToString());
        GameUIController.Instance.InvadeCastle();
    }
}
