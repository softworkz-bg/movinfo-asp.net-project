# [MovInfo ASP.NET Core Web App](https://movinfoweb20190511054627.azurewebsites.net/)

## Made by: <br />

[![Foo](https://i.ibb.co/gwvy7dK/Fotoram-io-4.png)](http://softworkz.org/)

<br /> <b>
Team Members: <br />
 &nbsp;	&bull; Georgi Despolov - GitLab Profile: @despolov<br />
 &nbsp;	&bull; Dimitar Mihov - GitLab Profile: @dimitarmihov<br />

## Team leader:
<b>  &nbsp; &bull; Dimitar Mihov</b> <br />

## Project is Live at:
[MovInfo Web App](https://movinfoweb20190511054627.azurewebsites.net/) <br />

## Kanban Board:
[AzureBoard: MovInfo Project](https://dev.azure.com/SoftWorkZbg/MovInfo-Asp.Net-App) <br />

## Technologies used: <br />
 <b>
 &bull; .NET Core 2.2<br />
 &bull; EntityFrameworkCore 2.2.3 <br />
 &bull; SQL Server Express 17.9 <br />
 &bull; VisualStudio 2017 Community<br />
 &bull; MS Test 1.3.2 <br />
 &bull; Moq 4.10.1 <br />
 &bull; Razor engine <br />
 &bull; Git/GitLab
 </b> <br /> <br />
 
## Project Overview:

Project features a Three-Layer architecture:<br />
Presentation Layer: consists of ASP.NET Core 2.2 Web Project with fully responsive beautiful design.<br />
Business Layer: Services and business features processing class library.<br />
Data Layer: Holding the configuration for the data-base and additional mappings of table-relationships.<br />
Project includes thorough unit testing for the services and business features.<br />
The App allows the user to Register with two distinct Roles: User and Manager.<br />
One super-user is seeded upon initial start-up of the application along with basic <br />
data-base information via JSON files and pre-made images.<br />
The User can login and enjoy different authorised features based on his User Role.<br />
Project is hosted in Azure and utilizes custom Domain name.<br />
Project includes caching for some parts of its Movie Collections to lower the data-base trips about frequent listings.<br />
Project was developed using end-to-end Feature branches (left un-deleted in the git-system for the purpose of showcasing the process)!



## Un-Logged users Can:
Browse all movies;<br />
Search Movies by Name and have paginated result;<br />
Open and view detailed info about movies and actors;<br />
Read user Reviews for the movie;<br />
Perform Advanced Movie search via jquery.DataTable interface;<br />
Perform Advanced search for Actors and preview Categories as well;<br />
Enjoy the various Carousels for Top Movies and more;<br />

## Logged users with role "USER" Can in addition to the previous:
Leave Reviews for movies;<br />
Edit their own Reviews;<br />
Delete their own Reviews;<br />

## Logged users with role "USER" Can in addition to the previous:
Leave Reviews for movies;<br />
Edit their own Reviews;<br />
Delete their own Reviews;<br />

## Logged users with role "MANAGER" Can in addition to the previous:
Add Movies using ajax search for actors and categories in the process;<br />
Edit Movies using ajax search for actors and categories in the process;<br />
Delete Movies;<br />
Add Actors using ajax search for movies;<br />
Edit Actors using ajax search for movies;<br />
Delete Actors;<br />
Edit Reviews;<br />
Delete Reviews;<br />

## Logged user with role "ADMIN" Can in addition to the previous:
Delete user accounts inside the special Admin Area<br />
that includes view for all users inside a table;
