# API Developer Assessment

## Notes

* in general majority of my time was spent on realization of "how to do it within the C# project". In case there is a working application which can be referenced to - it will simplify the process and speedup the timeline dramatically
* not sure if my implementation is correct. As I understood this assessment generates the dll library. I've implemented internal methods to setup the local restful interface and startup the site on run.
* refactored the form item names to keep the type of the objects in front of the IDs (camelCase).
* faced with the GitKraken bug when it removes the current branch and modified files if some of the files are blocked and you are switching to another branch.
* added test cases using AI. Revised them and understood the logic. Fixed the problem with the performance tests. HTTP contexts were not handled properly. Now all tests passess as expected.

## High level site map

![Site map](assessment-api-developer/docs/SiteMap.png)

## How to start

1. Open the project in VStudio
2. Click on the solution with the mouse right button
3. Open "Properties"
4. In the opened window
* select the "Configure Startup Projects" from the right pane
* ensure that assessment-api-developer is selected in the "Single startup project"
* press OK
5. Execute the project
* web browser must start with the default page.
* RESTful api is ready to use on the: https://localhost:44358/api/customers

## Applied Changes

### Models

* removed unnecessary CustomerDBContext model as we are working with the local list as a mock
* fixed the lists in the Models
* extracted model classes to stay aligned with the SOLID principles

### Controllers

* Created a new controller to manage OpenApi requests

### Services

#### CustomersService

* renamed the CustomersService to CustomerService

#### ApiCustomerService

* Added new ApiCustomerService

### Customers.aspx

* fixed the CodeBehind
* aligned the page with the master-content principle (like default.aspx)
* fixed the customer notes field
* added a new label to handle the output messages and make the page more interactive
* added 2 missing fields for customer contact
* removed the direct service call (decoupled from the CustomerService)
* implemented the WebApiCalls through the newly added service called RestfulCustomerService
* implemented ddlCustomers selection change method (to add new or update existing customer)
* added new button to delete the customer
* implemented ddlCountries selection change method (to populate the states/provinces with the proper list)
* added validation for the CustomerEmail and CustomerPhone fields
