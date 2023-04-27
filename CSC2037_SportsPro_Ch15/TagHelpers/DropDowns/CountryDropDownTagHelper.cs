using CSC2037_SportsPro_Ch15.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CSC2037_SportsPro_Ch15.TagHelpers.DropDowns
{
    [HtmlTargetElement(Attributes = "my-countries")]
    public class CountryDropDownTagHelper : BaseDropDownTagHelper
    {
        private IRepository<Country> data { get; set; }
        public CountryDropDownTagHelper(IRepository<Country> rep) => data = rep;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string selectedValue = base.GetSelectedValue(context);

            // get list from database
            var countries = data.List(new QueryOptions<Country> { OrderBy = c => c.Name });

            // add default option
            base.AddOption(output, "Select a country...", "");

            // add rest of options 
            foreach (var country in countries)
            {
                base.AddOption(output, country.Name, country.CountryID, selectedValue);
            }

        }

    }
}
