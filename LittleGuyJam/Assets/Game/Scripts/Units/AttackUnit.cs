using System.Collections;
using UnityEngine;

public class AttackUnit : Unit
{
    [SerializeField]
    CircleCollider2D protectionRange;

    [SerializeField]
    bool isAttacking;

    [SerializeField]
    Unit attackTarget;
    [SerializeField]
    Unit protectTarget;

    public AttackUnit(UnitAlignment _a) : base(_a)
    {
        ResetAttacker();
    }

    public void ResetAttacker()
    {
        isAttacking = false;
        attackTarget = null;
        protectTarget = null;
        Target = GameManager.instance.Barracks.transform.position;
        TargetAssigned = true;
    }

    public void UpdateAttacker()
    {
        if (CurrentState == UnitManager.UnitStates.Attack && !coroutineRunning && AutonomyBid())
        {
            StartCoroutine(Attack());
        }
        
        if (CurrentState == UnitManager.UnitStates.MoveAttack && !coroutineRunning && AutonomyBid())
        {
            StartCoroutine(MoveAttack());
        }
        
        if (CurrentState == UnitManager.UnitStates.Protect && !coroutineRunning && AutonomyBid())
        {
            StartCoroutine(Protect());
        }
        
        if (attackTarget != null || protectTarget != null)
        {
            TargetAssigned = true;
        }
        
        if (attackTarget == null && protectTarget == null)
        {
            TargetAssigned = false;
        }
    }

    public IEnumerator Attack()
    {
        coroutineRunning = true;

        if(Alignment == attackTarget.Alignment)
        {
            yield break;
        }

        if(isAttacking)
        {
            yield break;
        }

        if (Vector3.Distance(attackTarget.transform.position, transform.position) > data.AttackRange)
        {
            Target = attackTarget.gameObject.transform.position;
            TargetAssigned = true;
        }
        else
        {
            while (attackTarget.Health > 0)
            {
                isAttacking = true;
                CanMove = false;

                yield return new WaitForSeconds(data.AttackSpeed);
                CanMove = true;

                if (Health > 0)
                {
                    if (attackTarget.TakeDamage(data.AttackDamage) <= 0)
                    {
                        attackTarget = null;
                        break;
                    }
                }
            }

            isAttacking = false;
        }

        coroutineRunning = false;
    }

    public IEnumerator MoveAttack()
    {
        coroutineRunning = true;

        if(Alignment == attackTarget.Alignment)
        {
            yield break;
        }

        if(isAttacking)
        {
            yield break;
        }

        if (Vector3.Distance(attackTarget.transform.position, transform.position) > data.AttackRange)
        {
            Target = attackTarget.gameObject.transform.position;
            TargetAssigned = true;
        }
        else
        {
            while (attackTarget.Health > 0)
            {
                isAttacking = true;
                CanMove = false;

                yield return new WaitForSeconds(data.AttackSpeed);
                CanMove = true;

                if (Health > 0)
                {
                    if (attackTarget.TakeDamage(data.AttackDamage) <= 0) {
                        attackTarget = null;
                        break;
                    }
                }
            }

            isAttacking = false;
        }

        coroutineRunning = false;
    }

    public IEnumerator Protect()
    {
        coroutineRunning = true;

        if(Alignment != protectTarget.Alignment)
        {
            yield break;
        }

        if(isAttacking)
        {
            isAttacking = false;
        }

        if (Vector3.Distance(protectTarget.transform.position, transform.position) > data.AttackRange)
        {
            Target = protectTarget.gameObject.transform.position;
            TargetAssigned = true;
        }
        else
        {
            Unit nearestEnemy = gameObject.GetComponentInChildren<ProtectionCollider>().ReturnRandomTarget();

            if(nearestEnemy == null)
            {
                CanMove = false;
                yield break;    
            }

            Target = nearestEnemy.gameObject.transform.position;
            CanMove = true;
            TargetAssigned = true;

            yield return 0;
        }

        coroutineRunning = false;
    }

    public bool IsAttacking
    {
        get { return isAttacking; }
    }

}
