using Questions;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;
public class GeneratorTest : MonoBehaviour
{
    public LevelScriptableC[] levelsToTest;
    public int questions = 1000;
    public QuestionGenerator questionGenerator;

    private async void Start()
    {
        await Task.Delay(1000);

        foreach (var levelToTest in levelsToTest)
        {
            Debug.Log("Generating " + questions + " questions for level " + levelToTest.id);
            for (int i = 0; i < questions; i++)
            {
                List<QuestionTemplate> excludedHandwritten = levelToTest.questionTemplates.
                    Where(qt => qt.ContentType != QuestionTemplate.QuestionContent.handwriten).ToList();

                Questions.QuestionTemplate questionTemplate = excludedHandwritten[Random.Range(0, excludedHandwritten.Count)];
                Question question = questionGenerator.FromTemplate(questionTemplate, levelToTest.filters, null);

                if (question == null)
                {
                    Debug.LogWarning($"Question null at {i}, template: {questionTemplate.ContentType}");
                    break;
                }
                else if (question.Statement == null)
                {
                    Debug.LogWarning($"Statement null at {i}");
                    break;
                }
                else if (question.CorrectAnswer == null)
                {
                    Debug.LogWarning($"Correct answer null at {i}");
                    break;
                }
                //Debug.Log(i + " : " + question.Statement + " " + question.CorrectAnswer);
            }

            Debug.Log("Fnished testing " + levelToTest.id);
        }



        Debug.Log("Fnished testing");

    }
}
