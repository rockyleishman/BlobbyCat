using UnityEngine;

public class MinorCheckpoint : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            DataManager.Instance.PlayerStatusObject.CurrentMinorCheckpoint = this;
        }
    }
}
