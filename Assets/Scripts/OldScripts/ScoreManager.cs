using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static int score;

    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();

        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (score < 0)
        {
            score = 0;
        }

        text.text = score.ToString();
    }

    public static void Add(int points)
    {
        score += points;
    }

    public static void Subtract(int points)
    {
        score -= points;
    }

    public static void ResetScore()
    {
        score = 0;
    }
}
