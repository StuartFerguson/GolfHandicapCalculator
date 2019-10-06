namespace ManagementAPI.Service.Tests.General
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using Common;
    using IdentityModel;
    using Shouldly;
    using Xunit;

    public class ClaimsHelperTests
    {
        [Fact]
        public void ClaimsHelper_GetUserClaim_ClaimReturned()
        {
            List<Claim> claims = new List<Claim>
                                 {
                                     new Claim(JwtClaimTypes.Subject, "AF821DDC-75CD-4108-8256-EAD9C62182FA"),
                                     new Claim("testclaim", "testclaimvalue")
                                 };
                
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
            
            ClaimsPrincipal claimsPrincipal =new ClaimsPrincipal(claimsIdentity);

            Claim claim = ClaimsHelper.GetUserClaim(claimsPrincipal, "testclaim");

            claim.ShouldNotBeNull();
        }

        [Fact]
        public void ClaimsHelper_GetUserClaim_ClaimNotFound_DefaultReturned()
        {
            List<Claim> claims = new List<Claim>
                                 {
                                     new Claim(JwtClaimTypes.Subject, "AF821DDC-75CD-4108-8256-EAD9C62182FA"),
                                 };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            Claim result = ClaimsHelper.GetUserClaim(claimsPrincipal, "testclaim", Guid.Empty.ToString());
            result.Value.ShouldBe(Guid.Empty.ToString());
        }
    }
}