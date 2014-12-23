using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public partial class _SuggestedExchange : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            List<SuggestedExchange> suggestions = null;
            string conString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            SqlConnection con = new SqlConnection(conString);
            SqlCommand com = new SqlCommand("SELECT TOP 10 i2.ItemId MyItemId, i2.ItemName MyItemName, i2.ItemDesc MyItemDesc, i2.ContentType MyContentType, i2.Image MyImage, i2.Category MyCategory, i2.UserId MyUserId, " +
                                                   "i.ItemId, i.ItemName, i.ItemDesc, i.ContentType, i.Image, i.Category, i.UserId " +
                                            "FROM ExchangeDB..Item i " +
                                            "JOIN ExchangeDB..UserPreference up On up.ItemId = i.ItemId " +
                                            "LEFT JOIN ExchangeDB..UserPreference up2 ON i.UserId = up2.UserId " +
                                            "LEFT JOIN ExchangeDB..Item i2 ON i2.ItemId = up2.ItemId " +
                                            "LEFT JOIN ExchangeDB..Request r ON r.RequesterUserId IN (i.UserId,i2.UserId) AND r.RequesterItemId IN (i2.ItemId,i.ItemId) AND r.UserId IN (i.UserId,i2.UserId) AND r.ItemId IN (i2.ItemId,i.ItemId) " +
                                            "WHERE up2.userId = @UserId AND up2.ItemId IS NOT NULL AND i.IsActive = 1 AND i2.IsActive = 1 AND r.RequestId IS NULL " + 
                                            " UNION ALL " +
                                            "SELECT TOP 10 i.ItemId MyItemId, i.ItemName MyItemName, i.ItemDesc MyItemDesc, i.ContentType MyContentType, i.Image MyImage, i.Category MyCategory, i.UserId MyUserId, " +
		                                            "i2.ItemId, i2.ItemName, i2.ItemDesc, i2.ContentType, i2.Image, i2.Category, i2.UserId  " +
                                            "FROM (SELECT i.ItemId, i.UserId FROM ExchangeDB..RecentHistory r JOIN ExchangeDB..Users u ON u.UserId = r.UserId JOIN ExchangeDB..Item i ON i.ItemId = r.ItemId WHERE r.UserId = @UserId AND i.IsActive = 1 AND i.UserId != @UserId GROUP BY i.ItemId, i.UserId) x " +
                                            "JOIN ExchangeDB..RecentHistory r ON r.UserId = x.UserId " +
                                            "JOIN ExchangeDB..Item i ON i.ItemId = r.ItemId AND i.UserId = @UserId " +
                                            "JOIN ExchangeDB..Item i2 ON i2.ItemId = x.ItemId " +
                                            "WHERE i.IsActive = 1 AND i2.IsActive = 1", con);
            com.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["UserId"] == null ? 0 : Convert.ToInt32(Session["UserId"]);
            con.Open();
            var lDataAdapter = new SqlDataAdapter(com);
            var lDataSet = new DataSet();
            lDataAdapter.Fill(lDataSet);
            if (lDataSet.Tables[0].Rows.Count != 0)
            {
                suggestions = lDataSet.Tables[0].AsEnumerable().Select(row => new SuggestedExchange(row)).ToList();
            }
            con.Close();
            SuggestedList.DataSource = suggestions;
            SuggestedList.DataBind();
        }
    }

    protected void itemButton_Command(object sender, CommandEventArgs e)
    {
        string url = "~/ViewEditItem.aspx?ItemId=" + e.CommandArgument;
        Response.Redirect(url);
    }

    protected void requestButton_Command(object sender, CommandEventArgs e)
    {
        string[] ids = e.CommandArgument.ToString().Split('|');
        string conString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        SqlConnection con = new SqlConnection(conString);
        SqlCommand com = new SqlCommand("INSERT ExchangeDB..Request(RequesterUserId, RequesterItemId, UserId, ItemId, RequestComplete) " +
                                        "SELECT i1.UserId, i1.ItemId, i2.UserId, i2.ItemId, 0 " +
                                        "FROM (SELECT @MyItemId as ItemId1 ,@ItemId as ItemId2) x " +
                                        "JOIN ExchangeDB..Item i1 ON x.ItemId1 = i1.ItemId " +
                                        "JOIN ExchangeDB..Item i2 ON x.ItemId2 = i2.ItemId", con);
        com.Parameters.Add("@MyItemId", SqlDbType.Int).Value = Convert.ToInt32(ids[0]);
        com.Parameters.Add("@ItemId", SqlDbType.Int).Value = Convert.ToInt32(ids[1]);
        SqlCommand com2 = new SqlCommand("INSERT ExchangeDB..[Message](FromUserId, ToUserId, MessageBody, ItemId) " +
                                        "SELECT i1.UserId, i2.UserId, 'Requested an exchange for item', i2.ItemId " +
                                        "FROM (SELECT @MyItemId as ItemId1 ,@ItemId as ItemId2) x " +
                                        "JOIN ExchangeDB..Item i1 ON x.ItemId1 = i1.ItemId " +
                                        "JOIN ExchangeDB..Item i2 ON x.ItemId2 = i2.ItemId", con);
        com2.Parameters.Add("@MyItemId", SqlDbType.Int).Value = Convert.ToInt32(ids[0]);
        com2.Parameters.Add("@ItemId", SqlDbType.Int).Value = Convert.ToInt32(ids[1]);
        con.Open();
        try
        {
            com.ExecuteNonQuery();
            com2.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
        }
        con.Close();
        Response.Redirect(HttpContext.Current.Request.Url.ToString());
    }
}