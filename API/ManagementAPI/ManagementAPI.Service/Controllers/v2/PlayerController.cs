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
    using DataTransferObjects.Responses;
    using DataTransferObjects.Responses.v2;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Shared.CommandHandling;
    using Swashbuckle.AspNetCore.Annotations;
    using Swashbuckle.AspNetCore.Filters;
    using ClubMembershipResponse = DataTransferObjects.Responses.ClubMembershipResponse;
    using RegisterPlayerResponse = DataTransferObjects.Responses.v2.RegisterPlayerResponse;
    using RegisterPlayerResponseExample = Common.v2.RegisterPlayerResponseExample;
    using TournamentFormat = DataTransferObjects.Responses.v2.TournamentFormat;

    [Route(PlayerController.ControllerRoute)]
    [Authorize]
    [ApiController]
    [ApiVersion("2.0")]
    public class PlayerController : ControllerBase
    {
        #region Others

        /// <summary>
        /// The controller name
        /// </summary>
        public const String ControllerName = "players";

        /// <summary>
        /// The controller route
        /// </summary>
        private const String ControllerRoute = "api/" + PlayerController.ControllerName;

        #endregion

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
        public PlayerController(ICommandRouter commandRouter,
                                IManagmentAPIManager manager)
        {
            this.CommandRouter = commandRouter;
            this.Manager = manager;
        }

        #endregion

        [HttpPost]
        [SwaggerResponse(201, type: typeof(RegisterPlayerResponse))]
        [SwaggerResponseExample(201, typeof(RegisterPlayerResponseExample), jsonConverter: typeof(SwaggerJsonConverter))]
        [Route("")]
        [AllowAnonymous]
        public async Task<IActionResult> CreatePlayer([FromBody] RegisterPlayerRequest request,
                                                      CancellationToken cancellationToken)
        {
            Guid playerId = Guid.NewGuid();

            // Create the command
            RegisterPlayerCommand command = RegisterPlayerCommand.Create(playerId, request);

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.Created($"{PlayerController.ControllerRoute}/{playerId}", new RegisterPlayerResponse
                                                                                      {
                                                                                          PlayerId = playerId
                                                                                      });
        }

        [HttpGet]
        [SwaggerResponse(200, type:typeof(GetPlayerResponse))]
        [SwaggerResponseExample(200, typeof(GetPlayerResponseExample), jsonConverter:typeof(SwaggerJsonConverter))]
        [Route("{playerId}")]
        public async Task<IActionResult> GetPlayer([FromRoute] Guid playerId,
                                                     [FromQuery] Boolean includeMemberships,
                                                     [FromQuery] Boolean includeTournamentSignups,
                                                     CancellationToken cancellationToken)
        {
            // Get the Player Id claim from the user            
            Claim playerIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.PlayerId, playerId.ToString());

            Boolean validationResult = ClaimsHelper.ValidateRouteParameter(playerId, playerIdClaim);
            if (validationResult == false)
            {
                return this.Forbid();
            }

            GetPlayerDetailsResponse playerDetails = await this.Manager.GetPlayerDetails(Guid.Parse(playerIdClaim.Value), cancellationToken);

            List<ClubMembershipResponse> membershipList = null;
            if (includeMemberships)
            {
                membershipList = await this.Manager.GetPlayersClubMemberships(Guid.Parse(playerIdClaim.Value), cancellationToken);
            }

            PlayerSignedUpTournamentsResponse signedUpTournaments = null;
            if (includeTournamentSignups)
            {
                signedUpTournaments = await this.Manager.GetPlayerSignedUpTournaments(Guid.Parse(playerIdClaim.Value), cancellationToken);
            }

            GetPlayerResponse playerResponse = this.ConvertGetPlayerDetailsResponse(playerId, playerDetails, membershipList, signedUpTournaments);

            return this.Ok(playerResponse);
        }

        [HttpPut]
        [SwaggerResponse(200)]
        [Route("{playerId}/tournaments/{tournamentId}")]
        public async Task<IActionResult> SignInForTournament([FromRoute] Guid playerId,
                                                             [FromRoute] Guid tournamentId,
                                                             CancellationToken cancellationToken)
        {
            // Get the Player Id claim from the user            
            Claim playerIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.PlayerId, playerId.ToString());

            Boolean validationResult = ClaimsHelper.ValidateRouteParameter(playerId, playerIdClaim);
            if (validationResult == false)
            {
                return this.Forbid();
            }

            // Create the command
            SignUpForTournamentCommand command = SignUpForTournamentCommand.Create(tournamentId, Guid.Parse(playerIdClaim.Value));

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.Ok();
        }

        [HttpPut]
        [SwaggerResponse(200)]
        [Route("{playerId}/tournaments/{tournamentId}/scores")]
        public async Task<IActionResult> RecordPlayerScore([FromRoute] Guid playerId,
                                                           [FromRoute] Guid tournamentId,
                                                           [FromBody] RecordPlayerTournamentScoreRequest request,
                                                           CancellationToken cancellationToken)
        {
            // Get the Player Id claim from the user            
            Claim playerIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.PlayerId, playerId.ToString());

            Boolean validationResult = ClaimsHelper.ValidateRouteParameter(playerId, playerIdClaim);
            if (validationResult == false)
            {
                return this.Forbid();
            }

            // Create the command
            RecordPlayerTournamentScoreCommand command = RecordPlayerTournamentScoreCommand.Create(Guid.Parse(playerIdClaim.Value), tournamentId, request);

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.Ok();
        }

        /// <summary>
        /// Converts the get player details response.
        /// </summary>
        /// <param name="playerDetails">The player details.</param>
        /// <returns></returns>
        private GetPlayerResponse ConvertGetPlayerDetailsResponse(Guid playerId, GetPlayerDetailsResponse playerDetails, List<ClubMembershipResponse> membershipList, PlayerSignedUpTournamentsResponse signedUpTournaments)
        {
            GetPlayerResponse result = null;

            result = new GetPlayerResponse
                     {
                         Id = playerId,
                         EmailAddress = playerDetails.EmailAddress,
                         PlayingHandicap = playerDetails.PlayingHandicap,
                         MiddleName = playerDetails.MiddleName,
                         DateOfBirth = playerDetails.DateOfBirth,
                         ExactHandicap = playerDetails.ExactHandicap,
                         FirstName = playerDetails.FirstName,
                         FullName = playerDetails.FullName,
                         Gender = playerDetails.Gender,
                         HandicapCategory = playerDetails.HandicapCategory,
                         HasBeenRegistered = playerDetails.HasBeenRegistered,
                         LastName = playerDetails.LastName
                     };

            if (membershipList != null)
            {
                result.ClubMemberships = new List<DataTransferObjects.Responses.v2.ClubMembershipResponse>();

                foreach (ClubMembershipResponse clubMembershipResponse in membershipList)
                {
                    result.ClubMemberships.Add(new DataTransferObjects.Responses.v2.ClubMembershipResponse
                                               {
                                                   GolfClubId = clubMembershipResponse.GolfClubId,
                                                   GolfClubName = clubMembershipResponse.GolfClubName,
                                                   AcceptedDateTime = clubMembershipResponse.AcceptedDateTime,
                                                   MembershipId = clubMembershipResponse.MembershipId,
                                                   MembershipNumber = clubMembershipResponse.MembershipNumber,
                                                   RejectedDateTime = clubMembershipResponse.RejectedDateTime,
                                                   RejectionReason = clubMembershipResponse.RejectionReason,
                                                   Status = (DataTransferObjects.Responses.v2.MembershipStatus)clubMembershipResponse.Status,
                                               });
                }
            }

            if (signedUpTournaments != null)
            {
                result.SignedUpTournaments = new List<SignedUpTournamentResponse>();

                foreach (PlayerSignedUpTournament playerSignedUpTournament in signedUpTournaments.PlayerSignedUpTournaments)
                {
                    result.SignedUpTournaments.Add(new SignedUpTournamentResponse
                                                   {
                                                       GolfClubId = playerSignedUpTournament.GolfClubId,
                                                       MeasuredCourseId = playerSignedUpTournament.MeasuredCourseId,
                                                       TournamentFormat = (TournamentFormat)playerSignedUpTournament.TournamentFormat,
                                                       PlayerId = playerSignedUpTournament.PlayerId,
                                                       TournamentDate = playerSignedUpTournament.TournamentDate,
                                                       MeasuredCourseName = playerSignedUpTournament.MeasuredCourseName,
                                                       GolfClubName = playerSignedUpTournament.GolfClubName,
                                                       TournamentId = playerSignedUpTournament.TournamentId,
                                                       MeasuredCourseTeeColour = playerSignedUpTournament.MeasuredCourseTeeColour,
                                                       TournamentName = playerSignedUpTournament.TournamentName,
                                                       ScoreEntered = playerSignedUpTournament.ScoreEntered
                    });
                }
            }

            return result;
        }
    }
}
