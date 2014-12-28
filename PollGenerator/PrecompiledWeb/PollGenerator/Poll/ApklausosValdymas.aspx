
<%@ page language="C#" autoeventwireup="true" inherits="PollGenerator.Poll.Views.ApklausosValdymas, App_Web_3ou9upjt" title="Apklausos valdymas | Klausiu.lt" responseencoding="windows-1257" contenttype="text/HTML" culture="lt-LT" uiculture="lt-LT" masterpagefile="~/Shared/DefaultMaster.master" stylesheettheme="Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" Runat="Server">
        <h2>Apklausos valdymas</h2>
		
		<br />
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
         
        <cc1:TabContainer ID="TabContainer" runat="server" onload="TabContainer_Load" >
        
        
        <cc1:TabPanel ID="PreferencesTabPanel" runat="server" HeaderText="Nustatymai">
        <ContentTemplate>
        <br />
		Pavadinimas:
		<br />
		<br />
        <asp:TextBox ID="NameTextBox" runat="server" Width="350px"></asp:TextBox>
        <br />
        <br />
        Apra�ymas:
        <br />
        <br />
        <asp:TextBox ID="DescriptionTextBox" runat="server" Height="70px" 
            TextMode="MultiLine" Width="350px"></asp:TextBox>
        <br />
        <br />
        Prane�imas respodentui apie s�kmingai baigt� apklaus�:
        <br />
        <br />
        <asp:TextBox ID="EndDescTextBox" runat="server" Height="70px" 
            TextMode="MultiLine" Width="350px"></asp:TextBox>
        <br />
        <br />
        Kiek respondent� apklausti (0 - neribot� skai�i�):
        <br />
        <br />
        <asp:TextBox ID="PollLimitTextBox" runat="server"></asp:TextBox>
        <br />
        <br />
        Pabaigus apklaus� respodent� nukreipti internetiniu adresu (nurodykite piln� adres�, pvz., http://klausiu.lt):
        <br />
        <br />
        <asp:TextBox ID="RedirectUrlTextBox" runat="server" Width="300px"></asp:TextBox>
        <br />
        <br />
        Rodyti bendrus apklausos rezultatus respondentui:
        <br />
        <br />
        <asp:DropDownList ID="ShowResultsDropDownList" runat="server">
        <asp:ListItem Value="true">Taip</asp:ListItem>
        <asp:ListItem Value="false">Ne</asp:ListItem>
        </asp:DropDownList>
        <br />
        <br />
        Respendentas gali atsakyti � apklaus� kelis kartus:
        <br />
        <br />
        <asp:DropDownList ID="MultipleAnswersDropDownList" runat="server">
        <asp:ListItem Value="true">Taip</asp:ListItem>
        <asp:ListItem Value="false">Ne</asp:ListItem>
        </asp:DropDownList>
        <br />
        <br />
        Ar apklausa vie�a? (Pa�ym�kite tik tuo atveju jei apklausa pilnai paruo�ta naudojimui)
        <br />
        <br />
        <asp:DropDownList ID="PublicPollDropDownList" runat="server">
        <asp:ListItem Value="true">Taip</asp:ListItem>
        <asp:ListItem Value="false">Ne</asp:ListItem>
        </asp:DropDownList>
        <br />
        <br />
        Nuoroda � apklaus�:
        <br />
        <br />
        <asp:Label ID="LinkLabel" BorderWidth="1" runat="server" Text=""></asp:Label>
        <br />
        <br />
        Nuoroda � apklausos BB kod�:
        <br />
        <br />
        <asp:Label ID="BBLabel" runat="server" BorderWidth="1" Text=""></asp:Label>
        <br />
        <br />
        Nuoroda � apklausos HTML kod�:
        <br />
        <br />
        <asp:Label ID="HtmlLabel" BorderWidth="1" runat="server" Text=""></asp:Label>
        <br />
        <br />
        <asp:Button ID="SaveButton" runat="server" Text="I�saugoti" OnClick="SaveButton_Click" />
            
        </ContentTemplate>
        </cc1:TabPanel>
        
        
            
            
        
        
        
        <cc1:TabPanel ID="NewQuestionTabPanel" HeaderText="Naujas klausimas" runat="server" >
        
            <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
            <asp:Panel ID="NewQPanel" runat="server">
                <br />
		        <br />
		        Klausimas:
		        <br />
		        <br />
                <asp:TextBox ID="QuestionTextBox" runat="server" Width="350px" Height="" 
                    TextMode="SingleLine"></asp:TextBox>
                    
                 
                <asp:RequiredFieldValidator ID="QuestionRequiredFieldValidator" runat="server" ErrorMessage="�veskite klausimo pavadinim�." ControlToValidate="QuestionTextBox" ValidationGroup="NewQuestion"></asp:RequiredFieldValidator>   
                <br />   
                <br />
                
                Klausimo tipas:
                <br />
                <br />
                
                <asp:DropDownList ID="QuestionTypeDropDownList" runat="server" AutoPostBack="True" onselectedindexchanged="QuestionTypeDropDownList_SelectedIndexChanged" Width="350px">
                    <asp:ListItem Selected="True" Value="1">Tekstinis laukelis vienoje eilut�je</asp:ListItem>
                    <asp:ListItem Value="2">Didelis tekstinis laukas</asp:ListItem>
                    <asp:ListItem Value="3">Galima pasirinkti vien� atsakym� (i�krentantis meniu)</asp:ListItem>
                    <asp:ListItem Value="4">Galima pasirinkti kelis atsakymus (varnel�s)</asp:ListItem>
                    <asp:ListItem Value="5">Galima pasirinkti vien� atsakym� (rutuliukai)</asp:ListItem>
                    <asp:ListItem Value="6">Informacinis prane�imas</asp:ListItem>
                </asp:DropDownList>
                
                <br />
                <br />
                <asp:Button ID="ValueButton" runat="server" Text="Prid�ti atsakym�" 
                    Visible="false" onclick="ValueButton_Click" EnableViewState="False" />
                <br />
                <br />
                Ar respondentas privalo atsakyti � klausim�?
                <br />
                <asp:DropDownList ID="RequiredDropDownList" runat="server" ValidationGroup="NewQuestion">
                <asp:ListItem Value="T">Taip</asp:ListItem>
                <asp:ListItem Selected="True" Value="F">Ne</asp:ListItem>          
                </asp:DropDownList>
                <br />
                <br />
                <asp:Button ID="AddQuestionButton" runat="server" Text="Prid�ti klausim�" onclick="AddQuestionButton_Click" ValidationGroup="NewQuestion" />
        
        <asp:LinqDataSource ID="PollDataSource" runat="server" 
            ContextTypeName="PollDataContext" Select="new (Name, Id)" TableName="Polls">
        </asp:LinqDataSource>
        </asp:Panel>
            </ContentTemplate>
            </asp:UpdatePanel>
            </ContentTemplate>
            
        </cc1:TabPanel>
       
        
        
        
        
     
        
        
        
            <cc1:TabPanel ID="Panel3" HeaderText="Klausimai" runat="server">
                <ContentTemplate >
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <cc1:ReorderList ID="QuestionReorderList" runat="server"
                        PostBackOnReorder="true"  
                        DragHandleAlignment="Left"         
                        DataSourceID="QDataSource"
                        ItemInsertLocation="Beginning" 
                         OnInit="QuestionReorderList_Init"      
                        Width="400px" AllowReorder="True" DataKeyField="Id" SortOrderField="Sequence">       
                    <ItemTemplate>
                    <asp:BulletedList CssClass="reorderCue" runat="server" >
                    </asp:BulletedList>
                        <div class="itemArea">
                        <asp:Label ID="Sequence" runat="server" Text = '<%# Eval("Sequence")%>'></asp:Label> &nbsp;                            
                        <asp:Label ID="Name" runat="server" Text='<%# Eval("Name") %>'></asp:Label>    
                        <asp:ImageButton  ID="DeleteButton" CommandArgument='<%# Eval("Id") %>' OnClick="DeleteButton_Click" ImageUrl="~/App_Themes/delete.ico" ToolTip="I�trinti klausim�?" runat="server" />                     
                        </div>
                    </ItemTemplate>
                    <ReorderTemplate>
                        <asp:Panel ID="Panel2" runat="server" CssClass="" />
                    </ReorderTemplate>
                     <DragHandleTemplate>
                            <div class="dragHandle"></div>
                        </DragHandleTemplate>
                    </cc1:ReorderList>
                </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </cc1:TabPanel>
            
            

            
        
            <cc1:TabPanel ID="Panel4" HeaderText="Statistika" runat="server">
                <ContentTemplate >
                    
                </ContentTemplate>
            
            </cc1:TabPanel>
            
            
            
            
            <cc1:TabPanel ID="Panel5" HeaderText="Rezultatai" runat="server">
                <ContentTemplate >
                Paspaud� mygtuk� "Persi�sti" galite persisi�sti piln� ir detali� rezultat� ataskait� Excel (.xls) formatu.
                <br />
                <br />
                <asp:Button ID="ResultsButton" Enabled="false" runat="server" Text="Persi�sti" OnClick="ResultsButton_Click" CausesValidation="False" />
                
                    
                </ContentTemplate>
            
            </cc1:TabPanel>
    
        </cc1:TabContainer>
        
         <asp:LinqDataSource ID="QuestionsLinqDataSource" runat="server" 
            ContextTypeName="PollDataContext" Select="new (Name, Id, Sequence)" 
            TableName="Questions" OrderBy="Sequence" EnableDelete="True" 
            EnableInsert="True" EnableUpdate="True">
              <UpdateParameters>
                        
                        <asp:Parameter Name="Name" Type="String" />
                        <asp:Parameter Name="Id" Type="Int32" />
                        <asp:Parameter Name="Sequence" Type="Int32" />
             </UpdateParameters>
        </asp:LinqDataSource>
        
        <asp:LinqDataSource ID="PollLinqDataSource" runat="server" 
            ContextTypeName="PollDataContext" Select="new (Name, Id)" TableName="Polls">
        </asp:LinqDataSource>
        
    <asp:SqlDataSource ID="QDataSource" runat="server" 
            ConnectionString="<%$ ConnectionStrings:PollsDbConnectionString %>" 
            DeleteCommand="DELETE FROM [Question] WHERE [Id] = @Id" 
            InsertCommand="INSERT INTO [Question] ([Name], [Sequence]) VALUES (@Name, @Sequence)" 
            SelectCommand="SELECT [Id], [Name], [Sequence] FROM [Question] WHERE ([PollId] = @PollId) ORDER BY [Sequence]" 
            UpdateCommand="UPDATE [Question] SET [Name] = @Name, [Sequence] = @Sequence WHERE [Id] = @Id"
            ProviderName="System.Data.SqlClient">
        <SelectParameters>
            <asp:SessionParameter Name="PollId" SessionField="PollIdSession" Type="Int32" />
            
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Name" Type="String" />
            <asp:Parameter Name="Sequence" Type="Int32" />
            <asp:Parameter Name="Id" Type="Int32" />
        </UpdateParameters>
         <InsertParameters>
            <asp:Parameter Name="Name" Type="String" />
            <asp:Parameter Name="Sequence" Type="Int32" />
        </InsertParameters>
        </asp:SqlDataSource>
</asp:Content>
