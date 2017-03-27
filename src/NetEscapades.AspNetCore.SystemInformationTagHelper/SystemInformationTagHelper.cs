using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace NetEscapades.AspNetCore.TagHelpers
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemInfoTagHelper : TagHelper
    {
        private readonly IHtmlHelper _htmlHelper;
        private readonly IHostingEnvironment _hostingEnvironment;
        public SystemInfoTagHelper(IHtmlHelper htmlHelper, IHostingEnvironment hostingEnvironment)
        {
            _htmlHelper = htmlHelper;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Show the current <see cref="Environment.MachineName"/>. true by default
        /// </summary>
        [HtmlAttributeName("machine")]
        public bool IncludeMachine { get; set; } = true;

        /// <summary>
        /// Show the current OS
        /// </summary>
        [HtmlAttributeName("os")]
        public bool IncludeOs { get; set; } = true;

        /// <summary>
        /// Show the current OS architecture type
        /// </summary>
        [HtmlAttributeName("os-type")]
        public bool IncludeOsArchitecture { get; set; } = true;

        /// <summary>
        /// Show the current <see cref="IHostingEnvironment.EnvironmentName"/>
        /// </summary>
        [HtmlAttributeName("environment")]
        public bool IncludeEnvironment { get; set; } = true;

        /// <summary>
        /// Show the application's name
        /// </summary>
        [HtmlAttributeName("app-name")]
        public bool IncludeApplicationName { get; set; } = true;

        /// <summary>
        /// Show the application's version
        /// </summary>
        [HtmlAttributeName("app-version")]
        public bool IncludeApplicationVersion { get; set; } = true;

        /// <summary>
        /// Show the application's runtime framework
        /// </summary>
        [HtmlAttributeName("app-runtime")]
        public bool IncludeApplicationRuntime { get; set; } = true;

        /// <summary>
        /// Whether the output should be visible. If not, the output will be wrapped in an html comment 
        /// </summary>
        [HtmlAttributeName("visible")]
        public bool IsVisible { get; set; } = true;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "dl";                       // Replaces <system-info> with <dl>
            output.TagMode = TagMode.StartTagAndEndTag;  // <dl> is not self closing

            var encoded = BuildContent();
            output.Content.SetHtmlContent(encoded);

            if (!IsVisible)
            {
                output.PreElement.AppendHtml("<!--");
                output.PostElement.AppendHtml("-->");
            }
        }

        private string BuildContent()
        {
            var sb = new StringBuilder();
            if (IncludeEnvironment)
            {
                var environment = _htmlHelper.Encode(_hostingEnvironment.EnvironmentName);
                sb.Append($"<dt>Environment</dt><dd>{environment}</dd>");
            }
            if (IncludeMachine)
            {
                var machine = _htmlHelper.Encode(Environment.MachineName);
                sb.Append($"<dt>Machine</dt><dd>{machine}</dd>");
            }
            if (IncludeOs)
            {
                var os = _htmlHelper.Encode(RuntimeInformation.OSDescription);
                sb.Append($"<dt>OS</dt><dd>{os}</dd>");
            }
            if (IncludeOsArchitecture)
            {
                var version = _htmlHelper.Encode(RuntimeInformation.OSArchitecture);
                sb.Append($"<dt>OS Architecture</dt><dd>{version}</dd>");
            }
            if (IncludeApplicationName)
            {
                var version = _htmlHelper.Encode(PlatformServices.Default.Application.ApplicationName);
                sb.Append($"<dt>App Name</dt><dd>{version}</dd>");
            }
            if (IncludeApplicationVersion)
            {
                var version = _htmlHelper.Encode(PlatformServices.Default.Application.ApplicationVersion);
                sb.Append($"<dt>App Version</dt><dd>{version}</dd>");
            }
            if (IncludeApplicationRuntime)
            {
                var version = _htmlHelper.Encode(PlatformServices.Default.Application.RuntimeFramework);
                sb.Append($"<dt>Runtime Framework</dt><dd>{version}</dd>");
            }
            var unenecoded = sb.ToString();
            return (unenecoded);
        }
    }
}
