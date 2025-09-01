using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public enum UnitStates { Hold, Move, Collect, Store, Attack, MoveAttack, Protect, Inactive };

    public List<GameObject> activeUnitPool;
    public List<GameObject> inActiveUnitPool;

    public List<Unit> selectedUnits;

    public void UpdateUnits()
    {
        foreach (GameObject unit in activeUnitPool) {
            if (unit.GetComponent<Unit>().Role == Unit.UnitRole.resource)
            {
                // Run State Machine for changes in state
                UnitStateMachine(unit.GetComponent<ResourceUnit>());

                // Run new state behavior, basic
                unit.GetComponent<ResourceUnit>().UpdateUnit();

                // Run new state behavior, role
                unit.GetComponent<ResourceUnit>().UpdateResourcer();
            }
            else
            {
                // Run State Machine for changes in state
                UnitStateMachine(unit.GetComponent<AttackUnit>());

                // Run new state behavior, basic
                unit.GetComponent<AttackUnit>().UpdateUnit();

                // Run new state behavior, role
                unit.GetComponent<AttackUnit>().UpdateAttacker();
            }
        }

    }

    public void UnitStateMachine(Unit u)
    {
        if(u.Health <= 0)
        {
            u.CurrentState = UnitStates.Inactive;
        }

        switch(u.CurrentState)
        {
            case UnitStates.Hold:
                {
                    if(u.TargetAssigned)
                    {
                        u.CurrentState = UnitStates.Move;
                    }

                    if(u.Health <= 0)
                    {
                        u.CurrentState = UnitStates.Inactive;
                    }

                    break;
                }
            case UnitStates.Move:
                {
                    Debug.Log(name + " in state 'Move' ");

                    if (!u.TargetAssigned)
                    {
                        u.CurrentState = UnitStates.Hold;
                    }

                    if (u.Health <= 0)
                    {
                        u.CurrentState = UnitStates.Inactive;
                    }

                    break;
                }
            case UnitStates.Inactive:
                {
                    activeUnitPool.Remove(u.gameObject);
                    inActiveUnitPool.Add(u.gameObject);

                    u.gameObject.SetActive(false);

                    break;
                }

            default:
                {
                    if(u.CurrentState == UnitStates.Collect || u.CurrentState == UnitStates.Store)
                    {
                        ResourceStateMachine((ResourceUnit)u);
                    } 
                    else if(u.CurrentState == UnitStates.Attack || u.CurrentState == UnitStates.MoveAttack || u.CurrentState == UnitStates.Protect)
                    {
                        AttackStateMachine((AttackUnit)u);
                    }
                        break;
                }
        }

    }

    public void ResourceStateMachine(ResourceUnit u)
    {
        switch (u.CurrentState) { 
            case UnitStates.Collect:
                {
                    if(!u.IsGathering)
                    {
                        u.CurrentState = UnitStates.Store;
                    }
                    break;
                }
            case UnitStates.Store:
                {
                    if (!u.HasResources)
                    {
                        u.CurrentState = UnitStates.Collect;
                    }
                    break;
                }

            default:
                {
                    u.CurrentState = UnitStates.Hold;
                    break;
                }
        }
    }

    public void AttackStateMachine(AttackUnit u)
    {
        switch (u.CurrentState)
        {
            case UnitStates.Attack:
                {
                    if(!u.IsAttacking)
                    {
                        u.CurrentState = UnitStates.Hold;
                    }
                    break;
                }
            case UnitStates.MoveAttack:
                {
                    if (!u.IsAttacking)
                    {
                        u.CurrentState = UnitStates.Move;
                    }
                    break;
                }
            case UnitStates.Protect:
                {
                    if (!u.IsAttacking)
                    {
                        u.CurrentState = UnitStates.Hold;
                    }
                    break;
                }

            default:
                {
                    u.CurrentState = UnitStates.Hold;
                    break;
                }
        }
    }
}
