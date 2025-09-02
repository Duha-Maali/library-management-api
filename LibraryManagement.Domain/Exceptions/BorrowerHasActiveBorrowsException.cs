namespace LibraryManagement.Business.Exceptions;

public class BorrowerHasActiveBorrowsException : BusinessException
{
    public BorrowerHasActiveBorrowsException(int borrowerId)
        : base($"Cannot delete borrower with ID {borrowerId} due to active borrows") { }
}