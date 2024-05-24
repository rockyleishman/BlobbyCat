using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] public ActionTrigger OpenActionTrigger;
    [SerializeField] public ActionTrigger CloseActionTrigger;
    [SerializeField] public Collider2D Collider1;
    [SerializeField] public Collider2D Collider2;
    [SerializeField] public bool CollidersAreInClosedPosition = true;
    [Space(10)]
    [SerializeField] public bool RequiresKeyToOpen = true;
    [SerializeField] public bool StartsClosed = true;
    [SerializeField] public float GateSize = 3.0f;
    [SerializeField] public float GateTime = 1.0f;
    [SerializeField] public bool IsVertical = true;
    [SerializeField] public bool GateSlidesUpOrRight = true;

    private bool _isOpen;
    private bool _isUnlocked;

    private Vector3 _collider1ClosedPosition;
    private Vector3 _collider2ClosedPosition;
    private Vector3 _collider1OpenPosition;
    private Vector3 _collider2OpenPosition;

    private void Start()
    {
        //init fields
        _animator = GetComponent<Animator>();
        _isOpen = !StartsClosed;
        _isUnlocked = !RequiresKeyToOpen;
        SetColliderPositions();

        //show/hide UI prompts
        if (_isOpen)
        {
            if (OpenActionTrigger != null)
            {
                OpenActionTrigger.HideUIPrompt();
            }
            if (CloseActionTrigger != null)
            {
                CloseActionTrigger.ShowUIPrompt();
            }
        }
        else
        {
            if (OpenActionTrigger != null)
            {
                OpenActionTrigger.ShowUIPrompt();
            }
            if (CloseActionTrigger != null)
            {
                CloseActionTrigger.HideUIPrompt();
            }
        }
    }

    private void SetColliderPositions()
    {
        //collider 1
        if (CollidersAreInClosedPosition)
        {
            _collider1ClosedPosition = Collider1.transform.localPosition;

            if (IsVertical && Collider2 != null)
            {
                _collider1OpenPosition = _collider1ClosedPosition + new Vector3(0.0f, GateSize / 2.0f, 0.0f);
            }
            else if (IsVertical && GateSlidesUpOrRight)
            {
                _collider1OpenPosition = _collider1ClosedPosition + new Vector3(0.0f, GateSize, 0.0f);
            }
            else if (IsVertical)
            {
                _collider1OpenPosition = _collider1ClosedPosition + new Vector3(0.0f, -GateSize, 0.0f);
            }
            else if (Collider2 != null)
            {
                _collider1OpenPosition = _collider1ClosedPosition + new Vector3(GateSize / 2.0f, 0.0f, 0.0f);
            }
            else if (GateSlidesUpOrRight)
            {
                _collider1OpenPosition = _collider1ClosedPosition + new Vector3(GateSize, 0.0f, 0.0f);
            }
            else
            {
                _collider1OpenPosition = _collider1ClosedPosition + new Vector3(-GateSize, 0.0f, 0.0f);
            }
        }
        else
        {
            _collider1OpenPosition = Collider1.transform.localPosition;

            if (IsVertical && Collider2 != null)
            {
                _collider1ClosedPosition = _collider1OpenPosition + new Vector3(0.0f, -GateSize / 2.0f, 0.0f);
            }
            else if (IsVertical && GateSlidesUpOrRight)
            {
                _collider1ClosedPosition = _collider1OpenPosition + new Vector3(0.0f, -GateSize, 0.0f);
            }
            else if (IsVertical)
            {
                _collider1ClosedPosition = _collider1OpenPosition + new Vector3(0.0f, GateSize, 0.0f);
            }
            else if (Collider2 != null)
            {
                _collider1ClosedPosition = _collider1OpenPosition + new Vector3(-GateSize / 2.0f, 0.0f, 0.0f);
            }
            else if (GateSlidesUpOrRight)
            {
                _collider1ClosedPosition = _collider1OpenPosition + new Vector3(-GateSize, 0.0f, 0.0f);
            }
            else
            {
                _collider1ClosedPosition = _collider1OpenPosition + new Vector3(GateSize, 0.0f, 0.0f);
            }
        }

        //collider 2
        if (Collider2 != null)
        {
            if (CollidersAreInClosedPosition)
            {
                _collider2ClosedPosition = Collider2.transform.localPosition;

                if (IsVertical)
                {
                    _collider2OpenPosition = _collider2ClosedPosition + new Vector3(0.0f, -GateSize / 2.0f, 0.0f);
                }
                else
                {
                    _collider2OpenPosition = _collider2ClosedPosition + new Vector3(-GateSize / 2.0f, 0.0f, 0.0f);
                }
            }
            else
            {
                _collider2OpenPosition = Collider2.transform.localPosition;

                if (IsVertical)
                {
                    _collider2ClosedPosition = _collider2OpenPosition + new Vector3(0.0f, GateSize / 2.0f, 0.0f);
                }
                else
                {
                    _collider2ClosedPosition = _collider2OpenPosition + new Vector3(GateSize / 2.0f, 0.0f, 0.0f);
                }
            }
        }
        else
        {
            _collider2ClosedPosition = Vector3.zero;
            _collider2OpenPosition = Vector3.zero;
        }
    }

    public void Open()
    {
        if (!_isUnlocked && DataManager.Instance.LevelCollectionDataObject.KeysHeld > 0)
        {
            //unlock
            DataManager.Instance.LevelCollectionDataObject.KeysHeld--;
            _isUnlocked = true;
        }

        if (!_isOpen && _isUnlocked)
        {
            //start moving colliders
            StartCoroutine(OnOpen());

            //hide UI prompt
            if (OpenActionTrigger != null)
            {
                OpenActionTrigger.HideUIPrompt();
            }

            //play animation
            _animator.SetTrigger("Open");
        }        
    }

    private IEnumerator OnOpen()
    {
        float time = 0.0f;

        while (time < GateTime)
        {
            time += Time.deltaTime;

            Collider1.transform.localPosition = Vector3.Lerp(_collider1ClosedPosition, _collider1OpenPosition, time / GateTime);
            if (Collider2 != null)
            {
                Collider2.transform.localPosition = Vector3.Lerp(_collider2ClosedPosition, _collider2OpenPosition, time / GateTime);
            }

            yield return null;
        }

        _isOpen = true;

        //show UI prompt
        if (CloseActionTrigger != null)
        {
            CloseActionTrigger.ShowUIPrompt();
        }
    }

    public void Close()
    {
        if (_isOpen)
        {
            //start moving colliders
            StartCoroutine(OnClose());

            //hide UI prompt
            if (CloseActionTrigger != null)
            {
                CloseActionTrigger.HideUIPrompt();
            }

            //play animation
            _animator.SetTrigger("Close");
        }            
    }

    private IEnumerator OnClose()
    {
        float time = 0.0f;

        while (time < GateTime)
        {
            time += Time.deltaTime;

            Collider1.transform.localPosition = Vector3.Lerp(_collider1OpenPosition, _collider1ClosedPosition, time / GateTime);
            if (Collider2 != null)
            {
                Collider2.transform.localPosition = Vector3.Lerp(_collider2OpenPosition, _collider2ClosedPosition, time / GateTime);
            }

            yield return null;
        }

        _isOpen = false;

        //show UI prompt
        if (OpenActionTrigger != null)
        {
            OpenActionTrigger.ShowUIPrompt();
        }
    }
}
