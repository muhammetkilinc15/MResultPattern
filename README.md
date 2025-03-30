# ResultPattern for .NET

![NuGet Downloads](https://img.shields.io/nuget/dt/MResultPattern.svg)
![NuGet Version](https://img.shields.io/nuget/v/MResultPattern.svg)
![NuGet Pre-release Version](https://img.shields.io/nuget/vpre/MResultPattern.svg)
![License](https://img.shields.io/badge/license-MIT-blue.svg)



A robust implementation of the Result pattern for .NET applications, providing a standardized way to handle operation outcomes with proper status codes and error handling.

## Key Features

- **Structured responses** for both success and failure cases
- **Strongly-typed** results with generic support
- **HTTP status code** integration
- **Comprehensive error handling** with multiple message support
- **JSON-ready** serialization for API responses
- **Fluent API** for clean result handling

## Installation

```bash
dotnet add package ResultPattern
```

## Usage

#### Successful Operation
```csharp
var success = Result<Customer>.Success(new Customer { Id = 1, Name = "John" });
```

#### Failed Operation
```csharp
var failure = Result<Customer>.Failure(404, "Customer not found");
```

#### Web API Integration

```csharp
[HttpGet("{id}")]
public IActionResult GetCustomer(int id)
{
    var customer = _repository.Get(id);
    return customer != null 
        ? Ok(Result<Customer>.Success(customer))
        : NotFound(Result<Customer>.Failure(404, "Not found"));
}
```

#### Fluent Handling
```csharp
var result = _service.ProcessOrder()
    .OnSuccess(order => _logger.LogInformation($"Processed order {order.Id}"))
    .OnFailure(errors => _logger.LogError($"Failed: {string.Join(", ", errors)}"));
```

#### Multiple Errors
```csharp
var errors = new List<string> { "Invalid email", "Missing required field" };
var result = Result<User>.Failure(400, errors);

```

## API Reference
```csharp
Result<T>
T? Data - Operation data (null when failed)

List<string>? ErrorMessages - Error collection

int StatusCode - HTTP status code

bool IsSuccess - Success indicator

Static Methods:

Success(T data, int statusCode = 200)

Failure(int statusCode, List<string> errorMessages)

Failure(int statusCode, string errorMessage)

Failure(string errorMessage) (defaults to 500)
``` 
```csharp
Result
List<string>? ErrorMessages

int StatusCode

bool IsSuccess

Static Methods:

Success(int statusCode = 200)

Failure(int statusCode, List<string> errorMessages)

Failure(int statusCode, string errorMessage)

Failure(string errorMessage)
```
