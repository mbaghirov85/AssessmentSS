# API Developer Assessment

## Notes

## Applied Changes

### Models
* removed unnecessary CustomerDBContext model as we are working with the local list as a mock
* fixed the lists in the Models
* created new model classes to stay aligned with the SOLID principles

### Services

#### CustomersService
* Modified the CustomersService

#### ApiCustomerService
* Added new ApiCustomerService

### Customers.aspx
* fixed the CodeBehind
* fixed the customer notes field
* added 2 missing fields for customer contact
* removed the direct service call (decoupled from the CustomersService)
* implemented the WebApiCalls through the newly added service called ApiCustomerService
* implemented ddlCustomers selection change method (to add new or update existing customer)
* added new button to delete the customer
* implemented ddlCountries selection change method (to populate the states/provinces with the proper list)