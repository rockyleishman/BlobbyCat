using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private PlayerStatus _playerStatusObject;
    private GameStatus _gameStatusObject;

    [SerializeField] public MajorCheckpoint StartingCheckpoint;
    [Space(10)]
    [SerializeField] public bool IsHubLevel;
    [SerializeField] public bool HasDebugMenu;
    [Space(10)]
    [SerializeField] public Color LevelFadeColour = Color.black;
    [SerializeField] public float LevelFadeInTime = 1.0f;
    [SerializeField] public float DamageRespawnInTime = 0.5f;
    [SerializeField] public float DamageRespawnFadeOutTime = 1.0f;
    [SerializeField] public float DamageRespawnOutTime = 0.5f;
    [SerializeField] public float DamageRespawnFadeInTime = 1.0f;
    [SerializeField] public float DeathRespawnInTime = 0.5f;
    [SerializeField] public float DeathRespawnFadeOutTime = 1.0f;
    [SerializeField] public float DeathRespawnOutTime = 0.5f;
    [SerializeField] public float DeathRespawnFadeInTime = 1.0f;

    private bool _isRespawning;

    private void Awake()
    {
        //init fields
        _playerStatusObject = DataManager.Instance.PlayerStatusObject;
        _gameStatusObject = DataManager.Instance.GameStatusObject;
        _isRespawning = false;

        _playerStatusObject.MaxHitPoints = 3;/////////////////////////remove
        //set game status object
        SetGameStatus();

        //TODO: change when implementing persistant player data
        //reset player status object
        ResetPlayerStatus();
    }

    private void Start()
    {
        //fade in at start of level
        FadeManager.Instance.FadeIn(LevelFadeColour, LevelFadeInTime);
    }

    private void ResetPlayerStatus()
    {
        _playerStatusObject.Player = FindFirstObjectByType<Player>();

        _playerStatusObject.CurrentMajorCheckpoint = StartingCheckpoint;
        _playerStatusObject.CurrentMinorCheckpoint = StartingCheckpoint;

        _playerStatusObject.CurrentSubHitPoints = 0;
        _playerStatusObject.CurrentHitPoints = _playerStatusObject.MaxHitPoints;

        _playerStatusObject.IsFacingRight = true;

        ResetPlayerStates();
    }

    private void SetGameStatus()
    {
        //TODO: set based on save data/////////////////////////////////////////////////////////
        _gameStatusObject.unlockedDash = false;
        _gameStatusObject.unlockedClimb = false;
        _gameStatusObject.unlockedLiquidCat = false;
        _gameStatusObject.unlockedChonkMode = false;
        _gameStatusObject.unlockedDoubleJump = false;
    }

    private void ResetPlayerStates()
    {
        _playerStatusObject.IsGrounded = false;

        _playerStatusObject.IsCrouching = false;

        _playerStatusObject.IsAttacking = false;
        _playerStatusObject.IsSlapAttacking = false;
        _playerStatusObject.IsSlamAttacking = false;
        _playerStatusObject.IsSpinAttacking = false;
        _playerStatusObject.IsPounceAttacking = false;

        _playerStatusObject.HasDartToken = true;

        _playerStatusObject.HasGeneralJumpToken = true;
        _playerStatusObject.IsJumping = false;
        _playerStatusObject.IsDamageJumping = false;
        _playerStatusObject.HasHighJumpToken = false;
        _playerStatusObject.IsHighJumping = false;
        _playerStatusObject.HasSingleJumpToken = false;
        _playerStatusObject.IsSingleJumping = false;
        _playerStatusObject.HasDoubleJumpToken = false;
        _playerStatusObject.IsDoubleJumping = false;
        _playerStatusObject.HasTripleJumpToken = false;
        _playerStatusObject.IsTripleJumping = false;
        _playerStatusObject.IsPounceJumping = false;
    }

    public void DamageRespawn()
    {
        if (!_isRespawning)
        {
            StartCoroutine(DamageRespawnCoroutine());
        }
    }

    private IEnumerator DamageRespawnCoroutine()
    {
        _isRespawning = true;

        _playerStatusObject.Player.gameObject.SetActive(false);

        //TODO: effects

        //hide
        _playerStatusObject.Player.GetComponent<Renderer>().enabled = false;

        //in time
        yield return new WaitForSeconds(DamageRespawnInTime);

        //fade out time
        FadeManager.Instance.FadeOut(LevelFadeColour, DamageRespawnFadeOutTime);
        yield return new WaitForSeconds(DamageRespawnFadeOutTime);

        //teleport to minor checkpoint & zero velocity
        _playerStatusObject.Player.transform.position = _playerStatusObject.CurrentMinorCheckpoint.transform.position;
        _playerStatusObject.Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        //out time
        yield return new WaitForSeconds(DamageRespawnOutTime);

        //fade in time
        FadeManager.Instance.FadeIn(LevelFadeColour, DamageRespawnFadeInTime);

        //reset player states
        ResetPlayerStates();

        //enable player
        _playerStatusObject.Player.GetComponent<Renderer>().enabled = true;
        _playerStatusObject.Player.gameObject.SetActive(true);

        //TODO: respawn effects

        _isRespawning = false;
    }

    public void DeathRespawn()
    {
        if (!_isRespawning)
        {
            StartCoroutine(DeathRespawnCoroutine());
        }
    }

    private IEnumerator DeathRespawnCoroutine()
    {
        _isRespawning = true;

        //disable
        _playerStatusObject.Player.gameObject.SetActive(false);

        //TODO: play death animation & effects

        //hide
        _playerStatusObject.Player.GetComponent<Renderer>().enabled = false;

        //in time
        yield return new WaitForSeconds(DeathRespawnInTime);

        //fade out time
        FadeManager.Instance.FadeOut(LevelFadeColour, DeathRespawnFadeOutTime);
        yield return new WaitForSeconds(DeathRespawnFadeOutTime);

        //teleport to major checkpoint & zero velocity
        _playerStatusObject.Player.transform.position = _playerStatusObject.CurrentMajorCheckpoint.transform.position;
        _playerStatusObject.Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        //out time
        yield return new WaitForSeconds(DeathRespawnOutTime);

        //fade in time
        FadeManager.Instance.FadeIn(LevelFadeColour, DeathRespawnFadeInTime);

        //reset player states
        ResetPlayerStates();

        //enable player
        _playerStatusObject.Player.GetComponent<Renderer>().enabled = true;
        _playerStatusObject.Player.gameObject.SetActive(true);

        //TODO: respawn effects

        _isRespawning = false;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitLevel()
    {
        Application.Quit();//TODO: change when multiple levels /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
