namespace KeepFitStore.WEB.TagHelpers
{
    using System;
    using System.Text.RegularExpressions;

    using Microsoft.AspNetCore.Razor.TagHelpers;

    [HtmlTargetElement("div", Attributes = "[enum-dropdown]")]
    public class FromEnumToDropdownTagHelper : TagHelper
    {
        public FromEnumToDropdownTagHelper()
        {

        }

        public Array Types { get; set; }

        public string Area { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var html = string.Empty;
            var pattern = "^([A-Z]{1}[a-z]+)([A-Z]{1}[a-z]*)$";
            Regex regex = new Regex(pattern);

            foreach (var item in this.Types)
            {
                var itemValue = item.ToString();
                var currentItemRegex = regex.Match(itemValue);

                if (currentItemRegex.Success)
                {
                    itemValue = currentItemRegex.Groups[1].Value + " " + currentItemRegex.Groups[2].Value; 
                }

                if (this.Area != null)
                {
                    html += $@"<a href=""/{this.Area}/{this.Controller}/{this.Action}?type={item.ToString()}""><i class=""far fa-arrow-alt-circle-right mr-3 mb-2""></i>{itemValue}</a>";
                }
                else
                {
                    html += $@"<a href=""/{this.Controller}/{this.Action}?type={item.ToString()}""><i class=""far fa-arrow-alt-circle-right mr-3 mb-2""></i>{itemValue}</a>";
                }
            }

            output.Content.SetHtmlContent(html); 
        }
    }
}