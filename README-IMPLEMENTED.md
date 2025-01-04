# API Developer Assessment

## Notes

* refactored the form item names to keep the type of the objects in front of the IDs (camelCase).
* faced with the GitKraken bug when it removes the current branch and modified files if some of the files are blocked and you are switching to another branch.

## Applied Changes

### Models

* removed unnecessary CustomerDBContext model as we are working with the local list as a mock
* fixed the lists in the Models
* extracted model classes to stay aligned with the SOLID principles

### Controllers

* Created a new controller to manage OpenApi requests

### Services

modification and addition of services

#### CustomersService

* renamed the CustomersService to CustomerService

#### ApiCustomerService

* Added new ApiCustomerService

### Customers.aspx

* fixed the CodeBehind
* aligned the page with the master-content principle (like default.aspx)
* fixed the customer notes field
* added new label to handle the output messages and make the page more interactive
* added 2 missing fields for customer contact
* removed the direct service call (decoupled from the CustomerService)
* implemented the WebApiCalls through the newly added service called RestfulCustomerService
* implemented ddlCustomers selection change method (to add new or update existing customer)
* added new button to delete the customer
* implemented ddlCountries selection change method (to populate the states/provinces with the proper list)
* added validation for the CustomerEmail and CustomerPhone fields
