<%@ Page Async="true" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Customers.aspx.cs" Inherits="assessment_platform_developer.Customers" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<div>
		<div class="container body-content">
			<asp:Label ID="lblError" runat="server" Text="" CssClass="h2 form-label"></asp:Label>
		</div>
		<div class="container body-content">
			<h2>Customer Registry</h2>
			<asp:DropDownList runat="server" ID="ddlCustomers" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCustomers_SelectedIndexChanged"/>
		</div>
		<div class="container body-content">
			<div class="card">
				<div class="card-body">
					<div class="row justify-content-center">
						<div class="col-md-6">
							<h1><asp:Label ID="lblFormCaption" runat="server" Text ="Add Customer" CssClass="form-label"></asp:Label></h1>
							<div class="form-group">
								<asp:Label ID="lblCustomerName" runat="server" Text="Name" CssClass="form-label"></asp:Label>
								<asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" required="true"></asp:TextBox>
							</div>
							<div class="form-group">
								<asp:Label ID="lblCustomerAddress" runat="server" Text="Address" CssClass="form-label"></asp:Label>
								<asp:TextBox ID="txtCustomerAddress" runat="server" CssClass="form-control"></asp:TextBox>
							</div>
							<div class="form-group">
								<asp:Label ID="lblCustomerEmail" runat="server" Text="Email" CssClass="form-label"></asp:Label>
								<asp:RegularExpressionValidator ID="revCustomerEmail" runat="server" ControlToValidate="txtCustomerEmail" ErrorMessage=" (invalid email format)" ValidationExpression="[a-zA-Z0-9._%+\-]+@[a-zA-Z0-9.\-]+\.[a-zA-Z]{2,}" Display="Dynamic" ForeColor="Red"></asp:RegularExpressionValidator>
								<asp:TextBox ID="txtCustomerEmail" runat="server" CssClass="form-control" required="true"></asp:TextBox>
							</div>
							<div class="form-group">
								<asp:Label ID="lblCustomerPhone" runat="server" Text="Phone" CssClass="form-label"></asp:Label>
								<asp:TextBox ID="txtCustomerPhone" runat="server" CssClass="form-control" required="true"></asp:TextBox>
							</div>
							<div class="form-group">
								<asp:Label ID="lblCustomerCountry" runat="server" Text="Country" CssClass="form-label"></asp:Label>
								<asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"/>
							</div>
							<div class="form-group">
								<asp:Label ID="lblCustomerCity" runat="server" Text="City" CssClass="form-label"></asp:Label>
								<asp:TextBox ID="txtCustomerCity" runat="server" CssClass="form-control"></asp:TextBox>
							</div>
							<div class="form-group">
								<asp:Label ID="lblCustomerState" runat="server" Text="Province" CssClass="form-label"></asp:Label>
								<asp:DropDownList ID="ddlState" runat="server" CssClass="form-control"/>
							</div>
							<div class="form-group">
								<asp:Label ID="lblCustomerZip" runat="server" Text="Postal/Zip Code" CssClass="form-label"></asp:Label>
								<asp:RegularExpressionValidator ID="revCustomerZip" runat="server" ControlToValidate="txtCustomerZip" ErrorMessage=" (invalid postal code)" ValidationExpression="" Display="Dynamic" ForeColor="Red"></asp:RegularExpressionValidator>
								<asp:TextBox ID="txtCustomerZip" runat="server" CssClass="form-control"></asp:TextBox>
							</div>
							<div class="form-group">
								<asp:Label ID="lblCustomerNotes" runat="server" Text="Notes" CssClass="form-label"></asp:Label>
								<asp:TextBox ID="txtCustomerNotes" runat="server" CssClass="form-control"></asp:TextBox>
							</div>
							<h1>Customer contact details</h1>
							<div class="form-group">
								<asp:Label ID="lblContactName" runat="server" Text="Name" CssClass="form-label"></asp:Label>
								<asp:TextBox ID="txtContactName" runat="server" CssClass="form-control"></asp:TextBox>
							</div>
							<div class="form-group">
								<asp:Label ID="lblContactTitle" runat="server" Text="Contact Title" CssClass="form-label"></asp:Label>
								<asp:TextBox ID="txtContactTitle" runat="server" CssClass="form-control"></asp:TextBox>
							</div>
							<div class="form-group">
								<asp:Label ID="lblContactNotes" runat="server" Text="Notes" CssClass="form-label"></asp:Label>
								<asp:TextBox ID="txtContactNotes" runat="server" CssClass="form-control"></asp:TextBox>
							</div>
							<div class="form-group">
								<asp:Label ID="lblContactEmail" runat="server" Text="Email" CssClass="form-label"></asp:Label>
								<asp:TextBox ID="txtContactEmail" runat="server" CssClass="form-control"></asp:TextBox>
							</div>
							<div class="form-group">
								<asp:Label ID="lblContactPhone" class="col-form-label" runat="server" Text="Phone" CssClass="form-label"></asp:Label>
								<asp:TextBox ID="txtContactPhone" runat="server" CssClass="form-control"></asp:TextBox>
							</div>
							<div class="form-group">
								<asp:Button ID="btnAdd" class="btn btn-primary btn-md" runat="server" Text="Add" OnClick="btnAdd_Click" />
								<asp:Button ID="btnDelete" visible="false" class="btn btn-primary btn-md" runat="server" Text="Delete" OnClick="btnDelete_Click" />
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
</asp:Content>