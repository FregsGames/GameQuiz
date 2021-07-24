using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questions
{
    public class Question
    {
        public string Statement { get; set; }
        public string CorrectAnswer { get; set; }
        public List<string> WrongOptions { get; set; }

        public Question(string statement, string correctAnswer, List<string> wrongOptions)
        {
            Statement = statement;
            CorrectAnswer = correctAnswer;
            WrongOptions = wrongOptions;
        }

    }
}
