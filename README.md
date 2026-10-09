## CLAIMS API

Backend code application for insurance claims handling. Users can make operations like creating, reading and deleting COVERS (ship insurance) and CLAIMS (damage reports).



Build with .NET 9, ASP.NET Core, EF Core, MongoDB and SQL Server.



\-----------------------



## HOW TO START APP


\~\~Needs\~\~ .NET 9 SDK and Docker Desktop (running mode)

&#x20;

dotnet run --project Claims



The APIs can be tested in : \*\*https://localhost:7052/swagger\*\*



SQL Server and MongoDB are started automatically in Docker Desktop via Testcontainers.
The first start can take around 10 minutes while the images are downloaded.



\---------------------



## HOW TO RUN TESTS



dotnet test

\~\~ or

Test from Visual Studio Test -> Test Explorer -> Run All 



There are two types of tests:
 \~\~ Integration test : `ClaimsControllerTests` needs Docker in running mode
 \~\~ Unit tests : All other tests , need nothing else.



\--------------------



## WHAT I CHANGED

The cloned template didn't start at the first try, so i fixed a few things: the audit entities didn't match migration, `BsonDateTimeOptions(DateOnly)` isn't supported by the Mongo EF provider, and `.gitignore` was ignoring the `Data` folder.



\*\*Task 1 

Split the code into separated things/responsibilities: controllers, services and repositories. Controllers only deal with HTTP now. Added request DTOs so the client cant send data that logically cannot be sent. Everything is injected through interfaces.





\*\*Task 2
Added validators for clams and covers based on the rules written on the task.





\*\*Task 3

Auditing cannot block the requests anymore. The request puts a message on an in-memory queue, and a background service saves it to the database.





\*\*Task 4

Unit tests for the auditer (using moq), premium calculation formulas, validators, claim service. Also the existing integration test is improved so it checks if the return status is 200 OK, if the response is JSON format and if the response body is a list of claims.





\*\*Task 5
The old loop of the calculation charged some days two or three times because the if statements weren't chained with else. Fixed it and added tests. I calculated the expected values by hand from the rules/formulas in the task and then tested from the unit tests.






## Notes



* I counted the cover period without the end date, so from 1 November to 11 November is calculated 10 days.
* The audit queue is in memory, so its fine for this task but id use a proper message queue like Azure Service Bus in production








