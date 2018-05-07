using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using ChooseHealthy.Models;
using ChooseHealthy.Models.Common;

namespace ChooseHealthy.Helpers
{
    public class IntentRouter
    {
        public static CommonModel Process(CommonModel commonModel)
        {
            var intentsList = WebApiConfig.IntentHandlers;
            var intent = intentsList.FirstOrDefault(i => i.Key.ToLower() == commonModel.Request.Intent.ToLower());
            try
            {
                if (!string.IsNullOrWhiteSpace(intent.Key))
                {
                    return intent.Value(commonModel);
                }
                if (string.IsNullOrWhiteSpace(commonModel.Response.Text))
                {
                    commonModel.Response.Text = "Sorry, I don't understand that, please try again.";
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Debug.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
            }
            return commonModel;
        }
    }
}