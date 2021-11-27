
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using EventManager.API;
using EventManager.Core.EventOrganizer.Entities;
using EventManager.Core.EventOrganizer.Models;
using EventManager.Core.EventOrganizer.Specifications.Filters;
using EventManager.Core.Identity.Models;
using NUnit.Framework;
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

    private static HttpRequestMessage HttpRequest; 
    private static HttpResponseMessage HttpResponseMessage; 


    public const string AUTH_LOGIN_ENDPOINT = "http://localhost/api/v1/auth/login";
    public const string EVENT_ORGANIZER_ENDPOINT = "http://localhost/api/v1/organizer/events";

    private static string ALICE_ACCESS_TOKEN = null;
    private static string JHOND_ACCESS_TOKEN = null;
    private readonly CustomWebApiFactory<Startup> _factory;
    private static T ReadContent<T>(HttpContent content){
      using StreamReader streamReader = new StreamReader(new ReusableHttpContent(content).ReadAsStream());
      string contentStr = streamReader.ReadToEnd();
      return JsonSerializer.Deserialize<T>(contentStr, new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = true
      });
    }
    public Actionwords(CustomWebApiFactory<Startup> factory)
    {
      _factory = factory;
    }

    private T Post<T,K>(string url, K model, out System.Net.HttpStatusCode code){
       var client = _factory.CreateClient();
      var response = client.PostAsync(url, new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));
      response.Wait();
      code = response.Result.StatusCode;
      using StreamReader stream = new StreamReader(response.Result.Content.ReadAsStream());
      string content = stream.ReadToEnd();
      return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = true
      });
    }

     private T Get<T,K>(string url, K model, out System.Net.HttpStatusCode code){
       var client = _factory.CreateClient();
       var request = new HttpRequestMessage
      {
          Method = HttpMethod.Get,
          RequestUri = new Uri(url),
          Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
      };

      var response = client.SendAsync(request);
      response.Wait();
      code = response.Result.StatusCode;
      using StreamReader stream = new StreamReader(new ReusableHttpContent(response.Result.Content).ReadAsStream());
      string content = stream.ReadToEnd();
      return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = true
      });
    }

    public void CallerPresentsAValidAccessToken()
    {
      System.Net.HttpStatusCode code;
      ALICE_ACCESS_TOKEN = Post<TokenModel, AuthenticationModel>(AUTH_LOGIN_ENDPOINT,aliceCredentials,out code).Token;
      JHOND_ACCESS_TOKEN = Post<TokenModel, AuthenticationModel>(AUTH_LOGIN_ENDPOINT,jhondCredentials,out code).Token;
    }

    public void TheEndpointOfTheEventsControllerP1IsExist(string p1)
    {
      using var client = _factory.CreateClient();
      var result = client.SendAsync(new HttpRequestMessage(HttpMethod.Post, p1));
      result.Wait();
      Assert.AreNotEqual(result.Result.StatusCode, System.Net.HttpStatusCode.NotFound);
    }

    public void P1EventDoesntExistInTheDatabase(string p1)
    {
      System.Net.HttpStatusCode code;
      var events = Get<List<EventEntity>, EventFilter>(EVENT_ORGANIZER_ENDPOINT, new EventFilter(),out code );
      Assert.False(events.Exists(e => string.Equals(p1, e.Name)));
    }

    public void IPostANewEventToTheEndpoint()
    {
     HttpRequest = new()
      {
          Method = HttpMethod.Post,
          RequestUri = new Uri(EVENT_ORGANIZER_ENDPOINT),
          Content = new StringContent(JsonSerializer.Serialize(new EditableEventModel()), Encoding.UTF8, "application/json")
      };
      HttpRequest.Headers.Add("Authorization", "Bearer " + JHOND_ACCESS_TOKEN);
    }

    public void ITypeP1InTheLocationField(string p1)
    {
      var entityModel = ReadContent<EditableEventModel>(HttpRequest.Content);
      entityModel.Location = p1;
      HttpRequest.Content = new StringContent(JsonSerializer.Serialize(entityModel), Encoding.UTF8, "application/json");

    }

    public void ITypeP1InTheCountryField(string p1)
    {
      var entityModel = ReadContent<EditableEventModel>(HttpRequest.Content);
      entityModel.Country = p1;
      HttpRequest.Content = new StringContent(JsonSerializer.Serialize(entityModel), Encoding.UTF8, "application/json");
      
    }

    public void ITypeP1InTheNameField(string p1)
    {
      var entityModel = ReadContent<EditableEventModel>(HttpRequest.Content);
      entityModel.Name = p1;
      HttpRequest.Content = new StringContent(JsonSerializer.Serialize(entityModel), Encoding.UTF8, "application/json"); 
    }

    public void IShouldntTypeIdOrCreationDateOfTheEvent()
    {
      using StreamReader stream = new StreamReader(new ReusableHttpContent(HttpRequest.Content).ReadAsStream());
      string content = stream.ReadToEnd().ToLower();
      Assert.False(content.Contains("id", StringComparison.OrdinalIgnoreCase));
      Assert.False(content.Contains("createddate", StringComparison.OrdinalIgnoreCase));

    }

    public void IShouldHaveSeenHTTPStatusIsP1(int p1)
    {
      var response = _factory.CreateClient().SendAsync(HttpRequest);
      response.Wait();
      HttpResponseMessage = response.Result;
      Assert.AreEqual((int)HttpResponseMessage.StatusCode, p1);
    }

    public void IShouldHaveSeenHTTPStatusIsP1(string p1)
    {
      int httpCode;
      int.TryParse(p1, out httpCode);
      IShouldHaveSeenHTTPStatusIsP1(httpCode);
    }

    
    public static T DeserializeAnonymousType<T>(string json, T anonymousTypeObject, JsonSerializerOptions options = default)
        => JsonSerializer.Deserialize<T>(json, options);

    public void TheCertainEventIsCreatedInTheDatabaseWithAnUniqueIdOfTheEventAndCreationTimeOfTheEvent()
    {
      var entity = new EventEntity();
      var definition = new { entity.Id, entity.CreatedDate};
      
      using StreamReader stream = new StreamReader(new ReusableHttpContent(HttpResponseMessage.Content).ReadAsStream());
      string content = stream.ReadToEnd();
      var deser = DeserializeAnonymousType(content, definition, new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = true
      });
      var events = Get<List<EventEntity>, EventFilter>(EVENT_ORGANIZER_ENDPOINT, new EventFilter() { Id = deser.Id }, out var statuscode);
      Assert.True(events.Any(e => e.CreatedDate == deser.CreatedDate));
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