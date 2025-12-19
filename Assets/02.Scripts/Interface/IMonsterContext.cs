using UnityEngine;
using UnityEngine.AI;

public interface  IMonsterContext
{
    IMonsterState State { get; }
    void ChangeState(IMonsterState newState);
    void MoveTo(Vector3 position);
    void MoveRaw(Vector3 velocity);
    void RotateToward(Vector3 direction);
    
    Transform Transform { get; }
    Animator Animator { get; }
    NavMeshAgent NavMeshAgent { get; }
    
    GameObject Player { get; }
    
    MonsterStats Stats { get; }
    
    Vector3 OriginPosition { get; }
    
    float AttackDistance { get; }
    float DetectDistance { get; }
    float PointReach { get; }
    float DeathDelay {get;}
    float KnockbackDrag {get;} 
    float PatrolInterval {get;}
    float PatrolRadius {get;}
}
