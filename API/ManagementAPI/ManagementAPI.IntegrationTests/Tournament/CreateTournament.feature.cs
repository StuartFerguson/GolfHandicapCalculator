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
    [Xunit.TraitAttribute("Category", "tournament")]
    [Xunit.TraitAttribute("Category", "golfclubadministrator")]
    public partial class CreateTournamentFeature : Xunit.IClassFixture<CreateTournamentFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "CreateTournament.feature"
#line hidden
        
        public CreateTournamentFeature(CreateTournamentFeature.FixtureData fixtureData, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Create Tournament", "\tIn order to run a golf club handicapping system\r\n\tAs a club administrator\r\n\tI wa" +
                    "nt to be able to create club tournaments", ProgrammingLanguage.CSharp, new string[] {
                        "base",
                        "golfclub",
                        "tournament",
                        "golfclubadministrator"});
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
            TechTalk.SpecFlow.Table table75 = new TechTalk.SpecFlow.Table(new string[] {
                        "GolfClubNumber",
                        "EmailAddress",
                        "GivenName",
                        "MiddleName",
                        "FamilyName",
                        "Password",
                        "ConfirmPassword",
                        "TelephoneNumber"});
            table75.AddRow(new string[] {
                        "1",
                        "admin@testgolfclub1.co.uk",
                        "Admin",
                        "",
                        "User1",
                        "123456",
                        "123456",
                        "01234567890"});
#line 8
 testRunner.Given("the following golf club administrator has been registered", ((string)(null)), table75, "Given ");
#line 11
 testRunner.And("I am logged in as the administrator for golf club 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table76 = new TechTalk.SpecFlow.Table(new string[] {
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
            table76.AddRow(new string[] {
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
 testRunner.When("I create a golf club with the following details", ((string)(null)), table76, "When ");
#line 15
 testRunner.Then("the golf club is created successfully", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table77 = new TechTalk.SpecFlow.Table(new string[] {
                        "GolfClubNumber",
                        "MeasuredCourseNumber",
                        "Name",
                        "StandardScratchScore",
                        "TeeColour"});
            table77.AddRow(new string[] {
                        "1",
                        "1",
                        "Test Course",
                        "70",
                        "White"});
#line 16
 testRunner.When("I add a measured course to the club with the following details", ((string)(null)), table77, "When ");
#line hidden
            TechTalk.SpecFlow.Table table78 = new TechTalk.SpecFlow.Table(new string[] {
                        "GolfClubNumber",
                        "MeasuredCourseNumber",
                        "HoleNumber",
                        "LengthInYards",
                        "Par",
                        "StrokeIndex"});
            table78.AddRow(new string[] {
                        "1",
                        "1",
                        "1",
                        "348",
                        "4",
                        "10"});
            table78.AddRow(new string[] {
                        "1",
                        "1",
                        "2",
                        "402",
                        "4",
                        "4"});
            table78.AddRow(new string[] {
                        "1",
                        "1",
                        "3",
                        "207",
                        "3",
                        "14"});
            table78.AddRow(new string[] {
                        "1",
                        "1",
                        "4",
                        "405",
                        "4",
                        "8"});
            table78.AddRow(new string[] {
                        "1",
                        "1",
                        "5",
                        "428",
                        "4",
                        "2"});
            table78.AddRow(new string[] {
                        "1",
                        "1",
                        "6",
                        "477",
                        "5",
                        "12"});
            table78.AddRow(new string[] {
                        "1",
                        "1",
                        "7",
                        "186",
                        "3",
                        "16"});
            table78.AddRow(new string[] {
                        "1",
                        "1",
                        "8",
                        "397",
                        "4",
                        "6"});
            table78.AddRow(new string[] {
                        "1",
                        "1",
                        "9",
                        "130",
                        "3",
                        "18"});
            table78.AddRow(new string[] {
                        "1",
                        "1",
                        "10",
                        "399",
                        "4",
                        "3"});
            table78.AddRow(new string[] {
                        "1",
                        "1",
                        "11",
                        "401",
                        "4",
                        "13"});
            table78.AddRow(new string[] {
                        "1",
                        "1",
                        "12",
                        "421",
                        "4",
                        "1"});
            table78.AddRow(new string[] {
                        "1",
                        "1",
                        "13",
                        "530",
                        "5",
                        "11"});
            table78.AddRow(new string[] {
                        "1",
                        "1",
                        "14",
                        "196",
                        "3",
                        "5"});
            table78.AddRow(new string[] {
                        "1",
                        "1",
                        "15",
                        "355",
                        "4",
                        "7"});
            table78.AddRow(new string[] {
                        "1",
                        "1",
                        "16",
                        "243",
                        "4",
                        "15"});
            table78.AddRow(new string[] {
                        "1",
                        "1",
                        "17",
                        "286",
                        "4",
                        "17"});
            table78.AddRow(new string[] {
                        "1",
                        "1",
                        "18",
                        "399",
                        "4",
                        "9"});
#line 19
 testRunner.And("with the following holes", ((string)(null)), table78, "And ");
#line 39
 testRunner.Then("the measured course is added to the club successfully", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
        }
        
        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }
        
        [Xunit.FactAttribute(DisplayName="Create Tournament")]
        [Xunit.TraitAttribute("FeatureTitle", "Create Tournament")]
        [Xunit.TraitAttribute("Description", "Create Tournament")]
        public virtual void CreateTournament()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create Tournament", null, ((string[])(null)));
#line 41
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table79 = new TechTalk.SpecFlow.Table(new string[] {
                        "TournamentNumber",
                        "TournamentName",
                        "TournamentMonth",
                        "TournamentDay",
                        "TournamentFormat",
                        "PlayerCategory",
                        "MeasuredCourseName",
                        "GolfClubNumber"});
            table79.AddRow(new string[] {
                        "1",
                        "Test Tournament 1",
                        "April",
                        "5",
                        "Strokeplay",
                        "Gents",
                        "Test Course",
                        "1"});
#line 42
 testRunner.Given("When I create a tournament with the following details", ((string)(null)), table79, "Given ");
#line 45
 testRunner.Then("tournament number 1 for golf club 1 measured course \'Test Course\' will be created" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.0.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                CreateTournamentFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                CreateTournamentFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
