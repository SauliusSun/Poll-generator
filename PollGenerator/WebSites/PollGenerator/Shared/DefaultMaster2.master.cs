using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Shared_MasterPage2 : System.Web.UI.MasterPage
{
    private int index = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["itemName"] != null)
        {
            string selectedItem = (string)Session["itemName"];

            Menu1.DataBind();

            foreach (MenuItem item in Menu1.Items)
            {
                if (item.Text == selectedItem)
                {
                    item.Selected = true;
                }
                
            }
            
        }
        else
        {
            Menu1.DataBind();

            Menu1.Items[index].Selected = true;
        }

    }

    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        Session["itemName"] = e.Item.Text;
        
        Response.Redirect(e.Item.Value);

        
        //foreach (MenuItem item in Menu1.Items)
        //{

        //    if (item.Selected)
        //    {
        //        MenuItemStyle style;

        //        //Menu1.StaticSelectedStyle = "active";




        //    }
        //}
    }
}
