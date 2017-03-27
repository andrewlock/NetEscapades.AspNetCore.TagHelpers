using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NetEscapades.AspNetCore.TagHelpers
{
    /// <summary>
    /// <see cref="ITagHelper"/> implementation targeting any element that include include-if or 
    /// exclude-if elements.  
    /// </summary>
    [HtmlTargetElement(Attributes = Constants.IncludeIfAttributeName)]
    [HtmlTargetElement(Attributes = Constants.ExcludeIfAttributeName)]
    public class IfAttributeTagHelper : TagHelper
    {
        /// <inheritdoc />
        public override int Order => -1000;

        /// <summary>
        /// A value indicating whether to render the content inside the element.
        /// If <see cref="Exclude"/> is also true, the content will not be rendered.
        /// </summary>
        [HtmlAttributeName(Constants.IncludeIfAttributeName)]
        public bool? Include { get; set; }

        /// <summary>
        /// A value indicating whether to render the content inside the element.
        /// If <see cref="Exclude"/> is also true, the content will not be rendered.
        /// </summary>
        [HtmlAttributeName(Constants.ExcludeIfAttributeName)]
        public bool Exclude { get; set; } = false;

        /// <inheritdoc />
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            output.Attributes.RemoveAll(Constants.IncludeIfAttributeName);
            output.Attributes.RemoveAll(Constants.ExcludeIfAttributeName);

            if (DontRender)
            {
                //TODO: make this an option?
                output.TagName = null;
                output.SuppressOutput();
            }
        }

        private bool DontRender => Exclude || (Include.HasValue && !Include.Value);
    }
}
