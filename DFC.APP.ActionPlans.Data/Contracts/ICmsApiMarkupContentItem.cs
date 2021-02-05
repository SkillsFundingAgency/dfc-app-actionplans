namespace DFC.APP.ActionPlans.Data.Contracts
{
    public interface ICmsApiMarkupContentItem
    {
        string? Title { get; set; }

        public string? Content { get; set; }

        public string? HtmlBody { get; set; }
    }
}
