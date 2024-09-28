namespace WebApplication2022_Core_MVC_Repository.Models.Repo
{
    public interface IUserTableRepository
    {
        // 列出所有學員的紀錄。搭配 Repo控制器的「 List動作」
        IQueryable<UserTable> ListAllUsers();

        // 列出某一位學員的紀錄。搭配 Repo控制器的「 Details動作」
        UserTable GetUserById(int id);   // 確定只找出一筆記錄

        // 搜尋。搭配 Repo控制器的「 Search動作」
        IQueryable<UserTable> GetUserByName(string id);


        //===========================================
        // 新增。搭配 Repo控制器的「 Add動作」
        bool AddUser(UserTable _userTable);    // 新增成功則傳回 true，失敗則傳回false。
        //也可以寫成 void AddUser();   // 新增。沒有傳回值。

        // 刪除。搭配 Repo控制器的「 Delete動作」
        bool DeleteUser(int _ID);    // 刪除成功則傳回 true，失敗則傳回false。
        //也可以寫成void DeleteUser();   // 刪除。沒有傳回值。

    }
}
