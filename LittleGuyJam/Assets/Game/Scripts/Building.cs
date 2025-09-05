using Unity.VisualScripting;
using UnityEngine;

public class Building : MonoBehaviour
{
    public BuildingData data;

    private void OnMouseDown()
    {
        GameManager.instance.SendMessage("BuyUnit", this);
    }
}
