using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAnimation : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private GameObject panelTwo;
    [SerializeField]
    private float speed = 1f;

    private void Update()
    {
        panel.transform.position += Vector3.right * speed * Time.deltaTime;
        panelTwo.transform.position += Vector3.right * speed * Time.deltaTime;

        if(panel.transform.position.x > 1080)
        {
            panel.transform.position = new Vector3(-1080, 0, 0);
        }

        if (panelTwo.transform.position.x > 1080)
        {
            panelTwo.transform.position = new Vector3(-1080, 0, 0);
        }
    }
}
