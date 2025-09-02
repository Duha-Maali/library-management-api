namespace LibraryManagement.Business.Exceptions;

public class InvalidReturnDateException : BusinessException
{
    public InvalidReturnDateException()
        : base("ReturnDate cannot be before BorrowDate") { }
}
