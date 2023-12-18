
# Hotel Reservation

Welcome to the Hotel Reservation System backend developed using .NET! This backend application provides the necessary functionality to support a hotel reservation application, allowing users to browse available rooms, make reservations, and manage their bookings.



## Tech Stack

**Backend:** 
- .NET Core 8 
- EF Core 8

**Database:** 
- PostgreSQL


## Getting Started

Follow the steps below to set up and run the backend locally:


#### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine. (v8.0.1)
- Code editor of your choice (e.g., Visual Studio, Visual Studio Code).
- PostgreSQL database installed on your machine. You can download and install PostgreSQL from [here](https://www.postgresql.org/download/). (v15.5)


## Installation

Clone the repository to your local machine:

      git clone https://github.com/emreaknci/HotelReservation.git

Navigate to the project directory:

      cd HotelReservation

Install the required dependencies:

      dotnet restore

## Configuration

Open the `appsettings.json` file in your project and provide the necessary information for the PostgreSQL database connection string and JSON Web Token (JWT) settings.

        {
            // Other configurations...
            "ConnectionStrings": {
                "PostgreSQL": "User ID=yourUserID;Password=yourPassword;Host=yourHost;Port=yourPort;Database=HotelReservationDb;"

            },
            "TokenOptions": {
                "Audience": "yourAudience@yourDomain.com",
                "Issuer": "yourIssuer@yourDomain.com",
                "AccessTokenExpiration": 60, // Set the expiration time in minutes
                "SecurityKey": "yoursecretkey"
            },
            
            // Other configurations...
        }

## Database Migration

Run the database migration to create the necessary tables:

    dotnet ef database update

## Database Restore

Initialize the database using the backup file that comes with the project. Restore the backup file by following these steps:

1. Open the pgAdmin application and connect to the relevant database.
2. Right-click on the database name in the left-hand navigation pane.
3. Select 'Restore' from the menu that opens.
4. In the 'General' tab, enter the path to the 'HotelReservationDbBackup.sql' file in the 'Filename' field.
5. Switch to the 'Query Options' tab.
6. Enable the 'Clean Before Restore' and 'Include IF EXISTS clause' options.
7. Press the 'Restore' button at the bottom right to initiate the restore process.

Make sure to verify the success of the restore operation by refreshing the database in the pgAdmin interface and checking that the necessary tables and data have been restored.

**Note:** If you do not want to use the initial data, you can skip this step. However, there is one more thing you need to do. Inside the 'wwwroot' folder in the WebAPI project, under the 'Images' folder, remember to delete all files except 'no-image.jpg.'

## Run the Application


Start the application:

    dotnet run

## Contributing
I welcome contributions! If you find a bug or have a feature request, please open an issue. If you would like to contribute code, please fork the repository and create a pull request.

## License

This project is licensed under the [MIT](https://choosealicense.com/licenses/mit/)


## Authors

[![linkedin](https://img.shields.io/badge/linkedin-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/emreaknci/)


