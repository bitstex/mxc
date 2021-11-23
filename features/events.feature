@create-event @modify-event @list-events
Feature: Event organizer
  Logged in users can create a new event, list all events, query the specific event by guid of the event and modify the selected event  

Background: 
  Given caller presents a valid access token
   And the endpoint of the events controller "api/v1/organizer/events" is exist
   
Scenario: Jhond user post a valid event without a capacity to the endpoint
  Given "koncert1" event doesn't exist in the database
  When I post a new event to the endpoint
   And I type "Budapest" in the location field
   And I type "Magyarorszag" in the country field
   And I type "koncert1" in the name field
   But I shouldn't type id or creation date of the event
  Then I should have seen HTTP status is "200"
   And the certain event is created in the database with an unique id of the event and creation time of the event
   
Scenario: Alice user post a valid event with a capacity to the endpoint
  Given "koncert2" event doesn't exist in the database
  When I post a new event to the endpoint
   And I type "Budapest" in the location field
   And I type "Magyarorszag" in the country field
   And I type "koncert2" in the name field
   And I type "100" in the capacity field
   But I shouldn't type id or creation date of the event 
  Then I should have seen HTTP status is "200"
   And the certain event is created in the database with an unique id of the event and creation time of the event
   
Scenario: Jhond user post a valid event without a country to the endpoint
  Given "koncert3" event doesn't exist in the database
  When I post a new event to the endpoint
   And I type "Budapest" in the location field
   And I type "koncert3" in the name field
   And I type "100" in the capacity field
   But I shouldn't type id or creation date of the event 
  Then I should have seen HTTP status is "200"
   And the certain event is created in the database with an unique id of the event and creation time of the event
   
   
Scenario Outline: User want to create a new event but some parameters are worng
  Given "<name>" event doesn't exist in the database
  When "<username>" post a new event to the endpoint
   And I type "<location>" in the location field
   And I type "<country>" in the country field
   And I type "<name>" in the name field
   And I type "<capacity>" in the capacity field
   But I shouldn't type id or creation date of the event
  Then I should have seen HTTP status is "<code>"
   And I should have seen the "<message>" error message 
  Examples:
      |  username |     name     |  location     |       country   | capacity |  code    |                 message                 |
      |           |oldtimer autok| Pusztamonostor|   Magyarorszag  |    100   |   403    |      Please log in and try again        |   
      | jhond     |              | Pusztamonostor|   Magyarorszag  |          |   500    |      Name of the event is must          |
      | alice     |oldtimer autok|               |                 |          |   500    |      Location of the event is must      |
      | alice     |koncert       | Budapest      |   Magyarorszag  |    -1    |   500    |      The capacity of the event must be a positive number|
      | jhond     |              |               |                 |    100   |   500    |      [Name of the event is must, Location of the event is must]|   

Scenario: Jhond user post an event with more than 100 characters long location name to the endpoint
  Given "koncert4" event doesn't exist in the database
  When I post a new event to the endpoint
   And I type 101 characters long text in the location field
   And I type "Magyarorszag" in the country field
   And I type "koncert4" in the name field
   But I shouldn't type id or creation date of the event
  Then I should have seen HTTP status is "500"
   And I should have seen the "Length of the location is more than 100 characters long" error message 


Scenario Outline: Jhond user put a modificated event to the endpoint
  Given "<oldevent>" event does exist in the database
   And "<oldlocation>" is the location of the event
   And "<oldcountry>" is the country of the event
   And "<oldcapacity>" is the capacity of the event
   And "<newevent>" event doesn't exist in the database
  When I put the modificated event to the endpoint
   And I type "<newlocation>" in the location field
   And I type "<newcountry>" in the country field
   And I type "<newevent>" in the name field
   And I type "<newcapacity>" in the capacity field
   But I shouldn't type id or creation date of the event 
  Then I should have seen HTTP status is "200"
   And I should have seen the name of the event is "<newevent>"
   And I should have seen the location of the event is "<newlocation>"
   And I should have seen the country of the event is "<newcountry>"
   And I should have seen the capacity of the event is "<newcapacity>"
  Examples:
    | oldevent | newevent | oldlocation | newlocation | oldcountry | newcountry | oldcapacity | newcapacity |
    |koncert81 |koncert91 |Budapest     | Budapest    |            |            |             |             |
    |koncert82 |koncert82 |Budapest     | Pecs        |            |Magyarorszag|100          |             |
    |koncert83 |koncert93 |Budapest     | Siofok      |Magyarorszag|            |             | 100         | 

Scenario Outline: User want to modify the event but some parameters are worng
  Given "koncert101" event does exist in the database
  When "<username>" put the modificated event to the endpoint
   And I type "<location>" in the location field
   And I type "<country>" in the country field
   And I type "<name>" in the name field
   And I type "<capacity>" in the capacity field
   But I shouldn't type id or creation date of the event
  Then I should have seen HTTP status is "<code>"
   And I should have seen the "<message>" error message 
  Examples:
      |  username |     name     |  location     |       country   | capacity |  code    |                 message                 |
      |           |oldtimer autok| Pusztamonostor|   Magyarorszag  |    100   |   403    |      Please log in and try again        |   
      | jhond     |              | Pusztamonostor|   Magyarorszag  |          |   500    |      Name of the event is must          |
      | alice     |oldtimer autok|               |                 |          |   500    |      Location of the event is must      |
      | alice     |koncert       | Budapest      |   Magyarorszag  |    -1    |   500    |      The capacity of the event must be a positive number|
      | jhond     |              |               |                 |    100   |   500    |      [Name of the event is must, Location of the event is must]| 
      
Scenario: Jhond user put the event with more than 100 characters long location name to the endpoint
  Given "koncert4" event does exist in the database
  When I put the modificated event to the endpoint
   And I type 101 characters long text in the location field
   But I shouldn't type id or creation date of the event
  Then I should have seen HTTP status is "500"
   And I should have seen the "Length of the location is more than 100 characters long" error message 
  
Scenario: Jhond user want to query details about the certain event by event id
  Given id of the "kocnert103" event
   And "koncert103" event does exist in the database
   And "Budapest" is the location of the event
   And "Magyarorszag" is the country of the event
   And "100" is the capacity of the event
  When I get the event by id
  Then I should have seen HTTP status is "200"
   And I should have seen the name of the event is "koncert103"
   And I should have seen the location of the event is "Budapest"
   And I should have seen the country of the event is "Magyarorszag"
   And I should have seen the capacity of the event is "100"
   And I should have seen the id and creation date of the certain event
   
Scenario: Anonymus user want to query details about the certain event by event id
   When Anonymus get the event by id 
   Then Anonymus should have seen HTTP status is "403"
    And Anonymus should have seen the "Please log in and try again" error message 

Scenario: Anonymus user want to query all events
   When Anonymus get all event
   Then Anonymus should have seen HTTP status is "403"
    And Anonymus should have seen the "Please log in and try again" error message
    
Scenario Outline: Johnd user query all events with paginator
  Given I have "<total count>" events
   And I have "<page>" pages
   And I have "<limit>" elements per page
  When I get all events by the "<current page>". page
  Then I should see "<count>" events are there in the result of the api call
   And each elements of the result is contains all property of the event
  Examples:
    | total count | page | limit | current page  | count |
    | 0           |  0   | 10    |    0          |   0   |
    | 50          |  1   | 50    |    1          |   50  |
    | 100         | 10   | 10    |   10          |   10  |
    | 99          | 10   | 10    |   10          |   9   | 