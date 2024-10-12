# Architectural Decision Record: Generation of CLI Commands from Web API Controllers

## Context

We are developing a code generator that will create a .NET global tool (CLI) to communicate with a web API. This generator will analyze an existing source solution, identify the web API project, parse its controllers or minimal APIs, and then generate CLI commands using the `System.CommandLine` NuGet package. The generated CLI tool will allow users to interact with the web API via the command line, providing a streamlined and efficient way to automate API tasks.

The project needs to handle:

- Parsing of a C# solution to identify Web API components.
- Mapping of API endpoints (e.g., controllers, minimal APIs) to corresponding CLI commands.
- Generation of a standalone .NET tool (CLI) that follows best practices for command-line interface development.

## Decision

The tool will utilize reflection and/or Roslyn-based code parsing to analyze the web API project and extract the necessary metadata (e.g., controller names, methods, routes). The information will then be mapped to `System.CommandLine` commands that mimic the behavior of the web API endpoints.

### Key Elements of the Design

1. **API Discovery and Parsing:**
   - Use Roslyn to parse the source solution and identify web API projects based on project references, packages, or certain naming conventions (e.g., projects referencing `Microsoft.AspNetCore`).
   - Extract controller or minimal API definitions from the codebase, including routes, methods, parameters, and return types.
   
2. **Mapping API to CLI Commands:**
   - Each API controller or minimal API endpoint will be mapped to a corresponding CLI command.
   - For instance, a `GET /api/users` endpoint may be translated to a CLI command `dotnet-tool users list`.
   - Parameters in the API method (query parameters, path variables) will be reflected as CLI options or arguments. For example, `GET /api/users/{id}` will map to `dotnet-tool users get --id <id>`.

3. **Use of `System.CommandLine`:**
   - `System.CommandLine` will be the framework for defining the CLI commands and options.
   - A root command will be established (`dotnet-tool`), with subcommands corresponding to controllers (or API groups). Each subcommand will have further subcommands based on the HTTP methods (e.g., `get`, `post`).
   - Command descriptions and help texts will be generated based on the API documentation or method summaries.

4. **Code Generation Workflow:**
   - The generator will output C# code that defines the CLI using `System.CommandLine` constructs.
   - The generated tool will follow the .NET global tool pattern, which includes a `Program.cs` file that sets up the CLI commands and invokes them based on user input.
   - Special care will be taken to ensure proper error handling and validation of command-line arguments.

### Alternatives Considered

1. **Reflection vs. Roslyn for API Parsing:**
   - **Reflection** provides a runtime solution to inspect the controllers but lacks the flexibility to modify or extend the source. Additionally, it doesn't handle minimal APIs well.
   - **Roslyn** offers a more powerful, compile-time analysis and allows deeper insights into the code structure, enabling better support for both controllers and minimal APIs. Therefore, Roslyn was chosen as the parsing mechanism.

2. **Manual vs. Automatic Command Mapping:**
   - Manual mapping of API endpoints to CLI commands could provide more control, but would introduce significant maintenance overhead.
   - Automatic generation of commands based on API metadata offers a scalable solution, reducing manual intervention while ensuring consistency across API updates. Automatic mapping was chosen for its scalability and maintainability.

## Consequences

- **Positive:**
  - Automating the generation of CLI commands ensures that the tool is kept in sync with the web API, reducing manual maintenance.
  - Using `System.CommandLine` provides a flexible and well-supported framework for building robust CLI tools.
  - Developers will have a simple way to interact with the web API via the command line, improving automation and scripting capabilities.

- **Negative:**
  - The reliance on Roslyn introduces some complexity in parsing and analyzing the source solution, especially for larger projects.
  - The generated CLI tool will only be as accurate as the API parsing logic, which could become outdated if the web API evolves without corresponding updates to the generator.

## Implementation Plan

1. Develop a prototype parser using Roslyn to identify controllers and minimal APIs in the solution.
2. Implement the mapping logic to translate API endpoints into `System.CommandLine` commands.
3. Generate a basic `.NET global tool` that can serve as a CLI to interact with a sample web API.
4. Refine the generator to handle complex API structures (e.g., nested routes, authentication).
5. Document usage patterns and provide an easy-to-follow guide for developers to extend or modify the CLI tool.
