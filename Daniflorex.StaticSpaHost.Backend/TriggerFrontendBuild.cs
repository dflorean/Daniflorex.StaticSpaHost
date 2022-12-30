using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using System;

public class TriggerFrontendBuild :
    INotificationHandler<ContentPublishedNotification>,
    INotificationHandler<ContentUnpublishedNotification>,
    INotificationHandler<ContentDeletedNotification>,
    INotificationHandler<ContentMovedNotification>,
    INotificationHandler<ContentMovedToRecycleBinNotification>
{
    private readonly IConfiguration _config;
    public TriggerFrontendBuild(IConfiguration config)
    {
        _config = config;
    }

    public async void Handle(ContentPublishedNotification notification)
    {
        await TriggerBuild();
    }

    public async void Handle(ContentMovedNotification notification)
    {
        await TriggerBuild();
    }

    public async void Handle(ContentUnpublishedNotification notification)
    {
        await TriggerBuild();
    }

    public async void Handle(ContentDeletedNotification notification)
    {
        await TriggerBuild();
    }

    public async void Handle(ContentMovedToRecycleBinNotification notification)
    {
        await TriggerBuild();
    }

    private async Task TriggerBuild()
    {
        string organization = "azureadmin0382";
        string project = "StaticSpaHost";
        int pipelineId = 28;
        
        string azurePersonalAccessToken = _config["BuildPipelinePersonalAccessToken"];

        string baseUrl = $"https://dev.azure.com/{organization}/{project}/_apis/pipelines/{pipelineId}/runs?api-version=6.0-preview.1";

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var token = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", azurePersonalAccessToken)));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token);

            var bodyContent = new StringContent("{}"/*empty json*/, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(baseUrl, bodyContent);
            response.EnsureSuccessStatusCode();
        }
    }
}