# Smart Asset Tracking System

A C# .NET Console Application for tracking company assets across multiple offices.

## Technologies

- C#
- .NET Console Application
- Entity Framework Core
- SQL Server
- LINQ
- OOP
- CRUD Operations

## Features

- Add computer and mobile assets
- Display assets in a table
- Update asset information
- Delete assets
- Manage company offices
- Convert USD prices to local office currency
- Track asset lifecycle status
- Seed sample data
- Generate reports

## Asset Types

The project uses inheritance:

Asset
├── ComputerAsset
└── MobileAsset
## Reports

The application includes a reports menu with the following reports:

1. Total asset value per office
2. Asset count per office
3. Assets close to expiration
4. Most expensive asset

### Total Asset Value Per Office

Shows the total value of assets grouped by office.

### Asset Count Per Office

Shows how many assets are assigned to each office.

### Assets Close to Expiration

Shows assets that are close to the end of their 3-year lifecycle or have already expired.

### Most Expensive Asset

Shows the asset with the highest purchase price in USD.
