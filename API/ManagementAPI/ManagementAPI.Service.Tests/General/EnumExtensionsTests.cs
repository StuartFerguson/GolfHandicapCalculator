namespace ManagementAPI.Service.Tests.General
{
    using System;
    using Common;
    using Shouldly;
    using Xunit;

    public class EnumExtensionsTests
    {
        private enum SourceEnum
        {
            NotSet = 0,
            Value1 = 1,
            Value2 =2
        }

        private enum DestinationEnum
        {
            NotSet = 0,
            Value1 = 1,
            Value2 = 2
        }

        [Fact]
        public void EnumExtensions_ConvertTo_SourceConvertedToDestination()
        {
            SourceEnum source = SourceEnum.Value1;

            DestinationEnum destination = source.ConvertTo<DestinationEnum>();

            destination.ShouldBe(DestinationEnum.Value1);
        }

        [Fact]
        public void EnumExtensions_ConvertTo_SourceNotEnum_ErrorThrown()
        {
            String source = "Source";

            Should.Throw<ArgumentException>(() =>
                                            {
                                                DestinationEnum destination = source.ConvertTo<DestinationEnum>();
                                            });
        }

        [Fact]
        public void EnumExtensions_ConvertTo_DestinationNotEnum_ErrorThrown()
        {
            SourceEnum source = SourceEnum.Value1;

            Should.Throw<ArgumentException>(() =>
                                            {
                                                Int32 destination = source.ConvertTo<Int32>();
                                            });
        }
    }
}