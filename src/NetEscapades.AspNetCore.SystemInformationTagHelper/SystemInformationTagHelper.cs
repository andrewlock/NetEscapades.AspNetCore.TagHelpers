﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Encodings.Web;

namespace NetEscapades.AspNetCore.TagHelpers
{
    /// <summary>
    /// <see cref="ITagHelper"/> implementation targeting &lt;system-info&gt; elements that renders
    /// information based on the current system and environment. Can be rendered as a list or as an
    /// HTML comment
    /// </summary>
    public class SystemInfoTagHelper : TagHelper
    {
        private readonly HtmlEncoder _htmlEncoder;
        private readonly IHostingEnvironment _hostingEnvironment;
        public SystemInfoTagHelper(HtmlEncoder htmlEncoder, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _htmlEncoder = htmlEncoder;
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
                var environment = _htmlEncoder.Encode(_hostingEnvironment.EnvironmentName);
                sb.Append($"<dt>Environment</dt><dd>{environment}</dd>");
            }
            if (IncludeMachine)
            {
                var machine = _htmlEncoder.Encode(Environment.MachineName);
                sb.Append($"<dt>Machine</dt><dd>{machine}</dd>");
            }
            if (IncludeOs)
            {
                var os = _htmlEncoder.Encode(RuntimeInformation.OSDescription);
                sb.Append($"<dt>OS</dt><dd>{os}</dd>");
            }
            if (IncludeOsArchitecture)
            {
                var version = _htmlEncoder.Encode(RuntimeInformation.OSArchitecture.ToString());
                sb.Append($"<dt>OS Architecture</dt><dd>{version}</dd>");
            }
            if (IncludeApplicationName)
            {
                var version = _htmlEncoder.Encode(PlatformServices.Default.Application.ApplicationName);
                sb.Append($"<dt>App Name</dt><dd>{version}</dd>");
            }
            if (IncludeApplicationVersion)
            {
                var version = _htmlEncoder.Encode(PlatformServices.Default.Application.ApplicationVersion);
                sb.Append($"<dt>App Version</dt><dd>{version}</dd>");
            }
            if (IncludeApplicationRuntime)
            {
                var version = _htmlEncoder.Encode(PlatformServices.Default.Application.RuntimeFramework.ToString());
                sb.Append($"<dt>Runtime Framework</dt><dd>{version}</dd>");
            }
            var unenecoded = sb.ToString();
            return (unenecoded);
        }
    }
}
