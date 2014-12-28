
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SlaptazodzioAtstatymas.aspx.cs" Inherits="PollGenerator.Shell.Views.SlaptazodzioAtstatymas"
    Title="Vartotojo slaptažodžio atstatymas | Klausiu.lt" MasterPageFile="~/Shared/DefaultMaster.master" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" Runat="Server">
		<h2>Vartotojo slaptažodžio atstatymas</h2>
		
		<asp:PasswordRecovery ID="PasswordRecovery1" runat="server" 
            AnswerLabelText="Atsakymas:" AnswerRequiredErrorMessage="Įveskite atsakymą į klausimą." 
            GeneralFailureText="Nepavyko gauti slaptažodžio. Bandykite dar kartą." 
            QuestionFailureText="Nepavyko patikrinti atsakymo. Bandykite dar kartą." 
            QuestionInstructionText="Atsakykite į klausimą, kad gautumėte slaptažodį." 
            QuestionLabelText="Klausimas:" QuestionTitleText="Tapatybės patvirtinimas" 
            SubmitButtonText="Tęsti" 
            SuccessText="Vartotojo prisijungimo duomenys išsiųsti į el. paštą kurį nurodėte registracijos sistemoje metu." 
            UserNameFailureText="Nepavyko rasti šio vartotojo informacijos. Bandykite dar kartą." 
            UserNameInstructionText="Įveskite vartotojo vardą." 
            UserNameLabelText="Vartotojo vardas:" 
            UserNameRequiredErrorMessage="Įveskite vartotojo vardą." 
            UserNameTitleText="" 
            onsendingmail="PasswordRecovery1_SendingMail">
            <MailDefinition BodyFileName="~/SlaptazodzioAtstatymas.htm">
            </MailDefinition>
        </asp:PasswordRecovery>
</asp:Content>
