
// 디펜스퍼즐 전반적 게임의 상태
public enum GameState
{
    Playing,
    Pausing,
    GameOver,
    Win
}

// 유닛 종류
public enum UnitKind
{
    Knight = 0,
    Archer = 1,
    Hero = 2,
}

// 유닛의 상태
public enum UnitState
{
    None,
    Idle,
    Walk,
    Attack,
    Die
}

// 몬스터 종류
public enum MonsterKind
{
    Troll = 5,
}

// 몬스터의 상태
public enum MonsterState
{
    None,
    Walk,
    Attack,
    Die
}

// 퍼즐의 상태
public enum PuzzleState
{
    Normal = 0,
    Pressed = 1,
    Pop = 2,
}

public enum PuzzleColor
{
    Blue = 0,
    Green,
    Red
}

// 퍼즐매니저의 상태
public enum PuzzleManagerState
{
    Waiting = 0,
    Answering,
    BoardIsChanging,
    GameOver,
}

// BGM 종류
public enum BGMType
{
    Main = 0,
    Game,
}

// SFX 종류
public enum SFXType
{
    Knight = 0,
    Archer,
    Hero,
    Puzzle,
    Sold,
    Monster,
}
