using UnityEngine;

public class MapCollider : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManager.instance.UnitManager.selectedUnits.Count > 0)
        {
            foreach (Unit r in GameManager.instance.UnitManager.selectedUnits)
            {
                r.Target = transform.position;
                r.TargetAssigned = true;
                r.PlayerDirected = true;
                r.CanMove = true;
            }
        }
    }
}
