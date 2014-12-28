
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NaujaApklausa.aspx.cs" Inherits="PollGenerator.Poll.Views.NaujaApklausa"
    Title="Kurti naują apklausą | Klausiu.lt" MasterPageFile="~/Shared/DefaultMaster.master" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" Runat="Server">
        <h2>Kurti naują apklausą</h2>
		
		<br />
		Pavadinimas:
		<br />
		<br />
        <asp:TextBox ID="NameTextBox" runat="server" Width="350px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="PavadinimasRequiredFieldValidator" 
            runat="server" ErrorMessage="Nurodykite apklausos pavadinimą." 
            ControlToValidate="NameTextBox" Display="Dynamic"></asp:RequiredFieldValidator>
       
        <asp:CustomValidator ID="PollNameCustomValidator" runat="server" OnServerValidate="PollNameCustomValidation_ServerValidate" ErrorMessage="Šiuo pavadinimu apklausą jau esate sukūręs." ControlToValidate="NameTextBox" ValidateEmptyText="False"></asp:CustomValidator>   
        <br />
        <asp:RegularExpressionValidator ID="NameTextBoxRegularExpressionValidator" runat="server" ErrorMessage="Pavadinimas turi būti be specialiųjų simbolių." ControlToValidate="NameTextBox" ValidationExpression="^\s*[a-žA-Ž0-9,\s]+\s*$" Display="Dynamic"></asp:RegularExpressionValidator>
        
        <br />
        <br />
        Aprašymas:
        <br />
        <br />
        <asp:TextBox ID="DescriptionTextBox" runat="server" Height="70px" 
            TextMode="MultiLine" Width="350px"></asp:TextBox>
            
        <asp:RequiredFieldValidator ID="AprasymasRequiredFieldValidator" 
            runat="server" ErrorMessage="Nurodykite apklausos aprašymą." 
            ControlToValidate="DescriptionTextBox" Display="Dynamic"></asp:RequiredFieldValidator>
            
        <br />
        <br />
        Pranešimas respodentui apie sėkmingai baigtą apklausą:
        <br />
        <br />
        <asp:TextBox ID="EndDescTextBox" runat="server" Height="70px" 
            TextMode="MultiLine" Width="350px">Gerbiamas, respondente. Jūs sėkmingai baigėte apklausą. Ačiū už suteiktą laiką bei atsakymus.</asp:TextBox>
         
         <asp:RequiredFieldValidator ID="PabaigaRequiredFieldValidator" 
            runat="server" ErrorMessage="Nurodykite pranešimą respodentui apie apklausos pabaigą." 
            ControlToValidate="EndDescTextBox" Display="Dynamic"></asp:RequiredFieldValidator>   
       
        <br />
        <br />
        <asp:Button ID="CreateButton" runat="server" Text="Sukurti" 
        onclick="CreateButton_Click" />
                
</asp:Content>
