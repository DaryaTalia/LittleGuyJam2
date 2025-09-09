using UnityEngine;

public class InfoCanvas : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        GetComponent<Canvas>().sortingLayerName = "Objects";
        GetComponent<Canvas>().sortingOrder = 1000;
    }
}
