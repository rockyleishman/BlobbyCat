using UnityEngine;

public class MajorCheckpoint : MinorCheckpoint
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            DataManager.Instance.PlayerStatusObject.CurrentMajorCheckpoint = this;
            DataManager.Instance.PlayerStatusObject.CurrentMinorCheckpoint = this;
        }
    }
}
