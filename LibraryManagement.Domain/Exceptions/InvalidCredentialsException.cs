namespace LibraryManagement.Business.Exceptions;

public class InvalidCredentialsException : BusinessException
{
    public InvalidCredentialsException(string userName)
        : base($"Invalid login attempt for UserName: {userName}") { }
}
