namespace crickinfo_mvc_ef_core.Models.Interface
{
    public interface IUserRepo
    {
        User GetUserById(int id);
        IEnumerable<User> GetUsers();
        User Add(User user);
        User Update(User user);
        User Delete(int id);
    }
}
