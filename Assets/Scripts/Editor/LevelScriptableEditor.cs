using Questions;
using UnityEditor;
using UnityEngine;

public class LevelScriptableEditor : Editor
{
    SerializedProperty id;
    SerializedProperty levelTitle;
    SerializedProperty gamesContainer;
    SerializedProperty handwrittenQuestionSet;
    SerializedProperty questionTemplates;
    SerializedProperty state;
    SerializedProperty alwaysUnlocked;
    SerializedProperty winCondition;

    private LevelScriptable level;

    private void OnEnable()
    {
        //cupLevels = (serializedObject.targetObject as CupScriptable).levels;

        id = serializedObject.FindProperty("id");
        levelTitle = serializedObject.FindProperty("levelTitle");
        gamesContainer = serializedObject.FindProperty("gamesContainer");
        handwrittenQuestionSet = serializedObject.FindProperty("handwrittenQuestionSet");
        questionTemplates = serializedObject.FindProperty("questionTemplates");
        state = serializedObject.FindProperty("state");
        alwaysUnlocked = serializedObject.FindProperty("alwaysUnlocked");
        winCondition = serializedObject.FindProperty("winCondition");

        level = ((LevelScriptable)target);
    }

    public override void OnInspectorGUI()
    {
       /* //base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.PropertyField(id);
        EditorGUILayout.PropertyField(levelTitle);
        EditorGUILayout.PropertyField(gamesContainer);
        EditorGUILayout.PropertyField(handwrittenQuestionSet);

        EditorGUILayout.PropertyField(state);
        EditorGUILayout.PropertyField(alwaysUnlocked);
        EditorGUILayout.PropertyField(winCondition);
        EditorGUILayout.Separator();

        GUILayout.Label($"Questions:", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));

        for (int i = 0; i < questionTemplates.arraySize; i++)
        {
            //EditorGUILayout.PropertyField(questionTemplates.GetArrayElementAtIndex(i));

            QuestionTemplate current = level.questionTemplates[i];

            switch (current.ContentType)
            {
                case QuestionTemplate.QuestionContent.fromYear:

                    foreach (var year in current.years)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(year.Key + "");
                        GUILayout.Label(year.Value? "On" : "Off");
                        if (GUILayout.Button("Remove"))
                        {
                            current.years[year.Key] = !current.years[year.Key];
                            break;
                        }
                        GUILayout.EndHorizontal();
                    }

                    if (GUILayout.Button("Add All Years"))
                    {
                        EditorUtility.SetDirty(serializedObject.targetObject);
                        
                        if (level.gamesContainer != null)
                        {
                            foreach (var year in level.gamesContainer.Years().Keys)
                            {
                                current.years.Add(year, true);
                            }
                        }
                        serializedObject.ApplyModifiedProperties();
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        //Repaint();
                    }
                    break;
                case QuestionTemplate.QuestionContent.fromPlatform:
                    break;
                case QuestionTemplate.QuestionContent.fromCompany:
                    break;
                case QuestionTemplate.QuestionContent.handwriten:
                    break;
                case QuestionTemplate.QuestionContent.notFromYear:
                    break;
                case QuestionTemplate.QuestionContent.notFromPlatform:
                    break;
                case QuestionTemplate.QuestionContent.notFromCompany:
                    break;
                default:
                    break;
            }
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            if (GUILayout.Button("Remove"))
            {
                ((LevelScriptable)target).RemoveQuestion(i);
                Repaint();
            }
        }
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();


        if (GUILayout.Button("Add Question"))
        {
            ((LevelScriptable)target).AddQuestion();
            Repaint();
        }

        serializedObject.ApplyModifiedProperties();*/
    }
}
