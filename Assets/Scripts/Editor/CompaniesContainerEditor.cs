using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CompaniesContainer))]
public class CompaniesContainerEditor : Editor
{
    CompaniesContainer companiesContainer;
    
    private void OnEnable()
    {
        companiesContainer = (CompaniesContainer)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        foreach (var company in companiesContainer.allCompanies)
        {
            GUILayout.Label($"{company.name}", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 15 }, GUILayout.ExpandWidth(true));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
