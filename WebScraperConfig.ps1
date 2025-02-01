mkdir WebScraperSolution
cd WebScraperSolution

# Create a .NET solution
dotnet new sln

# Create the library project
dotnet new classlib -n WebScraperLib
dotnet add WebScraperLib package HtmlAgilityPack

# Create a console app to test the library
dotnet new console -n WebScraperApp
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Microsoft.Extensions.Http
dotnet add WebScraperApp reference WebScraperLib

# Create an xUnit test project
dotnet new xunit -n WebScraperTests
dotnet add WebScraperTests reference WebScraperLib
dotnet add WebScraperTests package Moq

# Add all projects to the solution
dotnet sln add WebScraperLib/WebScraperLib.csproj
dotnet sln add WebScraperApp/WebScraperApp.csproj
dotnet sln add WebScraperTests/WebScraperTests.csproj
