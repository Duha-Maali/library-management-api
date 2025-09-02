namespace LibraryManagement.Business.Exceptions;

public class BorrowNotReturnedException : BusinessException
{
    public BorrowNotReturnedException(int borrowId)
        : base($"Cannot delete borrow with ID {borrowId} that is not returned") { }
}
