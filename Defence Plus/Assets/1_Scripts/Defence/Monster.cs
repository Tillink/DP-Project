using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterStatus Status;                    // 스탯. 들어가는 인터페이스에 따라 다른 스탯을 가진다.

    public MonsterState _state = MonsterState.None; //몬스터의 현재 상태
    private Unit target;                            // 공격할 타겟
    private Transform Goal;                         // 몬스터가 도착할 지점

    private Animator anim;                      

    void Awake()
    {
        anim = GetComponent<Animator>();
        Goal = GameObject.Find("Castle").transform;
    }

    void Update()
    {
        if (Status != null && _state != MonsterState.Die)
        {
            CheckHP();
            SearchTarget();
            MoveControl();
            AttackControl(); 
        }
    }

    // Active true가 될 시의 이벤트
    void OnEnable()
    {
        if (Status != null)
        {
            gameObject.layer = LayerMask.NameToLayer("Monster");
            Status.HP = Status.MaxHP;
            transform.localPosition = Status.MonsterSpawnPos;
            SetState(MonsterState.Walk);
        }
    }

    private void CheckHP()
    {
        // HP가 0으로 떨어지면 상태가 Die로 변함 
        if (Status.HP <= 0)
        {
            SetState(MonsterState.Die);
        }
    }

    private Vector2 moveDirection = Vector2.left;
    // RayCast를 이용해 탐색 반경 내에서 타겟을 탐색
    private void SearchTarget()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, Status.SearchDistance, 1 << LayerMask.NameToLayer("Unit"));

        if (hit.transform != null)
            target = hit.transform.gameObject.GetComponent<Unit>();
        else
            target = null;

        Debug.DrawRay(transform.position, moveDirection * Status.SearchDistance, Color.red);
    }

    private void MoveControl()
    {
        //목표 지점과 거리가 멀고, 싸우는 중이 아니면 걷는다.
        if (_state == MonsterState.Walk)
        {
            Vector2 dir = Goal.transform.position - transform.position;
            dir.Normalize();

            transform.Translate(dir * Status.Speed * Time.deltaTime);
        }
    }

    private void AttackControl()
    {
        // 타겟이 있을 경우에만 공격 태세로 변환한다.
        if (_state == MonsterState.Walk)
        {
            if (target != null)
                SetState(MonsterState.Attack);
        }
        else if (_state == MonsterState.Attack)
        {
            if (target == null)
                SetState(MonsterState.Walk);
        }
    }

    #region Animation Event
    // Attack Animation에서 한 번 실행되는 이벤트
    public void AttackEvent()
    {
        if (target != null)
        {
            target.Status.HP -= Status.ATK;
            SoundManager.Instance.PlaySound((SFXType)Status.MonsterKind);
        }
    }
    // Die Animation에서 한 번 실행되는 이벤트
    public void DieEvent()
    {
        ScoreManager.Instance.GetMonsterKillPoint();
        transform.gameObject.layer = 0;
        Invoke("DestroyMonster", 1f);
    }

    // 몬스터를 죽은 것으로 처리 
    private void DestroyMonster()
    {
        MonsterManager.Instance.RemoveMonster(this);
    }
    #endregion
    
    // 상태 변화와 함께 상태에 맞는 애니메이션을 틀어줌
    public void SetState(MonsterState state)
    {
        if (_state != state)
        {
            _state = state;
            anim.SetTrigger(_state.ToString());
        }
    }
}
