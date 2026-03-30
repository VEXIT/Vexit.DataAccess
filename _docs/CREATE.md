|              |                                                       |
| ------------ | ----------------------------------------------------- |
| Copyright    | © 2026 VEXIT ® , Tomorrow is today... , www.vexit.com |
| Author       | Vex Tatarevic                                         |
| Date Created | 2026-03-10                                            |
| Date Updated |                                                       |

# Vexit.DataAccess - Creation Guide

## Create Workspace

Open terminal (GitBash on Windows) and type commands to create workspace

```bash
mkdir ~/dev/
cd ~/dev
mkdir -p Vexit.DataAccess/src Vexit.DataAccess/tests Vexit.DataAccess/_docs
```

Your project structure should now be:
```
~/dev/Vexit.DataAccess/
├── _docs/
├── src/
└── tests/
```

## Create .NET Class Library Project

```bash
dotnet new classlib -o Vexit.DataAccess/src -n Vexit.DataAccess

rm Vexit.DataAccess/src/Class1.cs # Remove default class file
```

## Add NuGet Packages

```bash
dotnet add Vexit.DataAccess/src package Microsoft.EntityFrameworkCore   # Entity Framework Core for database access and ORM functionality

dotnet list Vexit.DataAccess/src package                                # Verify project contains the added packages
```

## Add Project References

```bash
dotnet add Vexit.DataAccess/src reference Vexit/src
```

## Generate .gitignore file

```bash
dotnet new gitignore --output Vexit.DataAccess/src
```

## Set Initial Version

```bash
sed -i '/<\/PropertyGroup>/i\    <Version>1.0.0<\/Version>' Vexit.DataAccess/src/Vexit.DataAccess.csproj # Add version to Vexit.DataAccess.csproj
```

## Build

```bash
dotnet build Vexit.DataAccess/src
```

## Create Test Project

```bash
Vexit.Scripts/create-dotnet-unittests-project.sh Vexit.DataAccess/src Vexit.DataAccess/tests
```

## Run Tests

Below are a few different ways to run tests:

- Run tests - this command shows just the summary of the test execution

  ```bash
  dotnet test Vexit.DataAccess/tests/
  ```
- Run tests list - shows all test names

  ```bash
  dotnet test Vexit.DataAccess/tests/ --list-tests
  ```

- Run tests with detailed console output

  ```bash
  dotnet test Vexit.DataAccess/tests/ --logger "console;verbosity=normal"
  ```

- Run tests and generate an HTML report inside `TestResults/TestResults.html`

  ```bash
  dotnet test Vexit.DataAccess/tests/ --logger "html;LogFileName=TestResults.html"
  ```


---

*© 2026 VEXIT ® | Tomorrow is today...® | [www.vexit.com](https://www.vexit.com)*