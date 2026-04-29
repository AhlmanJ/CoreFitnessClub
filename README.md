# CoreFitnessClub - MVC School assignment

# Hi!

I'm studying to become a .NET Developer at EC Utbildning and this is my fifth school assignment. 
This assignment was done for the course "ASP.NET".

The focus of this assignment was to build a full-stack solution in ASP.NET Core MVC with data storage and authentication. The project structure was to be created according to Domain-Driven-Design and Clean Architecture. For this assignment, i received a design file from Figma that i would use to create a frontend for my application. The project was based on the idea that i would develop a web portal for a gym.


# Use of AI/ChatGPT in the learning-process? 
During the development of this application, in addition to following lectures and teacher-led lessons, i have searched for information online for code examples and explanations about how to write code and why. But to clarify how i use chatGPT in the learning process, i want to include this paragraph in my ReadMe. In my solution, i have written comments in my files in the various projects about where i have used AI/chatGPT. If i haven't found good information on the internet about how to write a code or if i don't know at all what a code should look like, i have asked AI/chatGPT to either give me a code example of how to do it and then asked AI to explain each part of the code. Or if i have found a code that i can use on the internet or in a lecture, i have asked AI to explain the parts of the code that have been unclear to me or that i haven't understood at all.
By using AI in this way, i find i learn more and gain a better understanding of how to write code and why.
So to sum this up, i use AI as an extra "teacher" who can explain code and give examples, just like our teacher does in our teacher-led lessons and i can ask the same question several times until i really understand.

# How to install and test this application?

- Clone the repository to Visual Studio.
- Download and install a server. I have been using Microsoft SQL Server 2025 Express along with SQL Server Management Studio which can be downloaded from Microsoft's website.
- You will now have to add the server/database to the backend solution in Visual Studio:
- In Visual Studio, click on "View" and choose "SQL Server Object Explorer".
- In the "SQL Server Object Explorer", click on "Add SQL Server" and select your server and it should now be connected to your solution.
- Once the server is connected to your solution, you need to add the connection string to " APPSETTINGS.JSON ". In this application, i use two different environments, a "development" environment that uses an in-memory database and a "production" environment that uses a real database.
- The next step is to migrate the entities to the database (tables) and since i have used two different environments for the application, you must "tell" which environment you want to direct the migration to during migration.
- In Visual Stuido, klick on "Tools" and select "NuGet Package Manager" and select "Package Manager Console".
- In the console, insert text to the console and run: $env:ASPNETCORE_ENVIRONMENT="Production"
- In the console, insert text to the console and run: Add-Migration "MigrationToDatabase"
- If migration is "OK", insert text to the console and run:  $env:ASPNETCORE_ENVIRONMENT="Production"
- In the console, insert text to the console and run: Update-Database
- Open SQL Server Management Studio and check tables in your server. They should now reflect the entities which you have in the solution in Visual Studio.
  
You should now be able to test the solution.
- If you run the application in "Development" mode, you will use the in-memory database and will therefore not save any information so that it remains if you restart the application.
- If you use "Production" mode, the server must be started and you will then save all information in the database when you use the application.

# NOTE!
----- > ADMIN ACCOUNT ARE SEEDED WHEN STARTING THE APPLICATION < ----- 

- In this application you will seed an "Admin" account on first start. Check APPSETTINGS.JSON for which USERNAME and PASSWORD to use when logging in.
- As an ADMIN you have access to the admin page which you can find on the profile page when you are logged in.
- On the admin page you can manage membership, add training sessions etc...

----- > Log in with a third-party login < -----

- To log in with GitHub, you must use the "Production" environment when running the application.
