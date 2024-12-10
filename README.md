# API Developer Assessment

In this assessment, you will demonstrate your ability to refactor code, implement common code and API design patterns and principles used in our codebase.

Knowledge of common design patterns used in .NET C# development will be beneficial.
## Assignment

The Customers.aspx page has code that renders a very simple form that allows users to add a customer to a dropdown list. The customers
dropdown list is loaded from a local data repository.

The main goal is to refactor and modify the underlying code to meet common coding disciplines and patterns while also building out API endpoints for the Customer. 
Design choices should demonstrate an understanding of testing and long term maintainability considerations.

The changes should include the following:

1. Refactoring of the model code to follow the SOLID design principle
2. Add intelligent validation to the fields on the form and via API. IE If a US State is provided, the Postal Code/ZIP field should validate that the 
value matches the expected ZIP code formats of ##### or #####-#### and vice versa
3. Implement RESTful API's to:
  - Create customers
  - Read a single customer by ID
  - Read all customers
  - Update
  - Delete
4. Allow updating and deletion of customer data listed in the customers drop down list


### Requirements

1. Complete the main goal, with a keen focus on high quality API endpoint design and functionality
2. Provide a git repository with your solution using a link to a publicly accessible repository
3. Creation of new objects is expected if SOLID design principles are implemented
4. Use the git commit history to show progress and your thought process

#### Extra credit

If time allows, add unit testing for the field validations.

## Delivery

1. Create a new repository with the initial code and complete your solution on top of it, do not fork the repository
2. Add a README-IMPLEMENTED.md with any details necessary
3. Once the solution is built, send your solution to your hiring contact
4. Please deliver the solution within 7 days of receiving this assessment
