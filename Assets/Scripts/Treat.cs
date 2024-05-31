using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treat : Collectable
{
    private const string SmallBrownAnimation = "SmallTreat_Brown_Floating";
    private const string SmallRedAnimation = "SmallTreat_Red_Floating";
    private const string SmallGreenAnimation = "SmallTreat_Green_Floating";
    private const string SmallBlueAnimation = "SmallTreat_Blue_Floating";
    private const string BrownAnimation = "Treat_Brown_Floating";
    private const string RedAnimation = "Treat_Red_Floating";
    private const string GreenAnimation = "Treat_Green_Floating";
    private const string BlueAnimation = "Treat_Blue_Floating";

    protected override void Start()
    {
        base.Start();

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
    }

    protected override void Collect()
    {
        //effects
        PoolManager.Instance.Spawn(CollectEffect.name, transform.position, transform.rotation);

        //gain sub hit point
        DataManager.Instance.PlayerStatusObject.Player.GetComponent<PlayerHitPointController>().GainSubHitPoint();

        //update HUD
        HUDManager.Instance.UpdateTreatCount(!PreviouslyCollected);

        //TODO: collect via manager
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
