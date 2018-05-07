using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChooseHealthy.Models.Common;

namespace ChooseHealthy.Handlers
{
    public class HelpIntent
    {
        internal static CommonModel Process(CommonModel commonModel)
        {
            commonModel.Response.Text = "Hi there, welcome to choose healthy, I can help you find a provider?";
            commonModel.Response.Prompt = "Would you like to find a provider? Please say yes or no.";
            commonModel.Session.EndSession = false;

            return commonModel;
        }
    }
}