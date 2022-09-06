using MediaBrowser.Controller.Configuration;
using MediaBrowser.Controller.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using JellyLyrics.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Reflection;

namespace JellyLyrics.Api;

/// <summary>
/// Controller for accessing trickplay data.
/// </summary>
[ApiController]
[Route("Items")]
public class LyricController : ControllerBase
{
    private readonly Assembly _assembly;
    private readonly string _trickplayScriptPath;

    private readonly ILogger<LyricController> _logger;
    private readonly ILibraryManager _libraryManager;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IServerConfigurationManager _configurationManager;

    private readonly PluginConfiguration _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="LyricController"/> class.
    /// </summary>
    public LyricController(
        ILibraryManager libraryManager,
        ILogger<LyricController> logger,
        ILoggerFactory loggerFactory,
        IServerConfigurationManager configurationManager)
    {
        _assembly = Assembly.GetExecutingAssembly();
        _trickplayScriptPath = GetType().Namespace + ".trickplay.js";

        _libraryManager = libraryManager;
        _logger = logger;
        _loggerFactory = loggerFactory;
        _configurationManager = configurationManager;

        _config = JellyLyricsPlugin.Instance!.Configuration;
    }

    /// <summary>
    /// Get an item's lyrics.
    /// </summary>
    /// <param name="itemId">Item id.</param>
    /// <response code="200">Lyrics successfully found and returned.</response>
    /// <response code="404">Item not found.</response>
    /// <returns>A JSON response as read from manfiest file, or a <see cref="NotFoundResult"/>.</returns>
    [HttpGet("{itemId}/Lyrics")]
    [Authorize(Policy = "DefaultAuthorization")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult> GetLyrics([FromRoute, Required] Guid itemId)
    {
        var item = _libraryManager.GetItemById(itemId);
        if (item == null)
        {
            return NotFound(new { });
        }

        string lrcFilePath = Path.ChangeExtension(item.Path, "lrc");
        if (!System.IO.File.Exists(lrcFilePath))
        {
            return NotFound(new { });
        }

        string lrcFileContent = System.IO.File.ReadAllText(@lrcFilePath);
        var lyricData = Kfstorm.LrcParser.LrcFile.FromText(lrcFileContent);

        if (lyricData == null)
        {
            return NotFound(new { });
        }

        List<object> lyrics = new List<object>();

        // Remove Brackets
        // If first char is numeric, add the line to the Lyrics
        foreach (var lyricLine in lyricData.Lyrics)
        {
            // Convert seconds to ticks
            double ticks = lyricLine.Timestamp.TotalSeconds * 10000000;
            lyrics.Add(new { start = Math.Ceiling(ticks), text = lyricLine.Content });
        }

        return Ok(new { results = lyrics });
    }
}
