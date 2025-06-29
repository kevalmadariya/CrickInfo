namespace crickinfo_mvc_ef_core.Models.Interfaces
{
    public interface IUserRepo
    {
        User GetUserById(int id);

        User GetUserByEmailId(string emailId);
        IEnumerable<User> GetUsers();
        User Add(User user);
        User Update(User user);
        string Delete(int id);
    }
}
