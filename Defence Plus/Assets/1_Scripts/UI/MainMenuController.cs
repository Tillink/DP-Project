using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainMenuController : MonoBehaviour 
{
    // 팝업창
    public GameObject HowToPlayPanel;
    public GameObject NextHowToPlayPanel;
    public GameObject QuitPanel;
    public GameObject RankingPanel;

    // Escape를 누를 경우를 위한 Panel Stack
    private Stack<GameObject> panels = new Stack<GameObject>();

    
	void Update () 
	{
#if UNITY_ANDROID
	    if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 열린 팝업창이 없으면 게임 종료 팝업창이 열리도록 함
            if (panels.Count == 0)
                Push(QuitPanel);
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

    // 게임 씬으로 이동
    public void GoToGame() 
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    // 게임 방법 팝업창 열기
    public void OpenHowToPlayPanel() 
    {
        Push(HowToPlayPanel);
        NextHowToPlayPanel.SetActive(false);
    }

    // 게임 방법 팝업창 닫기
    public void CloseHowToPlayPanel()
    {
        Pop();
    }

    // 게임 방법에서 다음 페이지로 이동
    public void MoveToNextPage()
    {
        NextHowToPlayPanel.SetActive(true);
    }

    // 게임 방법에서 처음 페이지로 이동
    public void BackToFirstPage()
    {
        NextHowToPlayPanel.SetActive(false);
    }

    // 랭킹팝업창 오픈
    public void OpenRankingPanel()
    {
        Push(RankingPanel);
    }

    // 랭킹팝업창 닫기
    public void CloseRankingPanel()
    {
        Pop();
    }

    // 게임 종료 팝업창 오픈
    public void OpenQuitPanel() 
    {
        Push(QuitPanel);
    }

    // 게임 종료 팝업창 닫기
    public void CloseQuitPanel()
    {
        Pop();
    }

    // 게임 종료
    public void QuitApplication()
    {
        Application.Quit();
    }
}
