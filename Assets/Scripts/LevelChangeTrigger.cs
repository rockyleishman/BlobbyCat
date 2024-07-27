using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChangeTrigger : MonoBehaviour
{
    [SerializeField] int LevelBuildIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerActionController player = other.GetComponent<PlayerActionController>();

        if (player != null)
        {
            SceneManager.LoadScene(LevelBuildIndex);
        }
    }
}
