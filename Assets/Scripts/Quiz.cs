using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
  [Header("Admin Settings")]
  [SerializeField] bool randomQuestions = false;
  [SerializeField] bool checkAnswerLast = true;
  [SerializeField] bool autoGetNextQuestion = true;

  [Header("Questions")]
  [SerializeField] TextMeshProUGUI questionText;
  [SerializeField] List<QuestionObject> questions = new List<QuestionObject>();
  int nextQuestionIndex = 0;
  QuestionObject currentQuestion;

  [Header("Answers")]
  [SerializeField] GameObject[] answerButtons;
  int correctAnswerIndex;
  bool hasAnsweredEarly = true;

  [Header("Button Colors")]
  [SerializeField] Sprite defaultAnswerSprite;
  [SerializeField] Sprite correctAnswerSprite;

  int totalQuestionsCount = 0;
  Dictionary<int, int> allAnswers; //questionIndex, resultIndex
  int score = 0;

  void Start()
  {
    totalQuestionsCount = questions.Count;
    allAnswers = new Dictionary<int, int>();
    GetQuestion();
  }

  void Update()
  {
    //    timerImage.fillAmount = timer.fillFraction;
    //     if (timer.loadNextQuestion)
    //     {
    //         if (progressBar.value == progressBar.maxValue)
    //         {unityunity
    //             isComplete = true;
    //             return;
    //         }

    //         hasAnsweredEarly = false;
    //         GetQuestion();
    //         timer.loadNextQuestion = false;
    //     }
    //     else if (!hasAnsweredEarly && !timer.isAnsweringQuestion)
    //     {
    //         DisplayAnswer(-1);
    //         SetButtonState(false);
    //     }
  }

  void GetQuestion()
  {
    if (questions.Count > 0)
    {
      // SetButtonState(true);
      // SetDefaultButtonSprites();
      if (randomQuestions)
      {
        GetRandomQuestion();
      }
      else
      {
        GetNextQuestion();
      }

      DisplayQuestion();
      // progressBar.value++;
      // scoreKeeper.IncrementQuestionsSeen();
    }
  }

  private void GetNextQuestion()
  {
    currentQuestion = questions[nextQuestionIndex++];
  }

  private void GetPreviousQuestion()
  {
    nextQuestionIndex = nextQuestionIndex - 2;
    currentQuestion = questions[nextQuestionIndex];
  }

  private void GetRandomQuestion()
  {
    int index = Random.Range(0, questions.Count);
    currentQuestion = questions[index];
    if (questions.Contains(currentQuestion))
    {
      questions.Remove(currentQuestion);
    }
  }

  private void DisplayQuestion()
  {

    questionText.text = currentQuestion.GetQuestion();

    for (int i = 0; i < answerButtons.Length; i++)
    {
      TextMeshProUGUI toggleText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
      toggleText.text = FindSelectionTitleByIndex(i) + currentQuestion.GetAnswer(i);
    }
  }

  private string FindSelectionTitleByIndex(int index)
  {
    switch (index)
    {
      case 0:
        return "A-) ";
      case 1:
        return "B-) ";
      case 2:
        return "C-) ";
      case 3:
        return "D-) ";
      case 4:
        return "E-) ";
      default:
        return "";
    }
  }

  public void onAnswerSelected(int index)
  {

    allAnswers.Add(nextQuestionIndex - 1, index);

    if (!checkAnswerLast)
    {
      if (index == currentQuestion.GetCorrectAnswerIndex())
      {
        score = score + ((1 / totalQuestionsCount) * 100);
      }
    }

    if (autoGetNextQuestion)
    {
      GetQuestion();
    }

  }



}
