using DG.Tweening;
using SuperMaxim.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cups;

public class CupSelectionScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject selectionContainer;

    [SerializeField]
    private MenuManager menuManager;

    [SerializeField]
    private GameObject leftButton;
    [SerializeField]
    private GameObject rightButton;

    private int cupIndex = 0;

    [SerializeField]
    private CupSection cupSection;
    [SerializeField]
    private CupSection cupSectionAlt;

    private CupSection currentSection;
    private CupSection alternativeSection;

    [SerializeField]
    private LevelSelectionScreen levelSelectionScreen;

    private List<CupScriptable> cups;

    private void OnEnable()
    {
        selectionContainer.SetActive(true);

        currentSection = cupSection;
        alternativeSection = cupSectionAlt;
    }

    private void Start()
    {
        cups = Cups.Instance.GetAllCups();

        currentSection.Setup(cups[cupIndex]);

        UpdateArrows();

        EnableButtons(true);
    }

    private void UpdateArrows()
    {
        leftButton.SetActive(cupIndex != 0);
        rightButton.SetActive(cupIndex < cups.Count - 1);
    }

    public void ArrowButton(int direction) // 1 right -1 left
    {
        EnableButtons(false);
        alternativeSection.gameObject.SetActive(true);
        cupIndex += direction;

        var temp = currentSection;
        currentSection = alternativeSection;
        alternativeSection = temp;

        currentSection.Setup(cups[cupIndex]);

        currentSection.transform.DOMoveX(direction * Screen.width, 0);

        currentSection.transform.DOMoveX(0, 0.5f);
        alternativeSection.transform.DOMoveX(direction  * -1 * Screen.width, 0.5f).OnComplete(() => EnableButtons(true));

        UpdateArrows();
    }

    private void EnableButtons(bool state)
    {
        currentSection.EnableButton(state);
        alternativeSection.EnableButton(state);
        alternativeSection.gameObject.SetActive(false);
    }

    public void SelectCup()
    {
        selectionContainer.SetActive(false);
        levelSelectionScreen.Setup(cups[cupIndex]);
    }

    public void ActivateCupSection()
    {
        selectionContainer.SetActive(true);
    }

    public void ReturnButton()
    {
        if (levelSelectionScreen.IsActive)
        {
            levelSelectionScreen.DeactivateSection();
            ActivateCupSection();
        }
        else
        {
            StartCoroutine(menuManager.Load());
        }
    }
}
