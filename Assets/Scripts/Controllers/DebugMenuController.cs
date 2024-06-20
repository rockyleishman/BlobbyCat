using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugMenuController : MonoBehaviour
{
    [SerializeField] public TMP_Text ChangeHPText;
    [SerializeField] public string MaxHPString;
    [SerializeField] public string OneHPString;
    [SerializeField] public TMP_Text UnlockDashText;
    [SerializeField] public string UnlockDashString;
    [SerializeField] public string LockDashString;
    [SerializeField] public TMP_Text UnlockClimbText;
    [SerializeField] public string UnlockClimbString;
    [SerializeField] public string LockClimbString;
    [SerializeField] public TMP_Text UnlockLiquidCatText;
    [SerializeField] public string UnlockLiquidCatString;
    [SerializeField] public string LockLiquidCatString;
    [SerializeField] public TMP_Text UnlockChonkModeText;
    [SerializeField] public string UnlockChonkModeString;
    [SerializeField] public string LockChonkModeString;
    [SerializeField] public TMP_Text UnlockDoubleJumpText;
    [SerializeField] public string UnlockDoubleJumpString;
    [SerializeField] public string LockDoubleJumpString;

    private PlayerStatus _playerStatusObject;
    private GameStatus _gameStatusObject;
    private LevelCollectionData _levelCollectionData;

    private bool _initialized;

    private void OnEnable()
    {
        //init fields
        if (!_initialized)
        {
            _playerStatusObject = DataManager.Instance.PlayerStatusObject;
            _gameStatusObject = DataManager.Instance.GameStatusObject;
            _levelCollectionData = DataManager.Instance.LevelCollectionDataObject;

            _initialized = true;
        }        

        //update text
        UpdateText();
    }

    private void UpdateText()
    {
        if (_playerStatusObject.CurrentHitPoints == _playerStatusObject.MaxHitPoints)
        {
            ChangeHPText.text = OneHPString;
        }
        else
        {
            ChangeHPText.text = MaxHPString;
        }

        if (_gameStatusObject.unlockedDart)
        {
            UnlockDashText.text = LockDashString;
        }
        else
        {
            UnlockDashText.text = UnlockDashString;
        }

        if (_gameStatusObject.unlockedClimb)
        {
            UnlockClimbText.text = LockClimbString;
        }
        else
        {
            UnlockClimbText.text = UnlockClimbString;
        }

        if (_gameStatusObject.unlockedLiquidCat)
        {
            UnlockLiquidCatText.text = LockLiquidCatString;
        }
        else
        {
            UnlockLiquidCatText.text = UnlockLiquidCatString;
        }

        if (_gameStatusObject.unlockedChonkMode)
        {
            UnlockChonkModeText.text = LockChonkModeString;
        }
        else
        {
            UnlockChonkModeText.text = UnlockChonkModeString;
        }

        if (_gameStatusObject.unlockedDoubleJump)
        {
            UnlockDoubleJumpText.text = LockDoubleJumpString;
        }
        else
        {
            UnlockDoubleJumpText.text = UnlockDoubleJumpString;
        }
    }

    public void ChangeHP()
    {
        if (GameManager.Instance.HasDebugMenu)
        {
            if (_playerStatusObject.CurrentHitPoints != _playerStatusObject.MaxHitPoints)
            {
                _playerStatusObject.CurrentHitPoints = _playerStatusObject.MaxHitPoints;
            }
            else
            {
                _playerStatusObject.CurrentHitPoints = 1;
            }
            UpdateText();
            HUDManager.Instance.UpdateHP();
        }            
    }

    public void AddMaxHP()
    {
        if (GameManager.Instance.HasDebugMenu && _playerStatusObject.MaxHitPoints < 9)
        {
            _playerStatusObject.MaxHitPoints += 1;
            _playerStatusObject.CurrentHitPoints = _playerStatusObject.MaxHitPoints;
            HUDManager.Instance.UpdateHP();
        }
    }

    public void AddKey()
    {
        if (GameManager.Instance.HasDebugMenu)
        {
            _levelCollectionData.KeysHeld++;
        }
    }

    public void AddTreat()
    {
        if (GameManager.Instance.HasDebugMenu)
        {
            _levelCollectionData.TreatsCollected++;
            HUDManager.Instance.UpdateTreatCount(true);
        }
    }

    public void AddYarn()
    {
        if (GameManager.Instance.HasDebugMenu)
        {
            //TODO
        }
    }

    public void AddMainItem()
    {
        if (GameManager.Instance.HasDebugMenu)
        {
            //TODO
        }
    }

    public void UnlockDash()
    {
        if (GameManager.Instance.HasDebugMenu)
        {
            _gameStatusObject.unlockedDart = !_gameStatusObject.unlockedDart;
            UpdateText();
        }
    }

    public void UnlockClimb()
    {
        if (GameManager.Instance.HasDebugMenu)
        {
            _gameStatusObject.unlockedClimb = !_gameStatusObject.unlockedClimb;
            UpdateText();
        }
    }

    public void UnlockLiquidCat()
    {
        if (GameManager.Instance.HasDebugMenu)
        {
            _gameStatusObject.unlockedLiquidCat = !_gameStatusObject.unlockedLiquidCat;
            UpdateText();
        }
    }

    public void UnlockChonkMode()
    {
        if (GameManager.Instance.HasDebugMenu)
        {
            _gameStatusObject.unlockedChonkMode = !_gameStatusObject.unlockedChonkMode;
            UpdateText();
        }
    }

    public void UnlockDoubleJump()
    {
        if (GameManager.Instance.HasDebugMenu)
        {
            _gameStatusObject.unlockedDoubleJump = !_gameStatusObject.unlockedDoubleJump;
            UpdateText();
        }
    }
}
