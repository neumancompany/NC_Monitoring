using NC_Monitoring.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace NC_Monitoring.Test.App
{
    public class ViewModelsTest
    {
        [Fact]
        public void MonitorFormViewModel_Url_Required()        
        {
            // arrange
            var checkMemberName = nameof(MonitorFormViewModel.Url);
            // act
            var actual = Utils.ValidateModel(new MonitorFormViewModel
            {
                Id = Guid.NewGuid(),
                Name = "m",
                VerificationValue = "2"
            });

            // assert
            Assert.Contains(actual, x =>x.MemberNames.Contains(checkMemberName));
        }

        [Theory]        
        [InlineData("www.test.cz", false)]
        [InlineData("test.cz", false)]        
        [InlineData("http://test.cz", true)]
        [InlineData("http://www.test.cz", true)]
        public void MonitorFormViewModel_Url_Format(string url, bool isValidFormat)
        {
            // arrange
            var checkMemberName = nameof(MonitorFormViewModel.Url);
            // act
            var actual = Utils.ValidateModel(new MonitorFormViewModel
            {
                Id = Guid.NewGuid(),
                Name = "m",
                VerificationValue = "2",
                Url = url,
            });

            // assert

            if (isValidFormat)
            {
                Assert.DoesNotContain(actual, x => x.MemberNames.Contains(checkMemberName));
            }
            else
            {
                Assert.Contains(actual, x => x.MemberNames.Contains(checkMemberName));
            }
        }
    }
}
