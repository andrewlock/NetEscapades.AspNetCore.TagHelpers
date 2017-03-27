using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace NetEscapades.AspNetCore.TagHelpers
{
    /// <summary>
    /// <see cref="ITagHelper"/> implementation targeting &lt;partial&gt; elements that renders
    /// a partial view. You can provide an optional 
    /// </summary>
    public class PartialTagHelper : TagHelper
    {
        private readonly IHtmlHelper _htmlHelper;

        public PartialTagHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// The name of the partial view used to create the HTML markup.
        /// </summary>
        [HtmlAttributeName("name")]
        public string Name { get; set; }

        /// <summary>
        /// A model to pass into the partial view.
        /// </summary>
        [HtmlAttributeName("model")]
        public object Model { get; set; }

        /// <summary>
        /// A <see cref="ViewDataDictionary"/> to pass into the partial view.</param>
        /// </summary>
        [HtmlAttributeName("view-data")]
        public ViewDataDictionary ViewData { get; set; }

        /// <summary>
        /// When true, renders the 
        /// </summary>
        [HtmlAttributeName("render")]
        public bool RenderDirectToStream { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(Name))
            {
                //no partial name provided
                return;
            }

            ((IViewContextAware)_htmlHelper).Contextualize(ViewContext);

            output.TagName = null;

            await (RenderDirectToStream
                    ? RenderPartialAsync()
                    : PartialAsync(output));

        }

        public async Task PartialAsync(TagHelperOutput output)
        {
            Task<IHtmlContent> contentTask;
            if (Model == null)
            {
                if (ViewData == null)
                {
                    contentTask = _htmlHelper.PartialAsync(Name);
                }
                else
                {
                    contentTask = _htmlHelper.PartialAsync(Name, ViewData);
                }
            }
            else
            {
                if (ViewData == null)
                {
                    contentTask = _htmlHelper.PartialAsync(Name, Model);
                }
                else
                {
                    contentTask = _htmlHelper.PartialAsync(Name, Model, ViewData);
                }
            }

            var content = await contentTask;

            output.Content.SetHtmlContent(content);
        }

        public Task RenderPartialAsync()
        {
            if (Model == null)
            {
                if (ViewData == null)
                {
                    return _htmlHelper.RenderPartialAsync(Name);
                }
                else
                {
                    return _htmlHelper.RenderPartialAsync(Name, ViewData);
                }
            }
            else
            {
                if (ViewData == null)
                {
                    return _htmlHelper.RenderPartialAsync(Name, Model);
                }
                else
                {
                    return _htmlHelper.RenderPartialAsync(Name, Model, ViewData);
                }
            }
        }
    }
}
