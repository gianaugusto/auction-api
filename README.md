# Car Auction Management System

This project implements a simple Car Auction Management System using C# and object-oriented design principles. The system handles different types of vehicles (Sedans, SUVs, Hatchbacks, and Trucks) and allows users to add vehicles to the auction inventory, search for vehicles, start and close auctions, and place bids.

## Design Decisions

1. **Vehicle Hierarchy**: We created a base `Vehicle` class with common properties and methods, and then created specific classes for each vehicle type (Hatchback, Sedan, SUV, Truck) that inherit from the base class. This allows for code reuse and easy extension if more vehicle types need to be added in the future.

2. **Auction Management**: The `Auction` class encapsulates the auction functionality, including starting an auction, placing bids, and closing an auction. This class is responsible for maintaining the state of an auction and ensuring that bids are valid.

3. **Auction Service**: The `AuctionService` class manages the overall auction process, including adding vehicles to the inventory, searching for vehicles, and managing active auctions. This class provides a higher-level interface for interacting with the auction system.

4. **Error Handling**: The system includes error handling for various scenarios, such as attempting to add a vehicle with a duplicate ID, starting an auction for a vehicle that doesn't exist or is already in an active auction, and placing a bid on a vehicle that doesn't have an active auction or with an invalid bid amount.

5. **Unit Tests**: The system includes unit tests for the `Auction` and `AuctionService` classes to ensure that the functionality works as expected and that error handling is properly implemented.

## Assumptions

1. The system does not include a user interface or database, as the focus is on the structure of the code and the quality of the tests.
2. The system assumes that all vehicle IDs are unique and that all bid amounts are in the same currency.
3. The system does not include any authentication or authorization mechanisms, as the focus is on the core auction functionality.

## Future Improvements

1. Add a user interface to allow users to interact with the system.
2. Implement a database to persist the auction data.
3. Add authentication and authorization mechanisms to secure the system.
4. Implement more advanced search functionality, such as searching by price range or vehicle features.
5. Add support for multiple currencies and currency conversion.
