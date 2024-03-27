using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI resultText;
    ScoreKepper scoreKepper;

    // Start is called before the first frame update
    void Awake()
    {
        scoreKepper = FindObjectOfType<ScoreKepper>();
    }

    public void ShowResultScore()
    {
        resultText.text = $"Congratulation!\n You Scored {scoreKepper.CalculateScore()}%";
    }
}
