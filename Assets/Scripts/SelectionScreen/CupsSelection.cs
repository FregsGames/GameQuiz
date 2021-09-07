using SuperMaxim.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupsSelection : MonoBehaviour
{
    [SerializeField]
    private Transform content;
    [SerializeField]
    private CupDropdown cupDropdownPrefab;
    private List<CupScriptable> cups;

    private void Start()
    {
        cups = Cups.Instance.GetAllCups();

        foreach (var cup in cups)
        {
            var dropdown = Instantiate(cupDropdownPrefab, content);
            dropdown.Setup(cup);
        }

        Messenger.Default.Subscribe<CupScriptable>(OnCupSelected);
    }

    private void OnCupSelected(CupScriptable obj)
    {
        if (obj == null)
        {
            content.gameObject.SetActive(true);
        }
        else
        {
            content.gameObject.SetActive(false);
        }
    }

    public void Back()
    {

    }

}
