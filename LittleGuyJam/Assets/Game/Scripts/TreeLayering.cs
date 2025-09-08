using UnityEngine;

public class TreeLayering : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GetComponent<SpriteRenderer>().sortingOrder = (int) -transform.position.y;
    }
}
