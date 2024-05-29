## 架構 
Database(存儲數據的實體) <=> DbContext(與數據庫交互的上下文)    
<=> Domain Model(與數據庫表直接對應的實體類)    <=> Repository(提供基本的數據庫操作) <=> Service(處理業務邏輯)(使用automapper) <=> DTO <=> Controller <=> ViewModel(格式化和處理特定於視圖的數據) <=> View



## 架構實現
>Database（資料庫）：實際的資料儲存。   
- Sqlite 實作建置
>DbContext：資料庫底層，管理實體和資料庫之間的關係
 使用Domain Model 映射Database（
>Domain Model：領域模型，表示業務領域中的資料和行為。  
>Repository：資料存取層，封裝資料庫操作，提供對資料的CRUD操作。
 - Unit of work 實現
>Service：業務邏輯層，處理應用程式的業務邏輯。  
 - AutoMapper 映射到Dto(ViewModel：視圖模型，封裝視圖所需的資料)

>Controller：控制器層，處理HTTP請求和回應，呼叫服務層的方法。
 - basecontroller 實作强類型的 RedirectToAction

>View：視圖層，呈現使用者介面。

## 功能實現

- 首頁
  登入 => 登入驗證頁面
  登出(controller裡 Or layout?)
- 紀錄狀態  
Session，一種讓Request變成stateful的機制  
Cookie  

 - 管理使用者資訊 UserManagementController.cs  
Delete => 刪除使用者 =>  ASP.NET Core Post => UserManagementController => Task<IActionResult> Delete  
Edit =>編輯使用者 =>  JavaScript Post => UserManagementController => Task<IActionResult> Edit  

- 登入驗證 LoginController.cs   
 CheckPermission、LoginController.cs   
 Login =>登入 => 使用者輸入(存到LoginViewModel) 和資料庫匹配 => 將匹配訊息存到Session =>_Layout.cshtml UserController CheckPermission()設定有權限才可瀏覽
   
   登入確認  
     
   登入紀錄
