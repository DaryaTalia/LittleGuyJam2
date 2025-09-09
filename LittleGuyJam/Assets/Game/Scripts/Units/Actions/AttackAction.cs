using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[System.Serializable]
public class AttackAction : IAction
{
    AttackUnit unit;
    Unit target;

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
        if (target.TakeDamage(unit.data.AttackDamage) <= 0)
        {
            Debug.Log(unit.name + " Killed " + target.name);
            target = null;
            isAttacking = false;
            unit.nearTarget = false;
            unit.actionQueue.Remove(this);
        } 
        else
        {
            Debug.Log(unit.name + " Attacked " + target.name);
            GameManager.instance.AudioManager.PlaySFX("Hit1");
        }        
    }

    public bool CheckCanExecute()
    {
        if(target == null)
        {
            unit.FindEnemy();
            return false;
        }

        if(target.Alignment == unit.Alignment)
        {
            return false;
        }

        if(Vector3.Distance(unit.gameObject.transform.position, target.transform.position) > unit.data.DistanceThreshold)
        {
            unit.nearTarget = false;
        }

        if (!unit.nearTarget 
            && unit.actionQueue.Count < 3 
            && unit.actionQueue.Last().GetType() != typeof(MoveAction))
        {
            MoveAction newMA = unit.NewMoveAction(false);
            newMA.Target = target;
            newMA.DistanceCap = unit.data.AttackRange;

            unit.actionQueue.Add(newMA);
            return false;
        }

        if (unit.nearTarget && target != null)
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

    public Unit Target
    {
        get { return target; }
        set { target = value; }
    }
}
