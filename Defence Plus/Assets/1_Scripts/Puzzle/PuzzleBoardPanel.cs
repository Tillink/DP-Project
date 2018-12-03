using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 터치가 퍼즐 영역에서 벗어날 경우 풀던 퍼즐들을 자동 캔슬
public class PuzzleBoardPanel : MonoBehaviour 
{
    void OnMouseExit()
    {
        PuzzleManager.Instance.CancleAnswer();
    }
}
