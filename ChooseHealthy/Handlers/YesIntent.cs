using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChooseHealthy.Models.Common;

namespace ChooseHealthy.Handlers
{
    public class YesIntent
    {
        internal static CommonModel Process(CommonModel commonModel)
        {
            commonModel.Response.Event = "PROVIDERS";
           
            return commonModel;
        }
        //{
        //    commonModel.Response.Text = "Yes!";          
        //    commonModel.Session.EndSession = false;
        //    return commonModel;
        //}
    }
}