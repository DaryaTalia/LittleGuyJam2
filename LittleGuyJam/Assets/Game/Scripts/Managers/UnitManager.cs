using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public enum UnitStates { Hold, Move, Gather, Store, Attack, MoveAttack, Protect, Inactive };
    public enum UnitAlignment { ally, enemy };
    public enum UnitRole { resource, melee, ranged };

    public GameObject activeUnitPool;
    public GameObject inactiveUnitPool;

    public List<Unit> selectedUnits;

    public void StartUnits()
    {
        CheckActiveUnitPool();

        foreach (Unit unit in activeUnitPool.GetComponentsInChildren<Unit>())
        {
            unit.StartUnit();
        }
    }

    public void UpdateUnits()
    {
        GameManager.instance.data.TotalUnits = activeUnitPool.GetComponentsInChildren<Unit>().Count();

        CheckActiveUnitPool();
        CheckInactiveUnitPool();
        RunUnitQueue();
    }

    void RunUnitQueue() {
        foreach (Unit unit in activeUnitPool.GetComponentsInChildren<Unit>())
        {
            if (unit.actionQueue.Count > 0)
            {
                IAction lastAction = unit.actionQueue
                                [unit.actionQueue.Count - 1];

                // Check each Unit's ActionQueue
                if (lastAction.CheckCanExecute())
                {
                    lastAction.Execute();
                }
                else
                {
                    unit.actionQueue.Remove(lastAction);
                }
            }
            else
            {
                unit.actionQueue.Add(unit.NewHoldAction(true));
            }
        }
    }

    void CheckActiveUnitPool()
    {
        if(activeUnitPool.GetComponentsInChildren<Unit>().Count() < 1)
        {
            return;
        }

        foreach (Unit unit in activeUnitPool.GetComponentsInChildren<Unit>())
        {
            if (!unit.gameObject.activeInHierarchy)
            {
                unit.gameObject.transform.SetParent(inactiveUnitPool.transform);              
            }
        }

    }

    void CheckInactiveUnitPool()
    {
        if (inactiveUnitPool.GetComponentsInChildren<Unit>().Count() < 1)
        {
            return;
        }

        foreach (Unit unit in activeUnitPool.GetComponentsInChildren<Unit>())
        {
            if (unit.gameObject.activeInHierarchy)
            {
                unit.gameObject.transform.SetParent(activeUnitPool.transform);
            }
        }
    }

    public void CheckHealth(Unit u)
    {
        if (u.Health <= 0)
        {
            u.CurrentState = UnitStates.Inactive;
        }
    }

}
