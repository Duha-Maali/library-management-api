namespace LibraryManagement.Business.Exceptions;

public class CategoryNameDuplicateException : BusinessException
{
    public CategoryNameDuplicateException(string categoryName)
        : base($"Category with Name: {categoryName} already exists") { }
}
