namespace DataGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using ManagementAPI.Service.Client;
    using ManagementAPI.Service.DataTransferObjects;
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    internal class Program
    {
        #region Fields

        /// <summary>
        /// The add measured course to club request
        /// </summary>
        public static AddMeasuredCourseToClubRequest AddMeasuredCourseToClubRequest = new AddMeasuredCourseToClubRequest
                                                                                      {
                                                                                          MeasuredCourseId = Guid.NewGuid(),
                                                                                          Name = "Test Course",
                                                                                          StandardScratchScore = 70,
                                                                                          TeeColour = "White",
                                                                                          Holes = new List<HoleDataTransferObject>
                                                                                                  {
                                                                                                      new HoleDataTransferObject
                                                                                                      {
                                                                                                          HoleNumber = 1,
                                                                                                          LengthInYards = 348,
                                                                                                          Par = 4,
                                                                                                          StrokeIndex = 10
                                                                                                      },
                                                                                                      new HoleDataTransferObject
                                                                                                      {
                                                                                                          HoleNumber = 2,
                                                                                                          LengthInYards = 402,
                                                                                                          Par = 4,
                                                                                                          StrokeIndex = 4
                                                                                                      },
                                                                                                      new HoleDataTransferObject
                                                                                                      {
                                                                                                          HoleNumber = 3,
                                                                                                          LengthInYards = 207,
                                                                                                          Par = 3,
                                                                                                          StrokeIndex = 14
                                                                                                      },
                                                                                                      new HoleDataTransferObject
                                                                                                      {
                                                                                                          HoleNumber = 4,
                                                                                                          LengthInYards = 405,
                                                                                                          Par = 4,
                                                                                                          StrokeIndex = 8
                                                                                                      },
                                                                                                      new HoleDataTransferObject
                                                                                                      {
                                                                                                          HoleNumber = 5,
                                                                                                          LengthInYards = 428,
                                                                                                          Par = 4,
                                                                                                          StrokeIndex = 2
                                                                                                      },
                                                                                                      new HoleDataTransferObject
                                                                                                      {
                                                                                                          HoleNumber = 6,
                                                                                                          LengthInYards = 477,
                                                                                                          Par = 5,
                                                                                                          StrokeIndex = 12
                                                                                                      },
                                                                                                      new HoleDataTransferObject
                                                                                                      {
                                                                                                          HoleNumber = 7,
                                                                                                          LengthInYards = 186,
                                                                                                          Par = 4,
                                                                                                          StrokeIndex = 16
                                                                                                      },
                                                                                                      new HoleDataTransferObject
                                                                                                      {
                                                                                                          HoleNumber = 8,
                                                                                                          LengthInYards = 397,
                                                                                                          Par = 4,
                                                                                                          StrokeIndex = 6
                                                                                                      },
                                                                                                      new HoleDataTransferObject
                                                                                                      {
                                                                                                          HoleNumber = 9,
                                                                                                          LengthInYards = 130,
                                                                                                          Par = 3,
                                                                                                          StrokeIndex = 18
                                                                                                      },
                                                                                                      new HoleDataTransferObject
                                                                                                      {
                                                                                                          HoleNumber = 10,
                                                                                                          LengthInYards = 399,
                                                                                                          Par = 4,
                                                                                                          StrokeIndex = 3
                                                                                                      },
                                                                                                      new HoleDataTransferObject
                                                                                                      {
                                                                                                          HoleNumber = 11,
                                                                                                          LengthInYards = 401,
                                                                                                          Par = 4,
                                                                                                          StrokeIndex = 13
                                                                                                      },
                                                                                                      new HoleDataTransferObject
                                                                                                      {
                                                                                                          HoleNumber = 12,
                                                                                                          LengthInYards = 421,
                                                                                                          Par = 4,
                                                                                                          StrokeIndex = 1
                                                                                                      },
                                                                                                      new HoleDataTransferObject
                                                                                                      {
                                                                                                          HoleNumber = 13,
                                                                                                          LengthInYards = 530,
                                                                                                          Par = 5,
                                                                                                          StrokeIndex = 11
                                                                                                      },
                                                                                                      new HoleDataTransferObject
                                                                                                      {
                                                                                                          HoleNumber = 14,
                                                                                                          LengthInYards = 196,
                                                                                                          Par = 3,
                                                                                                          StrokeIndex = 5
                                                                                                      },
                                                                                                      new HoleDataTransferObject
                                                                                                      {
                                                                                                          HoleNumber = 15,
                                                                                                          LengthInYards = 355,
                                                                                                          Par = 4,
                                                                                                          StrokeIndex = 7
                                                                                                      },
                                                                                                      new HoleDataTransferObject
                                                                                                      {
                                                                                                          HoleNumber = 16,
                                                                                                          LengthInYards = 243,
                                                                                                          Par = 4,
                                                                                                          StrokeIndex = 15
                                                                                                      },
                                                                                                      new HoleDataTransferObject
                                                                                                      {
                                                                                                          HoleNumber = 17,
                                                                                                          LengthInYards = 286,
                                                                                                          Par = 4,
                                                                                                          StrokeIndex = 17
                                                                                                      },
                                                                                                      new HoleDataTransferObject
                                                                                                      {
                                                                                                          HoleNumber = 18,
                                                                                                          LengthInYards = 399,
                                                                                                          Par = 4,
                                                                                                          StrokeIndex = 9
                                                                                                      }
                                                                                                  }
                                                                                      };

        /// <summary>
        /// The golf clubs
        /// </summary>
        private static readonly List<GolfClubDetails> GolfClubs = new List<GolfClubDetails>();

        /// <summary>
        /// The HTTP client
        /// </summary>
        private static readonly HttpClient HttpClient = new HttpClient();

        /// <summary>
        /// The players
        /// </summary>
        private static readonly List<PlayerDetails> Players = new List<PlayerDetails>();

        #endregion

        #region Methods

        /// <summary>
        /// Gets the response object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseMessage">The response message.</param>
        /// <returns></returns>
        protected static async Task<T> GetResponseObject<T>(HttpResponseMessage responseMessage)
        {
            T result = default;

            result = JsonConvert.DeserializeObject<T>(await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false));

            return result;
        }

        /// <summary>
        /// Creates the golf club data.
        /// </summary>
        /// <param name="numberOfClubs">The number of clubs.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private static async Task CreateGolfClubData(Int32 numberOfClubs,
                                                     CancellationToken cancellationToken)
        {
            String BaseAddressResolver(String service)
            {
                return Program.BaseAddress;
            }

            IGolfClubClient golfClubClient = new GolfClubClient(BaseAddressResolver, Program.HttpClient);

            for (Int32 i = 0; i < numberOfClubs; i++)
            {
                RegisterClubAdministratorRequest registerClubAdministratorRequest = new RegisterClubAdministratorRequest
                                                                                    {
                                                                                        EmailAddress = $"clubadministrator@testgolfclub{i}.co.uk",
                                                                                        ConfirmPassword = "123456",
                                                                                        Password = "123456",
                                                                                        TelephoneNumber = "1234567890"
                                                                                    };

                await golfClubClient.RegisterGolfClubAdministrator(registerClubAdministratorRequest, cancellationToken);

                String passwordToken = await Program.GetToken(TokenType.Password,
                                                              "developerClient",
                                                              "developerClient",
                                                              registerClubAdministratorRequest.EmailAddress,
                                                              registerClubAdministratorRequest.Password,
                                                              new List<String>
                                                              {
                                                                  "openid",
                                                                  "profile",
                                                                  "managementapi"
                                                              });

                CreateGolfClubRequest createGolfClubRequest = new CreateGolfClubRequest
                                                              {
                                                                  AddressLine1 = "Address Line 1",
                                                                  AddressLine2 = "Address Line 2",
                                                                  EmailAddress = $"contactus@testgolfclub{i}.co.uk",
                                                                  TelephoneNumber = "1234567890",
                                                                  Name = $"Test Golf Club {i}",
                                                                  PostalCode = "TE57 1NG",
                                                                  Region = "TestRegion",
                                                                  Town = "TestTown",
                                                                  Website = string.Empty
                                                              };

                CreateGolfClubResponse createGolfClubResponse = await golfClubClient.CreateGolfClub(passwordToken, createGolfClubRequest, cancellationToken);

                await golfClubClient.AddMeasuredCourseToGolfClub(passwordToken, Program.AddMeasuredCourseToClubRequest, cancellationToken);

                Program.GolfClubs.Add(new GolfClubDetails
                                      {
                                          AdminEmailAddress = registerClubAdministratorRequest.EmailAddress,
                                          AdminPassword = registerClubAdministratorRequest.Password,
                                          GolfClubId = createGolfClubResponse.GolfClubId,
                                          MeasuredCourseId = Program.AddMeasuredCourseToClubRequest.MeasuredCourseId,
                                          GolfClubName = createGolfClubRequest.Name
                                      });

                Console.WriteLine($"Created Golf Club {createGolfClubRequest.Name}");
            }
        }

        /// <summary>
        /// Creates the player data.
        /// </summary>
        /// <param name="numberPlayersPerClub">The number players per club.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private static async Task CreatePlayerData(Int32 numberPlayersPerClub,
                                                   CancellationToken cancellationToken)
        {
            String BaseAddressResolver(String service)
            {
                return Program.BaseAddress;
            }

            IGolfClubClient golfClubClient = new GolfClubClient(BaseAddressResolver, Program.HttpClient);
            IPlayerClient playerClient = new PlayerClient(BaseAddressResolver, Program.HttpClient);
            Int32 clubCounter = 0;
            foreach (GolfClubDetails golfClubDetails in Program.GolfClubs)
            {
                for (Int32 i = 0; i < numberPlayersPerClub; i++)
                {
                    RegisterPlayerRequest registerPlayerRequest = new RegisterPlayerRequest
                                                                  {
                                                                      DateOfBirth = DateTime.Now.AddYears(-30),
                                                                      EmailAddress = $"player{i}{clubCounter}@testplayer.co.uk",
                                                                      ExactHandicap = i,
                                                                      FirstName = $"Club {clubCounter}",
                                                                      Gender = "M",
                                                                      LastName = $"Test Player {i}",
                                                                      MiddleName = string.Empty
                                                                  };

                    RegisterPlayerResponse registerPlayerResponse = await playerClient.RegisterPlayer(registerPlayerRequest, cancellationToken);

                    Console.WriteLine($"Created Player {registerPlayerRequest.FirstName} {registerPlayerRequest.LastName}");

                    String passwordToken = await Program.GetToken(TokenType.Password,
                                                                  "developerClient",
                                                                  "developerClient",
                                                                  registerPlayerRequest.EmailAddress,
                                                                  "123456",
                                                                  new List<String>
                                                                  {
                                                                      "openid",
                                                                      "profile",
                                                                      "managementapi"
                                                                  });

                    await golfClubClient.RequestClubMembership(passwordToken, golfClubDetails.GolfClubId, cancellationToken);

                    Console.WriteLine($"Player {registerPlayerRequest.FirstName} {registerPlayerRequest.LastName} membership requested");

                    Program.Players.Add(new PlayerDetails
                                        {
                                            GolfClubId = golfClubDetails.GolfClubId,
                                            PlayerId = registerPlayerResponse.PlayerId,
                                            PlayingHandicap = Convert.ToInt32(registerPlayerRequest.ExactHandicap), // TODO: Rounding
                                            EmailAddress = registerPlayerRequest.EmailAddress
                                        });
                }

                clubCounter++;
            }
        }

        /// <summary>
        /// Creates the tournament data.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private static async Task CreateTournamentData(CancellationToken cancellationToken)
        {
            String BaseAddressResolver(String service)
            {
                return Program.BaseAddress;
            }

            ITournamentClient tournamentClient = new TournamentClient(BaseAddressResolver, Program.HttpClient);

            foreach (GolfClubDetails golfClubDetails in Program.GolfClubs)
            {
                List<CreateTournamentRequest> requests = Program.GetCreateTournamentRequests(golfClubDetails.MeasuredCourseId);

                String passwordToken = await Program.GetToken(TokenType.Password,
                                                              "developerClient",
                                                              "developerClient",
                                                              golfClubDetails.AdminEmailAddress,
                                                              golfClubDetails.AdminPassword,
                                                              new List<String>
                                                              {
                                                                  "openid",
                                                                  "profile",
                                                                  "managementapi"
                                                              });

                foreach (CreateTournamentRequest createTournamentRequest in requests)
                {
                    CreateTournamentResponse createTournamentResponse =
                        await tournamentClient.CreateTournament(passwordToken, createTournamentRequest, cancellationToken);

                    Console.WriteLine($"Tournament {createTournamentRequest.Name} created for Club {golfClubDetails.GolfClubName}");

                    // Get the players for this club
                    List<PlayerDetails> playerList = Program.Players.Where(p => p.GolfClubId == golfClubDetails.GolfClubId).ToList();

                    foreach (PlayerDetails player in playerList)
                    {
                        Int32 counter = 0;

                        ScoreResult scoreResult = ScoreResult.Under;
                        if (counter == 0)
                        {
                            scoreResult = ScoreResult.Under;
                            counter++;
                        }
                        else if (counter == 1)
                        {
                            scoreResult = ScoreResult.Buffer;
                            counter++;
                        }
                        else
                        {
                            scoreResult = ScoreResult.Over;
                            counter = 0;
                        }

                        RecordMemberTournamentScoreRequest recordMemberTournamentScoreRequest = new RecordMemberTournamentScoreRequest
                                                                                                {
                                                                                                    PlayingHandicap = player.PlayingHandicap,
                                                                                                    HoleScores =
                                                                                                        Program.GetHoleScores(player.PlayingHandicap, scoreResult)
                                                                                                };

                        String playerPasswordToken = await Program.GetToken(TokenType.Password,
                                                                            "developerClient",
                                                                            "developerClient",
                                                                            player.EmailAddress,
                                                                            "123456",
                                                                            new List<String>
                                                                            {
                                                                                "openid",
                                                                                "profile",
                                                                                "managementapi"
                                                                            });

                        await tournamentClient.RecordPlayerScore(playerPasswordToken,
                                                                 createTournamentResponse.TournamentId,
                                                                 recordMemberTournamentScoreRequest,
                                                                 cancellationToken);

                        Console.WriteLine($"Tournament Score Recorded for Player {player.EmailAddress} for Tournament {createTournamentRequest.Name} at Golf Club {golfClubDetails.GolfClubName}");
                    }
                }
            }
        }

        /// <summary>
        /// Determines the buffer zone.
        /// </summary>
        /// <param name="playingHandicap">The playing handicap.</param>
        /// <returns></returns>
        private static Int32 DetermineBufferZone(Decimal playingHandicap)
        {
            Int32 result = 0;

            if (playingHandicap < 5)
                result = 1;
            else if (playingHandicap >= 6 && playingHandicap <= 12)
                result = 2;
            else if (playingHandicap >= 13 && playingHandicap <= 20)
                result = 2;
            else if (playingHandicap >= 21 && playingHandicap <= 28)
                result = 4;
            else
                result = 5;

            return result;
        }

        /// <summary>
        /// Gets the create tournament requests.
        /// </summary>
        /// <param name="measuredCourseId">The measured course identifier.</param>
        /// <returns></returns>
        private static List<CreateTournamentRequest> GetCreateTournamentRequests(Guid measuredCourseId)
        {
            List<CreateTournamentRequest> result = new List<CreateTournamentRequest>();

            result.Add(new CreateTournamentRequest
                       {
                           Format = 1, // Strokeplay
                           Name = "April Medal",
                           MeasuredCourseId = measuredCourseId,
                           MemberCategory = 1, // Gents,
                           TournamentDate = new DateTime(DateTime.Now.Year, 4, 1)
                       });

            result.Add(new CreateTournamentRequest
                       {
                           Format = 1, // Strokeplay
                           Name = "May Medal",
                           MeasuredCourseId = measuredCourseId,
                           MemberCategory = 1, // Gents,
                           TournamentDate = new DateTime(DateTime.Now.Year, 5, 1)
                       });

            result.Add(new CreateTournamentRequest
                       {
                           Format = 1, // Strokeplay
                           Name = "June Medal",
                           MeasuredCourseId = measuredCourseId,
                           MemberCategory = 1, // Gents,
                           TournamentDate = new DateTime(DateTime.Now.Year, 6, 1)
                       });

            result.Add(new CreateTournamentRequest
                       {
                           Format = 1, // Strokeplay
                           Name = "July Medal",
                           MeasuredCourseId = measuredCourseId,
                           MemberCategory = 1, // Gents,
                           TournamentDate = new DateTime(DateTime.Now.Year, 7, 1)
                       });

            result.Add(new CreateTournamentRequest
                       {
                           Format = 1, // Strokeplay
                           Name = "August Medal",
                           MeasuredCourseId = measuredCourseId,
                           MemberCategory = 1, // Gents,
                           TournamentDate = new DateTime(DateTime.Now.Year, 8, 1)
                       });

            result.Add(new CreateTournamentRequest
                       {
                           Format = 1, // Strokeplay
                           Name = "September Medal",
                           MeasuredCourseId = measuredCourseId,
                           MemberCategory = 1, // Gents,
                           TournamentDate = new DateTime(DateTime.Now.Year, 9, 1)
                       });

            result.Add(new CreateTournamentRequest
                       {
                           Format = 1, // Strokeplay
                           Name = "October Medal",
                           MeasuredCourseId = measuredCourseId,
                           MemberCategory = 1, // Gents,
                           TournamentDate = new DateTime(DateTime.Now.Year, 10, 1)
                       });

            return result;
        }

        /// <summary>
        /// Gets the hole scores.
        /// </summary>
        /// <param name="playingHandicap">The playing handicap.</param>
        /// <param name="scoreResult">The score result.</param>
        /// <returns></returns>
        private static Dictionary<Int32, Int32> GetHoleScores(Int32 playingHandicap,
                                                              ScoreResult scoreResult)
        {
            Int32 measuredCoursePar = Program.AddMeasuredCourseToClubRequest.Holes.Sum(h => h.Par);

            // Determine the buffer zone
            Int32 bufferZone = Program.DetermineBufferZone(playingHandicap);

            Dictionary<Int32, Int32> resultDictionary = new Dictionary<Int32, Int32>();

            for (Int32 i = 1; i <= 18; i++)
                // Create a level par score
                resultDictionary.Add(i, Program.AddMeasuredCourseToClubRequest.Holes[i - 1].Par);

            if (scoreResult == ScoreResult.Buffer)
            {
                // Do Nothing
            }
            else if (scoreResult == ScoreResult.Under)
            {
                resultDictionary[12] = resultDictionary[12] - 1;
                resultDictionary[16] = resultDictionary[16] - 1;
                resultDictionary[18] = resultDictionary[18] - 1;
            }
            else if (scoreResult == ScoreResult.Over)
            {
                foreach (KeyValuePair<Int32, Int32> keyValuePair in resultDictionary)
                {
                    resultDictionary[keyValuePair.Key] = keyValuePair.Value + 1;

                    if (resultDictionary.Values.Sum() >= measuredCoursePar + playingHandicap + bufferZone) break;
                }
            }

            return resultDictionary;
        }

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <param name="tokenType">Type of the token.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="scopes">The scopes.</param>
        /// <returns></returns>
        private static async Task<String> GetToken(TokenType tokenType,
                                                   String clientId,
                                                   String clientSecret,
                                                   String userName = "",
                                                   String password = "",
                                                   List<String> scopes = null)
        {
            StringBuilder queryString = new StringBuilder();
            if (tokenType == TokenType.Client)
            {
                queryString.Append("grant_type=client_credentials");
                queryString.Append($"&client_id={clientId}");
                queryString.Append($"&client_secret={clientSecret}");
            }
            else if (tokenType == TokenType.Password)
            {
                queryString.Append("grant_type=password");
                queryString.Append($"&client_id={clientId}");
                queryString.Append($"&client_secret={clientSecret}");
                queryString.Append($"&username={userName}");

                queryString.Append($"&password={password}");

                if (scopes != null && scopes.Count > 0)
                {
                    String scopeString = "";
                    foreach (String scope in scopes) scopeString = $"{scopeString} {scope}";

                    queryString.Append($"&scope={scopeString}");
                }
            }

            String requestUri = $"{Program.SecurityServiceAddress}/connect/token";

            HttpResponseMessage httpResponse =
                await Program.MakeHttpPost(requestUri, queryString.ToString(), mediaType:"application/x-www-form-urlencoded").ConfigureAwait(false);

            TokenResponse token = await Program.GetResponseObject<TokenResponse>(httpResponse).ConfigureAwait(false);

            return token.AccessToken;
        }

        /// <summary>
        /// Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        private static async Task Main(String[] args)
        {
            const Int32 clubCount = 10;
            const Int32 playersPerClub = 36;

            CancellationToken cancellationToken = new CancellationToken();

            Console.WriteLine($"About to create {clubCount} Golf Clubs");
            await Program.CreateGolfClubData(clubCount, cancellationToken);
            Console.WriteLine($"{clubCount} Golf Clubs Created");

            Console.WriteLine($"About to create {clubCount * playersPerClub} Players");
            await Program.CreatePlayerData(playersPerClub, cancellationToken);
            Console.WriteLine($"Created {clubCount * playersPerClub} Players");

            Console.WriteLine("About to create Tournament Data");
            await Program.CreateTournamentData(cancellationToken);
            Console.WriteLine("Created Tournament Data");

            Console.WriteLine("Process Completed");
            Console.ReadKey();
        }

        /// <summary>
        /// Makes the HTTP get.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="bearerToken">The bearer token.</param>
        /// <returns></returns>
        private static async Task<HttpResponseMessage> MakeHttpGet(String requestUri,
                                                                   String bearerToken = "")
        {
            HttpResponseMessage result = null;

            using(HttpClient client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(bearerToken))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                result = await client.GetAsync(requestUri, CancellationToken.None).ConfigureAwait(false);
            }

            return result;
        }

        /// <summary>
        /// Makes the HTTP post.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="requestObject">The request object.</param>
        /// <param name="bearerToken">The bearer token.</param>
        /// <param name="mediaType">Type of the media.</param>
        /// <returns></returns>
        private static async Task<HttpResponseMessage> MakeHttpPost<T>(String requestUri,
                                                                       T requestObject,
                                                                       String bearerToken = "",
                                                                       String mediaType = "application/json")
        {
            HttpResponseMessage result = null;
            StringContent httpContent = null;
            if (requestObject is String)
            {
                httpContent = new StringContent(requestObject.ToString(), Encoding.UTF8, mediaType);
            }
            else
            {
                String requestSerialised = JsonConvert.SerializeObject(requestObject);
                httpContent = new StringContent(requestSerialised, Encoding.UTF8, mediaType);
            }

            using(HttpClient client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(bearerToken))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                result = await client.PostAsync(requestUri, httpContent, CancellationToken.None).ConfigureAwait(false);
            }

            return result;
        }

        #endregion

        #region Others

        /// <summary>
        /// The base address
        /// </summary>
        private const String BaseAddress = "http://192.168.1.132:5000";

        /// <summary>
        /// The security service address
        /// </summary>
        private const String SecurityServiceAddress = "http://192.168.1.132:5001";

        #endregion
    }
}