﻿using System;
using System.Text.RegularExpressions;
using Orchard.DynamicForms.Helpers;
using Orchard.DynamicForms.Services;
using Orchard.DynamicForms.Services.Models;
using Orchard.Localization;

namespace Orchard.DynamicForms.ValidationRules
{
    public class UrlAddress : ValidationRule
    {
        public UrlAddress() {
            RegexOptions = RegexOptions.Singleline | RegexOptions.IgnoreCase;
            Pattern = @"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$";
        }

        public string Pattern { get; set; }
        public RegexOptions RegexOptions { get; set; }
        
        public override void Validate(ValidateInputContext context)
        {
            if (!Regex.IsMatch(context.AttemptedValue, Pattern, RegexOptions))
            {
                var message = GetValidationMessage(context);
                context.ModelState.AddModelError(context.FieldName, message.Text);
            }
        }

        public override void RegisterClientAttributes(RegisterClientValidationAttributesContext context)
        {
            context.ClientAttributes["data-val-regex"] = GetValidationMessage(context).Text;
            context.ClientAttributes["data-val-regex-pattern"] = Pattern;
        }

        private LocalizedString GetValidationMessage(ValidationContext context)
        {
            return String.IsNullOrWhiteSpace(ErrorMessage)
                ? T("{0} is not a valid URL.", context.FieldName)
                : new LocalizedString(Tokenize(ErrorMessage, context));
        }
    }
}
