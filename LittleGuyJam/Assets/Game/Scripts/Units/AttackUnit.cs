using System.Collections;
using UnityEngine;
using static UnitManager;

public class AttackUnit : Unit
{
    [SerializeField]
    CircleCollider2D protectionRange;

    Unit attackTarget;

    public AttackUnit(UnitAlignment _a) : base(_a)
    {

    }

    public override bool AutonomyBid(string action)
    {
        int bid = Random.Range(0, data.RandomAutonomyMax);

        switch (action)
        {
            case "Action":
                {

                    return true;
                }
        }
        return false;
    }

    public AttackAction NewAttackAction(bool fromPlayer)
    {
        return new AttackAction();
    }

    public void FindTarget()
    {

    }

    public void FindEnemy()
    {

    }

    public void FindAlly()
    {

    }

    public Unit AttackTarget
    {
        get { return attackTarget; }
        set { attackTarget = value; }
    }
}
