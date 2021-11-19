@identity @login
Feature: Log in
  Users are created during a first run of the event manager application.
  
Background: 
  Given preconfigured users are "jhond" and "alice"
   And "jhond"'s password is "alma1234"
   And "alice"'s password is "korte1234"
   And the endpoint of the login service is "api/v1/auth/login"
  
Scenario Outline: Alice log in with her username and password
  When I call the endpoint of the  log in api
   And I fill "alice" in the username parameter
   And I fill "korte1234" in the password parameter
  Then I should have seen HTTP status is "200"
   And I should have valid access token
   
Scenario Outline: Jhond log in with him username and password
  When I call log in api
   And I fill "jhond" in the username parameter
   And I fill "alma1234" in the password parameter
  Then I should have seen HTTP status is "200"
   And I should have valid access token


Scenario Outline: Unable to log in with wrong credentials
  When I call log in api
   And I fill "<username>" in the username parameter
   And I fill "<password>" in the password parameter
  Then I should have seen HTTP status is "401"
   And I haven't a valid access token

  Examples:
    | username | password |
    |    admin | admin    |
    |          | korte1234|
    | alice    |          |
    | alice    | alma1234 |

