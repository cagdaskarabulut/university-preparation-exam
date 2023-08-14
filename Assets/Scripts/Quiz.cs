using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Timers;

public class Quiz : MonoBehaviour
{
  //! Settings 
  [Header("Mode Selection")]
  //- Mode
  [SerializeField] bool isManuelSelection = false;
  [SerializeField] bool isShortTermQuizAutoSelection = false;
  [SerializeField] bool isLongTermQuizAutoSelection = true;

  [Header("Manuel Selection Details")]
  //- Next and Previous Buttons (Changing from Interface or Quiz Mode)
  [SerializeField] bool isShowNextAndPreviousButtons = true;
  //- Timer For Question (Changing from Interface or Quiz Mode)
  [SerializeField] bool isEnabledTimerForQuestion = false;
  [SerializeField] Image timerImageForQuestion;
  private Timer timerForQuestion;
  //- Timer For Quiz (Changing from Interface or Quiz Mode)
  [SerializeField] bool isEnabledTimerForQuiz = true;
  [SerializeField] Slider timerSliderForQuiz;
  private Timer timerForQuiz;
  //- Page Components
  private RectTransform rectForQuestion;
  private GameObject[] routingButtons;

  [Header("Admin Settings")]
  //- Admin Settings 
  [SerializeField] bool isRandomQuestions = false; //_ Changing from Interface
  [SerializeField] bool isCheckAnswersAtFinal = true; //_Changing from Interface or Quiz Mode
  [SerializeField] bool isGetNextQuestionAutomatically = true; //_Changing from Interface or Quiz Mode

  //! Values 
  [SerializeField] int totalScore = 0;

  [Header("Questions")]
  //- Questions
  [SerializeField] TextMeshProUGUI questionText;
  [SerializeField] List<QuestionObject> questions = new List<QuestionObject>();
  private int nextQuestionIndex = 0;
  private QuestionObject currentQuestion;
  private int totalQuestionsCount = 0;

  [Header("Answers")]
  //- Answers
  [SerializeField] GameObject[] answerButtons;
  [SerializeField] Sprite defaultAnswerButtonSprite;
  [SerializeField] Sprite correctAnswerButtonSprite;
  private int correctAnswerIndex;
  private Dictionary<int, int> allAnswers; //_ questionIndex, resultIndex

  void SetupComponentsBeforeStart()
  {
    timerImageForQuestion = GameObject.Find("RemainingTimeForQuestion").GetComponent<Image>();
    timerSliderForQuiz = GameObject.Find("RemainingTimeForQuizSlider").GetComponent<Slider>();
    routingButtons = GameObject.FindGameObjectsWithTag("RoutingButtons");
    rectForQuestion = GameObject.Find("QuestionScrollView").GetComponent<RectTransform>();

    if (isManuelSelection || (!isShortTermQuizAutoSelection && !isLongTermQuizAutoSelection))
    {
      timerImageForQuestion.enabled = isEnabledTimerForQuestion;
      timerSliderForQuiz.gameObject.SetActive(isEnabledTimerForQuiz);
    }
    else if (isShortTermQuizAutoSelection)
    {
      isEnabledTimerForQuestion = true;
      timerImageForQuestion.enabled = isEnabledTimerForQuestion;

      isEnabledTimerForQuiz = false;
      timerSliderForQuiz.gameObject.SetActive(isEnabledTimerForQuiz);

      isShowNextAndPreviousButtons = false;
      foreach (GameObject routingButton in routingButtons)
      {
        routingButton.gameObject.SetActive(false);
      }

      isCheckAnswersAtFinal = false;

      isGetNextQuestionAutomatically = true;

    }
    else if (isLongTermQuizAutoSelection)
    {
      isEnabledTimerForQuestion = false;
      timerImageForQuestion.enabled = isEnabledTimerForQuestion;

      isEnabledTimerForQuiz = true;
      timerSliderForQuiz.gameObject.SetActive(isEnabledTimerForQuiz);

      isShowNextAndPreviousButtons = true;
      foreach (GameObject routingButton in routingButtons)
      {
        routingButton.gameObject.SetActive(true);
      }

      isCheckAnswersAtFinal = true;

      isGetNextQuestionAutomatically = false;
    }

    PrepareRoutingButtons(isShowNextAndPreviousButtons);

  }

  void Start()
  {
    SetupComponentsBeforeStart();
    //timerForQuestion = FindObjectOfType<Timer>();
    //timerImageForQuestion.fillAmount = timerForQuestion.fillFraction;
    //timerForQuiz = FindObjectOfType<Timer>();
    //timerSliderForQuiz.fillAmount = timerForQuiz.fillFraction;

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

  void PrepareRoutingButtons(bool isShowNextAndPreviousButtons)
  {
    if (isShowNextAndPreviousButtons)
    {
      foreach (GameObject routingButton in routingButtons)
      {
        routingButton.gameObject.SetActive(true);
      }
    }
    else
    {
      foreach (GameObject routingButton in routingButtons)
      {
        routingButton.gameObject.SetActive(false);
      }
    }

    if (!isShowNextAndPreviousButtons)
    {
      rectForQuestion.offsetMax = new Vector2(rectForQuestion.offsetMax.x, -350);
    }
  }

  void GetQuestion()
  {
    if (questions.Count > 0)
    {
      //timerImageForQuestion.fillAmount = timer.fillFraction;
      // SetButtonState(true);
      // SetDefaultButtonSprites();
      if (isRandomQuestions)
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

      //TODO Seçim yaptığında question sayacı ve resmi sıfırla
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
    //TODO Seçim yaptığında doğru şıkkı işaretle 
    

    //TODO 1- Seçim yaptığında hızlı moddaysa seçimden birkaç saniye sonraya kadar ses efekti çıkartıp 
    //TODO 2- sonunda doğru şıkkı farklı renkle işaretle , eğer doğru şıkkı seçmişse şıkkı yeşil yap 
    //TODO 3- değilse kırmızı yap ve doğru şıkkı yeşil yap

    //- Mevcut sorunun cevabını listeye ekle
    allAnswers.Add(nextQuestionIndex - 1, index);

    //- Seçim yaptığında anlık puan hesaplanması seçiliyse, puan hesaplamasını yap 
    if (!isCheckAnswersAtFinal)
    {
      if (index == currentQuestion.GetCorrectAnswerIndex())
      {
        totalScore += ((1 / totalQuestionsCount) * 100);
      }
    }

    //- Seçim yaptığında otomatik sıradaki soruyu getir ayarı seçiliyse otomatik yeni soruyu getir
    if (isGetNextQuestionAutomatically)
    {
      GetQuestion();
    }

  }
}

