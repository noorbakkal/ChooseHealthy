using System;
using System.Threading.Tasks;
using ApiAiSDK;
using ChooseHealthy.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace ChooseHealthy.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private const string clientAccessToken = "a49e7b12d73446a7b6d4e833a45d14b2"; // "e674ba939a9c46a694d1eed86403e5f9";
        private static AIConfiguration config = new AIConfiguration(clientAccessToken, SupportedLanguage.English);
        private static ApiAi apiAi = new ApiAi(config);
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var aiResponse = apiAi.TextRequest(string.IsNullOrWhiteSpace (activity.Text) ? "Hello" : activity.Text);
            var commonModel = CommonModelMapper.ApiAiToCommonModel(aiResponse);
            commonModel = IntentRouter.Process(commonModel);
            // calculate something for us to return
            int length = (activity.Text ?? string.Empty).Length;

            // return our reply to the user
            await context.PostAsync(aiResponse.Result.Fulfillment.Speech);
            //await context.PostAsync($"You sent {activity.Text} which was {length} characters");

            context.Wait(MessageReceivedAsync);
        }
    }
}