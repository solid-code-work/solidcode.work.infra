namespace solidcode.work.infra.Entities;

public enum MessageErrorType
{
    Success,        // 200 OK
    Validation,     // 400 Bad Request
    Unauthorized,   // 401 Unauthorized
    NotFound,       // 404 Not Found
    Conflict,       // 409 Conflict
    Exception,      // 500 Internal Server Error
    DatabaseError,  // 500 or custom DB error
    EmptyResult     // 204 No Content or custom
}

