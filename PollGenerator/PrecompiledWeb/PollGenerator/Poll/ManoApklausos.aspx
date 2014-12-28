
<%@ page language="C#" autoeventwireup="true" inherits="PollGenerator.Poll.Views.ManoApklausos, App_Web_3ou9upjt" title="Mano apklausos | Klausiu.lt" masterpagefile="~/Shared/DefaultMaster.master" stylesheettheme="Default" %>
<%@ Import Namespace="EncryptionUtility"%>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" Runat="Server">
        <h2>Mano apklausos</h2>
		
		<asp:GridView ID="MyPollsGridView" runat="server" AutoGenerateColumns="False" 
            DataSourceID="MyPollsDataSource" 
            ondatabinding="MyPollsGridView_DataBinding" CellPadding="4" ForeColor="Black" 
            GridLines="Vertical" ShowHeader="False" BackColor="White" 
            BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
            onrowdatabound="MyPollsGridView_RowDataBound1">
            
            <RowStyle BackColor="#F7F7DE" />
            
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="Id" 
                ReadOnly="True" SortExpression="Id" Visible="false" />
                <asp:TemplateField>
                <ItemTemplate>
                <asp:HyperLink ID="NameHyperlink" Font-Bold="true" ImageUrl='<%# Eval("Id") %>' Text='<%# Eval("Name") %>' runat="server" > </asp:HyperLink>
                </ItemTemplate>
                </asp:TemplateField>
            
            <asp:BoundField DataField="Description" 
                SortExpression="Description" ReadOnly="True" />
            <asp:BoundField DataField="Created" ReadOnly="True" 
                SortExpression="Created" DataFormatString="{0:d}" />
            <asp:TemplateField>
            <ItemTemplate>
            <asp:ImageButton ID="PreferenceButton" CommandArgument='<%# Eval("Id") %>' OnClick="PreferenceButton_Click" ImageUrl="~/App_Themes/clipboard_16.png" ToolTip="Peržiūrėti apklausos nustatymus?" runat="server" />                     
            </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField>
            <ItemTemplate>
            <asp:ImageButton ID="ViewButton"  CommandArgument='<%# Eval("Id") %>' OnClick="ViewButton_Click" ImageUrl="~/App_Themes/search_16.png" ToolTip="Peržiūrėti apklausą?" runat="server" />                     
            </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
            <ItemTemplate>
            <asp:ImageButton ID="DeleteButton" CommandArgument='<%# Eval("Id") %>' OnClick="DeleteButton_Click" ImageUrl="~/App_Themes/delete.ico" ToolTip="Ištrinti apklausą?" runat="server" />                     
            </ItemTemplate>
            </asp:TemplateField>
            
        </Columns>
            <FooterStyle BackColor="#CCCC99" />
            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" />
    </asp:GridView>
    <asp:LinqDataSource ID="MyPollsDataSource" runat="server" 
            ContextTypeName="PollDataContext" TableName="Polls" 
            onselecting="MyPollsDataSource_Selecting" 
            Select="new (Id, Name, Description, Created)" Where="UserName == @UserName">
        <WhereParameters>
            <asp:CookieParameter CookieName="user" Name="UserName" Type="String" />
        </WhereParameters>
    </asp:LinqDataSource>
    
    <script type="text/javascript">

        function Navigate() {
            javascript: window.open("http://localhost:4839/PollGenerator/Apklausa.aspx?ID=");
        } 

    </script>



    
</asp:Content>


