using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TitleController : MonoBehaviour
{
    public GameObject ClickText;    // 사용자에게 클릭을 유도하는 텍스트
    public bool IsActive = false;   // 텍스트의 Active 상태 여부

    void Start()
    {
        SoundManager.Instance.PlaySound(BGMType.Main);
        ChangeText();
    }

    // 일정 간격으로 텍스트의 Active를 변환
    void ChangeText()
    {
        IsActive = !IsActive;
        ClickText.SetActive(IsActive);

        Invoke("ChangeText", 0.5f);
    }

    void Update () 
	{
        // 클릭하면 메인메뉴로 넘어가기
	    if (Input.GetMouseButtonDown(0))
	    {
	        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
	    }

        // 뒤로가기를 누를 경우 게임 종료
#if UNITY_ANDROID
	    if (Input.GetKeyDown(KeyCode.Escape))
	    {
	         Application.Quit();
	    }
#endif
    }
}
