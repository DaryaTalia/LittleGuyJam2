using UnityEngine;

public class MapCollider : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManager.instance.UnitManager.selectedUnits.Count > 0)
        {
            foreach (Unit u in GameManager.instance.UnitManager.selectedUnits)
            {
                u.NextTarget = transform.position;
                u.NewMoveAction(true);
            }
        }
    }
}
