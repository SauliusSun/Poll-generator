using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;

namespace PollGenerator.Shell.Views
{
    public class DefaultViewPresenter : Presenter<IDefaultView>
    {
        public DefaultViewPresenter()
        {
        }
    }
}
