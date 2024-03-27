using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO currentQuestion;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;

    [Header("Buttons")]
    [SerializeField] Sprite defaultButtonSprite;
    [SerializeField] Sprite correctButtonSprite;
    bool hasAnsweredEarly = true;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKepper scoreKepper;

    [Header("ProgressBar")]
    [SerializeField] Slider progressBar;

    public bool isComplete = false;

    void Awake()
    {
        scoreKepper = FindObjectOfType<ScoreKepper>();
        timer = FindObjectOfType<Timer>();
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    void Update()
    {
        timerImage.fillAmount = timer.fillFraction;
        if (timer.loadNextQuestion)
        {
            if (progressBar.value == progressBar.maxValue)
            {
                isComplete = true;
                return;
            }

            hasAnsweredEarly = false;
            NextQuestion();
            timer.loadNextQuestion = false;
        }
        else if (!hasAnsweredEarly && !timer.isAnsweringQuestion)
        {
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }

    void NextQuestion()
    {
        if(questions.Count > 0)
        {
            SetButtonState(true);
            GetRandomQuestion();
            SetDefaultButtonSprite();
            DisplayQuestion();
            scoreKepper.IncrementQuestionsSeen();
            progressBar.value++;
        }
    }

    void GetRandomQuestion()
    {
        int index = UnityEngine.Random.Range(0, questions.Count);
        currentQuestion = questions[index];

        if (questions.Contains(currentQuestion))
        {
            questions.Remove(currentQuestion);
        }
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
        questionText.text = currentQuestion.GetQuestion();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestion.GetAnswer(i);
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
        hasAnsweredEarly = true;
        DisplayAnswer(index);

        timer.CancelTimer();
        SetButtonState(false);
        scoreText.text = $"Score : {scoreKepper.CalculateScore()}%";

        
    }

    void DisplayAnswer(int index)
    {
        Image buttonImage;

        if (index == currentQuestion.GetCorrectAnswerIndex())
        {
            questionText.text = "Correct!";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctButtonSprite;
            scoreKepper.IncrementCorrectAnswer();
        }
        else
        {
            int answerNum = currentQuestion.GetCorrectAnswerIndex();
            string correctAnswer = currentQuestion.GetAnswer(answerNum);
            questionText.text = $"Wrong! The Answer is \n{correctAnswer}";

            buttonImage = answerButtons[answerNum].GetComponent<Image>();
            buttonImage.sprite = correctButtonSprite;
        }
    }
}
