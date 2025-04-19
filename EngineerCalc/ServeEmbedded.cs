using System.Reflection;

namespace EngineerCalc;

internal sealed class EmbeddedServer
{
    private Dictionary<string, string> _fileRoutes;

    public EmbeddedServer()
    {
        _fileRoutes = new Dictionary<string, string>()
        {
            { "/", "/index.html" }
        };
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
            webApplication.MapGet(route.Key, (context) => Serve(context, route.Value));
        }
    }

    private static async Task Serve(HttpContext context, string resourceName)
    {
        await using Stream? source = typeof(EmbeddedServer).Assembly.GetManifestResourceStream(resourceName);
        if (source == null)
        {
            context.Response.StatusCode = 404;
            return;
        }
        context.Response.StatusCode = 200;
        context.Response.ContentLength = source.Length;
        await source.CopyToAsync(context.Response.Body);
    }
}
