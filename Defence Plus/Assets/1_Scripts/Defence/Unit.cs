using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour 
{
    public UnitStatus Status;                   // 스탯. 들어가는 인터페이스에 따라 다른 스탯을 가진다.

    public UnitState _state = UnitState.None;   // 유닛의 현재 상태
    private Monster target;                     // 공격할 타겟
    
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Status != null && _state != UnitState.Die)
        {
            CheckHP();
            SearchTarget();
            MoveControl();
            AttackControl();
        }
    }

    // Active true일 경우 실행
    public void OnEnable()
    {
        // 유닛의 상태 초기화
        if (Status != null)
        {
            gameObject.layer = LayerMask.NameToLayer("Unit");
            SetState(UnitState.Idle);
            Status.HP = Status.MaxHP;
            transform.localPosition = Status.UnitSpawnPos; 
        }
    }

    private void CheckHP()
    {
        // HP가 0으로 떨어지면 상태가 Die로 변함 
        if (Status.HP <= 0)
        {
            SetState(UnitState.Die);
        }
    }

    Vector2 moveDirection = Vector2.right;
    // RayCast를 이용해 탐색 반경 내에서 타겟을 탐색
    private void SearchTarget()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, Status.SearchDistance, 1 << LayerMask.NameToLayer("Monster"));

        if (hit.transform != null)
            target = hit.transform.gameObject.GetComponent<Monster>();
        else
            target = null;

        Debug.DrawRay(transform.position, moveDirection * Status.SearchDistance, Color.red);
    }

    private void MoveControl()
    {
        //목표 지점과 거리가 멀고, 싸우는 중이 아니면 걷는다.
        if (_state == UnitState.Idle || _state == UnitState.Walk)
        {
            Vector2 dir = Status.UnitGoalPos - (Vector2)transform.position;
            dir.Normalize();

            if (Vector2.Distance(transform.position, Status.UnitGoalPos) < 0.1f)
                SetState(UnitState.Idle);
            else
                SetState(UnitState.Walk);


            if (_state == UnitState.Walk)
                transform.Translate(dir * Status.Speed * Time.deltaTime);
        }
    }

    private void AttackControl()
    {
        // 타겟이 있을 경우에만 공격 태세로 변환한다.
        if (_state == UnitState.Idle || _state == UnitState.Walk)
        {
            if (target != null)
                SetState(UnitState.Attack);
        }
        else if (_state == UnitState.Attack)
        {
            if (target == null)
                SetState(UnitState.Idle);
        }
    }

    #region Animation Event
    // Attack Animation에서 한 번 실행되는 이벤트
    public void AttackEvent()
    {
        if (target != null)
        {
            target.Status.HP -= Status.ATK;
            PlayAttackSound();
        }
    }

    // Die Animation에서 한 번 실행되는 이벤트
    public void DieEvent()
    {
        transform.gameObject.layer = 0;
        Invoke("DestroyUnit", 1f);
    }

    private void PlayAttackSound()
    {
        SoundManager.Instance.PlaySound((SFXType)Status.UnitKind);
    }

    // 유닛을 죽은 것으로 처리
    private void DestroyUnit()
    {
        UnitManager.Instance.RemoveUnit(this);
    }
    #endregion


    // 상태 변화와 함께 상태에 맞는 애니메이션을 틀어줌
    public void SetState(UnitState state)
    {
        if (_state != state)
        {
            _state = state;
            anim.SetTrigger(_state.ToString());
        }
    }
}
