using UnityEngine;

[System.Serializable]
public class HoldAction : IAction
{
    Unit unit;

    bool fromPlayer;

    bool canMove;

    public void AssignUnit(Unit unit, bool player)
    {
        this.unit = unit;
        fromPlayer = player;
    }

    public void Execute()
    {
        Hold();
    }

    void Hold()
    {
        // Maintain Position
        Debug.Log(unit.name + " Holding");
        unit.gameObject.transform.position = unit.gameObject.transform.position;
    }

    public bool CheckCanExecute()
    {
        if (unit.GetType() == typeof(ResourceUnit))
        {
            ResourceUnit r = (ResourceUnit)unit;

            if (r.CollectedResources == r.data.MaxResources && r.AutonomyBid("Store"))
            {
                r.actionQueue.Add(this);
                r.actionQueue.Add(r.NewStoreAction(false));
                return false;
            }
            else
            if (r.CollectedResources < r.data.MaxResources && r.AutonomyBid("Gather"))
            {
                r.actionQueue.Add(this);
                r.actionQueue.Add(r.NewGatherAction(false));
                return false;
            }
        }
        else
        if (unit.GetType() == typeof(AttackUnit))
        {
            AttackUnit a = (AttackUnit)unit;
            if (a.AttackTarget != null && a.AutonomyBid("Attack"))
            {
                a.actionQueue.Add(this);
                a.actionQueue.Add(a.NewAttackAction(false));
                return false;
            } 
            else 
            if(a.AttackTarget == null)
            {
                a.FindEnemy();
            }
        }

        return true;
    }

    public bool FromPlayer
    {
        get { return fromPlayer; }
    }

    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }

}
