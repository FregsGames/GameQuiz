using System.Collections.Generic;

namespace Questions
{
    public class Question
    {
        public string Id { get; set; }
        public string Statement { get; set; }
        public string CorrectAnswer { get; set; }
        public List<string> WrongOptions { get; set; }
        public bool Handwritten { get; set; }

        public Question(string id, string statement, string correctAnswer, List<string> wrongOptions, bool handwritten = false)
        {
            Id = id;
            Statement = statement;
            CorrectAnswer = correctAnswer;
            WrongOptions = wrongOptions;
            Handwritten = handwritten;
        }

    }
}
