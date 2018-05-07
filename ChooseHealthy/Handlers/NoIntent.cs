using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChooseHealthy.Models.Common;

namespace ChooseHealthy.Handlers
{
    public class NoIntent
    {
        internal static CommonModel Process(CommonModel commonModel)
        {
            commonModel.Response.Text = "No!";
            commonModel.Session.EndSession = false;
            return commonModel;
        }
    }
}