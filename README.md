# Car Auction Management System

This is a simple Car Auction Management System implemented in C#. The system allows users to:

1. Add vehicles to the auction inventory
2. Search for vehicles by type, manufacturer, model, or year
3. Start and close auctions for vehicles
4. Place bids on vehicles during active auctions

## Design Decisions

### Object-Oriented Design

The system uses object-oriented design principles to model the different components:

1. **Vehicle** - An abstract base class that defines common properties for all vehicle types, such as ID, manufacturer, model, year, and starting bid. This class serves as the foundation for the inventory system.
2. **Vehicle Types** - Concrete classes for Hatchback, Sedan, SUV, and Truck that inherit from Vehicle. These classes represent specific types of vehicles in the inventory.
3. **InventoryService** - A service class that handles the business logic for managing inventory, including adding and searching for vehicles.
4. **Auction** - A class that represents an auction for a vehicle, including bid management
5. **AuctionService** - A service class that handles the business logic for managing auctions

### Repository Pattern

The repository pattern is used to abstract data access:

1. **IInventoryRepository** - An interface that defines the contract for inventory data access
2. **InventoryRepository** - A concrete implementation of the repository interface
3. **IAuctionRepository** - An interface that defines the contract for auction data access
4. **AuctionRepository** - A concrete implementation of the repository interface

### Dependency Injection

Dependency injection is used to manage dependencies:

1. Services are registered in the `Program.cs` file
2. Dependencies are injected through constructors

### Error Handling

Error handling is implemented for various scenarios:

1. Adding a vehicle with a duplicate ID
2. Starting an auction for a non-existent vehicle or one already in an active auction
3. Placing a bid on a non-active auction or with an invalid bid amount

## How to Use

### Adding a Vehicle

To add a vehicle to the inventory, send a POST request to `/api/inventory/vehicles` with a JSON body containing the vehicle details:

```json
{
  "id": "V001",
  "type": "Hatchback",
  "manufacturer": "Toyota",
  "model": "Yaris",
  "year": 2020,
  "startingBid": 5000.00,
  "numberOfDoors": 5
}
```

### Searching for Vehicles

To search for vehicles, send a GET request to `/api/inventory/vehicles` with query parameters:

```
/api/inventory/vehicles?type=SUV&manufacturer=Ford
```

### Starting an Auction

To start an auction for a vehicle, send a POST request to `/api/auction/auctions` with a JSON body:

```json
{
  "vehicleId": "V001"
}
```

### Placing a Bid

To place a bid on an active auction, send a POST request to `/api/auction/auctions/{auctionId}/bids` with a JSON body:

```json
{
  "bidderId": "Bidder1",
  "bidAmount": 6000.00
}
```

### Closing an Auction

To close an active auction, send a POST request to `/api/auction/auctions/{auctionId}/close`.

### Getting Active Auctions

To get a list of active auctions, send a GET request to `/api/auction/auctions/active`.

## Running the Application

1. Build and run the application using `dotnet run` in the `src/AutoAuction.API` directory
2. The API will be available at `https://localhost:5001/api/auction`
3. In development mode, Swagger UI will be available at `https://localhost:5001/swagger`

## Running Tests

To run the tests locally, use the command:
```
dotnet test
```

Alternatively, you can run the tests in the CI pipeline by pushing your changes to the repository. The pipeline will automatically execute the tests.

## Last build status
[![Docker Image CI](https://github.com/gianaugusto/auction-api/actions/workflows/docker-publish.yml/badge.svg)](https://github.com/gianaugusto/auction-api/actions/workflows/docker-publish.yml)
