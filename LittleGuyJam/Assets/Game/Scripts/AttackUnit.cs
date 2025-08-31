using System.Collections;
using UnityEngine;

public class AttackUnit : Unit
{
    [SerializeField]
    CircleCollider2D protectionRange;

    [SerializeField]
    bool isAttacking;

    public AttackUnit(UnitAlignment _a) : base(_a)
    {
        //StartCoroutine(Move(Vector3.zero));
    }

    public IEnumerator Attack(Unit attackTarget)
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
            StartCoroutine(Move(attackTarget.gameObject.transform.position));
        }
        else
        {
            while (attackTarget.Health > 0)
            {
                isAttacking = true;
                yield return new WaitForSeconds(data.AttackSpeed);
                attackTarget.TakeDamage(data.AttackDamage);
            }

            isAttacking = false;
        }          
    }

    public IEnumerator Protect(Unit protectTarget)
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
            StartCoroutine(Move(protectTarget.gameObject.transform.position));
        }
        else
        {
            Unit nearestEnemy = gameObject.GetComponentInChildren<ProtectionCollider>().ReturnRandomTarget();

            if(nearestEnemy == null)
            {
                yield break;    
            }

            StartCoroutine(Attack(nearestEnemy));

            yield return 0;
        }          
    }

    public bool IsAttacking
    {
        get { return isAttacking; }
    }

}
