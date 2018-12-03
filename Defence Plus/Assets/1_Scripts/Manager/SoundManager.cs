using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonMonobehaviour<SoundManager>
{
    public List<AudioClip> BGMs; // BGM 목록들
    public List<AudioClip> SFXs; // SFX 목록들
    AudioSource[] myAudio;       // 0 : BGM / 1 : 효과음

    // 이 매니저는 씬이 바뀌어도 삭제하지 않음
    protected override void OnAwake()
    {
        DontDestroyOnLoad(gameObject);
        myAudio = GetComponents<AudioSource>();  
    }

    // 음소거 상태
    public void Mute()
    {
        myAudio[0].mute = true;
        myAudio[1].mute = true;
    }
    
    // 음소거 상태 해제
    public void Replay()
    {
        myAudio[0].mute = false;
        myAudio[1].mute = false;
    }

    // 음소거 상태 여부
    public bool IsMute()
    {
        return myAudio[0].mute;
    }

    // BGM 타입에 따라 사운드 Play
    public void PlaySound(BGMType type)
    {
        myAudio[0].Stop();
        myAudio[0].clip = BGMs[(int) type];
        myAudio[0].Play();
    }

    // SFX 타입에 따라 사운드 Play
    public void PlaySound(SFXType type)
    {
        myAudio[1].Stop();
        myAudio[1].PlayOneShot(SFXs[(int)type]);
    }
}

