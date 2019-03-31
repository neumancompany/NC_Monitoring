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
    public class ChannelControllerTest
    {

        private IMapper mapper;

        public ChannelControllerTest()
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

            var controller =
                new ChannelsController(logger.Object,
                    repository.Object, mapper, manager.Object, It.IsAny<ApplicationUserManager>())
                    .SetValidator();

            // Act
            var controllerResult = await controller.Post(JsonConvert.SerializeObject(channel));
            
            // Assert
            var actual = (controllerResult as OkObjectResult)?.Value;
            
            expected.ShouldEqual(actual);
        }

        [Fact]
        public async Task Post_CalledRepositoryInsert()
        {
            // arrange
            var repository = new Mock<IChannelRepository>();
            repository.Setup(x => x.InsertAsync(It.IsAny<NcChannel>())).Returns(Task.CompletedTask);
            var controller =
                new ChannelsController(It.IsAny<ILogger<ChannelsController>>(),
                    repository.Object, mapper, It.IsAny<ChannelManager>(), null)
                    .SetValidator();

            // act
            await controller.Post("");

            // assert
            repository.VerifyAll();
        }

        [Theory]
        [InlineData(2, "old name", "new name")]
        [InlineData(5, "Stary nazev kanalu", "Novy nazev kanalu")]
        public async Task Put_Return_CorrectViewModel(int id, string oldName, string newName)
        {
            // Arrange
            var scenarios = new List<NcScenario>();
            var oldChannel = new NcChannel
            {
                Id = id,
                Name = oldName,
            };

            var newChannel = new NcChannel
            {
                Id = id,
                Name = newName,
            };

            var repository = new Mock<IChannelRepository>();
            repository.Setup(x => x.FindById(id)).Returns(oldChannel);
            repository.Setup(x => x.UpdateAsync(It.IsAny<NcChannel>())).Returns(Task.CompletedTask);

            var expected = new ChannelViewModel
            {
                Id = id,
                Name = newName,
                Scenarios = mapper.MapEnumerable<ScenarioListDTO>(scenarios).ToList(),
            }.ToExpectedObject();

            var controller =
                new ChannelsController(
                    It.IsAny<ILogger<ChannelsController>>(),
                    repository.Object, 
                    mapper, 
                    It.IsAny<IChannelManager>(), 
                    It.IsAny<ApplicationUserManager>())
                    .SetValidator();

            // Act            
            var controllerResult = await controller.Put(id, JsonConvert.SerializeObject(newChannel));
            var actual = (controllerResult as OkObjectResult)?.Value;

            // Assert            
            expected.ShouldEqual(actual);
        }

        [Fact]
        public async Task Put_Call_RepositoryFindById()
        {
            // act
            var channel = new NcChannel();

            var repository = new Mock<IChannelRepository>();
            repository.Setup(x => x.FindById(It.IsAny<int>())).Returns(channel);            

            var controller =
                new ChannelsController(It.IsAny<ILogger<ChannelsController>>(),
                    repository.Object, mapper, It.IsAny<ChannelManager>(), null)
                    .SetValidator();

            // arrange
            await controller.Put(It.IsAny<int>(), It.IsAny<string>());

            // assert
            repository.VerifyAll();
        }

        [Fact]
        public async Task Put_Call_RepositoryUpdate()
        {
            // act
            var channel = new NcChannel();

            var repository = new Mock<IChannelRepository>();
            repository.Setup(x => x.FindById(channel.Id)).Returns(channel);

            var controller =
                new ChannelsController(It.IsAny<ILogger<ChannelsController>>(),
                    repository.Object, mapper, It.IsAny<ChannelManager>(), null)
                    .SetValidator();

            // arrange
            await controller.Put(It.IsAny<int>(), "");

            // assert
            repository.Verify(x => x.UpdateAsync(channel));
            repository.VerifyAll();
        }


        [Fact]
        public async Task Delete_Call_RepositoryDelete()
        {
            // act
            var channel = new NcChannel();

            var repository = new Mock<IChannelRepository>();
            repository.Setup(x => x.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

            var controller =
                new ChannelsController(It.IsAny<ILogger<ChannelsController>>(),
                    repository.Object, mapper, It.IsAny<ChannelManager>(), null)
                    .SetValidator();

            // arrange
            await controller.Delete(It.IsAny<int>());

            // assert
            repository.VerifyAll();
        }

        [Fact]
        public async Task Load_Call_RepositoryGetAll()
        {
            // act
            var id = It.IsAny<int>();
            var channel = new NcChannel();

            var repository = new Mock<IChannelRepository>();

            var controller =
                new ChannelsController(
                    It.IsAny<ILogger<ChannelsController>>(),
                    repository.Object, 
                    mapper, 
                    It.IsAny<IChannelManager>(),
                    It.IsAny<ApplicationUserManager>())
                    .SetValidator();

            // arrange
            await controller.Delete(id);

            // assert
            repository.Verify(x => x.DeleteAsync(id), Times.Once());

        }

    }
}
