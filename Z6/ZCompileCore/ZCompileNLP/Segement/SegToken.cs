namespace ZCompileNLP.Segment
{
    public class SegToken
    {
        public string Word { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        //public int Line { get; set; }

        public SegToken(string word, int startIndex, int endIndex)
        {
            Word = word;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }

        public override string ToString()
        {
            return string.Format("[{0}, ({1}, {2})]", Word, StartIndex, EndIndex);
        }
    }
}