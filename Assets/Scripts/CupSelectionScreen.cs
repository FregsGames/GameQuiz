using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupSelectionScreen : MonoBehaviour
{
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
    private List<Cup> cups;
    private void OnEnable()
    {
        currentSection = cupSection;
        alternativeSection = cupSectionAlt;

        currentSection.Setup(cups[cupIndex]);

        UpdateArrows();
    }

    private void UpdateArrows()
    {
        leftButton.SetActive(cupIndex != 0);
        rightButton.SetActive(cupIndex < cups.Count - 1);
    }

    public void ArrowButton(int direction) // 1 right -1 left
    {
        EnableButtons(false);
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
    }
}
