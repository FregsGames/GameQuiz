using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
public class EndScreen : MonoBehaviour
{
    [SerializeField]
    private string completedIdText;
    [SerializeField]
    private string failedText;

    [SerializeField]
    private TextMeshProUGUI resultText;
    [SerializeField]
    private TextMeshProUGUI answersText;

    [SerializeField]
    private TextMeshProUGUI quoteText;
    [SerializeField]
    private TextMeshProUGUI quoteAuthor;

    [SerializeField]
    private QuotesList[] gameQuotes;

    public void Setup(bool completed, int answers, int total, bool newUnlocks)
    {
        resultText.text = Translations.instance.GetText(completed ? completedIdText : failedText);
        answersText.text = $"{answers}/{total}";

        QuotesList currentLanguageQuoteList = gameQuotes.FirstOrDefault(ql => ql.lang == Translations.instance.currentLanguage);

        Quote quote = new Quote();

        if (currentLanguageQuoteList != null)
        {
            quote = currentLanguageQuoteList.GetRandom();
        }
        else
        {
            QuotesList englishQuotes = gameQuotes.FirstOrDefault(ql => ql.lang == SystemLanguage.English);
            quote = englishQuotes.GetRandom();
        }

        quoteText.text = "\" " + quote.quote + "\"";
        quoteAuthor.text = quote.game;
    }

    public void LoadLobby()
    {
        SceneLoader.Instance.LoadLobby();
    }


    [Serializable]
    public class QuotesList
    {
        public SystemLanguage lang;
        public List<Quote> texts;

        public Quote GetRandom()
        {
            return texts[UnityEngine.Random.Range(0, texts.Count)];
        }

    }

    [Serializable]
    public struct Quote
    {
        public string quote;
        public string game;
    }
}
