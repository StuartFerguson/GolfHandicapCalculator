namespace ManagementAPI.Service.IntegrationTests.Common
{
    using System;
    using DataTransferObjects.Requests;

    public static class TestData
    {
        private static String ConfirmPassword = "123456";

        private static String EmailAddress = "testclubadministrator@golfclub.co.uk";

        private static String TelephoneNumber = "1234567890";

        private static String FamilyName = "User";

        private static String GivenName = "Test";

        private static String MiddleName = String.Empty;

        private static String Password = "123456";

        public static RegisterClubAdministratorRequest RegisterClubAdministratorRequest = new RegisterClubAdministratorRequest
                                                                                          {
                                                                                              ConfirmPassword = TestData.ConfirmPassword,
                                                                                              EmailAddress = TestData.EmailAddress,
                                                                                              TelephoneNumber = TestData.TelephoneNumber,
                                                                                              FamilyName = TestData.FamilyName,
                                                                                              GivenName = TestData.GivenName,
                                                                                              MiddleName = TestData.MiddleName,
                                                                                              Password = TestData.Password
                                                                                          };

        public static Guid GolfClubAdministratorUserId = Guid.Parse("0A05AD53-F7B1-4A18-A697-816DD32714B7");
    }
}