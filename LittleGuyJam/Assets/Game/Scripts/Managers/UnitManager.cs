using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public enum UnitAlignment { ally, enemy };
    public enum UnitRole { resource, melee, ranged };

    public GameObject activeUnitPool;
    public GameObject inactiveUnitPool;

    public List<Unit> selectedUnits;

    public GameObject enemyMeleePrefab;
    public GameObject enemyRangedPrefab;
    public Transform enemySpawn;

    public void StartUnits()
    {
        selectedUnits = new List<Unit>();

        CheckActiveUnitPool();

        foreach (Unit unit in activeUnitPool.GetComponentsInChildren<Unit>())
        {
            unit.StartUnit();
        }
    }

    public void UpdateUnits()
    {
        GameManager.instance.data.TotalUnits = activeUnitPool.
            GetComponentsInChildren<Unit>().
            Where(u => u.Alignment == UnitAlignment.ally).
            ToList().
            Count();

        CheckActiveUnitPool();
        CheckInactiveUnitPool();
        RunUnitQueue();
    }

    void RunUnitQueue() {
        foreach (Unit unit in activeUnitPool.GetComponentsInChildren<Unit>())
        {
            CheckHealth(unit);

            if (unit.actionQueue?.Count > 0)
            {
                IAction lastAction = unit.actionQueue.Last();

                // Check each Unit's ActionQueue
                if (lastAction.CheckCanExecute())
                {
                    lastAction.Execute();
                }
            }
            else
            {
                unit.actionQueue = new List<IAction>
                {
                    unit.NewHoldAction(true)
                };
            }
        }
    }

    public void DeactivatePool()
    {
        if (activeUnitPool.transform.childCount < 1)
        {
            return;
        }

        for (int i = 0; i < activeUnitPool.transform.childCount; i++)
        {
            activeUnitPool.transform.
                    GetChild(0).gameObject.SetActive(false);
            activeUnitPool.transform.
                    GetChild(0).gameObject.transform.
                    SetParent(inactiveUnitPool.transform);
        }
    }

    public void ActivatePool()
    {
        if (inactiveUnitPool.transform.childCount < 1)
        {
            return;
        }

        for (int i = 0; i < inactiveUnitPool.transform.childCount; i++)
        {
            inactiveUnitPool.transform.
                    GetChild(0).gameObject.SetActive(true);
            inactiveUnitPool.transform.
                    GetChild(0).gameObject.transform.
                    SetParent(activeUnitPool.transform);
        }
    }

    void CheckActiveUnitPool()
    {
        if(activeUnitPool.transform.childCount < 1)
        {
            return;
        }

        for (int i = 0; i < activeUnitPool.transform.childCount; i++)
        {
            if (!activeUnitPool.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                activeUnitPool.transform.
                    GetChild(i).gameObject.transform.
                    SetParent(inactiveUnitPool.transform);

                activeUnitPool.transform.
                    GetChild(i).gameObject.
                    SetActive(false);
            }
        }

    }

    void CheckInactiveUnitPool()
    {
        if (inactiveUnitPool.transform.childCount < 1)
        {
            return;
        }

        for (int i = 0; i < inactiveUnitPool.transform.childCount; i++)
        {
            if (inactiveUnitPool.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                inactiveUnitPool.transform.
                    GetChild(i).gameObject.transform.
                    SetParent(activeUnitPool.transform);

                activeUnitPool.transform.
                    GetChild(i).gameObject.
                    SetActive(true);

                activeUnitPool.transform.
                    GetChild(i).
                    GetComponent<Unit>().
                    StartUnit();
            }
        }
    }

    public void CheckHealth(Unit u)
    {
        if (u.Health <= 0)
        {
            u.gameObject.transform.SetParent(inactiveUnitPool.transform);
            u.gameObject.SetActive(false);
            GameManager.instance.AudioManager.PlaySFX("Hit2");
        }
    }

}
