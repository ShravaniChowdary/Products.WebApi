1. Update Connection Strings.
Open appsettings.json and update the SQL Server connection string accordingly.

2. Apply Migrations.
In the Package Manager Console, run the following commands individually to apply migrations:
Update-Database -Context "AuthDbContext"
Update-Database -Context "ProductDbContext"

3. Register a New User.
Use the /register endpoint and provide a valid email, username, and password.

4. Create the "Admin" Role.
Call the /add-role endpoint with "Admin" as the role name.

5. Assign the Admin Role to the User.
Use the /assign-role endpoint to assign the "Admin" role to the registered user.

6. Login to Retrieve JWT Token.
Use the /login endpoint with your credentials to receive a JWT token.

7. Use the JWT Token for Authorization.
In Swagger, use the built-in Authorize button.
Enter the token in the below format:
Bearer <your-token>
