using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AttackAction : IAction
{
    AttackUnit unit;

    bool fromPlayer;
    bool isAttacking;

    float timer;

    public void AssignUnit(Unit unit, bool player)
    {
        this.unit = (AttackUnit) unit;
        fromPlayer = player;
        isAttacking = true;
    }

    public void Execute()
    {
        if (!CheckCanExecute())
        {
            return;
        }

        Attack();
    }

    void Attack()
    {
        // Timer
        if (timer < unit.data.AttackSpeed)
        {
            timer++;
            return;
        }
        else
        {
            timer = 0;
        }

        // Attack
        if (unit.AttackTarget.Health > 0)
        {
            if (unit.AttackTarget.TakeDamage(unit.data.AttackDamage) <= 0)
            {
                unit.AttackTarget = null;
                isAttacking = false;
            }
        }
    }

    public bool CheckCanExecute()
    {
        if (!isAttacking)
        {
            return false;
        }

        if(unit.AttackTarget == null)
        {
            return false;
        }

        if(unit.AttackTarget.Alignment == unit.Alignment)
        {
            return false;
        }

        if (!unit.near)
        {
            return false;
        }

        return true;
    }

    public bool FromPlayer
    {
        get { return fromPlayer; }
    }

    public bool IsAttacking
    {
        get { return isAttacking; }
        set {  isAttacking = value; }
    }
}
