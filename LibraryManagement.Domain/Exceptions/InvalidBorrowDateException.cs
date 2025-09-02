namespace LibraryManagement.Business.Exceptions;

public class InvalidBorrowDateException : BusinessException
{
    public InvalidBorrowDateException()
        : base("DueDate must be after BorrowDate") { }
}
