using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreatRandomizer : MonoBehaviour
{
    private Animator playerAnimator;
    private int anim;

    public bool bigTreat;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();

        anim = Mathf.FloorToInt(Random.value * 40);
        if (anim == 40)
        {
            anim = 0;
        }

        playerAnimator.SetInteger("Animation", anim);

        playerAnimator.SetBool("Big", bigTreat);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
