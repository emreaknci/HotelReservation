
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

## Run the Application


Start the application:

    dotnet run

## Contributing
I welcome contributions! If you find a bug or have a feature request, please open an issue. If you would like to contribute code, please fork the repository and create a pull request.

## License

This project is licensed under the [MIT](https://choosealicense.com/licenses/mit/)




## Authors

[@emreaknci](https://www.github.com/emreaknci)






[![linkedin](https://img.shields.io/badge/linkedin-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/emreaknci/)


