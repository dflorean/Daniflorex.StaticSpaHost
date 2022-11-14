using Daniflorex.StaticSpaHost;
using Microsoft.AspNetCore.Mvc;
using System;
using Umbraco.Cms.Core;
using Umbraco.Cms.Web.Common.Controllers;
using UmbracoContentApi.Core.Resolvers;

namespace Daniflorex.StaticSpaHost.Backend.Endpoints;

//reference https://github.com/deMD/UmbracoContentApi
[Route("{language}/api/content")]
public class ContentApiController : UmbracoApiController
{
    private readonly Lazy<IContentResolver> _contentResolver;
    private readonly IPublishedContentQuery _publishedContent;

    public ContentApiController(
        Lazy<IContentResolver> contentResolver, IPublishedContentQuery publishedContent)
    {
        _contentResolver = contentResolver;
        _publishedContent = publishedContent;
    }

    [HttpGet("{id:guid}")]
    public IActionResult Get(Guid id)
    {
        //if (!string.IsNullOrEmpty(language))
        //{
        //    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(language);
        //    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(language);
        //}

        var content = _publishedContent.Content(id);
        return Ok(_contentResolver.Value.ResolveContent(content));
    }
}

