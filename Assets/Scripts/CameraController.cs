using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] public PlayerController player;

    [SerializeField] public bool isFollowingPlayer;

    [SerializeField] public float xOffset;
    [SerializeField] public float yOffset;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        isFollowingPlayer = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowingPlayer)
        {
            transform.position = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + yOffset, transform.position.z);
        }
        
    }
}
