using Microsoft.Extensions.Hosting;
using System.IO;
using System.Text;

namespace Shared.Other;

public interface IEmailTemplateLoader
{
    string LoadEmailTemplates(string templateName, Dictionary<string, string> replacements);
}

public class EmailTemplateLoader : IEmailTemplateLoader
{
    private readonly string _templatesPath;

    public EmailTemplateLoader(IHostEnvironment hostEnvironment)
    {
        var possibleRoots = new[]
        {
            Path.Combine(hostEnvironment.ContentRootPath, "..", "Shared", "Other"),
            Path.Combine(hostEnvironment.ContentRootPath, "Shared", "Other"),
            Path.Combine(AppContext.BaseDirectory, "Other"),
            Path.Combine(Directory.GetCurrentDirectory(), "Shared", "Other"),
        };

        foreach (var root in possibleRoots)
        {
            var fullPath = Path.GetFullPath(root);
            if (Directory.Exists(fullPath))
            {
                _templatesPath = fullPath;
                break;
            }
        }

        if (string.IsNullOrEmpty(_templatesPath))
        {
            _templatesPath = Path.GetFullPath(Path.Combine(hostEnvironment.ContentRootPath, "Shared", "Other"));
            Directory.CreateDirectory(_templatesPath);
        }
    }

    public string LoadEmailTemplates(string templateName, Dictionary<string, string> replacements)
    {
        var templatePath = Path.Combine(_templatesPath, templateName);
        
        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException($"Template '{templateName}' not found at '{templatePath}'");
        }

        var templateContent = File.ReadAllText(templatePath, Encoding.UTF8);
        
        if (replacements != null)
        {
            foreach (var replacement in replacements)
            {
                templateContent = templateContent.Replace($"{{{{ {replacement.Key} }}}}", replacement.Value);
            }
        }

        return templateContent;
    }
}