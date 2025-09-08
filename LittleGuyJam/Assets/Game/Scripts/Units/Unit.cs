using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnitManager;

public class Unit : GamePiece
{
    [SerializeField]
    UnitAlignment alignment;

    [SerializeField]
    UnitRole role;

    public UnitData data;

    [SerializeField]
    float health;

    [SerializeField]
    public List<IAction> actionQueue;

    public bool nearTarget;

    public bool coroutineRunning;
    public bool canBid;

    protected GamePiece nextTarget;

    [SerializeField]
    public GameObject infoUI;

    [SerializeField]
    public TextMeshProUGUI healthTextUI;

    public Unit(UnitAlignment _a)
    {
        alignment = _a;
        Reset();
    }

    public void StartUnit()
    {
        Reset();
    }

    public void Reset()
    {
        health = data.MaxHealth;
        canBid = true;
        actionQueue = new List<IAction>();
        actionQueue.Clear();
        actionQueue.Add(NewHoldAction(false));
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
        HoldAction action = new HoldAction();

        action.AssignUnit(this, fromPlayer);
        action.CanMove = true;

        return action;
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
            GameManager.instance.UnitManager.selectedUnits.Add(this);
        } 
        else
        {
            if (GameManager.instance.UnitManager.selectedUnits.Count > 0)
            {
                foreach (AttackUnit a in GameManager.instance.UnitManager.selectedUnits)
                {
                    a.NextTarget = this;
                    a.actionQueue.Add(a.NewAttackAction(true));
                    a.actionQueue.Last<IAction>().ConvertTo<AttackAction>().Target = this;
                    a.actionQueue.Add(a.NewMoveAction(true));
                    a.actionQueue.Last<IAction>().ConvertTo<MoveAction>().DistanceCap = a.data.AttackRange;   
                    GameManager.instance.UnitManager.selectedUnits.Remove(a);
                    GameManager.instance.UnitManager.selectedUnits.Clear();
                    break;
                }
            }
        }
    }

    private void OnMouseEnter()
    {
        if (infoUI != null)
        {
            infoUI.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        if (infoUI != null)
        {
            infoUI.SetActive(false);
        }
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

        Debug.Log(name + " Attacked ");

        if (health <= 0)
        {
            health = 0;
            Debug.Log(name + " Killed ");
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

        Debug.Log(name + " Healed ");

        if (health >= data.MaxHealth)
        {
            health = data.MaxHealth;
        }
    }

    public GamePiece NextTarget
    {
        get { return nextTarget; }
        set { nextTarget = value; }
    }

}
