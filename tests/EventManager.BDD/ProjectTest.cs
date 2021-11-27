using System.Net.Http;
using System.Text;
using EventManager.API;
using EventManager.Core.Identity.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Moq;
using NUnit.Framework;
using webapi.tests.Infrastructure;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Example
{

  [TestFixture]
  public class ProjectTest
  {
    #region Variables  
    Actionwords Actionwords;

    private readonly CustomWebApiFactory<EventManager.API.Startup> _factory = new();
    #endregion

    [OneTimeSetUp]
    protected void SetUp()
    {
      Actionwords = new Actionwords(_factory);
      Actionwords.TheEndpointOfTheEventsControllerP1IsExist(Actionwords.AUTH_LOGIN_ENDPOINT);
      Actionwords.TheEndpointOfTheEventsControllerP1IsExist(Actionwords.EVENT_ORGANIZER_ENDPOINT);

      SetupAsync().Wait();
      Actionwords.CallerPresentsAValidAccessToken();
    }

    public async Task SetupAsync()
    {
      var client = _factory
                .CreateClient();
      var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/auth/register")
      {
        Content = new StringContent(JsonSerializer.Serialize(Actionwords.aliceCredentials), Encoding.UTF8, "application/json")
      };
      await client.PostAsync("/api/v1/auth/register", request.Content);
      request.Content = new StringContent(JsonSerializer.Serialize(Actionwords.jhondCredentials), Encoding.UTF8, "application/json");
      await client.PostAsync("/api/v1/auth/register", request.Content);
    }

    [Test]
    public void JhondUserPostAValidEventWithoutACapacityToTheEndpoint()
    {
      // Given "koncert1" event doesn't exist in the database
      Actionwords.P1EventDoesntExistInTheDatabase("koncert1");
      // When I post a new event to the endpoint
      Actionwords.IPostANewEventToTheEndpoint();
      // And I type "Budapest" in the location field
      Actionwords.ITypeP1InTheLocationField("Budapest");
      // And I type "Magyarorszag" in the country field
      Actionwords.ITypeP1InTheCountryField("Magyarorszag");
      // And I type "koncert1" in the name field
      Actionwords.ITypeP1InTheNameField("koncert1");
      // But I shouldn't type id or creation date of the event
      Actionwords.IShouldntTypeIdOrCreationDateOfTheEvent();
      // Then I should have seen HTTP status is "200"
      Actionwords.IShouldHaveSeenHTTPStatusIsP1("200");
      // And the certain event is created in the database with an unique id of the event and creation time of the event
      Actionwords.TheCertainEventIsCreatedInTheDatabaseWithAnUniqueIdOfTheEventAndCreationTimeOfTheEvent();
    }

    [Test]
    public void AliceUserPostAValidEventWithACapacityToTheEndpoint()
    {
      // Given "koncert2" event doesn't exist in the database
      Actionwords.P1EventDoesntExistInTheDatabase("koncert2");
      // When I post a new event to the endpoint
      Actionwords.IPostANewEventToTheEndpoint();
      // And I type "Budapest" in the location field
      Actionwords.ITypeP1InTheLocationField("Budapest");
      // And I type "Magyarorszag" in the country field
      Actionwords.ITypeP1InTheCountryField("Magyarorszag");
      // And I type "koncert2" in the name field
      Actionwords.ITypeP1InTheNameField("koncert2");
      // And I type "100" in the capacity field
      Actionwords.ITypeP1InTheCapacityField(100);
      // But I shouldn't type id or creation date of the event
      Actionwords.IShouldntTypeIdOrCreationDateOfTheEvent();
      // Then I should have seen HTTP status is "200"
      Actionwords.IShouldHaveSeenHTTPStatusIsP1("200");
      // And the certain event is created in the database with an unique id of the event and creation time of the event
      Actionwords.TheCertainEventIsCreatedInTheDatabaseWithAnUniqueIdOfTheEventAndCreationTimeOfTheEvent();
    }

    [Test]
    public void JhondUserPostAValidEventWithoutACountryToTheEndpoint()
    {
      // Given "koncert3" event doesn't exist in the database
      Actionwords.P1EventDoesntExistInTheDatabase("koncert3");
      // When I post a new event to the endpoint
      Actionwords.IPostANewEventToTheEndpoint();
      // And I type "Budapest" in the location field
      Actionwords.ITypeP1InTheLocationField("Budapest");
      // And I type "koncert3" in the name field
      Actionwords.ITypeP1InTheNameField("koncert3");
      // And I type "100" in the capacity field
      Actionwords.ITypeP1InTheCapacityField(100);
      // But I shouldn't type id or creation date of the event
      Actionwords.IShouldntTypeIdOrCreationDateOfTheEvent();
      // Then I should have seen HTTP status is "200"
      Actionwords.IShouldHaveSeenHTTPStatusIsP1("200");
      // And the certain event is created in the database with an unique id of the event and creation time of the event
      Actionwords.TheCertainEventIsCreatedInTheDatabaseWithAnUniqueIdOfTheEventAndCreationTimeOfTheEvent();
    }
    public void UserWantToCreateANewEventButSomeParametersAreWorng(string username, string name, string location, string country, int? capacity, int code, string message)
    {
      // Given "<name>" event doesn't exist in the database
      Actionwords.P1EventDoesntExistInTheDatabase(name);
      // When "<username>" post a new event to the endpoint
      Actionwords.P1PostANewEventToTheEndpoint(username);
      // And I type "<location>" in the location field
      Actionwords.ITypeP1InTheLocationField(location);
      // And I type "<country>" in the country field
      Actionwords.ITypeP1InTheCountryField(country);
      // And I type "<name>" in the name field
      Actionwords.ITypeP1InTheNameField(name);
      // And I type "<capacity>" in the capacity field
      Actionwords.ITypeP1InTheCapacityField(capacity);
      // But I shouldn't type id or creation date of the event
      Actionwords.IShouldntTypeIdOrCreationDateOfTheEvent();
      // Then I should have seen HTTP status is "<code>"
      Actionwords.IShouldHaveSeenHTTPStatusIsP1(code);
      // And I should have seen the "<message>" error message
      Actionwords.IShouldHaveSeenTheP1ErrorMessage(message);
    }


    [Test]
    public void UserWantToCreateANewEventButSomeParametersAreWorng1()
    {
      UserWantToCreateANewEventButSomeParametersAreWorng("", "oldtimer autok", "Pusztamonostor", "Magyarorszag", 100, 403, "Please log in and try again");
    }

    [Test]
    public void UserWantToCreateANewEventButSomeParametersAreWorng2()
    {
      UserWantToCreateANewEventButSomeParametersAreWorng("jhond", "", "Pusztamonostor", "Magyarorszag", null, 500, "Name of the event is must");
    }

    [Test]
    public void UserWantToCreateANewEventButSomeParametersAreWorng3()
    {
      UserWantToCreateANewEventButSomeParametersAreWorng("alice", "oldtimer autok", "", "", null, 500, "Location of the event is must");
    }

    [Test]
    public void UserWantToCreateANewEventButSomeParametersAreWorng4()
    {
      UserWantToCreateANewEventButSomeParametersAreWorng("alice", "koncert", "Budapest", "Magyarorszag", -1, 500, "The capacity of the event must be a positive number");
    }

    [Test]
    public void UserWantToCreateANewEventButSomeParametersAreWorng5()
    {
      UserWantToCreateANewEventButSomeParametersAreWorng("jhond", "", "", "", 100, 500, "[Name of the event is must, Location of the event is must]");
    }



    [Test]
    public void JhondUserPostAnEventWithMoreThan100CharactersLongLocationNameToTheEndpoint()
    {
      // Given "koncert4" event doesn't exist in the database
      Actionwords.P1EventDoesntExistInTheDatabase("koncert4");
      // When I post a new event to the endpoint
      Actionwords.IPostANewEventToTheEndpoint();
      // And I type 101 characters long text in the location field
      Actionwords.IType101CharactersLongTextInTheLocationField();
      // And I type "Magyarorszag" in the country field
      Actionwords.ITypeP1InTheCountryField("Magyarorszag");
      // And I type "koncert4" in the name field
      Actionwords.ITypeP1InTheNameField("koncert4");
      // But I shouldn't type id or creation date of the event
      Actionwords.IShouldntTypeIdOrCreationDateOfTheEvent();
      // Then I should have seen HTTP status is "500"
      Actionwords.IShouldHaveSeenHTTPStatusIsP1("500");
      // And I should have seen the "Length of the location is more than 100 characters long" error message
      Actionwords.IShouldHaveSeenTheP1ErrorMessage("Length of the location is more than 100 characters long");
    }
    public void JhondUserPutAModificatedEventToTheEndpoint(string oldevent, string newevent, string oldlocation, string newlocation, string oldcountry, string newcountry, int? oldcapacity, int? newcapacity)
    {
      // Given "<oldevent>" event does exist in the database
      Actionwords.P1EventDoesExistInTheDatabase(oldevent);
      // And "<oldlocation>" is the location of the event
      Actionwords.P1IsTheLocationOfTheEvent(oldlocation);
      // And "<oldcountry>" is the country of the event
      Actionwords.P1IsTheCountryOfTheEvent(oldcountry);
      // And "<oldcapacity>" is the capacity of the event
      Actionwords.P1IsTheCapacityOfTheEvent(oldcapacity);
      // And "<newevent>" event doesn't exist in the database
      Actionwords.P1EventDoesntExistInTheDatabase(newevent);
      // When I put the modificated event to the endpoint
      Actionwords.IPutTheModificatedEventToTheEndpoint();
      // And I type "<newlocation>" in the location field
      Actionwords.ITypeP1InTheLocationField(newlocation);
      // And I type "<newcountry>" in the country field
      Actionwords.ITypeP1InTheCountryField(newcountry);
      // And I type "<newevent>" in the name field
      Actionwords.ITypeP1InTheNameField(newevent);
      // And I type "<newcapacity>" in the capacity field
      Actionwords.ITypeP1InTheCapacityField(newcapacity);
      // But I shouldn't type id or creation date of the event
      Actionwords.IShouldntTypeIdOrCreationDateOfTheEvent();
      // Then I should have seen HTTP status is "200"
      Actionwords.IShouldHaveSeenHTTPStatusIsP1("200");
      // And I should have seen the name of the event is "<newevent>"
      Actionwords.IShouldHaveSeenTheNameOfTheEventIsP1(newevent);
      // And I should have seen the location of the event is "<newlocation>"
      Actionwords.IShouldHaveSeenTheLocationOfTheEventIsP1(newlocation);
      // And I should have seen the country of the event is "<newcountry>"
      Actionwords.IShouldHaveSeenTheCountryOfTheEventIsP1(newcountry);
      // And I should have seen the capacity of the event is "<newcapacity>"
      Actionwords.IShouldHaveSeenTheCapacityOfTheEventIsP1(newcapacity);
    }

    [Test]
    public void JhondUserPutAModificatedEventToTheEndpoint1()
    {
      JhondUserPutAModificatedEventToTheEndpoint("koncert81", "koncert91", "Budapest", "Budapest", "", "", null, null);
    }

    [Test]
    public void JhondUserPutAModificatedEventToTheEndpoint2()
    {
      JhondUserPutAModificatedEventToTheEndpoint("koncert82", "koncert82", "Budapest", "Pecs", "", "Magyarorszag", 100, null);
    }

    [Test]
    public void JhondUserPutAModificatedEventToTheEndpoint3()
    {
      JhondUserPutAModificatedEventToTheEndpoint("koncert83", "koncert93", "Budapest", "Siofok", "Magyarorszag", "", null, 100);
    }


    public void UserWantToModifyTheEventButSomeParametersAreWorng(string username, string name, string location, string country, int? capacity, int code, string message)
    {
      // Given "koncert101" event does exist in the database
      Actionwords.P1EventDoesExistInTheDatabase("koncert101");
      // When "<username>" put the modificated event to the endpoint
      Actionwords.P1PutTheModificatedEventToTheEndpoint(username);
      // And I type "<location>" in the location field
      Actionwords.ITypeP1InTheLocationField(location);
      // And I type "<country>" in the country field
      Actionwords.ITypeP1InTheCountryField(country);
      // And I type "<name>" in the name field
      Actionwords.ITypeP1InTheNameField(name);
      // And I type "<capacity>" in the capacity field
      Actionwords.ITypeP1InTheCapacityField(capacity);
      // But I shouldn't type id or creation date of the event
      Actionwords.IShouldntTypeIdOrCreationDateOfTheEvent();
      // Then I should have seen HTTP status is "<code>"
      Actionwords.IShouldHaveSeenHTTPStatusIsP1(code);
      // And I should have seen the "<message>" error message
      Actionwords.IShouldHaveSeenTheP1ErrorMessage(message);
    }

    [Test]
    public void UserWantToModifyTheEventButSomeParametersAreWorng1()
    {
      UserWantToModifyTheEventButSomeParametersAreWorng("", "oldtimer autok", "Pusztamonostor", "Magyarorszag", 100, 403, "Please log in and try again");
    }

    [Test]
    public void UserWantToModifyTheEventButSomeParametersAreWorng2()
    {
      UserWantToModifyTheEventButSomeParametersAreWorng("jhond", "", "Pusztamonostor", "Magyarorszag", null, 500, "Name of the event is must");
    }

    [Test]
    public void UserWantToModifyTheEventButSomeParametersAreWorng3()
    {
      UserWantToModifyTheEventButSomeParametersAreWorng("alice", "oldtimer autok", "", "", null, 500, "Location of the event is must");
    }

    [Test]
    public void UserWantToModifyTheEventButSomeParametersAreWorng4()
    {
      UserWantToModifyTheEventButSomeParametersAreWorng("alice", "koncert", "Budapest", "Magyarorszag", -1, 500, "The capacity of the event must be a positive number");
    }

    [Test]
    public void UserWantToModifyTheEventButSomeParametersAreWorng5()
    {
      UserWantToModifyTheEventButSomeParametersAreWorng("jhond", "", "", "", 100, 500, "[Name of the event is must, Location of the event is must]");
    }



    [Test]
    public void JhondUserPutTheEventWithMoreThan100CharactersLongLocationNameToTheEndpoint()
    {
      // Given "koncert4" event does exist in the database
      Actionwords.P1EventDoesExistInTheDatabase("koncert4");
      // When I put the modificated event to the endpoint
      Actionwords.IPutTheModificatedEventToTheEndpoint();
      // And I type 101 characters long text in the location field
      Actionwords.IType101CharactersLongTextInTheLocationField();
      // But I shouldn't type id or creation date of the event
      Actionwords.IShouldntTypeIdOrCreationDateOfTheEvent();
      // Then I should have seen HTTP status is "500"
      Actionwords.IShouldHaveSeenHTTPStatusIsP1("500");
      // And I should have seen the "Length of the location is more than 100 characters long" error message
      Actionwords.IShouldHaveSeenTheP1ErrorMessage("Length of the location is more than 100 characters long");
    }

    [Test]
    public void JhondUserWantToQueryDetailsAboutTheCertainEventByEventId()
    {
      // Given id of the "kocnert103" event
      Actionwords.IdOfTheP1Event("kocnert103");
      // And "koncert103" event does exist in the database
      Actionwords.P1EventDoesExistInTheDatabase("koncert103");
      // And "Budapest" is the location of the event
      Actionwords.P1IsTheLocationOfTheEvent("Budapest");
      // And "Magyarorszag" is the country of the event
      Actionwords.P1IsTheCountryOfTheEvent("Magyarorszag");
      // And "100" is the capacity of the event
      Actionwords.P1IsTheCapacityOfTheEvent(100);
      // When I get the event by id
      Actionwords.IGetTheEventById();
      // Then I should have seen HTTP status is "200"
      Actionwords.IShouldHaveSeenHTTPStatusIsP1("200");
      // And I should have seen the name of the event is "koncert103"
      Actionwords.IShouldHaveSeenTheNameOfTheEventIsP1("koncert103");
      // And I should have seen the location of the event is "Budapest"
      Actionwords.IShouldHaveSeenTheLocationOfTheEventIsP1("Budapest");
      // And I should have seen the country of the event is "Magyarorszag"
      Actionwords.IShouldHaveSeenTheCountryOfTheEventIsP1("Magyarorszag");
      // And I should have seen the capacity of the event is "100"
      Actionwords.IShouldHaveSeenTheCapacityOfTheEventIsP1(100);
      // And I should have seen the id and creation date of the certain event
      Actionwords.IShouldHaveSeenTheIdAndCreationDateOfTheCertainEvent();
    }

    [Test]
    public void AnonymusUserWantToQueryDetailsAboutTheCertainEventByEventId()
    {
      // When Anonymus get the event by id
      Actionwords.AnonymusGetTheEventById();
      // Then Anonymus should have seen HTTP status is "403"
      Actionwords.AnonymusShouldHaveSeenHTTPStatusIsP1("403");
      // And Anonymus should have seen the "Please log in and try again" error message
      Actionwords.AnonymusShouldHaveSeenTheP1ErrorMessage("Please log in and try again");
    }

    [Test]
    public void AnonymusUserWantToQueryAllEvents()
    {
      // When Anonymus get all event
      Actionwords.AnonymusGetAllEvent();
      // Then Anonymus should have seen HTTP status is "403"
      Actionwords.AnonymusShouldHaveSeenHTTPStatusIsP1("403");
      // And Anonymus should have seen the "Please log in and try again" error message
      Actionwords.AnonymusShouldHaveSeenTheP1ErrorMessage("Please log in and try again");
    }
    public void JohndUserQueryAllEventsWithPaginator(int totalCount, int page, int limit, int currentPage, int count)
    {
      // Given I have "<total_count>" events
      Actionwords.IHaveP1Events(totalCount);
      // And I have "<page>" pages
      Actionwords.IHaveP1Pages(page);
      // And I have "<limit>" elements per page
      Actionwords.IHaveP1ElementsPerPage(limit);
      // When I get all events by the "<current_page>". page
      Actionwords.IGetAllEventsByTheP1Page(currentPage);
      // Then I should see "<count>" events are there in the result of the api call
      Actionwords.IShouldSeeP1EventsAreThereInTheResultOfTheApiCall(count);
      // And each elements of the result is contains all property of the event
      Actionwords.EachElementsOfTheResultIsContainsAllPropertyOfTheEvent();
    }

    [Test]
    public void JohndUserQueryAllEventsWithPaginator1()
    {
      JohndUserQueryAllEventsWithPaginator(0, 0, 10, 0, 0);
    }

    [Test]
    public void JohndUserQueryAllEventsWithPaginator2()
    {
      JohndUserQueryAllEventsWithPaginator(50, 1, 50, 1, 50);
    }

    [Test]
    public void JohndUserQueryAllEventsWithPaginator3()
    {
      JohndUserQueryAllEventsWithPaginator(100, 10, 10, 10, 10);
    }

    [Test]
    public void JohndUserQueryAllEventsWithPaginator4()
    {
      JohndUserQueryAllEventsWithPaginator(99, 10, 10, 10, 9);
    }



    [Test]
    public void AliceLogInWithHerUsernameAndPassword()
    {
      // When I call the endpoint of the  log in api
      Actionwords.ICallTheEndpointOfTheLogInApi();
      // And I fill "alice" in the username parameter
      Actionwords.IFillP1InTheUsernameParameter("alice");
      // And I fill "korte1234" in the password parameter
      Actionwords.IFillP1InThePasswordParameter("korte1234");
      // Then I should have seen HTTP status is "200"
      Actionwords.IShouldHaveSeenHTTPStatusIsP1("200");
      // And I should have valid access token
      Actionwords.IShouldHaveValidAccessToken();
    }

    [Test]
    public void JhondLogInWithHimUsernameAndPassword()
    {
      // When I call log in api
      Actionwords.ICallLogInApi();
      // And I fill "jhond" in the username parameter
      Actionwords.IFillP1InTheUsernameParameter("jhond");
      // And I fill "alma1234" in the password parameter
      Actionwords.IFillP1InThePasswordParameter("alma1234");
      // Then I should have seen HTTP status is "200"
      Actionwords.IShouldHaveSeenHTTPStatusIsP1("200");
      // And I should have valid access token
      Actionwords.IShouldHaveValidAccessToken();
    }
    public void UnableToLogInWithWrongCredentials(string username, string password)
    {
      // When I call log in api
      Actionwords.ICallLogInApi();
      // And I fill "<username>" in the username parameter
      Actionwords.IFillP1InTheUsernameParameter(username);
      // And I fill "<password>" in the password parameter
      Actionwords.IFillP1InThePasswordParameter(password);
      // Then I should have seen HTTP status is "401"
      Actionwords.IShouldHaveSeenHTTPStatusIsP1("401");
      // And I haven't a valid access token
      Actionwords.IHaventAValidAccessToken();
    }

    [Test]
    public void UnableToLogInWithWrongCredentials1()
    {
      UnableToLogInWithWrongCredentials("admin", "admin");
    }

    [Test]
    public void UnableToLogInWithWrongCredentials2()
    {
      UnableToLogInWithWrongCredentials("", "korte1234");
    }

    [Test]
    public void UnableToLogInWithWrongCredentials3()
    {
      UnableToLogInWithWrongCredentials("alice", "");
    }

    [Test]
    public void UnableToLogInWithWrongCredentials4()
    {
      UnableToLogInWithWrongCredentials("alice", "alma1234");
    }
  }
}