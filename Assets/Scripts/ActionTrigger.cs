using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionTrigger : MonoBehaviour
{
    [SerializeField] public GameObject UIPrompt;
    [SerializeField] public UnityEvent Action;

    private bool _showUIPrompt;

    private void Start()
    {
        UIPrompt.SetActive(false);
    }

    public void OnAction()
    {
        Action.Invoke();
    }

    public void ShowUIPrompt()
    {
        _showUIPrompt = true;
    }

    public void HideUIPrompt()
    {
        _showUIPrompt = false;
        UIPrompt.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerActionController player = other.GetComponent<PlayerActionController>();

        if (player != null)
        {
            if (_showUIPrompt)
            {
                UIPrompt.SetActive(true);
            }

            player.CurrentActionTrigger = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerActionController player = other.GetComponent<PlayerActionController>();

        if (player != null)
        {
            UIPrompt.SetActive(false);

            if (player.CurrentActionTrigger == this)
            {
                player.CurrentActionTrigger = null;
            }
        }
    }
}
