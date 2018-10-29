using System;
using System.ComponentModel.DataAnnotations;
using ManagementAPI.Service.Commands;
using ManagementAPI.Service.DataTransferObjects;

namespace ManagementAPI.Service.Tests
{
    public class ClubConfigurationTestData
    {
        public static CreateClubConfigurationRequest CreateClubConfigurationRequest = new CreateClubConfigurationRequest
        {
            Name = "Name",
            AddressLine1 = "Address Line 1",
            EmailAddress = "1@2.com",
            PostalCode = "TE57 1NG",
            Town = "Test Town",
            Website = "www.website.com",
            Region = "Test Region",
            TelephoneNumber = "123456789",
            AddressLine2 = "Address Line 2"
        };

        public static CreateClubConfigurationCommand GetCreateClubConfigurationCommand()
        {
            
            return CreateClubConfigurationCommand.Create(ClubConfigurationTestData.CreateClubConfigurationRequest);
        }

        public static Guid AggregateId = Guid.Parse("CD64A469-9593-49D6-988D-3842C532D23E");
        public static String Name = "Test Club Name";
        public static String AddressLine1 = "Address Line 1";
        public static String AddressLine2 = "Address Line 2";
        public static String Town = "Test Town";
        public static String Region = "Test Region";
        public static String PostalCode = "TE57 1NG";
        public static String TelephoneNumber = "123456789";
        public static String EmailAddress = "1@2.com";
        public static String Website = "www.website.com";

        public static ClubConfigurationAggregate.ClubConfigurationAggregate GetEmptyClubConfigurationAggregate()
        {
            ClubConfigurationAggregate.ClubConfigurationAggregate aggregate= ClubConfigurationAggregate.ClubConfigurationAggregate.Create(AggregateId);

            return aggregate;
        }
    }
}