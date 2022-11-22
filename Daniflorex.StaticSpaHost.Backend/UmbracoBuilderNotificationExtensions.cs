using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;

public static class UmbracoBuilderNotificationExtensions
{
    public static IUmbracoBuilder AddTriggerBuildNotifications(this IUmbracoBuilder builder)
    {
        builder
            .AddNotificationHandler<ContentPublishedNotification, TriggerFrontendBuild>()
            .AddNotificationHandler<ContentPublishedNotification, TriggerFrontendBuild>()
            .AddNotificationHandler<ContentPublishedNotification, TriggerFrontendBuild>()
            .AddNotificationHandler<ContentPublishedNotification, TriggerFrontendBuild>()
            .AddNotificationHandler<ContentPublishedNotification, TriggerFrontendBuild>();

        return builder;
    }
}