using StockportWebapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockportWebapp.ProcessedModels
{
    public class ProcessedPrivacyNotice : IProcessedContentType
    {
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Directorate { get; set; }
        public string ActivitiesAsset { get; set; }
        public string TransactionsActivity { get; set; }
        public string Purpose { get; set; }
        public string TypeOfData { get; set; }
        public string Legislation { get; set; }
        public string Obtained { get; set; }
        public string ExternallyShared { get; set; }
        public string RetentionPeriod { get; set; }
        public string Conditions { get; set; }
        public string ConditionsSpecial { get; set; }
        public string UrlOne { get; set; }
        public string UrlTwo { get; set; }
        public string UrlThree { get; set; }

        public ProcessedPrivacyNotice() { }

        public ProcessedPrivacyNotice(string slug, string title, string directorate, string activitiesAsset, string transactionsActivity, string purpose, string typeOfData, string legislation, string obtained, string externallyShared, string retentionPeriod, string conditions, string conditionsSpecial, string urlOne, string urlTwo, string urlThree)
        {
            Slug = slug;
            Title = title;
            Directorate = directorate;
            ActivitiesAsset = activitiesAsset;
            TransactionsActivity = transactionsActivity;
            Purpose = purpose;
            TypeOfData = typeOfData;
            Legislation = legislation;
            Obtained = obtained;
            ExternallyShared = externallyShared;
            RetentionPeriod = retentionPeriod;
            Conditions = conditions;
            ConditionsSpecial = conditionsSpecial;
            UrlOne = urlOne;
            UrlTwo = urlTwo;
            UrlThree = urlThree;
        }
    }
}
