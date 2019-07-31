# Zoo Authentication System
This is an authentication/authorization system rewritten from IT145. The original requirements were: 
* Write an authentication system that gives a user three attempts to log in. 
* After three failed attempts, the program should display a warning and then close.
* Upon successful authentication, the program should display only the details allowed for the role assigned to the logged in user.
* The user should have to option to log out of the system to allow a new user to log in.

This Cap Stone project is supposed to address and enhance three main concepts from the original program:
* Software Design and Engineering
* Algorithms and Data Structures
* Databases

### Software Design and Engineering
  I converted the application from the original Java Swing UI into a Windows Presentation Foundation (WPF) app using C#.Net and incorporating an MVVM design pattern. 

### Algorithms and Data Structures
  I modified the program to use a more secure SHA256 hashing algorithm for the passwords and implemented a locking behavior for max attempts rather than just closing the program. The account locks can be placed on a per-user basis rather than application wide. For the sake of simplicity in this application, I have also included the ability for the user to unlock an account that may have been locked due to reaching the maximum number of allowed failed log in attempts.

### Databases
  I decided to store the credentials in a relational database management system instead of a plain text file which was required to be used in the original application. The database also maintains the locked status of each user account.
