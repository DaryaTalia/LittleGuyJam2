using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MapCollider : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManager.instance.UnitManager.selectedUnits.Count > 0)
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));

            foreach (Unit u in GameManager.instance.UnitManager.selectedUnits)
            {
                u.NextTarget = worldPosition;
                u.actionQueue.Add(u.NewMoveAction(true));
                u.actionQueue.Last<IAction>().ConvertTo<MoveAction>().DistanceCap = u.data.DistanceThreshold;
                GameManager.instance.UnitManager.selectedUnits.Remove(u);
                break;
            }
        }
    }
}
