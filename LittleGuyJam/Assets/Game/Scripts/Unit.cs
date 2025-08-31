using System.Collections;
using UnityEngine;
[RequireComponent(typeof(UnitData))]

public class Unit : MonoBehaviour
{
    public enum UnitAlignment { player, enemy };

    [SerializeField]
    UnitAlignment alignment;
    [SerializeField]
    public UnitData data;

    [SerializeField]
    float health;
    [SerializeField]
    bool canMove;
    [SerializeField]
    bool isMoving;

    public Unit(UnitAlignment _a)
    {
        alignment = _a;
        health = data.MaxHealth;

    }

    public IEnumerator Move(Vector3 target)
    {
        if (!canMove)
        {
            yield break;
        }

        while (Vector3.Distance(gameObject.transform.position, target) < data.DistanceThreshold)
        {
            isMoving = true;
            gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.position, target, data.MovementSpeed);

            yield return 0;
        }
        isMoving = false;
    }

    // Accessors

    public UnitAlignment Alignment {
        get { return alignment; }
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
