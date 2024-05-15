using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private void Start()
    {
        //init fields
        _playerValuesObject = DataManager.Instance.PlayerValuesObject;
        _playerStatusObject = DataManager.Instance.PlayerStatusObject;
        _isHPVisible = false;
        _isTreatBarVisible = false;
        _isTreatCountVisible = false;

        //init animator parameters
        UpdateHP();
    }

    public void UpdateHP()
    {
        HPAnimator.SetInteger("MaxHP", _playerStatusObject.MaxHitPoints);
        HPAnimator.SetInteger("CurrentHP", _playerStatusObject.CurrentHitPoints);
    }

    public void UpdateTreatCount(bool isLargeTreat)
    {
        StopCoroutine("OnUpdateTreatCount");
        StartCoroutine(OnUpdateTreatCount(isLargeTreat));
    }

    public IEnumerator OnUpdateTreatCount(bool isLargeTreat)
    {
        //show HUD if hidden
        if (isLargeTreat && !_isTreatBarVisible && !_isTreatCountVisible)
        {
            //if large treat and HUD is hidden
            int targetY = -_playerValuesObject.HUDPixelsFromTop - (int)TreatCountTransform.rect.height / 2;
            while ((int)TreatCountTransform.localPosition.y > targetY)
            {
                TreatCountTransform.localPosition = new Vector3(TreatCountTransform.localPosition.x, TreatCountTransform.localPosition.y - 1.0f, 0.0f);
                TreatBarTransform.localPosition = new Vector3(TreatBarTransform.localPosition.x, TreatBarTransform.localPosition.y - 1.0f, 0.0f);

                yield return new WaitForSeconds(_playerValuesObject.HUDRevealTime / (_playerValuesObject.HUDPixelsFromTop + TreatCountTransform.rect.height));
            }
        }
        else if (isLargeTreat && _isTreatBarVisible && !_isTreatCountVisible)
        {
            //if large treat and count HUD is hidden
            int targetY = -_playerValuesObject.HUDPixelsFromTop - (int)TreatCountTransform.rect.height / 2;
            while ((int)TreatCountTransform.localPosition.y > targetY)
            {
                TreatCountTransform.localPosition = new Vector3(TreatCountTransform.localPosition.x, TreatCountTransform.localPosition.y - 1.0f, 0.0f);

                yield return new WaitForSeconds(_playerValuesObject.HUDRevealTime / (_playerValuesObject.HUDPixelsFromTop + TreatCountTransform.rect.height));
            }
        }
        else if (!isLargeTreat && !_isTreatBarVisible)
        {
            //if small treat and HUD is hidden
            int targetY = -_playerValuesObject.HUDPixelsFromTop - (int)TreatBarTransform.rect.height / 2;
            while ((int)TreatBarTransform.localPosition.y > targetY)
            {
                TreatBarTransform.localPosition = new Vector3(TreatBarTransform.localPosition.x, TreatBarTransform.localPosition.y - 1.0f, 0.0f);

                yield return new WaitForSeconds(_playerValuesObject.HUDRevealTime / (_playerValuesObject.HUDPixelsFromTop + TreatBarTransform.rect.height));
            }
        }

        //TODO: increase bar

        //TODO: increase count
    }
}
