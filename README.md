# WalletMate
**WalletMate** is a backend service for importing, processing, and managing financial data from the different banks, particularly from the Ukrainian bank **Monobank**.  
It validates date ranges, fetches client accounts and transactions, maps them into internal domain models, and persists them reliably with caching and rate-limiting support.

The project is designed with **clean architecture principles**, strong **domain modeling**, and explicit **UML documentation**.

## Key Features

- Secure integration with Bank's API  
- Built-in rate limiting and concurrency control  
- Import and persist accounts & transactions  
- Smart caching to reduce API load  
- Well-defined domain model  
- Fully documented with UML diagrams  

---

## Activity Diagram – Import Workflow

The activity diagram below illustrates the full workflow of importing data from Monobank, from validation to persistence.

<img width="514" height="1341" alt="Activity diagram" src="https://github.com/user-attachments/assets/d68d1560-69b4-4341-968d-07aaa4616808" />

### Flow Summary

1. Validate date range  
2. Convert dates to Unix timestamps  
3. Request client info from Monobank API  
4. Check if accounts exist  
5. Iterate through Monobank accounts  
6. Check if account already exists in DB  
7. Fetch transactions from cache or API  
8. Map transactions to internal models  
9. Create internal accounts  
10. Persist new accounts and commit changes  

This approach ensures **fail-fast validation**, **data consistency**, and **transactional safety**.

---

## Domain Model – Class Diagram

The class diagram represents the core domain entities and their relationships.

<img width="1542" height="490" alt="Class diagram" src="https://github.com/user-attachments/assets/9c293f1e-a8bd-43c0-aac1-e35304d99359" />

### Core Entities

- **BaseEntity**
  - Base Entity in the system
- **User**
  - Owns multiple accounts
- **Account**
  - Contains transactions
- **Transaction**
  - Represents monetary operations
- **Category**
  - Transaction classification
- **TransactionCategory**
  - Many-to-many relation between Transaction and Category
- **Currency (enum)**
  - USD, EUR, GBP, UAH

The domain model is designed to be:
- Strongly typed  
- Normalized  
- Easy to extend  

---

## State Machine – Monobank API Requests

This state machine describes how Monobank API calls are controlled and protected from overuse.

<img width="516" height="1052" alt="State machine" src="https://github.com/user-attachments/assets/eebff23f-bf8a-4410-a472-34241e15cba9" />

### State Explanation

- **Idle** – no active request  
- **WaitingForSemaphore** – concurrency control  
- **Acquired** – semaphore locked  
- **RateLimiting** – enforced delay (60 seconds)  
- **SendingRequest** – API request execution  
- **ProcessingResponse** – parsing and validation  
- **Completed** – successful response  
- **Failed** – error occurred  
- **ReleasingSemaphore** – guaranteed cleanup (`finally` block)  

This design prevents:
- API throttling  
- Race conditions  
- Resource leaks  

---

## Technologies Used

- C# / .NET  
- ASP.NET Core  
- Entity Framework Core  
- Monobank REST API  
- In-memory or distributed caching  
- UML (Activity, Class, State Machine diagrams)  

## Reliability & Safety

- Explicit exception handling  
- Atomic database commits  
- Semaphore-based concurrency control  
- Cache-first API strategy  
- Clear separation of concerns  

---

## 👩‍💻 Author

**WalletMate** is designed as a clean, production-ready backend service with a strong focus on correctness, scalability, and maintainability. Designed by Ivanna Kirinova
