using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitManager;

public class Unit : MonoBehaviour
{
    [SerializeField]
    UnitAlignment alignment;

    [SerializeField]
    UnitRole role;

    [SerializeField]
    UnitStates currentState;

    public UnitData data;

    [SerializeField]
    float health;

    public List<IAction> actionQueue;

    public bool near;
    public float distanceCap;

    public bool coroutineRunning;
    public bool canBid;

    protected Vector3 nextTarget;

    public Unit(UnitAlignment _a)
    {
        alignment = _a;
    }

    private void Awake()
    {
        Reset();
    }

    public void Reset()
    {
        health = data.MaxHealth;
        CurrentState = UnitStates.Hold;
        canBid = true;
    }

    public virtual bool AutonomyBid(string action) { return false; }

    public IEnumerator AutonomyBidRefresh()
    {
        coroutineRunning = true;
        canBid = false;
        Debug.Log(name + " can't bid... ");

        yield return new WaitForSeconds(data.BidRefreshRate);

        canBid = true;
        coroutineRunning = false;
        Debug.Log(name + " can bid... ");
    }

    public HoldAction NewHoldAction(bool fromPlayer)
    {
        return new HoldAction();
    }

    public MoveAction NewMoveAction(bool fromPlayer)
    {
        MoveAction action = new MoveAction();

        action.AssignUnit(this, fromPlayer);
        action.Target = nextTarget;

        return action;
    }


    private void OnMouseDown()
    {
        if(alignment == UnitAlignment.ally)
        {
            GameManager.instance.UnitManager.selectedUnits.Clear();
            GameManager.instance.UnitManager.selectedUnits.Add(this);

        } 
        else
        {
            if (GameManager.instance.UnitManager.selectedUnits.Count > 0)
            {
                foreach (AttackUnit a in GameManager.instance.UnitManager.selectedUnits)
                {
                    a.NextTarget = transform.position;
                    a.NewAttackAction(true);
                }
            }
        }
    }

    private void OnMouseOver()
    {
        // Show Info Tooltip
    }

    // Accessors 

    public UnitAlignment Alignment {
        get { return alignment; }
        set { alignment = value; }
        }

    public UnitRole Role {
        get { return role; }
        set { role = value; }
        }

    public UnitStates CurrentState
    {
        get { return currentState; }
        set { currentState = value; }
    }

    public float Health {
        get { return health; }
        }

    /// <summary>
    /// Evaluates damage and returns updated health value as a float.
    /// </summary>
    /// <param name="value">Damage Amount</param>
    /// <returns></returns>
    public float TakeDamage(float value)
    {
        health -= Mathf.Abs(value);

        if(health <= 0)
        {
            health = 0;
        }

        // Evaluate in manager for death condition
        return health;
    }

    /// <summary>
    /// Evaluates and applies healing amount.
    /// </summary>
    /// <param name="value">Healing Amount</param>
    /// <returns></returns>
    public void RestoreHealth(float value)
    {
        health += Mathf.Abs(value);

        if (health >= data.MaxHealth)
        {
            health = data.MaxHealth;
        }
    }

    public Vector3 NextTarget
    {
        get { return nextTarget; }
        set { nextTarget = value; }
    }

}
