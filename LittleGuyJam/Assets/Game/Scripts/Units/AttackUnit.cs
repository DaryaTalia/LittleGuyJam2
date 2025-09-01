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
        attackTarget = null;
        protectTarget = null;
    }

    public void UpdateAttacker()
    {
        if (CurrentState == UnitManager.UnitStates.Attack)
        {
            StartCoroutine(Attack());
        }
        else if (CurrentState == UnitManager.UnitStates.MoveAttack)
        {
            StartCoroutine(MoveAttack());
        }
        else if (CurrentState == UnitManager.UnitStates.Protect)
        {
            StartCoroutine(Protect());
        }
        else if (attackTarget != null || protectTarget != null)
        {
            TargetAssigned = true;
        }
        else if (attackTarget == null && protectTarget == null)
        {
            TargetAssigned = false;
        }
    }

    public IEnumerator Attack()
    {
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
            //StartCoroutine(Move(attackTarget.gameObject.transform.position));
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
    }

    public IEnumerator MoveAttack()
    {
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
            //StartCoroutine(Move(attackTarget.gameObject.transform.position));
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
    }

    public IEnumerator Protect()
    {
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
            //StartCoroutine(Move(protectTarget.gameObject.transform.position));
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

            //StartCoroutine(Attack(nearestEnemy));
            Target = nearestEnemy.gameObject.transform.position;
            CanMove = true;
            TargetAssigned = true;

            yield return 0;
        }          
    }

    public bool IsAttacking
    {
        get { return isAttacking; }
    }

}
