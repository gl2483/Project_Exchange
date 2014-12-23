using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            MenuItemCollection menuItems = NavigationMenu.Items;
            MenuItem Item = new MenuItem();
            foreach (MenuItem menuItem in menuItems)
            {
                if (menuItem.Text == "Home")
                    Item = menuItem;
            }
            menuItems.Remove(Item);
            foreach (MenuItem menuItem in menuItems)
            {
                if (menuItem.Text == "Suggested Exchange")
                    Item = menuItem;
            }
            menuItems.Remove(Item);
            foreach (MenuItem menuItem in menuItems)
            {
                if (menuItem.Text == "Request")
                    Item = menuItem;
            }
            menuItems.Remove(Item);
            //LogOutButton.Visible = false;
        }
        else {
            //LogOutButton.Visible = true;
        }
    }

    protected void HeadLoginStatus_LoggedOut(Object sender, System.EventArgs e)
    {
        HttpContext.Current.Session.Abandon();
        Response.Redirect("~/Default.aspx");
    }

    protected void LoggingOut(Object sender, System.EventArgs e)
    {
        HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
        cookie1.Expires = DateTime.Now.AddYears(-1);
        Response.Cookies.Add(cookie1);
    }
}
