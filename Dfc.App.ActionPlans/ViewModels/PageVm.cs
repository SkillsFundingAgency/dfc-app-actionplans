namespace Dfc.App.ActionPlans.ViewModels
{
    public abstract class PageVm
    {
        public class PageId
        {
            private PageId(string value)
            {
                Value = value;
            }

            public override string ToString()
            {
                return Value;
            }

            public string Value { get; }

            public static PageId Home { get; } = new PageId("HOME");
        }

        public PageId Id { get; }


        protected PageVm(PageId pageId)
        {
            Id = pageId;
        }
    }
}
