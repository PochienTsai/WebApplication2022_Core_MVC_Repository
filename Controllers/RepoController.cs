using Microsoft.AspNetCore.Mvc;

using WebApplication2022_Core_MVC_Repository.Models;  // 自己動手寫上命名空間 -- 「專案名稱.Models」。
using WebApplication2022_Core_MVC_Repository.Models.Repo;
// *** Repository（倉庫）******************************************
// (1).  搭配 Models 目錄。
// (2). 「介面（Interface）」檔名為 IUserTableRespository.cs
// (3). 「類別」檔名為 UserTableRepository.cs
//
//
// 拜託！拜託！不要只會 Copy程式碼！
// 拜託！拜託！  Program.cs設定檔  也比對一下！......每次程式改成 .NET Core都會在這裡漏掉
//
//*******************************************************************


namespace WebApplication2022_Core_MVC_Repository.Controllers
{
    public class RepoController : Controller
    {
        //******************************   連結 MVC_UserDB 資料庫  ************************** (start)
        #region
        // 注意 Startup.cs裡面的設定！必須使用  依賴性注入（DI）。所以後面不需要 = new IUserTableRepository ();
        private readonly IUserTableRepository _myDB;   // 程式碼放在 /Repositories目錄底下。一個介面＋一個類別。

        public RepoController(IUserTableRepository repository)
        {
            _myDB = repository;
        }
        #endregion
        //******************************   連結 MVC_UserDB 資料庫  ************************** (end)



        public IActionResult Index()
        {
            return View();   // 空白的畫面與動作。 無作用。
        }



        //===================================
        //== 新增 ==
        //===================================
        //*** 重點！！本範例 加入檢視畫面時，"沒有" 勾選「參考指令碼程式庫」，所以沒有產生表單驗證
        public ActionResult Create()
        {
            return View(); 
        }

        [HttpPost, ActionName("Create")]   // 把下面的動作名稱，改成 CreateConfirm 試試看？
        [ValidateAntiForgeryToken]   // 避免XSS、CSRF攻擊
        public ActionResult Create(UserTable _userTable)
        {
            if ((_userTable != null) && (ModelState.IsValid))
            {
                if (_myDB.AddUser(_userTable))
                {
                    //return Content(" 新增一筆記錄，成功！");    // 新增成功後，出現訊息（字串）。
                    return RedirectToAction("List");
                }
            }
            return Content(" 新增一筆記錄，*** 失敗！***");    // 新增失敗後，出現訊息（字串）。
        }


        //===================================
        //== 列表（Master） ==  暫無分頁功能。
        //===================================
        public ActionResult List()
        {
            IQueryable<UserTable> ListAll = _myDB.ListAllUsers();
            //第二種寫法：
            if (ListAll == null)
            {   // 找不到任何記錄
                //return NotFound();
                return Content(" 找不到任何記錄，*** List 失敗！***");
            }
            else
            {
                return View(ListAll.ToList());   //直接把 UserTables的全部內容 列出來
            }
        }


        //===================================
        //== 列出一筆記錄的明細（Details） ==
        //===================================
        //[HttpPost]    // 改成這樣會報錯。請輸入網址，看見了什麼？？？？ /UserDB/Details?_ID=4
        ////                 // 錯誤訊息 -- '/' 應用程式中發生伺服器錯誤。        找不到資源。 
        [HttpGet]
        public ActionResult Details(int _ID)    // 網址 http://xxxxxx/UserDB/Details?ID=1 
        {
            // 第四種寫法：
            UserTable ut = _myDB.GetUserById(_ID);

            if (ut == null)
            {   // 找不到這一筆記錄
                //return NotFound();
                return Content(" 找不到任何記錄，*** Details 失敗！***");
            }
            else
            {
                return View(ut);
            }
        }


        //===================================
        //== 刪除 ==
        //===================================

        //== 刪除前的 Double-Check，先讓您確認這筆記錄的內容？
        public ActionResult Delete(int _ID)    // 網址 http://xxxxxx/UserDB/Delete?_ID=1 
        {
            // 使用上方 Details動作的程式，先列出這一筆的內容，給使用者確認
            UserTable ut = _myDB.GetUserById(_ID);

            if (ut == null)
            {   // 找不到這一筆記錄
                //return NotFound();
                return Content(" 找不到任何記錄，*** 失敗！***");
            }
            else
            {
                return View(ut);
            }
        }

        //== 真正刪除這一筆，並回寫資料庫 ===============
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]   // 避免XSS、CSRF攻擊
        // 避免刪除一筆記錄的安全漏洞 http://stephenwalther.com/archive/2009/01/21/asp-net-mvc-tip-46-ndash-donrsquot-use-delete-links-because
        public ActionResult DeleteConfirm(int _ID)
        {
            if (ModelState.IsValid)
            {
                if (_myDB.DeleteUser(_ID))
                {
                    //return Content(" 刪除一筆記錄，成功！");    // 刪除成功後，出現訊息（字串）。
                    return RedirectToAction("List");
                }
            }
            return Content(" 刪除一筆記錄，*** Delete失敗！***");    // 刪除失敗後，出現訊息（字串）。
        }



    }
}
