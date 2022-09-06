using System.Text.RegularExpressions;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Controller.Configuration;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;
using Microsoft.Extensions.Logging;
using JellyLyrics.Configuration;

namespace JellyLyrics;

/// <summary>
/// JellyLyrics plugin.
/// </summary>
public class JellyLyricsPlugin : BasePlugin<PluginConfiguration>, IHasWebPages
{
    /// <inheritdoc />
    public override string Name => "JellyLyrics";

    /// <inheritdoc />
    public override Guid Id => Guid.Parse("5ca0cfcf-17c5-4446-9f0d-524c7a6617e9");

    /// <inheritdoc />
    public override string Description => "Returns an item's local lyrics file.";

    /// <summary>
    /// Initializes a new instance of the <see cref="JellyLyricsPlugin"/> class.
    /// </summary>
    public JellyLyricsPlugin(
        IApplicationPaths applicationPaths,
        IXmlSerializer xmlSerializer,
        ILogger<JellyLyricsPlugin> logger,
        IServerConfigurationManager configurationManager)
        : base(applicationPaths, xmlSerializer)
    {
        Instance = this;
    }

    /// <summary>
    /// Gets the current plugin instance.
    /// </summary>
    public static JellyLyricsPlugin? Instance { get; private set; }

    /// <inheritdoc />
    public IEnumerable<PluginPageInfo> GetPages()
    {
        return new[]
        {
            new PluginPageInfo
            {
                Name = "Jellylyric",
                EmbeddedResourcePath = GetType().Namespace + ".Configuration.configPage.html"
            }
        };
    }
}
