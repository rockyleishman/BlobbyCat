using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private PlayerController player;
    private CameraController mainCamera;

    public GameObject currentCheckpoint;

    public float respawnDelay;

    public GameObject deathParticle;
    public GameObject respawnParticle;

    private bool isRespawning;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        mainCamera = FindObjectOfType<CameraController>();

        isRespawning = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Respawn Player
    public void RespawnPlayer()
    {
        if(!isRespawning)
        {
            StartCoroutine("RespawnPlayerCo");
        }
        
    }

    // Respawn CoRoutine
    public IEnumerator RespawnPlayerCo()
    {
        // disable multiple instances of respawning
        isRespawning = true;

        Renderer playerRenderer = player.GetComponent<Renderer>();
        Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();

        // death
        player.enabled = false;
        mainCamera.isFollowingPlayer = false;
        playerRenderer.enabled = false;
        // death effects
        Instantiate(deathParticle, player.transform.position, player.transform.rotation);

        // respawn delay
        yield return new WaitForSeconds(respawnDelay);

        Debug.Log("Player Respawn");
        // respawn
        player.transform.position = currentCheckpoint.transform.position;
        playerRenderer.enabled = true;
        mainCamera.isFollowingPlayer = true;
        player.enabled = true;
        playerRigidbody.velocity = Vector2.zero;
        // respawn effects
        Instantiate(respawnParticle, currentCheckpoint.transform.position, currentCheckpoint.transform.rotation);

        // allow respawning
        isRespawning = false;
    }

}
