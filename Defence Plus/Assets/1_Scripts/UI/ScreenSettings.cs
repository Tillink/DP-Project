using UnityEngine;

public class ScreenSettings : MonoBehaviour 
{
    // It is in Title Scene Camera for First Screen Settings
    void Start () 
	{
        // 절전모드 해제
	    Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
