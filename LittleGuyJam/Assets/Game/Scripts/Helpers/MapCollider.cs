using UnityEngine;

public class MapCollider : MonoBehaviour
{
    private void OnMouseDown()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));

        if (GameManager.instance.UnitManager.selectedUnits.Count > 0)
        {
            foreach (Unit u in GameManager.instance.UnitManager.selectedUnits)
            {
                u.NextTarget = worldPosition;
                u.actionQueue.Insert(0, u.NewMoveAction(true));
            }
        }
    }
}
