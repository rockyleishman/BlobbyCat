using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRandomizer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Sprite sprite0;
    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;
    public Sprite sprite4;
    public Sprite sprite5;
    public Sprite sprite6;
    public Sprite sprite7;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        int rand = Mathf.FloorToInt(Random.value * 8);
        if (rand == 8)
        {
            rand = 0;
        }

        if (rand == 0)
        {
            spriteRenderer.sprite = sprite0;
        }
        else if (rand == 1)
        {
            spriteRenderer.sprite = sprite1;
        }
        else if(rand == 2)
        {
            spriteRenderer.sprite = sprite2;
        }
        else if (rand == 3)
        {
            spriteRenderer.sprite = sprite3;
        }
        else if (rand == 4)
        {
            spriteRenderer.sprite = sprite4;
        }
        else if (rand == 5)
        {
            spriteRenderer.sprite = sprite5;
        }
        else if (rand == 6)
        {
            spriteRenderer.sprite = sprite6;
        }
        else if (rand == 7)
        {
            spriteRenderer.sprite = sprite7;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
