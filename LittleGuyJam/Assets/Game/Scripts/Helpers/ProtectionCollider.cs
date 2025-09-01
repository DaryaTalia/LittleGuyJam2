using UnityEngine;

[RequireComponent(typeof(Collider2D))]

public class ProtectionCollider : MonoBehaviour
{
    System.Collections.Generic.List<Unit> targets;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Unit target = collision.gameObject.GetComponent<Unit>();

        if (target != null && gameObject.GetComponentInParent<Unit>().Alignment != collision.gameObject.GetComponent<Unit>().Alignment)
        {
            targets.Add(target);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Unit target = collision.gameObject.GetComponent<Unit>();

        if (target != null)
        {
            targets.Remove(target);
        }
    }

    public System.Collections.Generic.List<Unit> Targets
    {
        get { return targets; }
    }

    public Unit ReturnRandomTarget()
    {
        int random = Random.Range(0, targets.Count);
        return targets[random];
    }
}
