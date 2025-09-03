using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MoveAction : IAction
{
    Unit unit;

    bool fromPlayer;

    bool isMoving;
    Vector3 target;

    public void AssignUnit(Unit unit, bool player)
    {
        this.unit = unit;
        fromPlayer = player;
        isMoving = true;
    }

    public void Execute()
    {
        if (!CheckCanExecute())
        {
            return;
        }

        Move();
    }

    void Move()
    {
        // Translate Position
        if (Vector3.Distance(unit.gameObject.transform.position, target) > unit.distanceCap + unit.data.MovementSpeed)
        {
            //Debug.Log(name + " moving... ");

            isMoving = true;
            unit.gameObject.transform.position = Vector3.MoveTowards(unit.gameObject.transform.position, target, unit.data.MovementSpeed);

            //Debug.Log(name + " moved... ");
        } 
        else
        {
            //Debug.Log(name + " not moving... ");

            isMoving = false;
            unit.near = true;
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

        if (unit.near)
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

}
