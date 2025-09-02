namespace LibraryManagement.Business.Exceptions;

public class InvalidCategoryException : BusinessException
{
    public InvalidCategoryException(int categoryId)
        : base($"Invalid Category ID: {categoryId}") { }
}