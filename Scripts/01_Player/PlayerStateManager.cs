using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public bool IsDashing {get; set;}
    public bool IsAttacking {get; set;}
    public bool IsThrowing {get; set;}
    public bool IsJumping {get; set;}
    public bool IsGuarding { get; set; }
    public bool IsGuardHit { get; set; }
    public bool IsHurt { get; set; }


    public bool IsPerformingAction => IsDashing || IsAttacking || IsThrowing || IsGuarding || IsGuardHit || IsHurt;
    public bool CanMove => !IsDashing && !IsAttacking && !IsThrowing && !IsGuarding && !IsGuardHit && !IsHurt;


    public bool CanDash() => !IsAttacking && !IsThrowing && !IsGuarding && !IsHurt;
    public bool CanAttack() => !IsDashing && !IsThrowing && !IsGuarding && !IsHurt;
    public bool CanThrow() => !IsDashing && !IsAttacking && !IsGuarding && !IsHurt;
    public bool CanGuard() => !IsDashing && !IsAttacking && !IsThrowing && !IsHurt;

    public bool CanJump() => !IsGuarding && !IsHurt;
}