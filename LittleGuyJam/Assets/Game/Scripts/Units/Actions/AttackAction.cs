using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[System.Serializable]
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
        Attack();
    }

    void Attack()
    {
        // Timer
        if (timer < unit.data.AttackSpeed)
        {
            timer += Time.deltaTime;
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
                Debug.Log(unit.name + " Killed " + unit.AttackTarget.name);
                unit.AttackTarget = null;
                isAttacking = false;
                unit.nearTarget = false;
                unit.actionQueue.Remove(this);
            } 
            else
            {
                Debug.Log(unit.name + " Attacked " + unit.AttackTarget.name);
            }
        }
    }

    public bool CheckCanExecute()
    {
        if(unit.AttackTarget == null)
        {
            unit.FindEnemy();
            return false;
        }

        if(unit.AttackTarget.Alignment == unit.Alignment)
        {
            return false;
        }

        if (!unit.nearTarget 
            && unit.actionQueue.Count < 3 
            && unit.actionQueue.Last().GetType() != typeof(MoveAction))
        {
            MoveAction newMA = unit.NewMoveAction(false);
            newMA.Target = unit.NextTarget;
            newMA.DistanceCap = unit.data.AttackRange;

            unit.actionQueue.Add(this);
            unit.actionQueue.Add(newMA);
            return false;
        }

        if (unit.nearTarget && unit.AttackTarget != null)
        {
            isAttacking = true;
        }

        if(!isAttacking)
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
