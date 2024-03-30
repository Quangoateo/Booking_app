# Backend Folder Structure
## Database
* Using MySQL as the database, the ERD is as follows:
//erd picture
### Naming convention
* snake_case
* connection table using "2" between two table, ex. Room2Facility
## .NET
### Constants
* Constants store all the constants value that are used in the project.
### Controllers
* Controllers are the entry point of the API. It is responsible for handling the incoming HTTP request and return the response to the client.
### Data
* Data folder contains the database context and the migration folder. The database context is the main class that coordinates Entity Framework functionality for a given data model.
#### BookingAppContext
* The BookingAppContext class is derived from DbContext and it represents a session with the database and can be used to query and save instances of the entities to a database.
#### DbFactory
* The DbFactory class is used to create a new instance of the BookingAppContext.
#### OracleDbContext
* The OracleDbContext class is derived from DbContext and it represents a session with the database and can be used to query and save instances of the entities to a database.
#### RepositoryBase
* The RepositoryBase class is the base class for all the repository classes. It contains the common methods that are used in all the repository classes.The provided C# code defines a generic repository pattern for an Entity Framework Core DbContext. The repository pattern is a design pattern that mediates data access, providing a more abstract interface for accessing data stored in a database.
#### UnitOfWork
* The UnitOfWork class is used to manage the database context and the repository classes. It is responsible for creating the repository instances and saving the changes to the database.
### DLL

### DTO
* DTO (Data Transfer Object) is a data container that is used to transfer data between the layers of the application. It is used to encapsulate the data and send it from one layer to another. The DTOs are used to transfer the data between the API and the client.
* The DTO folder contains the classes that are used to transfer the data between the layers of the application.
### Helpers
* The Helpers folder contains the classes that are used to perform the common tasks in the application. The classes in the Helpers folder are used to perform the common tasks such as logging, Mapping, Authentication, etc.
### Installer
* The Installer folder contains the classes that are used to configure the services in the application. The classes in the Installer folder are used to configure the services such as the database context, the repository classes, the unit of work, the authentication, etc.
### Models
* The Models folder contains the classes that are used to represent the data in the application. The classes in the Models folder are used to represent the data such as the user, the role, the booking, the room, etc.
### Services
* The Services folder contains the classes that are used to perform the business logic in the application. The classes in the Services folder are used to perform the business logic such as the user authentication, the room booking, the room availability, etc.
## Flow of Data
* The flow of data in the application is as follows:
1. The client sends an HTTP request to the API.
2. The API receives the HTTP request and passes it to the **controller**.
3. The controller sends the request to the **service**.
4. The service performs the business logic and sends the request to the **repository**.
5. The **repository** performs the database operation and sends the response to the **service**.
6. The service sends the response to the **controller**.
7. The controller sends the response to the client.
# Implementation
## Add new API ( for normal API that not effect the system)
* Model - Add model of the respected table on the DB
* DTO - Add DTO model of the table as a new file ( could be configured to fit user demand)
* Data -> BookingAppContext - setup the DB and add logic to map the *model* -> the *database*
* Service - add new function - logic for the API
* Helper -> AutoMapper -> DtoToEFMapping profile and EFToDtoMapping profile - Add the map from model to Dto and reserve
* Installer -> ServiceInstaller
* Controllers -> add the endpoint
# API Endpoint Notes
> Fields ID, (Self)Guid, status, is auto generated, shouldn't receive any value on created
## Facility
* represent type of the facility, not represent a single facility
 