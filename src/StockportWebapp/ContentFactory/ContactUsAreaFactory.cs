namespace StockportWebapp.ContentFactory;

public class ContactUsAreaFactory(IContactUsCategoryFactory contactUsCategoryFactory)
{
    private readonly IContactUsCategoryFactory _contactUsCategoryFactory = contactUsCategoryFactory;

    public virtual ProcessedContactUsArea Build(ContactUsArea contactUsArea) =>
        new(
            contactUsArea.Title,
            contactUsArea.Slug,
            contactUsArea.Breadcrumbs,
            contactUsArea.PrimaryItems,
            contactUsArea.Alerts,
            contactUsArea.ContactUsCategories.Select(_contactUsCategoryFactory.Build),
            contactUsArea.InsetTextTitle,
            contactUsArea.InsetTextBody,
            contactUsArea.MetaDescription
        );
}