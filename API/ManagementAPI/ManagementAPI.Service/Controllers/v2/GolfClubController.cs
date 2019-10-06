using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementAPI.Service.Controllers.v2
{
    using System.Security.Claims;
    using System.Threading;
    using BusinessLogic.Commands;
    using BusinessLogic.Manager;
    using Common;
    using Common.v2;
    using DataTransferObjects.Requests;
    using IdentityModel;
    using ManagementAPI.Service.DataTransferObjects.Responses;
    using ManagementAPI.Service.DataTransferObjects.Responses.v2;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Shared.CommandHandling;
    using Swashbuckle.AspNetCore.Annotations;
    using Swashbuckle.AspNetCore.Filters;
    using CreateGolfClubResponseExample = Common.CreateGolfClubResponseExample;
    using CreateGolfClubResponsev2 = DataTransferObjects.Responses.v2.CreateGolfClubResponse;
    using GetGolfClubResponsev1 = DataTransferObjects.Responses.GetGolfClubResponse;
    using GetGolfClubResponsev2 = DataTransferObjects.Responses.v2.GetGolfClubResponse;
    using GolfClubUserResponse = DataTransferObjects.Responses.v2.GolfClubUserResponse;
    using MeasuredCourseListResponse = DataTransferObjects.Responses.v2.MeasuredCourseListResponse;
    using MembershipStatusv2 = DataTransferObjects.Responses.v2.MembershipStatus;
    using GetGolfClubResponseExamplev2 = Common.v2.GetGolfClubResponseExample;
    using CreateGolfClubResponseExamplev2 = Common.v2.CreateGolfClubResponseExample;

    [Route(GolfClubController.ControllerRoute)]
    [Authorize]
    [ApiController]
    [ApiVersion("2.0")]
    public class GolfClubController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The command router
        /// </summary>
        private readonly ICommandRouter CommandRouter;

        /// <summary>
        /// The manager
        /// </summary>
        private readonly IManagmentAPIManager Manager;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Controllers.GolfClubController" /> class.
        /// </summary>
        /// <param name="commandRouter">The command router.</param>
        /// <param name="manager">The manager.</param>
        public GolfClubController(ICommandRouter commandRouter,
                                  IManagmentAPIManager manager)
        {
            this.CommandRouter = commandRouter;
            this.Manager = manager;
        }

        #endregion

        /// <summary>
        /// Creates the golf club.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(201, type: typeof(CreateGolfClubResponsev2))]
        [SwaggerResponseExample(201, typeof(CreateGolfClubResponseExample), jsonConverter: typeof(SwaggerJsonConverter))]
        [Route("")]
        public async Task<IActionResult> CreateGolfClub([FromBody] CreateGolfClubRequest request,
                                                        CancellationToken cancellationToken)
        {
            if (ClaimsHelper.IsPasswordToken(this.User) == false)
            {
                return this.Forbid();
            }

            // Get the user id (subject) for the user
            Claim subjectIdClaim = ClaimsHelper.GetUserClaim(this.User, JwtClaimTypes.Subject);

            // Get the Golf Club Id claim from the user            
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId);

            Guid golfClubId = Guid.Parse(golfClubIdClaim.Value);
            Guid securityUserId = Guid.Parse(subjectIdClaim.Value);

            // Create the command
            CreateGolfClubCommand command = CreateGolfClubCommand.Create(golfClubId, securityUserId, request);

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.Created($"{GolfClubController.ControllerRoute}/{golfClubId}", new CreateGolfClubResponsev2
                                                                                      {
                                                                                          GolfClubId = golfClubId
                                                                                      });
        }

        /// <summary>
        /// Gets the golf club.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="includeMembers">if set to <c>true</c> [include members].</param>
        /// <param name="includeMeasuredCourses">if set to <c>true</c> [include measured courses].</param>
        /// <param name="includeUsers">if set to <c>true</c> [include users].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(200, type: typeof(GetGolfClubResponsev2))]
        [SwaggerResponseExample(200, typeof(GetGolfClubResponseExamplev2), jsonConverter: typeof(SwaggerJsonConverter))]
        [Route("{golfClubId}")]
        public async Task<IActionResult> GetGolfClub([FromRoute] Guid golfClubId,
                                                     [FromQuery] Boolean includeMembers,
                                                     [FromQuery] Boolean includeMeasuredCourses,
                                                     [FromQuery] Boolean includeUsers,
                                                     CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId, golfClubId);

            Boolean validationResult = ClaimsHelper.ValidateRouteParameter(golfClubId, golfClubIdClaim);
            if (validationResult == false)
            {
                return this.Forbid();
            }

            GetGolfClubResponsev1 getGolfClubResponsev1 = await this.Manager.GetGolfClub(Guid.Parse(golfClubIdClaim.Value), cancellationToken);

            List<GetGolfClubMembershipDetailsResponse> membersList = null;
            if (includeMembers)
            {
                membersList = await this.Manager.GetGolfClubMembersList(Guid.Parse(golfClubIdClaim.Value), cancellationToken);
            }

            GetMeasuredCourseListResponse measuredCourseList = null;
            if (includeMeasuredCourses)
            {
                 measuredCourseList = await this.Manager.GetMeasuredCourseList(Guid.Parse(golfClubIdClaim.Value), cancellationToken);
            }

            GetGolfClubUserListResponse users = null;

            if (includeUsers)
            {
                users = await this.Manager.GetGolfClubUsers(Guid.Parse(golfClubIdClaim.Value), cancellationToken);
            }

            GetGolfClubResponsev2 getGolfClubResponsev2 = this.ConvertGetGolfClubResponse(getGolfClubResponsev1, membersList, measuredCourseList, users);

            return this.Ok(getGolfClubResponsev2);
        }

        /// <summary>
        /// Gets the golf club list.
        /// </summary>
        /// <param name="includeMembers">if set to <c>true</c> [include members].</param>
        /// <param name="includeMeasuredCourses">if set to <c>true</c> [include measured courses].</param>
        /// <param name="includeUsers">if set to <c>true</c> [include users].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(200, type: typeof(List<GetGolfClubResponsev2>))]
        [SwaggerResponseExample(200, typeof(List<GetGolfClubResponseExamplev2>), jsonConverter: typeof(SwaggerJsonConverter))]
        [Route("")]
        public async Task<IActionResult> GetGolfClubList([FromQuery] Boolean includeMembers,
                                                     [FromQuery] Boolean includeMeasuredCourses,
                                                     [FromQuery] Boolean includeUsers,
                                                     CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId, Guid.Empty);

            if (golfClubIdClaim.Value != Guid.Empty.ToString())
            {
                // A golf club user cannot be calling this method
                return this.Forbid();
            }

            List<GetGolfClubResponsev1> golfClubList = await this.Manager.GetGolfClubList(cancellationToken);

            List< GetGolfClubResponsev2> response = new List<GetGolfClubResponsev2>();
            foreach (GetGolfClubResponsev1 getGolfClubResponse in golfClubList)
            {
                GetGolfClubResponsev2 getGolfClubResponsev2 = this.ConvertGetGolfClubResponse(getGolfClubResponse, null, null, null);
                response.Add(getGolfClubResponsev2);
            }
            
            return this.Ok(response);
        }

        /// <summary>
        /// Adds the measured course to golf club.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{golfClubId}/measuredcourses")]
        [SwaggerResponse(201, type: typeof(AddMeasuredCourseToClubResponse))]
        [SwaggerResponseExample(200, typeof(AddMeasuredCourseToClubResponseExample), jsonConverter: typeof(SwaggerJsonConverter))]
        public async Task<IActionResult> AddMeasuredCourseToGolfClub([FromRoute] Guid golfClubId,
                                                                     [FromBody] AddMeasuredCourseToClubRequest request,
                                                                     CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId, golfClubId);

            Boolean validationResult = ClaimsHelper.ValidateRouteParameter(golfClubId, golfClubIdClaim);
            if (validationResult == false)
            {
                return this.Forbid();
            }

            Guid golfClubIdValue = Guid.Parse(golfClubIdClaim.Value);
            Guid measuredCourseId = Guid.NewGuid();

            // Create the command
            AddMeasuredCourseToClubCommand command = AddMeasuredCourseToClubCommand.Create(golfClubIdValue, measuredCourseId, request);

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.Created($"{GolfClubController.ControllerRoute}/{golfClubId}/measuredcourses/{measuredCourseId}",
                                new AddMeasuredCourseToClubResponse
                                {
                                    GolfClubId = golfClubIdValue,
                                    MeasuredCourseId = measuredCourseId
                                });
        }

        /// <summary>
        /// Creates the match secretary.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{golfClubId}/users")]
        [SwaggerResponse(201, type: typeof(CreateMatchSecretaryResponse))]
        [SwaggerResponseExample(201, typeof(CreateMatchSecretaryResponseExample), jsonConverter: typeof(SwaggerJsonConverter))]
        public async Task<IActionResult> CreateMatchSecretary([FromRoute] Guid golfClubId,
                                                              [FromBody] CreateMatchSecretaryRequest request,
                                                              CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId, golfClubId);

            Boolean validationResult = ClaimsHelper.ValidateRouteParameter(golfClubId, golfClubIdClaim);
            if (validationResult == false)
            {
                return this.Forbid();
            }

            CreateMatchSecretaryCommand command = CreateMatchSecretaryCommand.Create(Guid.Parse(golfClubIdClaim.Value), request);

            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.Created($"{GolfClubController.ControllerRoute}/{golfClubId}/users/{request.EmailAddress}",
                                new CreateMatchSecretaryResponse
                                {
                                    GolfClubId = golfClubId,
                                    UserName = request.EmailAddress
                                });
        }

        [HttpPost]
        [Route("{golfClubId}/tournamentdivisions")]
        [SwaggerResponse(201, type: typeof(AddTournamentDivisionToGolfClubResponse))]
        [SwaggerResponseExample(201, typeof(AddTournamentDivisionToGolfClubResponseExample), jsonConverter: typeof(SwaggerJsonConverter))]
        public async Task<IActionResult> AddTournamentDivision([FromRoute] Guid golfClubId,
                                                               [FromBody] AddTournamentDivisionToGolfClubRequest request,
                                                               CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user            
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId, golfClubId);

            Boolean validationResult = ClaimsHelper.ValidateRouteParameter(golfClubId, golfClubIdClaim);
            if (validationResult == false)
            {
                return this.Forbid();
            }

            // Create the command
            AddTournamentDivisionToGolfClubCommand command = AddTournamentDivisionToGolfClubCommand.Create(Guid.Parse(golfClubIdClaim.Value), request);

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.Created($"{GolfClubController.ControllerRoute}/{golfClubId}/tournamentdivisions/{request.Division}",
                                new AddTournamentDivisionToGolfClubResponse
                                {
                                    GolfClubId = golfClubId,
                                    TournamentDivision = request.Division
                                });
        }

        private GetGolfClubResponsev2 ConvertGetGolfClubResponse(GetGolfClubResponsev1 getGolfClubResponsev1,
                                                  List<GetGolfClubMembershipDetailsResponse> membersList,
                                                  GetMeasuredCourseListResponse measuredCourseList,
                                                  GetGolfClubUserListResponse users)
        {
            GetGolfClubResponsev2 response = new GetGolfClubResponsev2
                                             {
                                                 TelephoneNumber = getGolfClubResponsev1.TelephoneNumber,
                                                 EmailAddress = getGolfClubResponsev1.EmailAddress,
                                                 Name = getGolfClubResponsev1.Name,
                                                 AddressLine1 = getGolfClubResponsev1.AddressLine1,
                                                 AddressLine2 = getGolfClubResponsev1.AddressLine2,
                                                 Id = getGolfClubResponsev1.Id,
                                                 PostalCode = getGolfClubResponsev1.PostalCode,
                                                 Region = getGolfClubResponsev1.Region,
                                                 Town = getGolfClubResponsev1.Town,
                                                 Website = getGolfClubResponsev1.Website,
                                             };

            if (membersList != null)
            {
                response.GolfClubMembershipDetailsResponseList = new List<GolfClubMembershipDetailsResponse>();

                foreach (GetGolfClubMembershipDetailsResponse getGolfClubMembershipDetailsResponse in membersList)
                {
                    response.GolfClubMembershipDetailsResponseList.Add(new GolfClubMembershipDetailsResponse
                                                                       {
                                                                           MembershipStatus = (MembershipStatusv2)getGolfClubMembershipDetailsResponse.MembershipStatus,
                                                                           PlayerId = getGolfClubMembershipDetailsResponse.PlayerId,
                                                                           Name = getGolfClubMembershipDetailsResponse.Name,
                                                                           MembershipNumber = getGolfClubMembershipDetailsResponse.MembershipNumber,
                                                                           PlayerDateOfBirth = getGolfClubMembershipDetailsResponse.PlayerDateOfBirth,
                                                                           PlayerFullName = getGolfClubMembershipDetailsResponse.PlayerFullName,
                                                                           PlayerGender = getGolfClubMembershipDetailsResponse.PlayerGender
                    });
                }
            }

            if (measuredCourseList != null)
            {
                response.MeasuredCourses = new List<MeasuredCourseListResponse>();

                foreach (DataTransferObjects.Responses.MeasuredCourseListResponse measuredCourseListResponse in measuredCourseList.MeasuredCourses)
                {
                    response.MeasuredCourses.Add(new MeasuredCourseListResponse
                                                 {
                                                     MeasuredCourseId = measuredCourseListResponse.MeasuredCourseId,
                                                     Name = measuredCourseListResponse.Name,
                                                     TeeColour = measuredCourseListResponse.TeeColour,
                                                     StandardScratchScore = measuredCourseListResponse.StandardScratchScore
                    });
                }
            }

            if (users != null)
            {
                response.Users = new List<GolfClubUserResponse>();

                foreach (DataTransferObjects.Responses.GolfClubUserResponse user in users.Users)
                {
                    response.Users.Add(new GolfClubUserResponse
                                       {
                        UserId = user.UserId,
                        FamilyName = user.FamilyName,
                        GivenName = user.GivenName,
                        MiddleName = user.MiddleName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        UserName = user.UserName,
                        UserType = user.UserType
                                       });
                }
            }

            return response;
        }

        #region Others

        /// <summary>
        /// The controller name
        /// </summary>
        public const String ControllerName = "golfclubs";

        /// <summary>
        /// The controller route
        /// </summary>
        private const String ControllerRoute = "api/" + GolfClubController.ControllerName;

        #endregion

    }
}
