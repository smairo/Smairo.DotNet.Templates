# Summary
These are my commonly used templates so i can kick off a new project fast and focus on coding straight away.

# Smairo.Template.Api
My opinionated way that the .net core Web API with Swagger, IdentitySerer4 and EfCore CodeFirst + Dapper should be done. If you wish to get documented api, that has authentication (IdentitySerer4 here) and do something with the database, then this template can cut amount of setup for you.

You can install api template from nuget:
```
dotnet new -i Smairo.Template.Api
```

After that you can use it to create a solution:
```
dotnet new api-swagger -n "NameForYourSolution"
```

# Smairo.Template.Console
Dependency injection console with coreapp2.0 ready to go.

You can install console template from nuget:
```
dotnet new -i Smairo.Template.Console
```

After that you can use it to create a solution:
```
dotnet new smairo-console -n "NameForYourSolution" -A "NameForYourApplication"
```

# Smairo.Template.AzureFunctions.Timer
Dependency injection azure function with v2 and timer trigger ready to go.

You can install timer function template from nuget:
```
dotnet new -i Smairo.Template.AzureFunction.Timer
```

After that you can use it to create a solution:
```
dotnet new smairo-function-timer -n "NameForYourSolution" -F "NameForYourFunction"
```