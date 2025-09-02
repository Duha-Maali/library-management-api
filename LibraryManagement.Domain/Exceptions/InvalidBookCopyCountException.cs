namespace LibraryManagement.Business.Exceptions;

public class InvalidBookCopyCountException : BusinessException
{
    public InvalidBookCopyCountException()
        : base("Total copies cannot be less than available copies") { }
}
