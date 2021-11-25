
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using EventManager.API;
using EventManager.Core.Identity.Models;
using Microsoft.AspNetCore.Http;
using webapi.tests.Infrastructure;

namespace Example
{

  public class Actionwords
  {
    public static readonly AuthenticationModel aliceCredentials = new()
    {
      Username = "alice",
      Password = "aPa$$word1234"
    };

    public static readonly AuthenticationModel jhondCredentials = new()
    {
      Username = "jhond",
      Password = "jPa$$word1234"
    };

    public const string AUTH_LOGIN_ENDPOINT = "api/v1/auth/login";

    public static string ALICE_ACCESS_TOKEN = null;
    public static string JHOND_ACCESS_TOKEN = null;
    private readonly CustomWebApiFactory<Startup> _factory;

    public Actionwords(CustomWebApiFactory<Startup> factory)
    {
      _factory = factory;
    }

    private TokenModel login(AuthenticationModel user)
    {
      var client = _factory.CreateClient();
      var response = client.PostAsync(AUTH_LOGIN_ENDPOINT, new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json"));
      response.Wait();
      using StreamReader stream = new StreamReader(response.Result.Content.ReadAsStream());
      string content = stream.ReadToEnd();
      TokenModel tokenModel = JsonSerializer.Deserialize<TokenModel>(content,new JsonSerializerOptions 
      {
          PropertyNameCaseInsensitive = true
      });
      return tokenModel;
    }

    public void CallerPresentsAValidAccessToken()
    {
      ALICE_ACCESS_TOKEN = login(aliceCredentials).Token;
      JHOND_ACCESS_TOKEN = login(jhondCredentials).Token;
    }

    public void TheEndpointOfTheEventsControllerP1IsExist(string p1)
    {

    }

    public void P1EventDoesntExistInTheDatabase(string p1)
    {

    }

    public void IPostANewEventToTheEndpoint()
    {

    }

    public void ITypeP1InTheLocationField(string p1)
    {

    }

    public void ITypeP1InTheCountryField(string p1)
    {

    }

    public void ITypeP1InTheNameField(string p1)
    {

    }

    public void IShouldntTypeIdOrCreationDateOfTheEvent()
    {

    }

    public void IShouldHaveSeenHTTPStatusIsP1(int p1)
    {

    }

    public void IShouldHaveSeenHTTPStatusIsP1(string p1)
    {
      int httpCode;
      int.TryParse(p1, out httpCode);
      IShouldHaveSeenHTTPStatusIsP1(httpCode);
    }

    public void TheCertainEventIsCreatedInTheDatabaseWithAnUniqueIdOfTheEventAndCreationTimeOfTheEvent()
    {

    }

    public void ITypeP1InTheCapacityField(int? p1)
    {

    }

    public void P1PostANewEventToTheEndpoint(string p1)
    {

    }

    public void IShouldHaveSeenTheP1ErrorMessage(string p1)
    {

    }

    public void IType101CharactersLongTextInTheLocationField()
    {

    }

    public void P1EventDoesExistInTheDatabase(string p1)
    {

    }

    public void P1IsTheLocationOfTheEvent(string p1)
    {

    }

    public void P1IsTheCountryOfTheEvent(string p1)
    {

    }

    public void P1IsTheCapacityOfTheEvent(int? p1)
    {

    }

    public void IPutTheModificatedEventToTheEndpoint()
    {

    }

    public void IShouldHaveSeenTheNameOfTheEventIsP1(string p1)
    {

    }

    public void IShouldHaveSeenTheLocationOfTheEventIsP1(string p1)
    {

    }

    public void IShouldHaveSeenTheCountryOfTheEventIsP1(string p1)
    {

    }

    public void IShouldHaveSeenTheCapacityOfTheEventIsP1(int? p1)
    {

    }

    public void P1PutTheModificatedEventToTheEndpoint(string p1)
    {

    }

    public void IdOfTheP1Event(string p1)
    {

    }

    public void IGetTheEventById()
    {

    }

    public void IShouldHaveSeenTheIdAndCreationDateOfTheCertainEvent()
    {

    }

    public void AnonymusGetTheEventById()
    {

    }

    public void AnonymusShouldHaveSeenHTTPStatusIsP1(string p1)
    {

    }

    public void AnonymusShouldHaveSeenTheP1ErrorMessage(string p1)
    {

    }

    public void AnonymusGetAllEvent()
    {

    }

    public void IHaveP1Events(int p1)
    {

    }

    public void IHaveP1Pages(int p1)
    {

    }

    public void IHaveP1ElementsPerPage(int p1)
    {

    }

    public void IGetAllEventsByTheP1Page(int p1)
    {

    }

    public void IShouldSeeP1EventsAreThereInTheResultOfTheApiCall(int p1)
    {

    }

    public void EachElementsOfTheResultIsContainsAllPropertyOfTheEvent()
    {

    }

    public void PreconfiguredUsersAreP1AndP2(string p1, string p2)
    {

    }

    public void P1sPasswordIsP2(string p1, string p2)
    {

    }

    public void TheEndpointOfTheLoginServiceIsP1(string p1)
    {

    }

    public void ICallTheEndpointOfTheLogInApi()
    {

    }

    public void IFillP1InTheUsernameParameter(string p1)
    {

    }

    public void IFillP1InThePasswordParameter(string p1)
    {

    }

    public void IShouldHaveValidAccessToken()
    {

    }

    public void ICallLogInApi()
    {

    }

    public void IHaventAValidAccessToken()
    {

    }
  }
}