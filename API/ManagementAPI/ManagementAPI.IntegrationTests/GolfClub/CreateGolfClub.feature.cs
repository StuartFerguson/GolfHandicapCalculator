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
namespace ManagementAPI.IntegrationTests.GolfClub
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Xunit.TraitAttribute("Category", "base")]
    [Xunit.TraitAttribute("Category", "golfclub")]
    public partial class CreateGolfClubFeature : Xunit.IClassFixture<CreateGolfClubFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "CreateGolfClub.feature"
#line hidden
        
        public CreateGolfClubFeature(CreateGolfClubFeature.FixtureData fixtureData, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Create Golf Club", "\tIn order to run a golf club handicapping system\r\n\tAs a club administrator\r\n\tI wa" +
                    "nt to be able to create my golf club", ProgrammingLanguage.CSharp, new string[] {
                        "base",
                        "golfclub"});
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
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "GolfClubNumber",
                        "EmailAddress",
                        "GivenName",
                        "MiddleName",
                        "FamilyName",
                        "Password",
                        "ConfirmPassword",
                        "TelephoneNumber"});
            table7.AddRow(new string[] {
                        "1",
                        "admin@testgolfclub1.co.uk",
                        "Admin",
                        "",
                        "User1",
                        "123456",
                        "123456",
                        "01234567890"});
#line 8
 testRunner.Given("the following golf club administrator has been registered", ((string)(null)), table7, "Given ");
#line 11
 testRunner.And("I am logged in as the administrator for golf club 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }
        
        [Xunit.FactAttribute(DisplayName="Create Golf Club")]
        [Xunit.TraitAttribute("FeatureTitle", "Create Golf Club")]
        [Xunit.TraitAttribute("Description", "Create Golf Club")]
        public virtual void CreateGolfClub()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create Golf Club", null, ((string[])(null)));
#line 13
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
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
            table8.AddRow(new string[] {
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
#line 14
 testRunner.When("I create a golf club with the following details", ((string)(null)), table8, "When ");
#line 17
 testRunner.Then("the golf club is created successfully", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.0.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                CreateGolfClubFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                CreateGolfClubFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
