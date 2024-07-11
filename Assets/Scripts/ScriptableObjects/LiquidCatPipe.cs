using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidCatPipe : PoolObject
{
    private Animator _animator;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        _animator.SetBool("IsIn", false);
    }

    public void PlayIn()
    {
        _animator.SetBool("IsIn", true);
    }

    public void PlayOut()
    {
        _animator.SetBool("IsIn", false);
    }
}
