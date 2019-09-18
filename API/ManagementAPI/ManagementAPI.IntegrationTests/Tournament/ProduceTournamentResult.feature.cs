// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:3.0.0.0
//      SpecFlow Generator Version:3.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace ManagementAPI.IntegrationTests.Tournament
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Xunit.TraitAttribute("Category", "base")]
    [Xunit.TraitAttribute("Category", "golfclub")]
    [Xunit.TraitAttribute("Category", "player")]
    [Xunit.TraitAttribute("Category", "tournament")]
    public partial class ProduceTournamentResultFeature : Xunit.IClassFixture<ProduceTournamentResultFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "ProduceTournamentResult.feature"
#line hidden
        
        public ProduceTournamentResultFeature(ProduceTournamentResultFeature.FixtureData fixtureData, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Produce Tournament Result", "\tIn order to run a golf club handicapping system\r\n\tAs a match secretary\r\n\tI need " +
                    "to be able to produce tournament results", ProgrammingLanguage.CSharp, new string[] {
                        "base",
                        "golfclub",
                        "player",
                        "tournament"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 7
#line hidden
            TechTalk.SpecFlow.Table table85 = new TechTalk.SpecFlow.Table(new string[] {
                        "GolfClubNumber",
                        "EmailAddress",
                        "GivenName",
                        "MiddleName",
                        "FamilyName",
                        "Password",
                        "ConfirmPassword",
                        "TelephoneNumber"});
            table85.AddRow(new string[] {
                        "1",
                        "admin@testgolfclub1.co.uk",
                        "Admin",
                        "",
                        "User1",
                        "123456",
                        "123456",
                        "01234567890"});
#line 8
 testRunner.Given("the following golf club administrator has been registered", ((string)(null)), table85, "Given ");
#line 11
 testRunner.And("I am logged in as the administrator for golf club 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table86 = new TechTalk.SpecFlow.Table(new string[] {
                        "GolfClubNumber",
                        "GolfClubName",
                        "AddressLine1",
                        "AddressLine2",
                        "Town",
                        "Region",
                        "PostalCode",
                        "TelephoneNumber",
                        "EmailAddress",
                        "WebSite"});
            table86.AddRow(new string[] {
                        "1",
                        "Test Golf Club 1",
                        "Test Golf Club Address Line 1",
                        "Test Golf Club Address Line",
                        "TestTown1",
                        "TestRegion",
                        "TE57 1NG",
                        "01234567890",
                        "testclub1@testclub1.co.uk",
                        "www.testclub1.co.uk"});
#line 12
 testRunner.When("I create a golf club with the following details", ((string)(null)), table86, "When ");
#line 15
 testRunner.Then("the golf club is created successfully", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table87 = new TechTalk.SpecFlow.Table(new string[] {
                        "GolfClubNumber",
                        "MeasuredCourseNumber",
                        "Name",
                        "StandardScratchScore",
                        "TeeColour"});
            table87.AddRow(new string[] {
                        "1",
                        "1",
                        "Test Course",
                        "70",
                        "White"});
#line 16
 testRunner.When("I add a measured course to the club with the following details", ((string)(null)), table87, "When ");
#line hidden
            TechTalk.SpecFlow.Table table88 = new TechTalk.SpecFlow.Table(new string[] {
                        "GolfClubNumber",
                        "MeasuredCourseNumber",
                        "HoleNumber",
                        "LengthInYards",
                        "Par",
                        "StrokeIndex"});
            table88.AddRow(new string[] {
                        "1",
                        "1",
                        "1",
                        "348",
                        "4",
                        "10"});
            table88.AddRow(new string[] {
                        "1",
                        "1",
                        "2",
                        "402",
                        "4",
                        "4"});
            table88.AddRow(new string[] {
                        "1",
                        "1",
                        "3",
                        "207",
                        "3",
                        "14"});
            table88.AddRow(new string[] {
                        "1",
                        "1",
                        "4",
                        "405",
                        "4",
                        "8"});
            table88.AddRow(new string[] {
                        "1",
                        "1",
                        "5",
                        "428",
                        "4",
                        "2"});
            table88.AddRow(new string[] {
                        "1",
                        "1",
                        "6",
                        "477",
                        "5",
                        "12"});
            table88.AddRow(new string[] {
                        "1",
                        "1",
                        "7",
                        "186",
                        "3",
                        "16"});
            table88.AddRow(new string[] {
                        "1",
                        "1",
                        "8",
                        "397",
                        "4",
                        "6"});
            table88.AddRow(new string[] {
                        "1",
                        "1",
                        "9",
                        "130",
                        "3",
                        "18"});
            table88.AddRow(new string[] {
                        "1",
                        "1",
                        "10",
                        "399",
                        "4",
                        "3"});
            table88.AddRow(new string[] {
                        "1",
                        "1",
                        "11",
                        "401",
                        "4",
                        "13"});
            table88.AddRow(new string[] {
                        "1",
                        "1",
                        "12",
                        "421",
                        "4",
                        "1"});
            table88.AddRow(new string[] {
                        "1",
                        "1",
                        "13",
                        "530",
                        "5",
                        "11"});
            table88.AddRow(new string[] {
                        "1",
                        "1",
                        "14",
                        "196",
                        "3",
                        "5"});
            table88.AddRow(new string[] {
                        "1",
                        "1",
                        "15",
                        "355",
                        "4",
                        "7"});
            table88.AddRow(new string[] {
                        "1",
                        "1",
                        "16",
                        "243",
                        "4",
                        "15"});
            table88.AddRow(new string[] {
                        "1",
                        "1",
                        "17",
                        "286",
                        "4",
                        "17"});
            table88.AddRow(new string[] {
                        "1",
                        "1",
                        "18",
                        "399",
                        "4",
                        "9"});
#line 19
 testRunner.And("with the following holes", ((string)(null)), table88, "And ");
#line 39
 testRunner.Then("the measured course is added to the club successfully", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table89 = new TechTalk.SpecFlow.Table(new string[] {
                        "TournamentNumber",
                        "TournamentName",
                        "TournamentMonth",
                        "TournamentDay",
                        "TournamentFormat",
                        "PlayerCategory",
                        "MeasuredCourseName",
                        "GolfClubNumber"});
            table89.AddRow(new string[] {
                        "1",
                        "Test Tournament 1",
                        "April",
                        "5",
                        "Strokeplay",
                        "Gents",
                        "Test Course",
                        "1"});
#line 40
 testRunner.Given("When I create a tournament with the following details", ((string)(null)), table89, "Given ");
#line 43
 testRunner.Then("tournament number 1 for golf club 1 measured course \'Test Course\' will be created" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table90 = new TechTalk.SpecFlow.Table(new string[] {
                        "PlayerNumber",
                        "EmailAddress",
                        "GivenName",
                        "MiddleName",
                        "FamilyName",
                        "Age",
                        "Gender",
                        "ExactHandicap"});
            table90.AddRow(new string[] {
                        "1",
                        "testplayer1@players.co.uk",
                        "Test",
                        "",
                        "Player1",
                        "25",
                        "M",
                        "2"});
#line 44
 testRunner.Given("the following players have registered", ((string)(null)), table90, "Given ");
#line 47
 testRunner.Given("I am logged in as player number 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 48
 testRunner.When("I request membership of club number 1 for player number 1 the request is successf" +
                    "ul", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 49
 testRunner.When("player number 1 signs up to play in tournament number 1 for golf club 1 measured " +
                    "course \'Test Course\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 50
 testRunner.Then("player number 1 is recorded as signed up for tournament number 1 for golf club 1 " +
                    "measured course \'Test Course\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table91 = new TechTalk.SpecFlow.Table(new string[] {
                        "PlayerNumber",
                        "Hole1",
                        "Hole2",
                        "Hole3",
                        "Hole4",
                        "Hole5",
                        "Hole6",
                        "Hole7",
                        "Hole8",
                        "Hole9",
                        "Hole10",
                        "Hole11",
                        "Hole12",
                        "Hole13",
                        "Hole14",
                        "Hole15",
                        "Hole16",
                        "Hole17",
                        "Hole18"});
            table91.AddRow(new string[] {
                        "1",
                        "4",
                        "4",
                        "3",
                        "4",
                        "4",
                        "5",
                        "3",
                        "4",
                        "3",
                        "4",
                        "4",
                        "4",
                        "5",
                        "3",
                        "4",
                        "4",
                        "4",
                        "4"});
#line 51
 testRunner.When("a player records the following score for tournament number 1 for golf club 1 meas" +
                    "ured course \'Test Course\'", ((string)(null)), table91, "When ");
#line 54
 testRunner.Then("the scores recorded by the players are recorded against tournament number 1 for g" +
                    "olf club 1 measured course \'Test Course\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 55
 testRunner.When("I request to complete the tournament number 1 for golf club 1 measured course \'Te" +
                    "st Course\' the tournament is completed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
        }
        
        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }
        
        [Xunit.FactAttribute(DisplayName="Produce Tournament Result")]
        [Xunit.TraitAttribute("FeatureTitle", "Produce Tournament Result")]
        [Xunit.TraitAttribute("Description", "Produce Tournament Result")]
        [Xunit.TraitAttribute("Category", "EndToEnd")]
        public virtual void ProduceTournamentResult()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Produce Tournament Result", null, new string[] {
                        "EndToEnd"});
#line 58
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 7
this.FeatureBackground();
#line 59
 testRunner.When("I request to produce a tournament result for tournament number 1 for golf club 1 " +
                    "measured course \'Test Course\' the results are produced", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.0.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                ProduceTournamentResultFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                ProduceTournamentResultFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
