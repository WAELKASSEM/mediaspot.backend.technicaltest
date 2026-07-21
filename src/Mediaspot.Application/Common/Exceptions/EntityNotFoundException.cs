namespace Mediaspot.Application.Common.Exceptions;

public class EntityNotFoundException : Exception
{
    private EntityNotFoundException(string message) : base(message)
    {
        
    }
    public static EntityNotFoundException ForType<T>(Guid id)
    {
        return new EntityNotFoundException($" Entity of Type : {typeof(T).Name} with Id : {id} doesn't exist");
    }
}
