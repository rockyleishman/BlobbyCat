using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Switch : MonoBehaviour
{
    [SerializeField] public Sprite UntouchedDeactivatedSprite;
    [SerializeField] public Sprite UntouchedActivatedSprite;
    [SerializeField] public Sprite DeactivatedSprite;
    [SerializeField] public Sprite ActivatedSprite;
    [SerializeField] public Sprite FinishedDeactivatedSprite;
    [SerializeField] public Sprite FinishedActivatedSprite;
    [Space(10)]
    [SerializeField] public bool HasLight;
    [SerializeField] public Color UntouchedDeactivatedLight;
    [SerializeField] public Color UntouchedActivatedLight;
    [SerializeField] public Color DeactivatedLight;
    [SerializeField] public Color ActivatedLight;
    [Space(10)]
    [SerializeField] public GameObject[] ObjectsToActivateOnActivation;
    [SerializeField] public GameObject[] ObjectsToDeactivateOnActivation;
    [Space(10)]
    [SerializeField] public bool IsReversible = false;
    [SerializeField] public bool StartActivated = false;

    private SpriteRenderer _spriteRenderer;
    private Light2D _light;

    private bool _isActivated;
    private bool _isTouched;

    private void Start()
    {
        //init fields
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _light = GetComponentInChildren<Light2D>();
        _isActivated = StartActivated;
        _isTouched = false;

        //turn light on/off
        if (HasLight)
        {
            _light.enabled = true;
        }
        else
        {
            _light.enabled = false;
        }

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
                _isTouched = true;

                ActivateObjects();

                ChangeSprite();
            }
            else if (!_isTouched)
            {
                _isActivated = !StartActivated;
                _isTouched = true;

                ActivateObjects();

                ChangeSprite();
            }
        }        
    }

    private void ActivateObjects()
    {
        foreach (GameObject obj in ObjectsToActivateOnActivation)
        {
            obj.SetActive(_isActivated);
        }

        foreach (GameObject obj in ObjectsToDeactivateOnActivation)
        {
            obj.SetActive(!_isActivated);
        }
    }

    private void ChangeSprite()
    {
        if (_isActivated && _isTouched && IsReversible)
        {
            _spriteRenderer.sprite = ActivatedSprite;
            _light.color = ActivatedLight;
        }
        else if (_isActivated && _isTouched)
        {
            _spriteRenderer.sprite = FinishedActivatedSprite;
            _light.color = ActivatedLight;
        }
        else if (_isTouched && IsReversible)
        {
            _spriteRenderer.sprite = DeactivatedSprite;
            _light.color = DeactivatedLight;
        }
        else if (_isTouched)
        {
            _spriteRenderer.sprite = FinishedDeactivatedSprite;
            _light.color = DeactivatedLight;
        }
        else if (_isActivated)
        {
            _spriteRenderer.sprite = UntouchedActivatedSprite;
            _light.color = UntouchedActivatedLight;
        }
        else
        {
            _spriteRenderer.sprite = UntouchedDeactivatedSprite;
            _light.color = UntouchedDeactivatedLight;
        }
    }
}
