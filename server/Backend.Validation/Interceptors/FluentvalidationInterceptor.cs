using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Backend.Validation.Interceptors
{
    public class FluentvalidationInterceptor : IValidatorInterceptor
    {
        public ValidationResult AfterAspNetValidation(ActionContext actionContext, IValidationContext validationContext, ValidationResult result)
        {
            if (!result.IsValid)
            {
                JObject errorObject = new JObject();
                int i = 1;

                foreach (ValidationFailure error in result.Errors)
                {
                    errorObject.Add($"{i} - {error.PropertyName}\n", error.ErrorMessage);
                    i++;
                }

                string errorMessage = errorObject.ToString(Formatting.None)
                                                 .Replace(@"\n", "");

                throw new Exception(errorMessage);
            }

            return result;
        }

        public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext)
        {
            return commonContext;
        }
    }
}
