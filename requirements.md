# Car Auction Management System – Requirements Document

## Overview
This project is a Car Auction Management System written in C# using .NET 8. The system is console-based (no UI) and is designed to manage a list of vehicles, allow searching, and handle auction operations including bidding. It will include unit tests and Docker support.

## Functional Requirements

### 1. Vehicle Management
The system must support four types of vehicles:

- **Hatchback** – Attributes: Number of doors, Manufacturer, Model, Year, Starting Bid
- **Sedan** – Attributes: Number of doors, Manufacturer, Model, Year, Starting Bid
- **SUV** – Attributes: Number of seats, Manufacturer, Model, Year, Starting Bid
- **Truck** – Attributes: Load capacity, Manufacturer, Model, Year, Starting Bid

Each vehicle has a unique identifier (e.g., string Id).

#### Operations:
- Add a new vehicle to the inventory.
- Validate that the vehicle ID is unique. If the ID already exists, an exception should be thrown.
- Store all vehicles in an in-memory data structure (no database is required).

### 2. Search Vehicles
Users must be able to search vehicles using the following filters:
- Vehicle type (Hatchback, Sedan, SUV, Truck)
- Manufacturer
- Model
- Year

Search results must include all matching vehicles from the current inventory.

### 3. Auction Management
#### Auction Rules:
- Only one auction can be active for a vehicle at any time.
- Each auction must track:
  - The vehicle it's tied to
  - Whether it's active
  - The current highest bid
  - A history of all bids (optional but preferred)

#### Auction Operations:
- **Start an auction for a vehicle:**
  - Validate that the vehicle exists.
  - Ensure it is not already in an active auction.
- **Close an auction for a vehicle.**
- **Place a bid:**
  - Only allowed if the auction is active.
  - Bid amount must be greater than the current highest bid.
  - Otherwise, throw an exception.

## Error Handling Requirements
You must handle and throw specific exceptions for the following conditions:
- **DuplicateVehicleException** – when adding a vehicle with an ID already in use.
- **VehicleNotFoundException** – when trying to access a vehicle that does not exist.
- **AuctionAlreadyActiveException** – when starting an auction on a vehicle that is already in an active auction.
- **AuctionNotActiveException** – when placing a bid on an inactive auction.
- **InvalidBidException** – when the bid is not higher than the current highest bid.
- **InvalidInputException** – for other invalid values (e.g., null or out-of-range data).

## Technical Requirements
- Language: C# with .NET 8
- Testing Framework: xUnit
- Mocking Library: Moq (optional, for future extensibility)
- No database or file storage required – use in-memory collections only.
- Include a Dockerfile and docker-compose.yml for running the app and tests.
- Code should be clean, readable, and follow SOLID principles.
- Use OOP design (abstractions/interfaces, inheritance, encapsulation).

## Unit Testing Requirements
Write unit tests to cover:
- Adding vehicles
- Preventing duplicate vehicle IDs
- Searching vehicles by various filters
- Starting and closing auctions
- Placing valid and invalid bids
- All defined exceptions and edge cases

Use xUnit for testing. Each method in the services must have at least one corresponding test case.

## Deliverables
The developer must provide:
- Source code with:
  - All class and interface definitions
  - Vehicle types modeled appropriately
  - Services for inventory and auction logic
  - Unit tests for all functional operations
- Dockerfile for building and running the application
- docker-compose.yml for managing container setup
- README.md with:
  - Overview of the project
  - Setup instructions (including Docker)
  - Brief explanation of the architecture and design decisions

## Assumptions
- The system is single-threaded and in-memory.
- Vehicle identifiers are case-insensitive strings.
- The current bid is a decimal number.
- There is no user authentication or ownership logic.
- Console logging or simple output is acceptable for auction feedback.
