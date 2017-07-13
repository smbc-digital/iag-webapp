using System;

namespace StockportWebapp.Attributes
{
    public class QuestionValidatorTypeAttribute : Attribute
    {
        public string Name { get; }

        public QuestionValidatorTypeAttribute(string name)
        {
            Name = name;
        }

    }
}