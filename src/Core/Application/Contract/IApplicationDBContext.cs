namespace Application
{
    public interface IApplicationDBContext
    {
        DbSet<Event> Events {set;get;}
    }
}
