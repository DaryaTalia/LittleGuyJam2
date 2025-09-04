using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class MoveAction : IAction
{
    Unit unit;

    bool fromPlayer;

    bool isMoving;
    Vector3 target;
    float distanceCap;

    public void AssignUnit(Unit unit, bool player)
    {
        this.unit = unit;
        fromPlayer = player;
        isMoving = true;
    }

    public void Execute()
    {
        Move();
    }

    void Move()
    {
        // Translate Position
        if (Vector3.Distance(unit.gameObject.transform.position, target) > distanceCap + unit.data.MovementSpeed)
        {
            Debug.Log(unit.name + " Moving");

            isMoving = true;
            unit.gameObject.transform.position = Vector3.MoveTowards(unit.gameObject.transform.position, target, unit.data.MovementSpeed * Time.deltaTime);
        } 
        else
        {
            isMoving = false;
            unit.nearTarget = true;
            Debug.Log(unit.name + " Near Target");
            unit.actionQueue.Remove(this);
        }
    }

    public bool CheckCanExecute()
    {
        if (!isMoving)
        {
            return false;
        }

        if (target == null)
        {
            return false;
        }

        if (unit.nearTarget)
        {
            return false;
        }

        return true;
    }

    public bool FromPlayer
    {
        get { return fromPlayer; }
    }

    public bool IsMoving
    {
        get { return isMoving; }
        set { isMoving = value; }
    }

    public Vector3 Target
    {
        get { return target; }
        set { target = value; }
    }

    public float DistanceCap
    {
        get { return distanceCap; }
        set { distanceCap = value; }
    }

}
