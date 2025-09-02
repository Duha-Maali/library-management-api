namespace LibraryManagement.Business.Exceptions;

public class CategoryInUseException : BusinessException
{
    public CategoryInUseException(int categoryId)
        : base($"Cannot delete category with ID {categoryId} as it is used by existing books") { }
}