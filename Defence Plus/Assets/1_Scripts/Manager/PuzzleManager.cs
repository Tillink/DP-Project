using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class PuzzleManager : SingletonMonobehaviour<PuzzleManager>
{
    private const int MAX_ROW = 5;          // 퍼즐판의 최대 행
    private const int MAX_COL = 5;          // 퍼즐판의 최대 열
    private const int MIN_QUESTION_NUM = 3; //문제의 최소치
    private const int MAX_QUESTION_NUM = 8; //문제의 최대치

    public GameObject Puzzle;               // 퍼즐 Prefab

    public Transform PuzzlePanel;           // 퍼즐이 생성될 위치

    public List<int> Percentages = new List<int>();                 // 해당 number 퍼즐이 등장할 확률 리스트

    public List<Sprite> ButtonImages;                               // 퍼즐 number 별로 전해줄 퍼즐 색깔 이미지
    public List<Material> PopMaterials = new List<Material>();      // 색깔별 퍼즐들이 터질 때 쓰는 Material 

    public PuzzleManagerState _state = PuzzleManagerState.Waiting;  // 퍼즐 게임의 현재 상태

    public Text QuestionText; // 퍼즐의 문제가 출력되는 텍스트
    private int question = 0; // 퍼즐게임에 제시되는 문제
    public int Question
    {
        get { return question; }
        set
        {
            question = value;
            QuestionText.text = question.ToString();
        }
    }

    private ObjectPoolStack<Puzzle> inactivePuzzles;
    private List<Puzzle> activePuzzles = new List<Puzzle>();    // 현재 판에 있는 퍼즐들 
    public List<Puzzle> selectedPuzzles = new List<Puzzle>();   // 사용자가 문제를 풀기 위해 선택한 퍼즐들

    private List<Puzzle> puzzlesToRemove = new List<Puzzle>();  // 삭제할 퍼즐들


    void Start ()
    {
        ObjectPoolSetting();
        InitPuzzleGame();
        ScoreManager.Instance.GivePuzzleCoinBonus();
        SoundManager.Instance.PlaySound(BGMType.Game);
    }

    // 퍼즐의 생성, 삭제 방식을 Object Pool로 변경
    private void ObjectPoolSetting()
    {
        inactivePuzzles = new ObjectPoolStack<Puzzle>(5, () =>
        {
            GameObject p = Instantiate(Puzzle, PuzzlePanel);
            p.SetActive(false);

            Puzzle pzl = p.GetComponent<Puzzle>();
            return pzl;
        });
    }

    // 퍼즐판의 모든 행과 열에 퍼즐들을 생성
    private void InitPuzzleGame()
    {
        for (int i = 1; i <= MAX_ROW; i++)
        {
            for (int j = 1; j <= MAX_COL; j++)
            {
                CreatePuzzle(j, i);
            }
        }

        GiveNewQuestion();
    }

    // 새로운 퍼즐을 생성
    private void CreatePuzzle(int x, int y)
    {
        Puzzle pzl = inactivePuzzles.GetObject();

        pzl.x = x;
        pzl.y = y;
        int number = CreateByPercentages();
        pzl.number = number;
        pzl.puzzleImage = ButtonImages[number - 1];
        pzl.materials[(int)PuzzleState.Pop] = PopMaterials[number - 1];

        pzl.gameObject.SetActive(true);

        activePuzzles.Add(pzl);
    }
    
    // 정해진 확률에 따라 숫자를 반환
    private int CreateByPercentages()
    {
        int max = Percentages.Sum(x => x);
        int number = 0;
        int num = Random.Range(1, max+1);

        for (int i = 0; i < Percentages.Count; i++)
        {
            number += Percentages[i];
            if (num <= number)
            {
                return i+1;
            }
        }

        return -1;
    }

    // 새로운 문제 제출
    public void GiveNewQuestion()
    {
        int q = Question;
        while (true)
        {
            Question = Random.Range(MIN_QUESTION_NUM, MAX_QUESTION_NUM + 1);
            if (q != Question)
                break;
        }
    }

    #region Method For Puzzle Trigger
    // 매개변수로 온 퍼즐을 사용자가 선택한 퍼즐들 리스트에 추가
    public void AddToAnswer(Puzzle pzl)
    {
        // 현재 선택된 퍼즐들의 근접 퍼즐이 아니면 추가할 수 없음
        if (selectedPuzzles.Count != 0)
        {
            Puzzle p = selectedPuzzles[selectedPuzzles.Count - 1];
            if (pzl.x < p.x - 1 || pzl.x > p.x + 1 || pzl.y < p.y - 1 || pzl.y > p.y + 1)
                return;
        }
        selectedPuzzles.Add(pzl);
        CheckAnswer();
    }

    // 해당 퍼즐을 사용자가 선택했는가의 여부 반환
    public bool IsSelected(Puzzle pzl)
    {
        return selectedPuzzles.Contains(pzl);
    }
    #endregion

    // 정답을 체크
    private void CheckAnswer()
    {
        int sum = 0;

        foreach (Puzzle pzl in selectedPuzzles)
        {
            sum += pzl.number;
        }

        //정답을 맞췄을 경우 퍼즐을 맞춘 것으로 처리
        if (sum == Question)
        {
            PuzzleIsSolved();
        }
        // 정답보다 숫자가 오버된 경우 지금까지 선택된 퍼즐들을 자동 캔슬
        else if (sum > Question)
        {
            CancleAnswer();
        }     
    }

    // 사용자가 정답을 풀었을 경우 퍼즐판을 변화시키고 퍼즐골드 증가
    private void PuzzleIsSolved()
    {
        SetState(PuzzleManagerState.BoardIsChanging);
        Invoke("GiveNewQuestion", 0.3f);
        RemovePuzzles();

        ScoreManager.Instance.GivePuzzleCoin();
        SoundManager.Instance.PlaySound(SFXType.Puzzle);
    }

    
    // 퍼즐을 삭제시키며 파티클 생성
    private void RemovePuzzles()
    {
        for (int i = selectedPuzzles.Count-1; i >= 0; i--)
        {
            Puzzle pzl = selectedPuzzles[i];
            pzl.SetPuzzle(PuzzleState.Pop);

            puzzlesToRemove.Add(pzl);
            selectedPuzzles.Remove(pzl);
            activePuzzles.Remove(pzl);

            // Particle을 모아둔 Pool로부터 Particle을 받아옴
            ParticleManager.Instance.ShowParticle((PuzzleColor)pzl.number-1, pzl.transform);

            StartCoroutine(RemovePuzzle(pzl, 0.5f));
        }

        GetPuzzlesInfoToRemove();
        RefillPuzzles();
        StartCoroutine("ShiftPuzzlesDown");
    }

    // 삭제해야할 퍼즐들 삭제(비활성화)
    IEnumerator RemovePuzzle(Puzzle pzl, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        inactivePuzzles.ReturnObject(pzl);
        pzl.gameObject.SetActive(false);
    }


    private Dictionary<int, int> removedPoses;      // Key : x좌표 Value : x좌표 별 지워진 퍼즐들 중 가장 밑에 위치한 퍼즐의 y좌표
    private Dictionary<int, int> removedAmounts;    // Key : x좌표 Value : x좌표 별 지워진 퍼즐의 양
    // 삭제시킬 퍼즐을 담아둔 리스트로부터 퍼즐이 얼마나 삭제되는 지 정보를 파악한다.
    private void GetPuzzlesInfoToRemove()
    {
        //linq를 통해 지워진 퍼즐을 x그룹으로 묶어 받아옴
        var query = from puzzle in puzzlesToRemove
                     group puzzle by puzzle.x into g
                     select new
                     {
                         key = g.Key,
                         Count = g.Count(),
                         MaxValue = g.OrderByDescending(gx => gx.y).FirstOrDefault().y
                     };

        removedPoses = query.ToDictionary(g => g.key, g => g.MaxValue);
        removedAmounts = query.ToDictionary(g => g.key, g => g.Count);
    }

    // 퍼즐이 사라진 곳에 새로운 퍼즐들을 생성시켜 줌
    private void RefillPuzzles()
    {
        for (int i = 1; i <= activePuzzles.Count; i++)
        {
            if (!removedAmounts.ContainsKey(i)) continue;

            // 맨 위에서 있다가 내려오기 위한 배치 방식
            for (int j = 0; j < removedAmounts[i]; j++)
                CreatePuzzle(i, 0 - j);
        }
    }

    // 삭제된 퍼즐들 자리를 채우기 위해 퍼즐들이 아래로 내려옴
    IEnumerator ShiftPuzzlesDown()
    {
        yield return new WaitForSeconds(0.5f);

        float delayTime = 0.07f;

        // 가장 많은 퍼즐이 사라진 x 좌표 탐색
        int max_amount = removedAmounts.Max(x => x.Value);
        
        // 사라지지 않은 퍼즐들 중 퍼즐이 사라진 x좌표에 관여되어 있는 퍼즐을 전부 가져옴
        var query2 = from x in activePuzzles
                     where removedPoses.ContainsKey(x.x)
                     select x;

        List<Puzzle> upPuzzles = query2.Where(x => x.y < removedPoses[x.x]).ToList();


        // 가져온 퍼즐들을 퍼즐이 사라진 양만큼 Shift Down
        for (int i = 1; i <= max_amount; i++)
        {
            yield return new WaitForSeconds(delayTime);
            foreach (Puzzle pzl in upPuzzles)
            {
                if (removedAmounts[pzl.x] >= i)
                {
                    if (PuzzleCanMoveToDown(pzl))
                    {
                        pzl.y++;
                        pzl.SetPositionByXY();
                    }
                }
            }
        }

        // 리스트를 비우고 퍼즐을 풀 수 있도록 함
        puzzlesToRemove.Clear();
        SetState(PuzzleManagerState.Waiting);
    }


    // 밑으로 내려가도 되는 퍼즐인 지에 대한 여부 반환
    private bool PuzzleCanMoveToDown(Puzzle pzl)
    {
        if (pzl.y == 5) return false;

        int i = 1;
        while (pzl.y + i <= 5)
        {
            var query = from x in activePuzzles
                where x.x == pzl.x & x.y == pzl.y + i
                select x;

            if (query.Count() == 0)
                return true;

            i++;
        }

        return false;
    }

    // 사용자가 선택한 퍼즐들을 전부 초기화
    public void CancleAnswer()
    {
        _state = PuzzleManagerState.Waiting;

        foreach (var pzl in selectedPuzzles)
        {
           pzl.SetPuzzle(PuzzleState.Normal);
        }
        selectedPuzzles.Clear();
    }


    public void SetState(PuzzleManagerState state)
    {
        _state = state;
    }
}
