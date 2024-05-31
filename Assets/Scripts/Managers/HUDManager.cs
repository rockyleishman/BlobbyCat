using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class HUDManager : Singleton<HUDManager>
{
    private PlayerValues _playerValuesObject;
    private PlayerStatus _playerStatusObject;

    [SerializeField] public RectTransform HPTransform;
    [SerializeField] public Animator HPAnimator;
    [SerializeField] public RectTransform TreatBarTransform;
    [SerializeField] public Animator TreatBarAnimator;
    [SerializeField] public RectTransform TreatCountTransform;
    [SerializeField] public Animator TreatCountSymbolAnimator;
    [SerializeField] public Animator TreatCountThousandsAnimator;
    [SerializeField] public Animator TreatCountHundredsAnimator;
    [SerializeField] public Animator TreatCountTensAnimator;
    [SerializeField] public Animator TreatCountOnesAnimator;

    private bool _isHPVisible;
    private bool _isTreatBarVisible;
    private bool _isTreatCountVisible;

    private Coroutine _onUpdateHPCoroutine;
    private Coroutine _delayedHPHideCoroutine;
    private Coroutine _onUpdateTreatCountCoroutine;
    private Coroutine _delayedTreatHideCoroutine;

    private void Awake()
    {
        //init fields
        _playerValuesObject = DataManager.Instance.PlayerValuesObject;
        _playerStatusObject = DataManager.Instance.PlayerStatusObject;
        _isHPVisible = false;
        _isTreatBarVisible = false;
        _isTreatCountVisible = false;
        _onUpdateHPCoroutine = null;
        _delayedHPHideCoroutine = null;
        _onUpdateTreatCountCoroutine = null;
        _delayedTreatHideCoroutine = null;

        //init animator parameters
        UpdateHP();
        UpdateTreatCount(true);
    }

    public void UpdateHP()
    {
        //only allow one instance of OnUpdateHP to run
        if (_onUpdateHPCoroutine != null)
        {
            StopCoroutine(_onUpdateHPCoroutine);
        }
        //stop hide coroutine
        if (_delayedHPHideCoroutine != null)
        {
            StopCoroutine(_delayedHPHideCoroutine);
        }
        //run OnUpdateHP
        _onUpdateHPCoroutine = StartCoroutine(OnUpdateHP());
    }

    private IEnumerator OnUpdateHP()
    {
        //update max hp
        if (HPAnimator.GetInteger("MaxHP") != _playerStatusObject.MaxHitPoints)
        {
            HPAnimator.SetInteger("MaxHP", _playerStatusObject.MaxHitPoints);

            //update treat bar position
            switch (_playerStatusObject.MaxHitPoints)
            {
                case 3:
                    {
                        float positionX = -GetComponentInParent<CanvasScaler>().referenceResolution.x / 2.0f + _playerValuesObject.HUDPixelsFromSide + _playerValuesObject.HUD3HPLength + _playerValuesObject.HUDPixelsInBetween + TreatBarTransform.rect.width / 2.0f;
                        TreatBarTransform.anchoredPosition = new Vector2(positionX, TreatBarTransform.anchoredPosition.y);
                        TreatCountTransform.anchoredPosition = new Vector2(positionX, TreatCountTransform.anchoredPosition.y);
                    }
                    break;

                case 4:
                    {
                        float positionX = -GetComponentInParent<CanvasScaler>().referenceResolution.x / 2.0f + _playerValuesObject.HUDPixelsFromSide + _playerValuesObject.HUD4HPLength + _playerValuesObject.HUDPixelsInBetween + TreatBarTransform.rect.width / 2.0f;
                        TreatBarTransform.anchoredPosition = new Vector2(positionX, TreatBarTransform.anchoredPosition.y);
                        TreatCountTransform.anchoredPosition = new Vector2(positionX, TreatCountTransform.anchoredPosition.y);
                    }
                    break;

                case 5:
                    {
                        float positionX = -GetComponentInParent<CanvasScaler>().referenceResolution.x / 2.0f + _playerValuesObject.HUDPixelsFromSide + _playerValuesObject.HUD5HPLength + _playerValuesObject.HUDPixelsInBetween + TreatBarTransform.rect.width / 2.0f;
                        TreatBarTransform.anchoredPosition = new Vector2(positionX, TreatBarTransform.anchoredPosition.y);
                        TreatCountTransform.anchoredPosition = new Vector2(positionX, TreatCountTransform.anchoredPosition.y);
                    }
                    break;

                case 6:
                    {
                        float positionX = -GetComponentInParent<CanvasScaler>().referenceResolution.x / 2.0f + _playerValuesObject.HUDPixelsFromSide + _playerValuesObject.HUD6HPLength + _playerValuesObject.HUDPixelsInBetween + TreatBarTransform.rect.width / 2.0f;
                        TreatBarTransform.anchoredPosition = new Vector2(positionX, TreatBarTransform.anchoredPosition.y);
                        TreatCountTransform.anchoredPosition = new Vector2(positionX, TreatCountTransform.anchoredPosition.y);
                    }
                    break;

                case 7:
                    {
                        float positionX = -GetComponentInParent<CanvasScaler>().referenceResolution.x / 2.0f + _playerValuesObject.HUDPixelsFromSide + _playerValuesObject.HUD7HPLength + _playerValuesObject.HUDPixelsInBetween + TreatBarTransform.rect.width / 2.0f;
                        TreatBarTransform.anchoredPosition = new Vector2(positionX, TreatBarTransform.anchoredPosition.y);
                        TreatCountTransform.anchoredPosition = new Vector2(positionX, TreatCountTransform.anchoredPosition.y);
                    }
                    break;

                case 8:
                    {
                        float positionX = -GetComponentInParent<CanvasScaler>().referenceResolution.x / 2.0f + _playerValuesObject.HUDPixelsFromSide + _playerValuesObject.HUD8HPLength + _playerValuesObject.HUDPixelsInBetween + TreatBarTransform.rect.width / 2.0f;
                        TreatBarTransform.anchoredPosition = new Vector2(positionX, TreatBarTransform.anchoredPosition.y);
                        TreatCountTransform.anchoredPosition = new Vector2(positionX, TreatCountTransform.anchoredPosition.y);
                    }
                    break;

                case 9:
                default:
                    {
                        float positionX = -GetComponentInParent<CanvasScaler>().referenceResolution.x / 2.0f + _playerValuesObject.HUDPixelsFromSide + _playerValuesObject.HUD9HPLength + _playerValuesObject.HUDPixelsInBetween + TreatBarTransform.rect.width / 2.0f;
                        TreatBarTransform.anchoredPosition = new Vector2(positionX, TreatBarTransform.anchoredPosition.y);
                        TreatCountTransform.anchoredPosition = new Vector2(positionX, TreatCountTransform.anchoredPosition.y);
                    }
                    break;
            }
        }

        //show HUD if hidden
        if (!_isHPVisible)
        {
            int targetY = -_playerValuesObject.HUDPixelsFromTop - (int)(HPTransform.rect.height / 2);
            while ((int)HPTransform.anchoredPosition.y > targetY)
            {
                HPTransform.anchoredPosition = new Vector2(HPTransform.anchoredPosition.x, HPTransform.anchoredPosition.y - 1.0f);

                yield return new WaitForSeconds(_playerValuesObject.HUDRevealTime / (_playerValuesObject.HUDPixelsFromTop + HPTransform.rect.height));
            }

            HPTransform.anchoredPosition = new Vector2(HPTransform.anchoredPosition.x, targetY);

            _isHPVisible = true;
        }

        //update hp
        HPAnimator.SetInteger("CurrentHP", _playerStatusObject.CurrentHitPoints);

        //hide after delay
        _delayedHPHideCoroutine = StartCoroutine(DelayedHPHide());
    }

    private IEnumerator DelayedHPHide()
    {
        float hpRevealTimer = _playerValuesObject.HUDShowHPTime;

        //wait until after changes made and delay expired
        while (hpRevealTimer > 0.0f)
        {
            hpRevealTimer -= Time.deltaTime;

            if (HPAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Change"))
            {
                hpRevealTimer = _playerValuesObject.HUDShowHPTime;
            }

            yield return null;
        }

        //hide HP HUD
        _isHPVisible = false;

        int targetY = (int)(HPTransform.rect.height / 2);
        while ((int)HPTransform.anchoredPosition.y < targetY)
        {
            HPTransform.anchoredPosition = new Vector2(HPTransform.anchoredPosition.x, HPTransform.anchoredPosition.y + 1.0f);

            yield return new WaitForSeconds(_playerValuesObject.HUDHideTime / (_playerValuesObject.HUDPixelsFromTop + HPTransform.rect.height));
        }

        HPTransform.anchoredPosition = new Vector2(HPTransform.anchoredPosition.x, targetY);
    }

    public void UpdateTreatCount(bool isLargeTreat)
    {
        //only allow one instance of OnUpdateTreatCount to run
        if (_onUpdateTreatCountCoroutine != null)
        {
            StopCoroutine(_onUpdateTreatCountCoroutine);            
        }
        //stop hide coroutine
        if (_delayedTreatHideCoroutine != null)
        {
            StopCoroutine(_delayedTreatHideCoroutine);
        }
        //run OnUpdateTreatCount
        _onUpdateTreatCountCoroutine = StartCoroutine(OnUpdateTreatCount(isLargeTreat));
    }

    private IEnumerator OnUpdateTreatCount(bool isLargeTreat)
    {
        //show HUD if hidden
        if (isLargeTreat && !_isTreatBarVisible && !_isTreatCountVisible)
        {
            //if large treat and HUD is hidden
            int targetY = -_playerValuesObject.HUDPixelsFromTop - (int)(TreatCountTransform.rect.height / 2);
            while ((int)TreatCountTransform.anchoredPosition.y > targetY)
            {
                TreatCountTransform.anchoredPosition = new Vector2(TreatCountTransform.anchoredPosition.x, TreatCountTransform.anchoredPosition.y - 1.0f);
                TreatBarTransform.anchoredPosition = new Vector2(TreatBarTransform.anchoredPosition.x, TreatBarTransform.anchoredPosition.y - 1.0f);
                
                yield return new WaitForSeconds(_playerValuesObject.HUDRevealTime / (_playerValuesObject.HUDPixelsFromTop + TreatCountTransform.rect.height));
            }

            TreatCountTransform.anchoredPosition = new Vector2(TreatCountTransform.anchoredPosition.x, targetY);
            TreatBarTransform.anchoredPosition = new Vector2(TreatBarTransform.anchoredPosition.x, targetY);

            _isTreatCountVisible = true;
            _isTreatBarVisible = true;
        }
        else if (isLargeTreat && _isTreatBarVisible && !_isTreatCountVisible)
        {
            //if large treat and count HUD is hidden
            int targetY = -_playerValuesObject.HUDPixelsFromTop - (int)(TreatCountTransform.rect.height / 2);
            while ((int)TreatCountTransform.anchoredPosition.y > targetY)
            {
                TreatCountTransform.anchoredPosition = new Vector2(TreatCountTransform.anchoredPosition.x, TreatCountTransform.anchoredPosition.y - 1.0f);
                
                yield return new WaitForSeconds(_playerValuesObject.HUDRevealTime / (_playerValuesObject.HUDPixelsFromTop + TreatCountTransform.rect.height));
            }

            TreatCountTransform.anchoredPosition = new Vector2(TreatCountTransform.anchoredPosition.x, targetY);

            _isTreatCountVisible = true;
        }
        else if (!isLargeTreat && !_isTreatBarVisible)
        {
            //if small treat and HUD is hidden
            int targetY = -_playerValuesObject.HUDPixelsFromTop - (int)(TreatBarTransform.rect.height / 2);
            while ((int)TreatBarTransform.anchoredPosition.y > targetY)
            {
                TreatBarTransform.anchoredPosition = new Vector2(TreatBarTransform.anchoredPosition.x, TreatBarTransform.anchoredPosition.y - 1.0f);
                
                yield return new WaitForSeconds(_playerValuesObject.HUDRevealTime / (_playerValuesObject.HUDPixelsFromTop + TreatBarTransform.rect.height));
            }

            TreatBarTransform.anchoredPosition = new Vector2(TreatBarTransform.anchoredPosition.x, targetY);

            _isTreatBarVisible = true;
        }

        //update bar
        TreatBarAnimator.SetInteger("SubHitPoints", _playerStatusObject.CurrentSubHitPoints);

        //TODO: increase count

        //hide after delay
        _delayedTreatHideCoroutine = StartCoroutine(DelayedTreatHide());
    }

    private IEnumerator DelayedTreatHide()
    {
        float treatRevealTimer = _playerValuesObject.HUDShowTreatTime;

        //wait until after changes made and delay expired
        while (treatRevealTimer > 0.0f)
        {
            treatRevealTimer -= Time.deltaTime;

            if (TreatBarAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Change"))
            {
                treatRevealTimer = _playerValuesObject.HUDShowTreatTime;
            }
            //TODO: same for numbers

            yield return null;
        }

        //hide treat HUD
        _isTreatCountVisible = false;
        _isTreatBarVisible = false;

        int targetY = (int)(TreatCountTransform.rect.height / 2);
        while ((int)TreatCountTransform.anchoredPosition.y < targetY)
        {
            TreatCountTransform.anchoredPosition = new Vector2(TreatCountTransform.anchoredPosition.x, TreatCountTransform.anchoredPosition.y + 1.0f);
            TreatBarTransform.anchoredPosition = new Vector2(TreatBarTransform.anchoredPosition.x, TreatBarTransform.anchoredPosition.y + 1.0f);
                
            yield return new WaitForSeconds(_playerValuesObject.HUDHideTime / (_playerValuesObject.HUDPixelsFromTop + TreatCountTransform.rect.height));
        }

        TreatCountTransform.anchoredPosition = new Vector2(TreatCountTransform.anchoredPosition.x, targetY);
        TreatBarTransform.anchoredPosition = new Vector2(TreatBarTransform.anchoredPosition.x, targetY);
    }
}
