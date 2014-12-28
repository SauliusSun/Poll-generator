
<%@ page language="C#" autoeventwireup="true" inherits="PollGenerator.Shell.Views.Atsakyk, App_Web_gdhqcrod" title="Atsakyk! | Klausiu.lt" masterpagefile="~/Shared/DefaultMaster.master" stylesheettheme="Default" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" Runat="Server">
		<h2>Naujausios apklausos laukia tavo atsakymø!</h2>
		
        <asp:GridView ID="PublicPollsGridView" runat="server" 
            AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE" 
            BorderStyle="None" BorderWidth="1px" CellPadding="4" 
            DataSourceID="PublicPollsLinqDataSource" ForeColor="Black" GridLines="Vertical" 
            ShowHeader="False" onrowdatabound="PublicPollsGridView_RowDataBound1">
            <RowStyle BackColor="#F7F7DE" />
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" 
                ReadOnly="True" SortExpression="Id" Visible="false" />
                <asp:TemplateField>
                <ItemTemplate>
                <asp:HyperLink ID="NameHyperlink" Target="_blank" Font-Bold="true" ImageUrl='<%# Eval("Id") %>' Text='<%# Eval("Name") %>'  runat="server" > </asp:HyperLink>
                </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Description" HeaderText="Description" 
                    ReadOnly="True" SortExpression="Description" />
                <asp:BoundField DataField="Created" HeaderText="Created" ReadOnly="True" 
                    SortExpression="Created" DataFormatString="{0:d}" />
            </Columns>
            <FooterStyle BackColor="#CCCC99" />
            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" />
        
        </asp:GridView>
        <asp:LinqDataSource ID="PublicPollsLinqDataSource" runat="server" 
            ContextTypeName="PollDataContext" OrderBy="Created" 
            Select="new (Name, Description, Created, Id)" TableName="Polls" 
            Where="PublicPoll == @PublicPoll">
            <WhereParameters>
                <asp:Parameter DefaultValue="True" Name="PublicPoll" Type="Boolean" />
            </WhereParameters>
        </asp:LinqDataSource>
</asp:Content>
