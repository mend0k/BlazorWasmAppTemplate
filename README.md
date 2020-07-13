# BlazorWasmAppTemplate
Starter template for a Blazor Web Assembly project


Blazor Webassembly template with custom authentication using JWT based authentication (you can still add 3rd party auth like google auth etc... if you desire) and SQL data access.  Template is currently using Blazor WebAssembly version 3.2.0


A lot of code is blazor wasm's template code combined with samSandersonMS auth example code from MissionControl. I added sql access and comments trying to explain what's going on. 


- Built using Blazor WebAssembly version 3.2.0
- Uses local storage to store token
- Comments explaining what is happening as clearly as possible (at least I tried)
- Using DapperWrapper sql package

You should be able to jump straight into the core functionality/features of your app with this template. 


SIDE NOTE: 'DapperWrapper' like its name implies... acts as a wrapper for the dapper package. Its main goal is to completely abstract all sql (or as much as possible) and make db interactions more seamless. 


Instead of setting up connection, base classes and interfaces, writing out tedious and/or long queries & "sql boiler code" you can just:

1. Ensure that your default connection string in your appsettings.json file is called "Default"

2. Perform a db transaction in a single line. 


Examples:


  a. "_model.LoadRecordWhere<User>("FirstName = 'paul'")"; // returns the first record of model of type 'User' with a first name of 'paul'
  
  
  b. "_model.SelectAll<User>(); // returns all records in the User table
  
  
  c. "_model.SelectWhereOrderByJoin(tbl1Name, tbl2Name, joinOn, sWhere = "", sOrderBy = "")" // joins tables 1 on table 2 on the specified join column with optional sWhere & sOrderBy params
