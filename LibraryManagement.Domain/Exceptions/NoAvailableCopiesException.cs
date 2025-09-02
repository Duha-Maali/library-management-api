namespace LibraryManagement.Business.Exceptions;

public class NoAvailableCopiesException : BusinessException
{
    public NoAvailableCopiesException(int bookId)
        : base($"No available copies of the book with BookId: {bookId}") { }
}
