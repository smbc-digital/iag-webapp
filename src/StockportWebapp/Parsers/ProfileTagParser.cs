using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using StockportWebapp.Models;
using StockportWebapp.Utils;

namespace StockportWebapp.Parsers
{
    
    public class ProfileTagParser : IDynamicTagParser<Profile>
    {
        private readonly IViewRender _viewRenderer;
        private readonly ILogger<ProfileTagParser> _logger;

        public ProfileTagParser(IViewRender viewRenderer, ILogger<ProfileTagParser> logger)
        {
            _viewRenderer = viewRenderer;
            _logger = logger;
        }

        protected Regex TagRegex => new Regex("{{PROFILE:(\\s*[/a-zA-Z0-9][^}]+)}}", RegexOptions.Compiled);

        public string Parse(string content, IEnumerable<Profile> profiles)
        { 
            var matches = TagRegex.Matches(content);

            foreach (Match match in matches)
            {
                var tagDataIndex = 1;
                var profileSlug = match.Groups[tagDataIndex].Value;
                var profile = GetProfileMatchingSlug(profiles, profileSlug);
                if (profile != null)
                {

                    var profileHtml = string.IsNullOrEmpty(profile.Body)
                        ? _viewRenderer.Render("ProfileWithoutBody", profile)
                        : _viewRenderer.Render("Profile", profile);

                    content = TagRegex.Replace(content, profileHtml, 1);
                }
            }
            return RemoveEmptyTags(content);
        }

        private string RemoveEmptyTags(string content)
        {
            return TagRegex.Replace(content, string.Empty);
        }

        private Profile GetProfileMatchingSlug(IEnumerable<Profile> profiles, string slug)
        {
            return profiles.FirstOrDefault(s => s.Slug == slug);
        }
    }
}