using System.Collections;
using UnityEngine;
using static UnitManager;
[RequireComponent(typeof(UnitData))]

public class Unit : MonoBehaviour
{
    public enum UnitAlignment { player, enemy };

    [SerializeField]
    UnitAlignment alignment;

    public enum UnitRole { resource, melee, ranged };

    [SerializeField]
    UnitRole role;

    [SerializeField]
    UnitStates currentState;

    [SerializeField]
    public UnitData data;

    [SerializeField]
    float health;
    [SerializeField]
    bool canMove;
    [SerializeField]
    bool isMoving;

    bool targetAssigned;
    Vector3 target;

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
        if (CurrentState == UnitManager.UnitStates.Hold)
        {
            StartCoroutine(Hold());
        }
        else if (CurrentState == UnitManager.UnitStates.Move)
        {
            Debug.Log(name + " StartCoroutine 'Move' ");

            StartCoroutine(Move());
        }
        else if (CurrentState == UnitManager.UnitStates.Inactive)
        {
            StopAllCoroutines();
        }
    }

    public IEnumerator Hold()
    {
        yield return 0;
    }

    public IEnumerator Move()
    {
        if (!canMove)
        {
            Debug.Log(name + " can't Move, breaking ");
            yield break;
        }

        Debug.Log(name + " can Move, continue... ");

        while (Vector3.Distance(gameObject.transform.position, target) > data.DistanceThreshold)
        {
            isMoving = true;
            gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.position, target, data.MovementSpeed);

            Debug.Log(name + " distance: " + Vector3.Distance(gameObject.transform.position, target));

            yield return 0;
        }
        isMoving = false;
    }

    private void OnMouseDown()
    {
        GameManager.instance.UnitManager.selectedUnits.Clear();
        GameManager.instance.UnitManager.selectedUnits.Add(this);
    }

    private void OnMouseDrag()
    {
        if (!GameManager.instance.UnitManager.selectedUnits.Contains(this))
        {
            GameManager.instance.UnitManager.selectedUnits.Add(this);
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
