using System;
using UnityEngine;

namespace Questions
{
    [Serializable]
    public class QuestionTemplate
    {
        public enum QuestionStructure
        {
            normal = 1
        }

        public enum QuestionContent
        {
            fromYear = 1,
            fromPlatform = 2,
            fromCompany = 3,
            handwriten = 4,
            notFromYear = 5,
            notFromPlatform = 6,
            notFromCompany = 7,
        }

        public CupScriptable cup { get; set; }

        public QuestionContent ContentType => contentType;
        public string ExtraData => extraData;

        [SerializeField]
        private QuestionContent contentType;

        private string extraData;

    }
}