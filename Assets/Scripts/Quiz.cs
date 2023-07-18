using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionObject> questions = new List<QuestionObject>();
    QuestionObject currentQuestion;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    bool hasAnsweredEarly = true;

    [Header("Button Colors")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    void Start(){
        //questionText.text = "test";
        GetNextQuestion();
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
    //         GetNextQuestion();
    //         timer.loadNextQuestion = false;
    //     }
    //     else if (!hasAnsweredEarly && !timer.isAnsweringQuestion)
    //     {
    //         DisplayAnswer(-1);
    //         SetButtonState(false);
    //     }
    }

    void GetNextQuestion()
    {
        if (questions.Count > 0)
        {
            // SetButtonState(true);
            // SetDefaultButtonSprites();
            GetRandomQuestion();
            DisplayQuestion();
            // progressBar.value++;
            // scoreKeeper.IncrementQuestionsSeen();
        }
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

    private void DisplayQuestion(){

        questionText.text = currentQuestion.GetQuestion();
    
        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI toggleText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();

            //TODO DENEME Start 
            // TextMeshProUGUI myButton = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            // TextMeshProUGUI myText = myButton.GetComponentInChildren<TextMeshProUGUI>();
            // Debug.Log("item 1:"+test);
            // Debug.Log("item 2:"+test.text);
            // myText.text = FindSelectionTitleByIndex(i) + currentQuestion.GetAnswer(i);
            //TODO DENEME End

            toggleText.text = FindSelectionTitleByIndex(i) + currentQuestion.GetAnswer(i);
        }
    }
    
    private string FindSelectionTitleByIndex(int index){
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
            default :
                return "";
        }
    }

}
