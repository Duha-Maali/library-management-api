namespace LibraryManagement.Business.Exceptions;

public class EntityNotFoundException : BusinessException
{
    public EntityNotFoundException(string entityName, int id)
        : base($"No {entityName} found with ID: {id}") { }
}
