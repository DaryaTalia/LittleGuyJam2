using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        if (actionQueue.Last<IAction>().FromPlayer)
        {
            canBid = true;
        }

        if (canBid)
        {
            int bid = Random.Range(0, data.RandomAutonomyMax);

            switch (action)
            {
                case "Action":
                    {
                        if (bid < data.RandomAttackAutonomy)
                        {
                            return true;
                        }
                        StartCoroutine(AutonomyBidRefresh());
                        return false;
                    }
            }

        }

        return false;
    }

    public AttackAction NewAttackAction(bool fromPlayer)
    {
        AttackAction action = new AttackAction();

        action.AssignUnit(this, fromPlayer);
        FindEnemy();
        nextTarget = attackTarget.transform.position;
        nearTarget = false;

        return action;
    }

    public void FindEnemy()
    {
        List<Unit> enemies = new List<Unit>();

        if (Alignment == UnitAlignment.ally)
        {
            foreach(Unit u in GameManager.instance.
                UnitManager.activeUnitPool.GetComponentsInChildren<Unit>())
            {
                if(u.Alignment == UnitAlignment.enemy)
                {
                    enemies.Add(u);
                }
            }
        }
        else
        {
            foreach (Unit u in GameManager.instance.
                UnitManager.activeUnitPool.GetComponentsInChildren<Unit>())
            {
                if (u.Alignment == UnitAlignment.ally)
                {
                    enemies.Add(u);
                }
            }
        }

        int randomTarget = Random.Range(0, enemies.Count);

        if (enemies.Count > 0)
        {
            attackTarget = enemies[randomTarget].GetComponent<Unit>();
            nextTarget = attackTarget.transform.position;
        }
    }

    public void FindAlly()
    {
        List<Unit> allies;

        if (Alignment == UnitAlignment.ally)
        {
            allies = GameManager.instance.
                UnitManager.activeUnitPool.GetComponentsInChildren<Unit>().
                ToList().FindAll(u => u.GetComponent<Unit>().Alignment == UnitAlignment.ally);
        }
        else
        {
            allies = GameManager.instance.
                UnitManager.activeUnitPool.GetComponentsInChildren<Unit>().
                ToList().FindAll(u => u.GetComponent<Unit>().Alignment == UnitAlignment.enemy);
        }

        int randomTarget = Random.Range(0, allies.Count);

        if (allies.Count > 0)
        {            
            nextTarget = allies[randomTarget].
                GetComponent<Unit>().transform.position;
        }
    }

    public Unit AttackTarget
    {
        get { return attackTarget; }
        set { attackTarget = value; }
    }
}
