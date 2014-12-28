using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using PollGenerator.Shell.Views;
using Microsoft.Practices.ObjectBuilder;
using DataAccess;

public partial class ShellDefault : Microsoft.Practices.CompositeWeb.Web.UI.Page, IDefaultView
{
    private DefaultViewPresenter _presenter;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this._presenter.OnViewInitialized();

            DbAccessManager DbManager = new DbAccessManager();

            int pollCount = DbManager.GetPollCount();

            int questionCount = DbManager.GetQuestionCount();

            int answerCount = DbManager.GetAnswerCount();

            ContentPlaceHolder content = (ContentPlaceHolder)Master.FindControl("DefaultContent");

            content.Controls.Add(new LiteralControl("<h2>" + "&nbsp; &nbsp; &nbsp;" + pollCount.ToString() + "&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;  " + questionCount.ToString() + "&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" + answerCount.ToString() + "</h2>"));

        }
        this._presenter.OnViewLoaded();
    }

    [CreateNew]
    public DefaultViewPresenter Presenter
    {
        get
		{
			return this._presenter;
		}
        set
        {
            if(value == null)
                throw new ArgumentNullException("value");

            this._presenter = value;
            this._presenter.View = this;
        }
    }
}
