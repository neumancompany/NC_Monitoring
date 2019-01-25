using AutoMapper;
using Castle.Core.Logging;
using ExpectedObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Logging;
using Moq;
using NC.AspNetCore.Extensions;
using NC_Monitoring.Business.DTO;
using NC_Monitoring.Business.Interfaces;
using NC_Monitoring.Business.Managers;
using NC_Monitoring.Controllers.Api;
using NC_Monitoring.Data.Interfaces;
using NC_Monitoring.Data.Models;
using NC_Monitoring.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NC_Monitoring.Test.App.Controllers.Api
{
    public class ApiChannelControllerTest
    {

        private IMapper mapper;

        public ApiChannelControllerTest()
        {
            mapper = Utils.CreateMapper();
        }

        [Fact]
        public async Task Post_Return_CorrectViewModel()
        {
            // Arrange
            int id = 20;
            string name = "Test";
            var scenarios = new List<NcScenario>();
            var channel = new NcChannel
            {
                Id = id,
                Name = name,
            };

            var logger = new Mock<ILogger<ChannelsController>>();
            var repository = new Mock<IChannelRepository>();            
            var manager = new Mock<IChannelManager>();
            
            var expected = new ChannelViewModel
            {
                Id = id,
                Name = name,
                Scenarios = mapper.MapEnumerable<ScenarioListDTO>(scenarios).ToList(),
            }.ToExpectedObject();
            
            // Act
            var controller = 
                new ChannelsController(logger.Object, 
                    repository.Object, mapper, manager.Object, It.IsAny<ApplicationUserManager>())
                    .SetValidator();

            var controllerResult = await controller.Post(JsonConvert.SerializeObject(channel));
            
            // Assert
            var actual = (controllerResult as OkObjectResult)?.Value;
            
            expected.ShouldEqual(actual);
        }

        [Fact]
        public async Task Post_CalledRepositoryInsert()
        {
            // act
            var repository = new Mock<IChannelRepository>();
            repository.Setup(x => x.InsertAsync(It.IsAny<NcChannel>())).Returns(Task.CompletedTask);

            // arrange
            var controller =
                new ChannelsController(It.IsAny<ILogger<ChannelsController>>(),
                    repository.Object, mapper, It.IsAny<ChannelManager>(), null)
                    .SetValidator();

            await controller.Post("");

            // assert
            repository.VerifyAll();
        }

        

    }
}
