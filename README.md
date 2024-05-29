## 架構 
Database <=> DbContext  <=> Domain Model  <=> Repository <=> Service <=> DTO <=> Controller <=> ViewModel <=> View
## 架構實現
>Database（資料庫）：實際的資料儲存。   
- Sqlite 實作建置

>DbContext：資料庫底層，管理實體和資料庫之間的關係
 使用Domain Model 映射Database  

>Domain Model：與數據庫表直接對應的實體類   

>Repository：資料存取層，封裝資料庫操作，提供對資料的CRUD操作。

>Service：業務邏輯層，處理應用程式的業務邏輯。  
 - AutoMapper 映射到Dto
>ViewModel：視圖模型，封裝視圖所需的資料

>Controller：控制器層，處理HTTP請求和回應，呼叫服務層的方法。
 - basecontroller 實作强類型的 RedirectToAction
  
>View：視圖層，呈現使用者介面。

## 功能實現

- 首頁
- 使用者相關 UserController  
  登入功能 => 登入驗證頁面Login.cshtml
  使用UserViewModel 接收 使用者輸入  ，使用userService.Authenticate進行使用者資料驗證同時記性資料錯誤alarm提示
  ，驗證成功後，使用userService.SignInAsync進行Cookies紀錄登入狀態 權限  

  登出 userService.SignOutAsync() 移除身分驗證Cookies 並導向首頁
-
 - 管理使用者資訊 UserManagementController.cs  
Delete => 刪除使用者 =>  ASP.NET Core Post => UserManagementController => Task<IActionResult> Delete  
Edit =>編輯使用者 =>  JavaScript Post => UserManagementController => Task<IActionResult> Edit  


   
   登入確認  
     
   登入紀錄
