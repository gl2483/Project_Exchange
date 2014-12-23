using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["UserId"] != null)
            {
                RecentHistory.Visible = true;
                currentUserId.Text = Session["UserId"].ToString();
                List<Item2> items = null;
                string conString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                SqlConnection con = new SqlConnection(conString);
                SqlCommand com = new SqlCommand("SELECT TOP 20 i.ItemId, ItemName, ItemDesc, ContentType, Image, Category, i.CreatedOn, UserName, IsActive, i.UserId, 1 AS Preference, MAX(r.CreatedOn) FROM ExchangeDB..RecentHistory r JOIN ExchangeDB..Users u ON u.UserId = r.UserId JOIN ExchangeDB..Item i ON i.ItemId = r.ItemId WHERE r.UserId = @UserId AND i.IsActive = 1 GROUP BY i.ItemId, ItemName, ItemDesc, ContentType, Image, Category, i.CreatedOn, UserName, IsActive, i.UserId Order by MAX(r.CreatedOn) Desc", con);
                com.Parameters.Add("@UserId", SqlDbType.Int).Value = Convert.ToInt32(Session["UserId"]);
                con.Open();
                var lDataAdapter = new SqlDataAdapter(com);
                var lDataSet = new DataSet();
                lDataAdapter.Fill(lDataSet);
                if (lDataSet.Tables[0].Rows.Count == 0)
                {
                    con.Close();
                }
                else
                {
                    items = lDataSet.Tables[0].AsEnumerable().Select(row => new Item2(row, Session["UserId"] == null ? 0 : Convert.ToInt32(Session["UserId"]))).ToList();
                    con.Close();
                }
                RecentHistory.DataSource = items;
                RecentHistory.DataBind();
            }
        }
    }

    protected void recentItemButton_Command(object sender, CommandEventArgs e)
    {
        string url = "~/ViewEditItem.aspx?ItemId=" + e.CommandArgument;
        if (Session["UserId"] != null)
            AddRecentHistory(Convert.ToInt32(Session["UserId"]), Convert.ToInt32(e.CommandArgument));
        Response.Redirect(url);
    }

    protected void AddRecentHistory(int pUserId, int pItemId)
    {
        string conString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        SqlConnection con = new SqlConnection(conString);
        SqlCommand com = new SqlCommand("Insert ExchangeDB..RecentHistory (UserId, ItemId) VALUES (@UserId, @ItemId)", con);
        com.Parameters.Add("@UserId", SqlDbType.Int).Value = pUserId;
        com.Parameters.Add("@ItemId", SqlDbType.Int).Value = pItemId;
        con.Open();
        try
        {
            com.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
        }
        con.Close();
    }
}
