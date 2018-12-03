using UnityEngine;

public class MonsterManager : SingletonMonobehaviour<MonsterManager>
{
    public GameObject Monster;                  // Monster Prefab
    public ObjectPoolStack<Monster> monsters;   // 몬스터 Object Pool

    public Transform MonsterZone;               // 몬스터 스폰 위치

    private int sortingOrder = 1;               // 몬스터의 Sprite Layer 우선순위


    void Start()
    {
        ObjectPoolSetting();
        Invoke("CreateMonster", 30f);
    }

    // Monster 생성, 삭제를 Object Pool 방식으로 변경
    private void ObjectPoolSetting()
    {
        monsters = new ObjectPoolStack<Monster>(3, () =>
        {
            MonsterStatus MonsterStatus = new TrollStatus();

            GameObject obj = Instantiate(Monster, MonsterZone);
            obj.SetActive(false);

            Monster monster = obj.GetComponent<Monster>();
            monster.Status = MonsterStatus;

            return monster;
        });
    }

    // 몬스터 한 마리를 지정된 위치에 생성(활성화)
    private void CreateMonster()
    {
        Monster monster = monsters.GetObject();

        SpriteRenderer sr = monster.gameObject.GetComponent<SpriteRenderer>();
        sr.sortingOrder = sortingOrder++;
        
        monster.gameObject.SetActive(true);
        Invoke("CreateMonster", 5f);
    }

    // 몬스터가 죽었을 경우 비활성화
    public void RemoveMonster(Monster monster)
    {
        monster.gameObject.SetActive(false);
        monsters.ReturnObject(monster);
    }
}
