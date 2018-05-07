using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ApiAiSDK.Model;
using ChooseHealthy.Helpers;

namespace ChooseHealthy.Controllers
{
    public class ApiAiController : ApiController
    {
        public dynamic Post(AIResponse aIResponse)
        {
            var commonModel = CommonModelMapper.ApiAiToCommonModel(aIResponse);
            if (commonModel == null)
            {
                return null;
            }
            try
            {
                commonModel = IntentRouter.Process(commonModel);
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
            return CommonModelMapper.CommonModelToApiAi(commonModel);
        }

        public string Get()
        {
            return "Hello DialogFlow!";
        }
    }

}
