using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using StockportWebapp.Validation;

namespace StockportWebapp.Models
{
    public class PaymentSubmission
    {
        public ProcessedPayment Payment { get; set; } = new ProcessedPayment();
        [Required]
        public string Reference { get; set; } = "";
        [Required]
        public decimal Amount { get; set; } = 0;
    }
}
