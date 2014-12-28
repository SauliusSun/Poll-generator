using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;

namespace PollGenerator.Poll.Views
{
    public class DefaultViewPresenter : Presenter<IDefaultView>
    {
        private IPollController _controller;

        public DefaultViewPresenter([CreateNew] IPollController controller)
        {
            this._controller = controller;
        }
    }
}
