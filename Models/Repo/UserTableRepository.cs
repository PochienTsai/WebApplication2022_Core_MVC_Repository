//******************************************************************
using WebApplication2022_Core_MVC_Repository.Models;  // 自己動手寫上命名空間 -- 「專案名稱.Models」。
// *** Repository（倉庫）******************************************
// (1).  搭配 Models 目錄。
// (2). 「介面（Interface）」檔名為 IUserTableRespository.cs
// (3). 「類別」檔名為 UserTableRepository.cs
//
//
// 拜託！拜託！不要只會 Copy程式碼！
// 拜託！拜託！  Program.cs設定檔  也比對一下！......每次程式改成 .NET Core都會在這裡漏掉
//*******************************************************************


namespace WebApplication2022_Core_MVC_Repository.Models.Repo
{
    public class UserTableRepository : IUserTableRepository, IDisposable    // *** 重點 ******
    {    //                                          // **********************   **********不寫IDisposable會出現這種「警告」https://msdn.microsoft.com/library/ms182172.aspx

        //**********   連結 MVC_UserDB 資料庫  *********************** (start)
        private readonly MVC_UserDBContext _db = new MVC_UserDBContext();
        //**********   連結 MVC_UserDB 資料庫  *********************** (end)


        private readonly ILogger<UserTableRepository> _logger;
        public UserTableRepository(ILogger<UserTableRepository> logger, MVC_UserDBContext context)
        {  //                                                                                          ****************************（自己動手加上）
            _logger = logger;
            _db = context;    //*****************************（自己動手加上）
           // https://blog.givemin5.com/asp-net-mvc-core-tao-bi-hen-jiu-di-yi-lai-zhu-ru-dependency-injection/
        }



        //*************************************請自己動手加上這一段。不然會報錯！
        public UserTableRepository()
        {
        }
        //*************************************請自己動手加上這一段。不然會報錯！


        public bool AddUser(UserTable _userTable)
        {
            try
            {
                _db.UserTables.Add(_userTable);
                _db.SaveChanges();
                return true;
            }
            catch
            {
                //throw new NotImplementedException();
                return false;
            }
        }

        public bool DeleteUser(int _ID)
        {
            try
            {
                //// 第二種方法（作法類似後續的 Edit動作）
                //// 必須先鎖定、先找到這一筆記錄。找得到，才能刪除！
                //UserTable ut = _db.UserTables.Find(_ID);
                //_db.Entry(ut).State = System.Data.Entity.EntityState.Deleted;  //確認刪除一筆（狀態：Deleteed）
                //_db.SaveChanges();
                ////**** 刪除以後，必須執行 .SaveChanges()方法，才能真正去DB刪除這一筆記錄 ****

                // 第三種方法。必須先鎖定、先找到這一筆記錄。找得到，才能刪除！
                UserTable ut = _db.UserTables.Find(_ID);
                _db.UserTables.Remove(ut);
                _db.SaveChanges();

                //return Content(" 刪除一筆記錄，成功！");    // 刪除成功後，出現訊息（字串）。
                return true;
            }
            catch
            {
                //throw new NotImplementedException();
                return false;
            }
        }

        // Details。主表明細的「明細」。
        public UserTable GetUserById(int id)
        {
            return _db.UserTables.Find(id);
            //throw new NotImplementedException();
        }


        // 搜尋。 
        public IQueryable<UserTable> GetUserByName(string id)
        {
            return _db.UserTables.Where(s => s.UserName.Contains(id));
            //throw new NotImplementedException();
        }

        public IQueryable<UserTable> ListAllUsers()
        {
            return _db.UserTables;
            //throw new NotImplementedException();        
        }


        //public void Db_Dispose()
        //{   // 關閉資料庫的連結並釋放資源。
        //        _db.Dispose();
        //        throw new NotImplementedException();
        //}

        //========================================================
        // 不寫下面這一段IDisposable，
        // 「建置」=>「程式碼效能分析」會出現 "警告"訊息 https://msdn.microsoft.com/library/ms182172.aspx

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {   // dispose managed resources
                _db.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }

}
