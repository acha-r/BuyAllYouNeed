# BuyAllYouNeed
This repository contains a powerful e-commerce RESTful API built using MongoDB as the backend database and ASP.NET Core. 
BuyAllYouNeed provides a feature-rich platform for managing products, user accounts, and payments. With integrated support for text search 
and a Paystack payment gateway, this API offers a seamless and secure e-commerce experience.

## Features
### Product Management
Text Search: Utilize MongoDB's text search functionality to enable efficient and accurate product searches. Users can easily find desired 
products based on relevant keywords.

Admin Functions: Admins have the ability to add, update, and remove products from the system. This ensures up-to-date product listings and 
allows for easy management of the product catalog.

### User Management
Account Creation: Users can create new accounts to access a seamless shopping experience. The API handles the 
account creation process securely.

Login: Users can securely log in to their accounts using authentication mechanisms provided by ASP.NET Core. This ensures the protection of 
user information.

Profile Management: Users can manage their profiles, update personal information, and view order history. This feature enhances the user 
experience and provides personalized services.

### Payment Integration
Paystack Integration: BuyAllYouNeed integrates with the Paystack payment gateway, allowing users to make test purchases securely. This 
ensures a smooth payment process for customers and facilitates seamless transactions.

## Tech Stack
BuyAllYouNeed is built using the following technologies:

C#: A versatile and powerful programming language that provides a robust foundation for building scalable and efficient applications.

ASP.NET Core: A high-performance web framework for building modern web applications. ASP.NET Core offers a range of features for developing RESTful APIs, 
ensuring reliable communication and security.

MongoDB: A flexible and scalable NoSQL database that offers high performance and easy scalability. 
MongoDB serves as the backend database, providing efficient data storage and retrieval.

## Getting Started
To start using AllYouNeedShop, follow these steps:

- Clone the repository
- Install the necessary dependencies: `dotnet restore`
- Configure the MongoDB connection string in the appsettings.json file.
- Build the project: dotnet build
- Run the application: dotnet run
- Ensure you have the required versions of .NET Core SDK and MongoDB installed on your machine before proceeding.

## API Documentation
AllYouNeedShop provides detailed API documentation, including information about each endpoint, request/response formats, and authentication mechanisms.
