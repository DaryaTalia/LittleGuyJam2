using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public enum UnitStates { Hold, Move, Gather, Store, Attack, MoveAttack, Protect, Inactive };

    public List<GameObject> activeUnitPool;
    public List<GameObject> inactiveUnitPool;

    public List<Unit> selectedUnits;

    public void UpdateUnits()
    {
        GameManager.instance.data.TotalUnits = activeUnitPool.Count;

        foreach (GameObject unit in activeUnitPool) {
            if (unit.GetComponent<Unit>().Role == Unit.UnitRole.resource)
            {
                // Run State Machine for changes in state
                ResourceStateMachine(unit.GetComponent<ResourceUnit>());

                // Run new state behavior, role
                unit.GetComponent<ResourceUnit>().UpdateResourcer();

                // Run new state behavior, basic
                unit.GetComponent<ResourceUnit>().UpdateUnit();
            }
            else
            {
                // Run State Machine for changes in state
                AttackStateMachine(unit.GetComponent<AttackUnit>());

                // Run new state behavior, basic
                unit.GetComponent<AttackUnit>().UpdateUnit();

                // Run new state behavior, role
                unit.GetComponent<AttackUnit>().UpdateAttacker();
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

    public void ResourceStateMachine(ResourceUnit u)
    {
        CheckHealth(u);

        switch (u.CurrentState) {
            case UnitStates.Hold:
                {
                    //Debug.Log(name + " in state 'Hold' ");

                    if (u.CanMove)
                    {
                        //Debug.Log(name + " changing state to 'Move' ");
                        u.CurrentState = UnitStates.Move;
                    }

                    break;
                }

            case UnitStates.Move:
                {
                    //Debug.Log(name + " in state 'Move' ");

                    if (!u.CanMove)
                    {
                        //Debug.Log(name + " changing state to 'Hold' ");
                        u.CurrentState = UnitStates.Hold;
                    }

                    if (u.TargetAssigned && u.HasResources && !u.IsMoving)
                    {
                        //Debug.Log(name + " changing state to 'Store' ");
                        u.CurrentState = UnitStates.Store;
                    }

                    if (u.TargetAssigned && !u.HasResources && !u.IsMoving)
                    {
                        //Debug.Log(name + " changing state to 'Gather' ");
                        u.CurrentState = UnitStates.Gather;
                    }

                    break;
                }

            case UnitStates.Gather:
                {
                    //Debug.Log(name + " in state 'Gather' ");

                    if (u.HasResources || u.CanMove)
                    {
                        //Debug.Log(name + " changing state to 'Move' ");
                        u.CurrentState = UnitStates.Move;
                    }

                    break;
                }

            case UnitStates.Store:
                {
                    //Debug.Log(name + " in state 'Store' ");

                    if (!u.HasResources || u.CanMove)
                    {
                        //Debug.Log(name + " changing state to 'Move' ");
                        u.CurrentState = UnitStates.Move;
                    }
                    break;
                }

            case UnitStates.Inactive:
                {
                    Debug.Log(name + " in state 'Inactive' ");

                    Debug.Log(name + " Stopping All Coroutines ");
                    u.StopAllCoroutines();

                    activeUnitPool.Remove(u.gameObject);
                    inactiveUnitPool.Add(u.gameObject);

                    u.gameObject.SetActive(false);

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
        CheckHealth(u);

        switch (u.CurrentState)
        {
            case UnitStates.Hold:
                {
                    if (u.TargetAssigned)
                    {
                        u.CurrentState = UnitStates.Move;
                    }

                    if (u.Health <= 0)
                    {
                        u.CurrentState = UnitStates.Inactive;
                    }

                    // Derived Variables

                    if (u.Role == Unit.UnitRole.resource)
                    {
                        if (!!u.IsMoving && u.TargetAssigned)
                        {
                            ResourceUnit r = u.GetComponent<ResourceUnit>();

                            if (r.HasResources)
                            {
                                //u.CurrentState == 
                            }
                        }
                    }

                    if (u.Role == Unit.UnitRole.melee || u.Role == Unit.UnitRole.ranged)
                    {

                    }

                    break;
                }
            case UnitStates.Move:
                {
                    Debug.Log(name + " in state 'Move' ");

                    // Unit Variables

                    if (!u.TargetAssigned)
                    {
                        u.CurrentState = UnitStates.Hold;
                    }

                    if (u.Health <= 0)
                    {
                        u.CurrentState = UnitStates.Inactive;
                    }

                    // Derived Variables

                    if (u.Role == Unit.UnitRole.resource)
                    {

                    }

                    if (u.Role == Unit.UnitRole.melee || u.Role == Unit.UnitRole.ranged)
                    {

                    }

                    break;
                }
            case UnitStates.Inactive:
                {
                    activeUnitPool.Remove(u.gameObject);
                    inactiveUnitPool.Add(u.gameObject);

                    u.gameObject.SetActive(false);

                    break;
                }

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
