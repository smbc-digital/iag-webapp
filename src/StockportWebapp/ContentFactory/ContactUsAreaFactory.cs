namespace StockportWebapp.ContentFactory;

public class ContactUsAreaFactory
{
    private readonly IContactUsCategoryFactory _contactUsCategoryFactory;

    public ContactUsAreaFactory(IContactUsCategoryFactory contactUsCategoryFactory) =>
        _contactUsCategoryFactory = contactUsCategoryFactory;

    public virtual ProcessedContactUsArea Build(ContactUsArea contactUsArea)
    {
        List<ProcessedContactUsCategory> processedContactUsCategories = new();
        foreach (var contactUsCategory in contactUsArea.ContactUsCategories)
        {
            processedContactUsCategories.Add(_contactUsCategoryFactory.Build(contactUsCategory));
        }

        return new ProcessedContactUsArea(
            contactUsArea.Title,
            contactUsArea.Slug,
            contactUsArea.CategoriesTitle,
            contactUsArea.Breadcrumbs,
            contactUsArea.PrimaryItems,
            contactUsArea.Alerts,
            processedContactUsCategories,
            contactUsArea.InsetTextTitle,
            contactUsArea.InsetTextBody,
            contactUsArea.MetaDescription
        );
    }
}
