using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; // ConfigurationBuilder會用到這個命名空間
using Microsoft.Extensions.DependencyInjection;
using WebApplication2022_Core_MVC_Repository.Models;
using WebApplication2022_Core_MVC_Repository.Models.Repo;

//*******************************************************************
// 拜託！拜託！不要只會 Copy程式碼！
// 拜託！拜託！  Program.cs設定檔  也比對一下！......每次程式改成 .NET Core都會在這裡漏掉
//*******************************************************************


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//**** 讀取 appsettings.json 設定檔裡面的資料（資料庫連結字串）****
////作法一：
builder.Services.AddDbContext<MVC_UserDBContext>(options =>
    options.UseSqlServer(
        "Server=.\\sqlexpress;Database=MVC_UserDB;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true"
    )
);

//// 這裡需要新增兩個命名空間，請使用「顯示可能的修正」讓系統自己加上。

////作法二： 讀取設定檔的內容
//// 資料來源  程式碼  https://github.com/CuriousDrive/EFCore.AllDatabasesConsidered/blob/main/MS%20SQL%20Server/Northwind.MSSQL/Program.cs
//builder.Services.AddDbContext<MVC_UserDBContext>(
//        options =>
//        {
//            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
//        });

////作法三： 讀取設定檔的內容
//var configurationBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
//IConfiguration config = configurationBuilder.Build();   // ConfigurationBuilder會用到 Microsoft.Extensions.Configuration命名空間
//string DBconnectionString = config["ConnectionStrings:DefaultConnection"];  // appsettings.josn檔裡面的「資料庫連結字串」
//builder.Services.AddDbContext<MVC_UserDBContext>(options => options.UseSqlServer(DBconnectionString));

//======================
//使用完成，會"自動"關閉資料庫連線。 https://docs.microsoft.com/zh-tw/aspnet/core/data/ef-mvc/crud?view=aspnetcore-5.0
//======================
// 您會呼叫 .AddDbContext() 擴充方法 來在 ASP.NET Core DI 容器中佈建 DbContext 類別。
// 根據預設，該方法會將服務存留期設定為 Scoped。
// Scoped 表示內容物件的存留期會與 Web 要求的存留期保持一致，並且在 Web 要求結束時會自動呼叫 Dispose 方法。


//*******************************************************************
// 拜託！拜託！不要只會 Copy程式碼！
// 拜託！拜託！  Program.cs設定檔  也比對一下！......每次程式改成 .NET Core都會在這裡漏掉
//*******************************************************************
builder.Services.AddScoped<IUserTableRepository, UserTableRepository>(); //** 重點！ ** (設計的Interface, 實作後的class)

// https://docs.microsoft.com/zh-tw/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-5.0#service-registration-methods
// [微軟文件] 相依性插入可透過下列方式解決這些問題：
// -- 使用介面或基底類別來將相依性資訊抽象化。
// -- 在服務容器中註冊相依性。 ASP.NET Core 提供內建服務容器 IServiceProvider。 服務通常會在應用程式的方法中註冊 Startup.ConfigureServices 。
// -- 將服務「插入」到服務使用位置之類別的建構函式。 架構會負責建立相依性的執行個體，並在不再需要時將它捨棄。
//
// 註冊在 DI 容器的 Service 有分三種生命週期：  https://blog.johnwu.cc/article/ironman-day04-asp-net-core-dependency-injection.html
// == Transient -- 每次注入時，都重新 new 一個新的實例。
// == Scoped -- 每個 Request 都重新 new 一個新的實例，同一個 Request 不管經過多少個 Pipeline 都是用同一個實例。上例所使用的就是 Scoped。
// == Singleton --被實例化後就不會消失，程式運行期間只會有一個實例。
//
// 看這張表格，一目了然
// https://www.c-sharpcorner.com/article/understanding-addtransient-vs-addscoped-vs-addsingleton-in-asp-net-core/


//=== 分 隔 線 ===============================================================

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Repo}/{action=List}/{_ID?}");

app.Run();
