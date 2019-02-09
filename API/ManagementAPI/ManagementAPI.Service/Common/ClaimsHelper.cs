using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;

namespace ManagementAPI.Service.Common
{
    public class ClaimsHelper
    {
        #region public static Claim GetUserClaim(ClaimsPrincipal user, String customClaimType)        
        /// <summary>
        /// Gets the user claims.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="customClaimType">Type of the custom claim.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">No claim [{customClaimType}] found for user id [{userIdClaim.Value}</exception>
        public static Claim GetUserClaim(ClaimsPrincipal user, String customClaimType)
        {
            // Get the user id (subject claim). Note: This will ALWAYS be present
            Claim userIdClaim = user.Claims.Single(c => c.Type == JwtClaimTypes.Subject);

            // Get the claim from the token
            Claim userClaim = null;

            try
            {
                userClaim = user.Claims.Single(c => c.Type == customClaimType);
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException($"No claim [{customClaimType}] found for user id [{userIdClaim.Value}");
            }

            return userClaim;
        }
        #endregion

    }
}
