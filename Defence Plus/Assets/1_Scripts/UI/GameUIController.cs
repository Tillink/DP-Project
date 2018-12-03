using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Game Scene의 UI를 담당
public class GameUIController : SingletonMonobehaviour<GameUIController>
{
    // 팝업창
    public GameObject PausePanel;
    public GameObject QuitPanel;
    public GameObject ResultPanel;
    public Text ResultText;         // 결과창의 결과를 출력하는 텍스트
    public Text ScoreText;          // 결과창의 점수를 출력하는 텍스트

    public Image CastleImage;       // 성 이미지
    public Sprite InvidedCastle;    // 무너진 성 Sprite
    public Image MusicImage;
    public Sprite[] MusicSprites;

    // Escape를 누를 경우를 위한 Panel Stack
    private Stack<GameObject> panels = new Stack<GameObject>(); 


    void Update()
    {
#if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 열린 팝업창이 없으면 일시정지 팝업창이 열리도록 함
            if (panels.Count == 0)
                OpenPausePanel();
            else
                Pop();
        }
#endif
    }

    // 팝업창을 열면서 stack에 추가
    public void Push(GameObject obj)
    {
        panels.Push(obj);
        obj.SetActive(true);
    }

    // 팝업창을 닫으면서 stack에서 꺼냄
    public void Pop()
    {
        GameObject obj = panels.Pop();
        obj.SetActive(false);
    }

    // 성 이미지를 변경
    public void InvadeCastle()
    {
        CastleImage.sprite = InvidedCastle;
    }

    // 결과 팝업창 오픈
    public void OpenResultPanel(string result)
    {
        ResultPanel.SetActive(true);
        ResultText.text = result;
        ScoreText.text = ScoreManager.Instance.Score.ToString();
    }

    #region 버튼항목
    // 일시정지 팝업창 오픈
    public void OpenPausePanel() 
    {
        GameManager.Instance.PauseGame();
        Push(PausePanel);
    }

    // 팝업창을 닫고 게임으로
    public void Continue() 
    {
        GameManager.Instance.ContinueGame();
        Pop();
    }

    // 메인메뉴로 이동
    public void GoToMainMenu() 
    {
        GameManager.Instance.ContinueGame();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        SoundManager.Instance.PlaySound(BGMType.Main);
    }

    // 사운드 On/Off
    public void SoundOnOff()
    {
        if (SoundManager.Instance.IsMute())
        {
            MusicImage.sprite = MusicSprites[0];
            SoundManager.Instance.Replay();
        }
        else
        {
            MusicImage.sprite = MusicSprites[1];
            SoundManager.Instance.Mute();
        }
    }

    // 게임 종료 팝업창 오픈
    public void OpenQuitPanel()
    {
        Push(QuitPanel);
    }

    // 게임 종료 팝업창 닫음
    public void CloseQuitPanel()
    {
        Pop();
    }

    // 게임 종료
    public void QuitApplication()
    {
        Application.Quit();
    }
    #endregion
}
