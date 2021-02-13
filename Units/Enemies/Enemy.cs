using UnityEngine;

public class Enemy : Unit
{
    [SerializeField] protected float attackCD = 1f;
    protected bool attackOnCD = false;

    //==================== PROTECTED ====================//
    protected virtual void ResetAttackCD()
    {
        attackOnCD = false;
    }
}
