using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CSC2037_SportsPro_Ch15.TagHelpers.DropDowns
{
    public abstract class BaseDropDownTagHelper : TagHelper
    {
        // no Process() method - just helper methods for tag helpers that inherit this class

        protected string GetSelectedValue(TagHelperContext context)
        {
            var aspfor = (ModelExpression)context.AllAttributes["asp-for"].Value;
            return aspfor.Model?.ToString() ?? "";
        }

        protected void AddOption(TagHelperOutput output, string text, string value, string selectedValue = "")
        {
            TagBuilder option = new TagBuilder("option");
            option.InnerHtml.Append(text);
            option.Attributes["value"] = value;

            if (value == selectedValue)
                option.Attributes["selected"] = "selected";

            output.Content.AppendHtml(option);
        }

    }
}
