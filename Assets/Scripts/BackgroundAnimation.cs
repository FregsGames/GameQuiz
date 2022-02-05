using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAnimation : MonoBehaviour
{
    [SerializeField]
    private RectTransform panel;
    [SerializeField]
    private RectTransform panelTwo;
    [SerializeField]
    private float speed = 1f;

    private void Update()
    {
        panel.anchoredPosition += Vector2.right * speed * Time.deltaTime;
        panelTwo.anchoredPosition += Vector2.right * speed * Time.deltaTime;

        if(panel.anchoredPosition.x > 1080)
        {
            panel.anchoredPosition = new Vector3(-1080, 0, 0);
        }

        if (panelTwo.anchoredPosition.x > 1080)
        {
            panelTwo.anchoredPosition = new Vector3(-1080, 0, 0);
        }
    }
}
