# Travel Agency Application

This project simulates a multi-agency travel booking system. Several travel agencies use transport companies to organize trips to tourist attractions, and they require a system to reserve seats on behalf of clients. The application was developed in **parallel in Java and C#** to demonstrate cross-platform interoperability and networking.

## Key Features

- **Login:** Each agency employee logs in to access the application. Upon successful authentication, all available trips are displayed, including the tourist attraction, transport company, departure time, price, and available seats.  
- **Search:** Employees can search trips to a specific tourist destination within a certain departure time range. Matching trips are displayed with company name, price, departure time, and available seats.  
- **Reservation:** Employees can reserve tickets for clients by entering client details (name, phone number, number of tickets). Reservations are instantly visible to all other agencies, and trips with no available seats are highlighted in red.  
- **Logout:** Users can securely log out from the system.  

## Technical Implementation

- **Model and Persistence:** Java and C# implementations with relational databases. ORM tools like **Hibernate** were used for at least two entities. Repository classes are logged, and database connection details are retrieved from configuration files.  
- **Services & GUI:** Controllers call service methods, which interact with repositories. Desktop GUIs were developed for both Java and C#.  
- **Networking:** Implemented client-server architecture with sockets and threads. Clients receive automatic updates when trip data changes. The server was implemented in one language (Java or C#) and clients in the other, demonstrating interoperability.  
- **REST API & Web Client:** REST services were developed for the core entities, and a web client was implemented with JWT authentication for secure client access.
