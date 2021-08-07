namespace Assets.Scripts.Payloads
{
    public class AnswerGiven
    {
        public bool Correct { get; set; }
        public AnswerGiven(bool correct)
        {
            Correct = correct;
        }
    }
}
