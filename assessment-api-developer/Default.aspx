<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="assessment_platform_developer._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <section class="row" aria-labelledby="aspnetTitle">
            <h1 id="aspnetTitle">.NET API Developer Assessment</h1>
            <p class="lead">This small project is set up similarly to how our current application is structured but on a very minute scale. 
	            In this assessment you will demonstrate your ability to refactor code, implement common design patterns used in our codebase and implement basic CRUD API's.</p>
        </section>


            <section class="row" aria-labelledby="gettingStartedTitle">
                <h2 id="gettingStartedTitle">Getting started</h2>
                <p>
                    Everything you need to complete this assessment is included in the project.
                </p> 
                <p>
                    The project is set up to add Customers to a data store and view them in a dropdown list.
                    The main goal of this assessment is to refactor the code to improve the overall structure and functionality 
	                of the Customers page, Apply SOLID design principles to all classes and then add RESTful API's that will allow users
	                to Create, Read, Update and Delete customers from the data store.
					
                    API's should follow the OpenAPI specification and be documented using Swagger.
                </p>
            </section>
    </main>

</asp:Content>
