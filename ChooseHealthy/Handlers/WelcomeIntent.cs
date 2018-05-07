using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChooseHealthy.Models.Common;

namespace ChooseHealthy.Handlers
{
    public class WelcomeIntent
    {
        internal static CommonModel Process(CommonModel commonModel)
        {
            if (commonModel.Request.Channel == "alexa")
            {
                commonModel.Response.Text = "Hi there, would you like to find a provider?";
                commonModel.Response.Prompt = "Would you like to find a provider? Please say yes or no.";
            }
            else
            {
                commonModel.Response.Text = "Hi google, would you like to find a provider?";
                commonModel.Response.Prompt = "Would you like to find a provider? Please say yes or no.";
            }
            commonModel.Session.EndSession = false;
            return commonModel;
        }
    }
}