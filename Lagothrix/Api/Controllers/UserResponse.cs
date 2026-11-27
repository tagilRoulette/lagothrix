namespace Api.Controllers;

public record UserResponse(string Id, string Name, string Email, DateTime RegistrationDateTime);