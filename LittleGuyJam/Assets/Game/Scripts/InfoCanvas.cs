using UnityEngine;

public class InfoCanvas : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        GetComponent<Canvas>().sortingLayerName = "UI";
        GetComponent<Canvas>().sortingOrder = 10;
    }
}
