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
        if (activeBuilding)
        {
            GameManager.instance.SendMessage("BuyUnit", this);
        }
    }

    private void OnMouseEnter()
    {
        if (infoUI != null)
        {
            infoUI.SetActive(true);

            GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, .5f);
        }
    }

    private void OnMouseExit()
    {
        if (infoUI != null)
        {
            infoUI.SetActive(false);

            GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 1f);
        }
    }
}
