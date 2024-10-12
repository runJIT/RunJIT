# Architectural Decision Record: Transformation of API Endpoints to CLI Commands

## Context

In our tool, we need to transform web API endpoints into command-line interface (CLI) commands. The APIs we work with might be defined using either the traditional ASP.NET Core Controller pattern or the newer minimal API pattern.
Our goal is to generate a `CLI` based user friendly client to be able to call our `APIs`  

### Example 1: Controller-Based API
```csharp
[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("v1/projects/{projectId}/todos")]
public class ToDosController : ControllerBase
{
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetAllToDoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [SwaggerOperation(OperationId = "get-all-todos", Tags = new[] { "ToDos" })]
    public Task<GetAllToDoResponse> GetAllToDoAsync(long projectId, long id)
    {
        throw new NotImplementedException();
    }
}
```

Example 2: Minimal API

```csharp
internal static RouteHandlerBuilder MapGetById(this IEndpointRouteBuilder routeGroupBuilder)
{
    return routeGroupBuilder.MapGet("todos/{id}", (int id, [FromServices] GetToDoById getToDoById) =>
    {
        var result = getToDoById.Execute(id);
        if (result is null)
        {
            return Results.NotFound();
        }
        return Results.Ok(result);
    })
    .WithSummary("Get ToDo by Id")
    .Produces(200, typeof(Todo))
    .Produces(400, typeof(ProblemDetails))
    .Produces(500, typeof(ProblemDetails))
    .WithName("getToDoByIdV1")
    .WithTags("ToDo")
    .WithDescription("Test Description")
    .WithOpenApi()
    .WithMetadata(new ObsoleteAttribute("This endpoint is obsolete. Use /new-endpoint instead."))
    .MapToApiVersion(1);
}
```

# Solutions

## Option 1: Transform API Parameters to CLI Options

These APIs have routes like /v1/projects/{projectId}/todos/{id} and might take parameters via path segments, query parameters, or even request bodies.

We are considering two primary options for transforming these API calls into CLI commands.

Decision
Option 1: Transform API Parameters to CLI Options
In this approach, each parameter in the API (whether it's a path parameter, query parameter, etc.) is directly mapped to a corresponding CLI option or argument. This makes the CLI command syntax highly descriptive and ensures that individual parameters are exposed explicitly in the command line.

Example API Call:

```bash
GET /v1/projects/1/todos/2
```

CLI Command Transformation:

```
// Version as command
myApiCli todos v1 getById --projectId 1 --id 2

// Version as parameter
myApiCli todos getById --projectId 1 --id 2 --version 1

```

### Pros:

* Explicit Parameters: Each parameter is individually represented in the command, making it clear what is being passed to the API.
* Command-Line Familiarity: Users of the CLI will recognize the structure of options and arguments that map directly to the API's parameters.
* Type Safety: The CLI options can enforce data types, validation, and required fields based on API metadata.

### Cons:

* Less Flexibility for Extended APIs: If the API is extended with additional parameters, the CLI must be updated to reflect those new parameters.
* Verbose CLI Commands: For APIs with many parameters, the CLI commands can become long and unwieldy.


## Option 2: Use JSON Payloads for Parameters

This approach allows users to pass a JSON payload as a single input to the CLI command, encapsulating all the required parameters. This method is more flexible and can handle future API changes without requiring changes to the CLI command structure itself.

Example API Call:

```bash
GET /v1/projects/1/todos/2
```

CLI Command Transformation:

```
myApiCli todos v1 getById
{ 
  "projectId": 1,
  "id": 2
}
```

```
myApiCli todos getById --version 1
{ 
  "projectId": 1,
  "id": 2
}
```

```
myApiCli todos getById
{ 
  "version": 1
  "projectId": 1,
  "id": 2
}
```

### Pros:

* Flexibility: Since the entire request is encapsulated in a JSON object, this approach can easily handle extended or modified APIs without changing the CLI command structure.
* Cleaner Command: The command itself is simple, with the payload handling all the details of the parameters.
* Future-Proofing: The CLI does not need to be updated if the API introduces new parameters, provided the API continues to accept a JSON payload.

### Cons:

* Complexity for Users: Users of the CLI will need to format their input as JSON, which might introduce a barrier for those unfamiliar with JSON or who prefer traditional command-line arguments.
* Error Handling: Validation of the JSON payload may be more challenging compared to handling individual CLI options.

### Option 3: Hybrid Approach (Both Options)

We choose to support both options to provide users with flexibility based on their use case.

1. **Option 1** will be the default for simple use cases where clarity and explicit control of parameters are preferred.
2. **Option 2** will be available as an alternative, particularly for cases where the API is complex, frequently evolving, or where users prefer to work with JSON for bulk operations.

### Implementation Details

- For **Option 1**, the generator will:
  - Extract parameters from API routes, query strings, and request bodies, mapping them to `System.CommandLine` options or arguments.
  - Generate commands with appropriate flags for each parameter.

- For **Option 2**, the generator will:
  - Support a `--json` flag or argument where the user can pass a JSON payload containing all parameters.
  - Parse the JSON payload within the CLI and pass it as a request body or appropriate parameters to the API.

### Consequences

#### Positive

- **Flexibility:** Supporting both options ensures that the CLI is flexible and can adapt to various user preferences and API complexities.
- **Scalability:** Option 2 ensures that as APIs evolve, the CLI can continue to function without frequent updates.
- **User-Friendly:** Option 1 will provide a simple, user-friendly experience for those familiar with command-line interfaces.

#### Negative

- **Complexity:** Supporting two different modes of operation adds complexity to both the CLI tool and the generator.
- **Increased Codebase:** The generator will need to support two modes, increasing the maintenance burden.

