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

    public Unit(UnitAlignment _a)
    {
        alignment = _a;
        health = data.MaxHealth;
        CurrentState = UnitStates.Hold;
    }

    private void Awake()
    {
        health = data.MaxHealth;
        canMove = true;
    }

    public void UpdateUnit()
    {
        if (CurrentState == UnitManager.UnitStates.Hold && !coroutineRunning)
        {
            Debug.Log(name + " StartCoroutine 'Hold' ");

            StartCoroutine(Hold());
        }
        
        if (CurrentState == UnitManager.UnitStates.Move && !isMoving && !coroutineRunning)
        {
            Debug.Log(name + " StartCoroutine 'Move' ");

            StartCoroutine(Move());
        }
    }

    public IEnumerator Hold()
    {
        coroutineRunning = true;

        if (targetAssigned)
        {
            Debug.Log(name + " Target Assigned, can Move ");
            canMove = true;
        }

        yield return 0;

        if (!targetAssigned)
        {
            Debug.Log(name + " Target not Assigned, can't Move ");
            canMove = false;
        }

        coroutineRunning = false;
    }

    public IEnumerator Move()
    {
        coroutineRunning = true;

        if (!canMove)
        {
            Debug.Log(name + " can't Move, breaking ");
            yield break;
        }

        while (Vector3.Distance(gameObject.transform.position, target) > data.DistanceThreshold + data.MovementSpeed)
        {
            Debug.Log(name + " moving... ");

            isMoving = true;
            gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.position, target, data.MovementSpeed);

            yield return 0;

            Debug.Log(name + " moved... ");
        }

        Debug.Log(name + " not moving... ");

        isMoving = false;

        coroutineRunning = false;
    }

    private void OnMouseDown()
    {
        GameManager.instance.UnitManager.selectedUnits.Clear();
        GameManager.instance.UnitManager.selectedUnits.Add(this);
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

}
