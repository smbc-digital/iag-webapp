namespace StockportWebapp.QuestionBuilder.Entities
{
    public class QuestionOption
    {
        public string Label { get; set; }
        public string Value { get; set; }
        public string TertiaryInformation { get; set; }
        public string SubLabel { get; set; }
        public string Image { get; set; }
        public bool IsSelected { get; set; }
    }
}