using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NetEscapades.AspNetCore.TagHelpers
{
    /// <summary>
    /// <see cref="ITagHelper"/> implementation targeting any element that include include-if or 
    /// exclude-if elements.  
    /// </summary>
    public class IfTagHelper : TagHelper
    {
        /// <inheritdoc />
        public override int Order => -1000;

        /// <summary>
        /// A value indicating whether to render the content inside the element.
        /// If <see cref="Exclude"/> is also true, the content will not be rendered.
        /// </summary>
        [HtmlAttributeName(Constants.IncludeIfAttributeName)]
        public bool Include { get; set; } = true;

        /// <summary>
        /// A value indicating whether to render the content inside the element.
        /// </summary>
        [HtmlAttributeName(Constants.ExcludeIfAttributeName)]
        public bool Exclude { get; set; } = false;

        /// <inheritdoc />
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }
            if (output == null) { throw new ArgumentNullException(nameof(output)); }

            // Always strip the outer tag name as we never want <if> to render
            output.TagName = null;

            if (Include && !Exclude)
            {
                return;
            }

            output.SuppressOutput();
        }
    }
}
