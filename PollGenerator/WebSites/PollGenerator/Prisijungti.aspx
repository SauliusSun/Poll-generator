
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Prisijungti.aspx.cs" Inherits="PollGenerator.Shell.Views.Prisijungti"
    Title="Prisijungti | Klausiu.lt" MasterPageFile="~/Shared/DefaultMaster.master" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" Runat="Server">
        <h2>Prisijungti</h2>           
		
        <asp:Login ID="LoginControl"  runat="server" 
            FailureText="Neteisingai įvestas vartotojo vardas arba slaptažodis." 
            LoginButtonText="Prisijungti" PasswordLabelText="Slaptažodis:" 
            PasswordRequiredErrorMessage="Įveskite slaptažodį." 
            RememberMeText="Atsiminti mane" TitleText="" 
            UserNameLabelText="Vartotojo vardas:" 
            UserNameRequiredErrorMessage="Įveskite vartotojo vardą." 
            CreateUserUrl="~/Registracija.aspx" DestinationPageUrl="~/Poll/ManoApklausos.aspx" 
            OnLoggedIn="LoginControl_LoggedIn"
            PasswordRecoveryUrl="~/SlaptazodzioAtstatymas.aspx" PasswordRecoveryText="Užmiršote slaptažodį?">
        </asp:Login> 
        <br />
        Naujas vartotojas? <a href="Registracija.aspx" ><b>Registruokitės</b></a>.
		
</asp:Content>
