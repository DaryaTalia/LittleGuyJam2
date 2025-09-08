using Unity.VisualScripting;
using UnityEngine;

public class Building : GamePiece
{
    public BuildingData data;

    [SerializeField]
    public GameObject infoUI;

    [SerializeField]
    bool activeBuilding;

    private void OnMouseDown()
    {
        GameManager.instance.SendMessage("BuyUnit", this);
    }

    private void OnMouseEnter()
    {
        if (infoUI != null && activeBuilding)
        {
            infoUI.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        if (infoUI != null)
        {
            infoUI.SetActive(false);
        }
    }
}
