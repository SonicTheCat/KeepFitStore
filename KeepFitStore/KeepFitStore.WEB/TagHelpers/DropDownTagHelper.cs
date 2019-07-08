namespace KeepFitStore.WEB.TagHelpers
{
    using System;

    using Microsoft.AspNetCore.Razor.TagHelpers;

    [HtmlTargetElement("div", Attributes = "[keep-fit-enums]")]
    public class DropDownTagHelper : TagHelper
    {
        public DropDownTagHelper()
        {

        }

        public Array Types { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var content = string.Empty; 
            foreach (var item in this.Types)
            {
                content += $@"<a href=""/Products/Proteins/Index?type={item.ToString()}"">{item.ToString()}</a>"; 
            }

            output.Content.SetHtmlContent(content); 
        }
    }
}