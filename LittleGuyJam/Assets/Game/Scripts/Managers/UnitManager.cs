using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public enum UnitStates { Hold, Move, Gather, Store, Attack, MoveAttack, Protect, Inactive };
    public enum UnitAlignment { ally, enemy };
    public enum UnitRole { resource, melee, ranged };

    public List<GameObject> activeUnitPool;
    public List<GameObject> inactiveUnitPool;

    public List<Unit> selectedUnits;

    public void UpdateUnits()
    {
        GameManager.instance.data.TotalUnits = activeUnitPool.Count;

        // Check each Unit
        foreach (GameObject unit in activeUnitPool)
        {
            IAction lastAction = unit.GetComponent<Unit>().actionQueue
                [unit.GetComponent<Unit>().actionQueue.Count - 1];

            // Check each Unit's ActionQueue
            if (lastAction.CheckCanExecute())
            {
                lastAction.Execute();
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
