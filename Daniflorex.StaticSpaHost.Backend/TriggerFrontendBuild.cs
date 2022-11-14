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
        string project = "Umbraco9Headless-Test";
        int pipelineId = 19;
        string azurePersonalAccessToken = "mkxhzfwxhaswevhcc3xo3rztivpy2z4jrjb2tv43b6rkzhkzq5fa";

        string baseUrl = $"https://dev.azure.com/{organization}/{project}/_apis/pipelines/{pipelineId}/runs?api-version=6.0-preview.1";

        //var response = await baseUrl
        //    .WithBasicAuth("", azurePersonalAccessToken)
        //    .PostAsync();

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var token = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", azurePersonalAccessToken)));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token);

            //    var repoGuid = "Put GUID Here"; // You can get GUID for repo from the URL when you select the rpo of interest under  Repos is Project Settings
            //    var bodyJson = @"{
            //    ""parameters"": {
            //        ""parameterName"": ""parameterValue""
            //    },
            //    ""variables"": {},
            //    ""resources"": {
            //        ""repositories"": {
            //            ""self"": {
            //                ""repository"": {
            //                    ""id"": """ + repoGuid + @""",
            //                    ""type"": ""azureReposGit""
            //                },
            //                ""refName"": ""refs/heads/master""
            //            }
            //        }
            //    }
            //}";

            var bodyContent = new StringContent("{}"/*empty json*/, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(baseUrl, bodyContent);
            response.EnsureSuccessStatusCode();
        }
    }
}