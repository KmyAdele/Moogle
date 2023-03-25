namespace MoogleEngine
{
    public class Documents
    {
        public int id;
        public string title;
        public string text;
        public string [] text_words;
        public Dictionary<string,double> term_frequency;
        public double score;

    }
}