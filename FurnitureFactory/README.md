# Project Info  
## General Info  
This project is implemented using ASP.Net Core Framework, uses Microsoft SQL server and Entity Framework Core for database access. The users and role functionality comes from the Identity system, however a custom controller was implemented to allow the admin user to easily manipulate other users. There is client and server side data validation and error handling.  

## Business Logic  
There are three types of users:  
* Anonymous (not registered and not logged in users)  
* Logged in users  
* Administrators  
