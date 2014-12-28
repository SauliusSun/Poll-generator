
<%@ page language="C#" autoeventwireup="true" inherits="PollGenerator.Shell.Views.NaujasVartotojas, App_Web_gdhqcrod" title="Registracija | Klausiu.lt" masterpagefile="~/Shared/DefaultMaster.master" stylesheettheme="Default" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" Runat="Server">
		<h2>Naujo vartotojo registracija</h2>
		
		<asp:CreateUserWizard ID="CreateUserWizard1" runat="server"
            AutoGeneratePassword="True" CancelDestinationPageUrl="~/Apie.aspx" 
            FinishDestinationPageUrl="~/Poll/ManoApklausos.aspx" LoginCreatedUser="False" 
    AnswerLabelText="Saugumo atsakymas:" 
    AnswerRequiredErrorMessage="Įveskite saugumo klausimo atsakymą." 
    ConfirmPasswordCompareErrorMessage="Slaptažodis ir slaptažodžio patvirtinimas turi sutapti." 
    ConfirmPasswordRequiredErrorMessage="Įveskite slaptažodžio patvirtinimą." 
    CreateUserButtonText="Sukurti" EmailLabelText="El. paštas:" 
    EmailRegularExpressionErrorMessage="Įveskite kitą el. paštą." 
    EmailRequiredErrorMessage="Įveskite el. paštą." PasswordLabelText="Slaptažodis:" 
    PasswordRegularExpressionErrorMessage="Įveskite kitą slaptažodį." 
    PasswordRequiredErrorMessage="Įveskite slaptažodį." 
    QuestionLabelText="Saugumo klausimas:" 
    QuestionRequiredErrorMessage="Įveskite saugumo klausimą." 
    UserNameLabelText="Vartotojas:" 
    UserNameRequiredErrorMessage="Įveskite vartotojo vardą." 
            onsendingmail="CreateUserWizard1_SendingMail" 
            DuplicateEmailErrorMessage="Šiuo el. pašto adresu vartotojas jau užregistruotas." 
            DuplicateUserNameErrorMessage="Tokiu vardu vartotojas jau egzistuoja." 
            InvalidAnswerErrorMessage="Prašome įvesti kitokį saugumo klausimą." 
            InvalidEmailErrorMessage="Prašome įvesti teisingą el. paštą." 
            InvalidPasswordErrorMessage="Slaptažodžio minimalus ilgis: {0}." 
            InvalidQuestionErrorMessage="Prašome įvesti kitokį saugumo klausimą."     
            UnknownErrorMessage="Vartotojas nebuvo sukurtas. Prašome pabandyti dar kartą." 
            DisableCreatedUser="False" oncreateduser="CreateUserWizard1_CreatedUser">
        <MailDefinition BodyFileName="~/Registracija.htm" From="info@klausiu.lt" 
            Priority="Normal" Subject="Pranešimas apie sukurtą vartotoją Klausiu.lt apklausų sistemoje">
        </MailDefinition>
        <WizardSteps>
            <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server" 
                Title="">
            </asp:CreateUserWizardStep>
            <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server" Title="Registracijos pabaiga.">
                <ContentTemplate>
                    <table border="0">
                        <tr>
                            <td align="center" colspan="2">
                                Pabaiga</td>
                        </tr>
                        <tr>
                            <td>
                                Jūsų vartotojas sėkmingai sukurtas. Prisijungimo duomenis galite sužinoti prisijungę į registracijos metu nurodytą el. paštą.</td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:CompleteWizardStep>
        </WizardSteps>
    </asp:CreateUserWizard>
    
</asp:Content>
