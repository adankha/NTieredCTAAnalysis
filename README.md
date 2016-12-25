# NTieredCTAAnalysis
An N-Tiered C# GUI Application used to analyze information regarding Chicago CTA Train Station

IDE Used: Microsoft Visual Studios 2015
Languages: C# and Microsoft SQL
Structure of the program: N-Tiered / OOP

The program illustrated here loads in a csv file, parses the data, and stores into a database.

BusinessTierLogic.cs - This file is the business logic. It acts as the interface between the UI and the data store.

BusinessTierObjects.cs - This file is used to store / create the classes that define the objects in the data. These objects carry the data that is normally displayed in the presentation tier

DataAccessTier.cs - This file is used to open the database connection, use the passed in string query, extract the information from the DB, then send it back to the caller.

Form1.cs - The file that is used for the C# Application.
