# Example Web API
Example including:
- Three layered .NET Core 8 Web Application
- JWT Bearer tokens
- Authentication and Authorisation using Identity
- Example setup of controller
- Configured Program.cs enabling the above

Can be used with a .NET MAUI application where the JWT token can be fetched and passed using HTTP requests. 


# Steps to get up-and-running
1. In _appsettings.json_ (`ExampleWebApi.Api`) make sure the connectionstring is correct for your (local) database. The example project uses the EF SQLServer package. 

2. Add a migration and update your database:
    - In the Package Manager Console (PMC) set `ExampleWebApi.Infrastructure` as the 'Default Project' 
    - In PMC run: `Add-Migration initial`
    - In PMC run: `update-database`

3. Run the API and test the functionality via the Swagger-page (or another tool). 
    - Check the `/test` endpoint: it should work! 
    - Check the `/testauth` route: it should **not** work
    - Create a new user via `/api/Authentication/register`
    - Login via `/api/Authentication/login`, checkout the response and copy the `token`-string
    - Use this token in the authentication header of your next requests. In Swagger: click Authorize on top of the page. Fill in `Bearer <paste_the_copied_token>`, **important**: there is a space after `Bearer`.
    - The `/testauth` route now works (and prints out you unique user_id)

 