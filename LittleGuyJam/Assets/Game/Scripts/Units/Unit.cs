using System.Collections;
using UnityEngine;
using static UnitManager;

public class Unit : MonoBehaviour
{
    public enum UnitAlignment { player, enemy };

    public enum UnitRole { resource, melee, ranged };

    [SerializeField]
    UnitAlignment alignment;

    [SerializeField]
    UnitRole role;

    [SerializeField]
    UnitStates currentState;

    public UnitData data;

    [SerializeField]
    float health;

    [SerializeField]
    bool canMove;

    [SerializeField]
    bool isMoving;

    bool targetAssigned;
    Vector3 target;

    public bool coroutineRunning;
    public bool canBid;
    public bool playerDirected;

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
        isMoving = false;
        canMove = true;
        targetAssigned = false;
        canBid = true;
        playerDirected = false;
    }

    public void UpdateUnit()
    {
        if (CurrentState == UnitManager.UnitStates.Hold && !coroutineRunning)
        {
            Debug.Log(name + " StartCoroutine 'Hold' ");

            StartCoroutine(Hold());
        }
        
        if (CurrentState == UnitManager.UnitStates.Move && targetAssigned && !isMoving && !coroutineRunning)
        {
            Debug.Log(name + " StartCoroutine 'Move' ");

            if (playerDirected)
            {
                StartCoroutine(Move());
            }
            else if (canBid)
            {
                if (AutonomyBid())
                {
                    StartCoroutine(Move());

                }
                else
                {
                    StartCoroutine(AutonomyBidRefresh());
                }
            }
        }
    }

    public bool AutonomyBid()
    {
        int bid = Random.Range(0, data.RandomAutonomyMax);

        switch (currentState)
        {
            case UnitStates.Move:
                {
                    if(bid > data.RandomMoveAutonomy)
                    {
                        // Lost Bid, go back to Hold state
                        Debug.Log(name + " Lost Bid, go back to 'Hold' ");
                        currentState = UnitStates.Hold;
                        Target = GameManager.instance.Storage.transform.position;
                        return false;
                    }
                    // Won Bid, continue in current state
                    Debug.Log(name + " Won Bid, continue current state ");
                    return true;
                }

            case UnitStates.Gather:
                {
                    if(bid > data.RandomGatherAutonomy)
                    {
                        // Lost Bid, go back to Hold state
                        Debug.Log(name + " Lost Bid, go back to 'Hold' ");
                        currentState = UnitStates.Hold;
                        Target = GameManager.instance.Storage.transform.position;
                        return false;
                    }
                    // Won Bid, continue in current state
                    Debug.Log(name + " Won Bid, continue current state ");
                    return true;
                }

            case UnitStates.Store:
                {
                    if(bid > data.RandomStoreAutonomy)
                    {
                        // Lost Bid, go back to Hold state
                        Debug.Log(name + " Lost Bid, go back to 'Hold' ");
                        currentState = UnitStates.Hold;
                        Target = GameManager.instance.Storage.transform.position;
                        return false;
                    }
                    // Won Bid, continue in current state
                    Debug.Log(name + " Won Bid, continue current state ");
                    return true;
                }

            case UnitStates.Attack:
                {
                    if(bid > data.RandomAttackAutonomy)
                    {
                        // Lost Bid, go back to Hold state
                        Debug.Log(name + " Lost Bid, go back to 'Hold' ");
                        currentState = UnitStates.Hold;
                        Target = GameManager.instance.Barracks.transform.position;
                        return false;
                    }
                    // Won Bid, continue in current state
                    Debug.Log(name + " Won Bid, continue current state ");
                    return true;
                }

            case UnitStates.MoveAttack:
                {
                    if(bid > data.RandomMoveAttackAutonomy)
                    {
                        // Lost Bid, go back to Hold state
                        Debug.Log(name + " Lost Bid, go back to 'Hold' ");
                        currentState = UnitStates.Hold;
                        Target = GameManager.instance.Barracks.transform.position;
                        return false;
                    }
                    // Won Bid, continue in current state
                    Debug.Log(name + " Won Bid, continue current state ");
                    return true;
                }
            
            case UnitStates.Protect:
                {
                    if(bid > data.RandomProtectAutonomy)
                    {
                        // Lost Bid, go back to Hold state
                        Debug.Log(name + " Lost Bid, go back to 'Hold' ");
                        currentState = UnitStates.Hold;
                        Target = GameManager.instance.Barracks.transform.position;
                        return false;
                    }
                    // Won Bid, continue in current state
                    Debug.Log(name + " Won Bid, continue current state ");
                    return true;
                }

            default:
                {
                    Debug.Log(name + " Lost Bid, go back to 'Hold' ");
                    currentState = UnitStates.Hold;
                    return false;
                }
        }
    }

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

    public IEnumerator Hold()
    {
        coroutineRunning = true;

        if (targetAssigned)
        {
            //Debug.Log(name + " Target Assigned, can Move ");
            canMove = true;
        }

        yield return 0;

        if (!targetAssigned)
        {
            //Debug.Log(name + " Target not Assigned, can't Move ");
            canMove = false;
        }

        coroutineRunning = false;
    }

    public IEnumerator Move()
    {
        coroutineRunning = true;

        if (!canMove)
        {
            //Debug.Log(name + " can't Move, breaking ");
            yield break;
        }

        while (Vector3.Distance(gameObject.transform.position, target) > data.DistanceThreshold + data.MovementSpeed)
        {
            //Debug.Log(name + " moving... ");

            isMoving = true;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, data.MovementSpeed);

            yield return 0;

            //Debug.Log(name + " moved... ");
        }

        //Debug.Log(name + " not moving... ");

        isMoving = false;

        coroutineRunning = false;
        playerDirected = false;
    }

    private void OnMouseDown()
    {
        GameManager.instance.UnitManager.selectedUnits.Clear();
        GameManager.instance.UnitManager.selectedUnits.Add(this);
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

    public bool TargetAssigned
    {
        get { return targetAssigned; }
        set { targetAssigned = value; }
    }

    public Vector3 Target
    {
        get { return target; }
        set { target = value; }
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

    public float MovementSpeed {
        get { return data.MovementSpeed; }
        }

    public bool CanMove {
        get { return canMove; }
        set { canMove = value; }
        }

    public bool IsMoving {
        get { return isMoving; }
        }

    public float DistanceThreshold
    {
        get { return data.DistanceThreshold; }
    }

    public bool PlayerDirected
    {
        get { return playerDirected;  }
        set {  playerDirected = value; }
    }

}
