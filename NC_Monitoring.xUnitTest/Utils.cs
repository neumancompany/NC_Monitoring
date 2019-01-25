using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Moq;
using NC_Monitoring.Mapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NC_Monitoring.Test
{
    public static class Utils
    {
        public static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => MapperConfigure.Configure(cfg));

            return config.CreateMapper();
        }

        /// <summary>
        /// Nastaveni validatoru pro controller, ktery pouziva metodu TryValidateModel
        /// </summary>
        /// <typeparam name="TController"></typeparam>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static TController SetValidator<TController>(this TController controller) where TController : ControllerBase
        {
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            controller.ObjectValidator = objectValidator.Object;

            return controller;
        }

        /// <summary>
        /// Kontrola validace modelu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new System.ComponentModel.DataAnnotations.ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}
