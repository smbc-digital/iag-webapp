using System.Linq;

namespace StockportWebapp.ContentFactory;

public class ContactUsAreaFactory(IContactUsCategoryFactory contactUsCategoryFactory)
{
    private readonly IContactUsCategoryFactory _contactUsCategoryFactory = contactUsCategoryFactory;

    public virtual ProcessedContactUsArea Build(ContactUsArea contactUsArea) =>
        new ProcessedContactUsArea(
            contactUsArea.Title,
            contactUsArea.Slug,
            contactUsArea.Breadcrumbs,
            contactUsArea.PrimaryItems,
            contactUsArea.Alerts,
            new List<ProcessedContactUsCategory>(contactUsArea.ContactUsCategories.Select(_contactUsCategoryFactory.Build)),
            contactUsArea.InsetTextTitle,
            contactUsArea.InsetTextBody,
            contactUsArea.MetaDescription
        );
}