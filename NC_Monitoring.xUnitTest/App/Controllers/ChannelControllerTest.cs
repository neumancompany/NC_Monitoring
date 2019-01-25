using AutoMapper;
using ExpectedObjects;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NC.AspNetCore.Extensions;
using NC_Monitoring.Business.DTO;
using NC_Monitoring.Business.Interfaces;
using NC_Monitoring.Controllers;
using NC_Monitoring.Data.Models;
using NC_Monitoring.Mapper;
using NC_Monitoring.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NC_Monitoring.Test.App.Controllers
{
    public class ChannelControllerTest
    {
        private IMapper mapper;

        public ChannelControllerTest()
        {
            mapper = Utils.CreateMapper();
        }

        [Fact]        
        public void Details()
        {
            // Arrange
            var id = 20;
            var name = "Kanal";
            var scenarios = new List<NcScenario>();

            var channel = new NcChannel { Id = id, Name = name };            
            var mock = new Mock<IChannelManager>();
            mock.Setup(x => x.FindById(id)).Returns(channel);
            mock.Setup(x => x.GetScenariosByChannelId(id)).Returns(scenarios);

            var expected = new ChannelViewModel
            {
                Id = id,
                Name = name,
                Scenarios = mapper.MapEnumerable<ScenarioListDTO>(scenarios).ToList(),
            }.ToExpectedObject();

            // Act
            var controller = new ChannelsController(mock.Object, mapper);
            var result = controller.Details(id);

            // Assert
            var actual = (result as ViewResult)?.Model;

            expected.ShouldEqual(actual);
        }
    }
}
