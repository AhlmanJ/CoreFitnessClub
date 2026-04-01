namespace Application.Common.Results;

public enum ErrorTypes
{
    BadRequest = 400,
    Unauthorized = 401,
    NotFound = 404,
    Conflict = 409,
    InternalServerError = 500
}

public record Result(bool Success, ErrorTypes? ErrorTypes = null, string? ErrorMessage = null)
{
    public static Result Ok() => new(true);
    public static Result BadRequest(string message) => new(false, Results.ErrorTypes.BadRequest, message);
    public static Result Unauthorized(string message) => new(false, Results.ErrorTypes.Unauthorized, message);
    public static Result NotFound(string message) => new(false, Results.ErrorTypes.NotFound, message);
    public static Result Conflict(string message) => new(false, Results.ErrorTypes.Conflict, message);
    public static Result InternalServerError(string message = "An unexpected error occurred") => new(false, Results.ErrorTypes.InternalServerError, message);
}

public record Result<T>(bool Success, T? Value = default, ErrorTypes? ErrorTypes = null, string? ErrorMessage = null)
{
    public static Result<T> Ok(T Value) => new(true, Value);
    public static Result<T> BadRequest(string message) => new(false, default, Results.ErrorTypes.BadRequest, message);
    public static Result<T> Unauthorized(string message) => new(false, default, Results.ErrorTypes.Unauthorized, message);
    public static Result<T> NotFound(string message) => new(false, default, Results.ErrorTypes.NotFound, message);
    public static Result<T> Conflict(string message, T? Value = default) => new(false, Value, Results.ErrorTypes.Conflict, message);
    public static Result<T> InternalServerError(string message = "An unexpected error occurred") => new(false, default, Results.ErrorTypes.InternalServerError, message);
}