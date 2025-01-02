<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Customers.aspx.cs" Inherits="AssessmentPlatformDeveloper.Customers" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta charset="utf-8" />
	<meta name="viewport" content="width=device-width, initial-scale=1.0" />
	<title><%: Page.Title %> RPM API Developer Assessment</title>

	<asp:PlaceHolder runat="server">
		<%: Scripts.Render("~/bundles/modernizr") %>
	</asp:PlaceHolder>

	<webopt:bundlereference runat="server" path="~/Content/css" />
	<link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />

</head>
<body>
<form id="form1" runat="server">
	<asp:ScriptManager runat="server">
		<Scripts>
			<%--To learn more about bundling scripts in ScriptManager see https://go.microsoft.com/fwlink/?LinkID=301884 --%>
			<%--Framework Scripts--%>
			<asp:ScriptReference Name="MsAjaxBundle" />
			<asp:ScriptReference Name="jquery" />
			<asp:ScriptReference Name="WebForms.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebForms.js" />
			<asp:ScriptReference Name="WebUIValidation.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebUIValidation.js" />
			<asp:ScriptReference Name="MenuStandards.js" Assembly="System.Web" Path="~/Scripts/WebForms/MenuStandards.js" />
			<asp:ScriptReference Name="GridView.js" Assembly="System.Web" Path="~/Scripts/WebForms/GridView.js" />
			<asp:ScriptReference Name="DetailsView.js" Assembly="System.Web" Path="~/Scripts/WebForms/DetailsView.js" />
			<asp:ScriptReference Name="TreeView.js" Assembly="System.Web" Path="~/Scripts/WebForms/TreeView.js" />
			<asp:ScriptReference Name="WebParts.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebParts.js" />
			<asp:ScriptReference Name="Focus.js" Assembly="System.Web" Path="~/Scripts/WebForms/Focus.js" />
			<asp:ScriptReference Name="WebFormsBundle" />
			<%--Site Scripts--%>
		</Scripts>
	</asp:ScriptManager>

	<nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-dark bg-dark">
		<div class="container body-content">
			<a class="navbar-brand" runat="server" href="~/">RPM API Developer Assessment</a>
			<button type="button" class="navbar-toggler" data-bs-toggle="collapse" data-bs-target=".navbar-collapse" title="Toggle navigation" aria-controls="navbarSupportedContent"
			        aria-expanded="false" aria-label="Toggle navigation">
				<span class="navbar-toggler-icon"></span>
			</button>
			<div class="collapse navbar-collapse d-sm-inline-flex justify-content-between">
				<ul class="navbar-nav flex-grow-1">
					<li class="nav-item">
						<a class="nav-link" runat="server" href="~/">Home</a>
					</li>
					<li class="nav-item">
						<a class="nav-link" runat="server" href="~/Customers">Customers</a>
					</li>
				</ul>
			</div>
		</div>
	</nav>

	<div>
		<div class="container body-content">
			<h2>Customer Registry</h2>
			<asp:DropDownList runat="server" ID="ddlCustomers" CssClass="form-control"/>
		</div>

		<div class="container body-content">
			<div class="card">

				<div class="card-body">

					<div class="row justify-content-center">

						<div class="col-md-6">
							<h1>Add customer</h1>
							<div class="form-group">
								<asp:Label ID="txtCustomerName" runat="server" Text="Name" CssClass="form-label"></asp:Label>
								<asp:TextBox ID="lblCustomerName" runat="server" CssClass="form-control"></asp:TextBox>
							</div>

							<div class="form-group">
								<asp:Label ID="lblCustomerAddress" runat="server" Text="Address" CssClass="form-label"></asp:Label>
								<asp:TextBox ID="txtCustomerAddress" runat="server" CssClass="form-control"></asp:TextBox>
							</div>

							<div class="form-group">
								<asp:Label ID="lblCustomerEmail" runat="server" Text="Email" CssClass="form-label"></asp:Label>
								<asp:TextBox ID="txtCustomerEmail" runat="server" CssClass="form-control"></asp:TextBox>
							</div>

							<div class="form-group">
								<asp:Label ID="lblCustomerPhone" runat="server" Text="Phone" CssClass="form-label"></asp:Label>
								<asp:TextBox ID="txtCustomerPhone" runat="server" CssClass="form-control"></asp:TextBox>
							</div>

							<div class="form-group">
								<asp:Label ID="lblCustomerCity" runat="server" Text="City" CssClass="form-label"></asp:Label>
								<asp:TextBox ID="txtCustomerCity" runat="server" CssClass="form-control"></asp:TextBox>
							</div>

							<div class="form-group">
								<asp:Label ID="lblCustomerState" runat="server" Text="Province/State" CssClass="form-label"></asp:Label>
								<asp:DropDownList ID="ddlState" runat="server" CssClass="form-control"/>
							</div>

							<div class="form-group">
								<asp:Label ID="lblCustomerZip" runat="server" Text="Postal/Zip Code" CssClass="form-label"></asp:Label>
								<asp:TextBox ID="txtCustomerZip" runat="server" CssClass="form-control"></asp:TextBox>
							</div>

							<div class="form-group">
								<asp:Label ID="lblCustomerCountry" runat="server" Text="Country" CssClass="form-label"></asp:Label>
								<asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control"/>
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
								<asp:Button ID="btnAdd" class="btn btn-primary btn-md" runat="server" Text="Add" OnClick="AddButton_Click" />
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
</form>
</body>
</html>