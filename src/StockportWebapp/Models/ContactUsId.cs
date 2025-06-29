﻿namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class ContactUsId(string name, string slug, string emailAddress, string successPageButtonText)
{
    public string Name { get; } = name;
    public string Slug { get; } = slug;
    public string EmailAddress { get; } = emailAddress;
    public string SuccessPageButtonText { get; } = successPageButtonText;
}