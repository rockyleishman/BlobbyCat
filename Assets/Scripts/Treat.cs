using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treat : Collectable
{
    private PlayerValues _playerValuesObject;
    private PlayerStatus _playerStatusObject;

    private Animator _animator;
    private SpriteRenderer _renderer;

    private float _suctionTimer;

    private const string SmallBrownAnimation = "SmallTreat_Brown_Floating";
    private const string SmallRedAnimation = "SmallTreat_Red_Floating";
    private const string SmallGreenAnimation = "SmallTreat_Green_Floating";
    private const string SmallBlueAnimation = "SmallTreat_Blue_Floating";
    private const string BrownAnimation = "Treat_Brown_Floating";
    private const string RedAnimation = "Treat_Red_Floating";
    private const string GreenAnimation = "Treat_Green_Floating";
    private const string BlueAnimation = "Treat_Blue_Floating";

    private void Start()
    {
        //init fields
        _playerValuesObject = DataManager.Instance.PlayerValuesObject;
        _playerStatusObject = DataManager.Instance.PlayerStatusObject;
        _suctionTimer = 0.0f;
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        
        //start in floating animation state
        _animator.SetBool("IsFloating", true);

        //randomize animation
        RandomizeAnimation();
    }

    private void RandomizeAnimation()
    {
        int colourIndex = Random.Range(0, 4);

        if (PreviouslyCollected)
        {
            switch (colourIndex)
            {                
                case 1:
                    _animator.Play(SmallRedAnimation, -1, Random.value);
                    break;

                case 2:
                    _animator.Play(SmallGreenAnimation, -1, Random.value);
                    break;

                case 3:
                    _animator.Play(SmallBlueAnimation, -1, Random.value);
                    break;

                case 0:
                default:
                    _animator.Play(SmallBrownAnimation, -1, Random.value);
                    break;
            }
        }
        else
        {
            switch (colourIndex)
            {
                case 1:
                    _animator.Play(RedAnimation, -1, Random.value);
                    break;

                case 2:
                    _animator.Play(GreenAnimation, -1, Random.value);
                    break;

                case 3:
                    _animator.Play(BlueAnimation, -1, Random.value);
                    break;

                case 0:
                default:
                    _animator.Play(BrownAnimation, -1, Random.value);
                    break;
            }
        }

        _renderer.flipX = Random.value > 0.5f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            Collect();
        }
    }

    private void Collect()
    {
        //TODO: collect via manager
        StopAllCoroutines();
        Destroy(gameObject);
    }

    public void Suck()
    {
        StartCoroutine(SuckCoroutine());
    }

    private IEnumerator SuckCoroutine()
    {
        while (true)
        {
            _suctionTimer += Time.deltaTime;

            transform.position = Vector3.Lerp(transform.position, _playerStatusObject.Player.transform.position, Mathf.Clamp01(_suctionTimer * _playerValuesObject.TreatSuction));

            yield return null;
        }
    }
}
