using System.Collections.Generic;
using System.Linq;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Alexa.NET.Response.Directive;
using ApiAiSDK.Model;
using ChooseHealthy.Models.Common;

namespace ChooseHealthy.Helpers
{
    public class CommonModelMapper
    {
        internal static CommonModel AlexaToCommonModel(SkillRequest skillRequest)
        {
            var commonModel = new CommonModel()
            {
                Id = skillRequest.Request.RequestId
            };

            var requestType = skillRequest.GetRequestType();

            commonModel.Request.Channel = "alexa";
            if (requestType == typeof(IntentRequest))
            {
                var intentRequest = skillRequest.Request as IntentRequest;
                commonModel.Request.State = intentRequest.DialogState;
                commonModel.Request.Intent = intentRequest.Intent.Name;
                if (intentRequest.Intent.Slots != null)
                    commonModel.Request.Parameters = intentRequest.Intent.Slots.ToList()
                        .ConvertAll(s => new KeyValuePair<string, string>(s.Value.Name,s.Value.Value));
            }
            else if (requestType == typeof(LaunchRequest))
            {
                commonModel.Request.Intent = "DefaultWelcomIntent";

            }
            else if (requestType == typeof(SessionEndedRequest))
            {
                return null;
            }
            return commonModel;
        }

        internal static CommonModel ApiAiToCommonModel(AIResponse aiResponse)
        {
            var commonModel = new CommonModel()
            {
                Id = aiResponse.Id
            };

            commonModel.Session.Id = aiResponse.SessionId;
            commonModel.Request.Intent = aiResponse.Result.Metadata.IntentName;
            commonModel.Request.State = aiResponse.Result.ActionIncomplete ? "IN_PROGRESS" : "COMPLETED";
            commonModel.Request.Channel = aiResponse.Result.Source;
            commonModel.Request.Parameters = aiResponse.Result.Parameters.ToList()
                .ConvertAll(a => new KeyValuePair<string, string>(a.Key, a.Value.ToString()));
          return commonModel;
        }

        internal static SkillResponse CommonModelToAlexa(CommonModel commonModel)
        {
            var response = new SkillResponse()
            {
                Version = "1.0",
                Response = new ResponseBody()
            };

            if (commonModel.Request.State == "STARTED" || commonModel.Request.State == "IN_PROGRESS")
            {
                var directive = new DialogDelegate
                {
                    UpdatedIntent = new Intent
                    {
                        Name = commonModel.Request.Intent,
                        Slots = commonModel.Request.Parameters?.ToDictionary(p => p.Key, p => new Slot { Name = p.Key, Value = p.Value })
                    }
                };

                response.Response.Directives.Add(directive);
                response.Response.ShouldEndSession = false;

                return response;
            }
            if (string.IsNullOrWhiteSpace(commonModel.Response.Ssml))
            {
                response.Response.OutputSpeech = new PlainTextOutputSpeech
                {
                    Text = commonModel.Response.Text
                };
            }
            else
            {
                response.Response.OutputSpeech = new SsmlOutputSpeech { Ssml = "<speak>" + commonModel.Response.Ssml + "</speak>" };
            }
            if (commonModel.Response.Card != null)
            {
                response.Response.Card = new SimpleCard
                {
                    Title = commonModel.Response.Card.Title,
                    Content = commonModel.Response.Card.Text
                };
            }

            response.Response.OutputSpeech = new PlainTextOutputSpeech { Text = commonModel.Response.Text };
            if (!string.IsNullOrWhiteSpace(commonModel.Response.Prompt))
            {
                response.Response.Reprompt = new Reprompt { OutputSpeech = new PlainTextOutputSpeech { Text = commonModel.Response.Prompt } };
            }

            response.Response.ShouldEndSession = commonModel.Session.EndSession;

            return response;
        }

        internal static dynamic CommonModelToApiAi(CommonModel commonModel)
        {
            if (string.IsNullOrWhiteSpace(commonModel.Response.Event))
                return new
                {
                    speech = (string.IsNullOrWhiteSpace(commonModel.Response.Ssml)  || commonModel.Request.Channel == "agent") ? commonModel.Response.Text : "<speak>" + commonModel.Response.Ssml + "</speak>",
                    displayText = commonModel.Response.Text,
                    data = new {slack = new { text = commonModel.Response.Text }  , google = new { expectUserResponse = !commonModel.Session.EndSession } },
                    source = "Choose Healthy Bot"
                };
            else
                return new
                {
                    followupEvent = new
                    {
                        name = commonModel.Response.Event
                    }
                };

        }
    }
}