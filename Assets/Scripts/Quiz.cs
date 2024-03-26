using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] QuestionSO question;
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    [SerializeField] Sprite defaultButtonSprite;
    [SerializeField] Sprite correctButtonSprite;

    void Start()
    {
        NextQuestion();
        //DisplayQuestion();
    }

    void NextQuestion()
    {
        SetButtonState(true);
        SetDefaultButtonSprite();
        DisplayQuestion();
    }

    void SetDefaultButtonSprite()
    {
        Image buttonImage;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultButtonSprite;
        }
    }

    void DisplayQuestion()
    {
        questionText.text = question.GetQuestion();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = question.GetAnswer(i);
        }
    }

    void SetButtonState(bool state)
    {
        Button button;

        for(int i = 0; i < answerButtons.Length; i++)
        {
            button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }

    public void OnAnswerSelected(int index)
    {
        Image buttonImage;

        if (index == question.GetCorrectAnswerIndex())
        {
            questionText.text = "Correct!";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctButtonSprite;
        }
        else
        {
            int answerNum = question.GetCorrectAnswerIndex();
            string correctAnswer = question.GetAnswer(answerNum);
            questionText.text = $"Wrong! The Answer is \n{correctAnswer}";

            buttonImage = answerButtons[answerNum].GetComponent<Image>();
            buttonImage.sprite = correctButtonSprite;
        }

        SetButtonState(false);
    }
}
