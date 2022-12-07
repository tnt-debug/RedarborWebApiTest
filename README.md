# Redarbor WebAPI test

In this repository you can find the redarbor test of the WebAPI. 
This project creates the necessary endpoints to perform basic CRUD operations of and entity in the db.

Uses MVC pattern, EF for writes and Dapper for reads. MSUnit test project appended in the sln also.

# How to use

1. Download or clone the project.
2. Replace the "DefaultConnection" key in appsettings.json with your database parameters:

  "ConnectionStrings": {
    "DefaultConnection": "Server={server};Database=redarbor;Integrated Security=False;Uid={userdb};Pwd={password};"
  }

3. Also replace the appsettings.test.json in the unit test project "EmployeesTests".

4. Run it!