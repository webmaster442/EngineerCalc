using System.Reflection;

using Microsoft.AspNetCore.StaticFiles;

namespace EngineerCalc;

internal sealed class EmbeddedServer
{
    private readonly Dictionary<string, string> _fileRoutes;
    private readonly FileExtensionContentTypeProvider _mimes = new();

    public EmbeddedServer()
    {
        _fileRoutes = new Dictionary<string, string>();
        var embeddedFiles = typeof(EmbeddedServer).Assembly.GetManifestResourceNames();
        foreach (var embeddedFile in embeddedFiles)
        {
            var shortName = string.Join('.', embeddedFile.Split('.')[^2..]);
            _fileRoutes.Add(shortName, embeddedFile);
        }
    }

    public void MapEmbeddedFilesAsRoutes(WebApplication webApplication)
    {
        foreach (var route in _fileRoutes)
        {
            if (route.Key == "index.html")
                webApplication.MapGet("/", (context) => Serve(context, route.Value));

            webApplication.MapGet(route.Key, (context) => Serve(context, route.Value));
        }
    }

    private async Task Serve(HttpContext context, string resourceName)
    {
        await using Stream? source = typeof(EmbeddedServer).Assembly.GetManifestResourceStream(resourceName);
        if (source == null)
        {
            context.Response.StatusCode = 404;
            return;
        }
        if (!_mimes.TryGetContentType(resourceName, out string? mime))
            mime = "application/octet-stream";

        context.Response.StatusCode = 200;
        context.Response.ContentLength = source.Length;
        context.Response.ContentType = mime;
        await source.CopyToAsync(context.Response.Body);
    }
}
