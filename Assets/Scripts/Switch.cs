using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] public Sprite DeactivatedSprite;
    [SerializeField] public Sprite ActivatedSprite;
    [Space(10)]
    [SerializeField] public GameObject[] ObjectsToActivateOnActivation;
    [SerializeField] public GameObject[] ObjectsToDeactivateOnActivation;
    [Space(10)]
    [SerializeField] public bool IsReversible = false;
    [SerializeField] public bool StartActivated = false;

    private SpriteRenderer _spriteRenderer;

    private bool _isActivated;

    private void Start()
    {
        //init fields
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _isActivated = StartActivated;

        //set sprite
        ChangeSprite();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerAttackHitbox playerAttack = other.GetComponent<PlayerAttackHitbox>();

        if (playerAttack != null)
        {
            if (IsReversible)
            {
                _isActivated = !_isActivated;
            }
            else
            {
                _isActivated = !StartActivated;
            }
        }

        foreach (GameObject obj in ObjectsToActivateOnActivation)
        {
            obj.SetActive(_isActivated);
        }

        foreach (GameObject obj in ObjectsToDeactivateOnActivation)
        {
            obj.SetActive(!_isActivated);
        }

        ChangeSprite();
    }

    private void ChangeSprite()
    {
        if (_isActivated)
        {
            _spriteRenderer.sprite = ActivatedSprite;
        }
        else
        {
            _spriteRenderer.sprite = DeactivatedSprite;
        }
    }
}
