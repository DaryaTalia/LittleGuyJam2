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
        AttackAction action = new AttackAction();

        action.AssignUnit(this, fromPlayer);
        nextTarget = AttackTarget.transform.position;

        return action;
    }

    public void FindEnemy()
    {
        List<Unit> enemies;

        if (Alignment == UnitAlignment.ally)
        {
            enemies = GameManager.instance.
                UnitManager.activeUnitPool.GetComponentsInChildren<Unit>().
                ToList().FindAll(u => u.GetComponent<Unit>().Alignment == UnitAlignment.enemy);
        }
        else
        {
            enemies = GameManager.instance.
                UnitManager.activeUnitPool.GetComponentsInChildren<Unit>().
                ToList().FindAll(u => u.GetComponent<Unit>().Alignment == UnitAlignment.ally);
        }

        int randomTarget = Random.Range(0, enemies.Count);

        if (enemies.Count > 0)
        {
            attackTarget = enemies[randomTarget].GetComponent<Unit>();
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
            attackTarget = allies[randomTarget].GetComponent<Unit>();
        }
    }

    public Unit AttackTarget
    {
        get { return attackTarget; }
        set { attackTarget = value; }
    }
}
