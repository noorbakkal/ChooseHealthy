using System;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using ChooseHealthy.Models.Common;

namespace ChooseHealthy.Handlers
{
    public class ProviderIntent
    {
        internal static CommonModel Process(CommonModel commonModel)
        {
           // var time = commonModel.Request.Parameters.FirstOrDefault(p => p.Key == "Time");
           // var date = commonModel.Request.Parameters.FirstOrDefault(p => p.Key == "Date");
            var number = commonModel.Request.Parameters.FirstOrDefault(p => p.Key == "Number");
            var providerTypes = commonModel.Request.Parameters.FirstOrDefault(p => p.Key == "Types");
            var code = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
            short.TryParse(number.Value, out short distance);
            try
            {
                //using (var data = new ChooseHealthy.Data.NoorDataBaseEntities())
                //{
                //    data.providers.Add(new Data.provider
                //    {
                //        Code = code,
                //        Distance = distance,
                //        Time = "5", // time.Value,
                //        Date = "Sunday",
                //        ProviderTypes = "poditry" //providerTypes.Value
                //    });
                //    data.SaveChanges();
                //}
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


            commonModel.Response.Ssml = $"Perfect, I found John Smith a {providerTypes.Value} provider within {number.Value}. Bot code is <say-as interpret-as=\"spell-out\">212</say-as>.";

            commonModel.Response.Text = $"Perfect, I found John Smith a {providerTypes.Value} provider within {number.Value}. Bot code is 212";

            commonModel.Response.Card = new Card
            {
                Title = "Choose Healthy Providers",
                Text = $"Your {providerTypes.Value} provider within {number.Value}. Your code 212"
            };
            commonModel.Session.EndSession = true;
            return commonModel;
        }
    }
}