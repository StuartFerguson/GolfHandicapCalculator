namespace ManagmentAPI.TestDataGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;
    using ManagementAPI.Service.Client;
    using ManagementAPI.Service.DataTransferObjects;

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
        /// Creates the golf club datav2.
        /// </summary>
        /// <param name="testDataGenerator">The test data generator.</param>
        /// <param name="numberOfGolfClubs">The number of golf clubs.</param>
        /// <param name="lastClubCount">The last club count.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private static async Task CreateGolfClubDatav2(ITestDataGenerator testDataGenerator,
                                                       Int32 numberOfGolfClubs,
                                                       Int32 lastClubCount,
                                                       CancellationToken cancellationToken)
        {
            List<Task> tasks = new List<Task>();
            Int32 startIndex = 0 + lastClubCount;
            Int32 endIndex = numberOfGolfClubs + lastClubCount;
            for (Int32 i = startIndex; i < endIndex; i++)
            {
                RegisterClubAdministratorRequest registerClubAdministratorRequest = new RegisterClubAdministratorRequest
                                                                                    {
                                                                                        EmailAddress = $"clubadministrator@testgolfclub{i}.co.uk",
                                                                                        ConfirmPassword = "123456",
                                                                                        Password = "123456",
                                                                                        TelephoneNumber = "1234567890"
                                                                                    };

                await testDataGenerator.RegisterGolfClubAdministrator(registerClubAdministratorRequest, cancellationToken);

                String token = await testDataGenerator.GetToken(TokenType.Password,
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

                CreateGolfClubResponse createGolfClubResponse = await testDataGenerator.CreateGolfClub(token, createGolfClubRequest, cancellationToken);

                await testDataGenerator.AddMeasuredCourseToGolfClub(token, Program.AddMeasuredCourseToClubRequest, cancellationToken);
                
                AddTournamentDivisionToGolfClubRequest division1 = new AddTournamentDivisionToGolfClubRequest
                                                                   {
                                                                       Division = 1,
                                                                       StartHandicap = -10,
                                                                       EndHandicap = 7
                                                                   };
                await testDataGenerator.AddTournamentDivision(token, division1, cancellationToken);
                AddTournamentDivisionToGolfClubRequest division2 = new AddTournamentDivisionToGolfClubRequest
                                                                   {
                                                                       Division = 2,
                                                                       StartHandicap = 6,
                                                                       EndHandicap = 12
                                                                   };
                await testDataGenerator.AddTournamentDivision(token, division2, cancellationToken);
                AddTournamentDivisionToGolfClubRequest division3 = new AddTournamentDivisionToGolfClubRequest
                                                                   {
                                                                       Division = 3,
                                                                       StartHandicap = 13,
                                                                       EndHandicap = 21
                                                                   };
                await testDataGenerator.AddTournamentDivision(token, division3, cancellationToken);
                AddTournamentDivisionToGolfClubRequest division4 = new AddTournamentDivisionToGolfClubRequest
                                                                   {
                                                                       Division = 4,
                                                                       StartHandicap = 22,
                                                                       EndHandicap = 28
                                                                   };
                await testDataGenerator.AddTournamentDivision(token, division4, cancellationToken);

                Program.GolfClubs.Add(new GolfClubDetails
                                      {
                                          AdminEmailAddress = registerClubAdministratorRequest.EmailAddress,
                                          AdminPassword = registerClubAdministratorRequest.Password,
                                          GolfClubId = createGolfClubResponse.GolfClubId,
                                          MeasuredCourseId = Program.AddMeasuredCourseToClubRequest.MeasuredCourseId,
                                          GolfClubName = createGolfClubRequest.Name
                                      });
            }
        }

        /// <summary>
        /// Creates the player datav2.
        /// </summary>
        /// <param name="testDataGenerator">The test data generator.</param>
        /// <param name="numberPlayersPerClub">The number players per club.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private static async Task CreatePlayerDatav2(ITestDataGenerator testDataGenerator,
                                                     Int32 lastClub,
                                                     Int32 numberPlayersPerClub,
                                                     CancellationToken cancellationToken)
        {
            Int32 clubCounter = lastClub + 1;
            foreach (GolfClubDetails golfClubDetails in Program.GolfClubs)
            {
                for (Int32 i = 0; i < numberPlayersPerClub; i++)
                {
                    RegisterPlayerRequest registerPlayerRequest = new RegisterPlayerRequest
                                                                  {
                                                                      DateOfBirth = DateTime.Now.AddYears(-30),
                                                                      EmailAddress = $"player{i}.{clubCounter}@testplayer.co.uk",
                                                                      ExactHandicap = i,
                                                                      FirstName = $"Club {clubCounter}",
                                                                      Gender = "M",
                                                                      LastName = $"Test Player {i}",
                                                                      MiddleName = string.Empty
                                                                  };

                    RegisterPlayerResponse registerPlayerResponse = await testDataGenerator.RegisterPlayer(registerPlayerRequest, cancellationToken);

                    Console.WriteLine($"Created Player {registerPlayerRequest.FirstName} {registerPlayerRequest.LastName}");

                    String token = await testDataGenerator.GetToken(TokenType.Password,
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

                    await testDataGenerator.RequestClubMembership(token, golfClubDetails.GolfClubId, cancellationToken);

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
        /// Creates the tournament datav2.
        /// </summary>
        /// <param name="testDataGenerator">The test data generator.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private static async Task CreateTournamentDatav2(ITestDataGenerator testDataGenerator,
                                                         CancellationToken cancellationToken)
        {
            foreach (GolfClubDetails golfClubDetails in Program.GolfClubs)
            {
                List<CreateTournamentRequest> requests = Program.GetCreateTournamentRequests(golfClubDetails.MeasuredCourseId);

                String passwordToken = testDataGenerator.GetToken(golfClubDetails.AdminEmailAddress);

                foreach (CreateTournamentRequest createTournamentRequest in requests)
                {
                    CreateTournamentResponse createTournamentResponse =
                        await testDataGenerator.CreateTournament(passwordToken, createTournamentRequest, cancellationToken);

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

                        RecordPlayerTournamentScoreRequest recordPlayerTournamentScoreRequest = new RecordPlayerTournamentScoreRequest
                                                                                                {
                                                                                                    PlayingHandicap = player.PlayingHandicap,
                                                                                                    HoleScores =
                                                                                                        Program.GetHoleScores(player.PlayingHandicap, scoreResult)
                                                                                                };

                        String playerPasswordToken = testDataGenerator.GetToken(player.EmailAddress);

                        await testDataGenerator.SignUpPlayerForTournament(playerPasswordToken, createTournamentResponse.TournamentId, cancellationToken);

                        await testDataGenerator.RecordPlayerScore(playerPasswordToken,
                                                                  createTournamentResponse.TournamentId,
                                                                  recordPlayerTournamentScoreRequest,
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
        /// Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        private static async Task Main(String[] args)
        {
            const Int32 lastClub = 0;
            const Int32 clubCount = 1;
            const Int32 playersPerClub = 37;

            // Create the data generator class
            String BaseAddressResolver(String service)
            {
                if (service == "SecurityService")
                {
                    return Program.SecurityServiceAddress;
                }

                return Program.BaseAddress;
            }

            IGolfClubClient golfClubClient = new GolfClubClient(BaseAddressResolver, Program.HttpClient);
            IPlayerClient playerClient = new PlayerClient(BaseAddressResolver, Program.HttpClient);
            ITournamentClient tournamentClient = new TournamentClient(BaseAddressResolver, Program.HttpClient);

            ITestDataGenerator testDataGenerator = new TestDataGenerator(golfClubClient, playerClient, tournamentClient, BaseAddressResolver);

            CancellationToken cancellationToken = new CancellationToken();
            Stopwatch sw = Stopwatch.StartNew();

            Console.WriteLine($"About to create {clubCount} Golf Clubs");
            await Program.CreateGolfClubDatav2(testDataGenerator, clubCount, lastClub, cancellationToken);
            sw.Stop();
            Console.WriteLine($"{clubCount} Golf Clubs Created in {sw.ElapsedMilliseconds} ms");

            Console.WriteLine($"About to create {clubCount * playersPerClub} Players");
            sw = Stopwatch.StartNew();
            await Program.CreatePlayerDatav2(testDataGenerator, lastClub, playersPerClub, cancellationToken);
            sw.Stop();
            Console.WriteLine($"Created {clubCount * playersPerClub} Players in {sw.ElapsedMilliseconds} ms");

            Console.WriteLine("About to create Tournament Data");
            sw = Stopwatch.StartNew();
            await Program.CreateTournamentDatav2(testDataGenerator, cancellationToken);
            sw.Stop();
            Console.WriteLine($"Created Tournament Data in {sw.ElapsedMilliseconds} ms");

            Console.WriteLine("Process Completed");
        }

        #endregion

        #region Others

        /// <summary>
        /// The base address
        /// </summary>
        private const String BaseAddress = "http://127.0.0.1:5000";

        /// <summary>
        /// The security service address
        /// </summary>
        private const String SecurityServiceAddress = "http://127.0.0.1:5001";

        #endregion
    }
}