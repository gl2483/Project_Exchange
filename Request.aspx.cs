using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


public partial class Request : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            List<RequestEntity> requests = null;
            string conString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            SqlConnection con = new SqlConnection(conString);
            SqlCommand com = new SqlCommand("SELECT i2.ItemId MyItemId, i2.ItemName MyItemName, i2.ItemDesc MyItemDesc, i2.ContentType MyContentType, i2.Image MyImage, i2.Category MyCategory, i2.UserId MyUserId, " +
                                                    "i.ItemId, i.ItemName, i.ItemDesc, i.ContentType, i.Image, i.Category, i.UserId, r.RequestId, 0 As IsMyRequest, 'MyRequest' AS CssClass " +
                                            "FROM ExchangeDB..Request r " +
                                            "JOIN ExchangeDB..Item i ON r.RequesterItemId = i.ItemId " +
                                            "JOIN ExchangeDB..Item i2 ON r.ItemId = i2.ItemId " +
                                            "WHERE i2.UserId = @UserId AND r.RequestComplete = 0 " +
                                            "UNION ALL " +                                             
                                            "SELECT i.ItemId MyItemId, i.ItemName MyItemName, i.ItemDesc MyItemDesc, i.ContentType MyContentType, i.Image MyImage, i.Category MyCategory, i.UserId MyUserId, " +    
                                                    "i2.ItemId, i2.ItemName, i2.ItemDesc, i2.ContentType, i2.Image, i2.Category, i2.UserId, r.RequestId, 1 As IsMyRequest, 'Requested' AS CssClass " +
                                            "FROM ExchangeDB..Request r " +
                                            "JOIN ExchangeDB..Item i ON r.RequesterItemId = i.ItemId " +
                                            "JOIN ExchangeDB..Item i2 ON r.ItemId = i2.ItemId " +
                                            "WHERE i.UserId = @UserId AND r.RequestComplete = 0", con);
            com.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["UserId"] == null ? 0 : Convert.ToInt32(Session["UserId"]);
            con.Open();
            var lDataAdapter = new SqlDataAdapter(com);
            var lDataSet = new DataSet();
            lDataAdapter.Fill(lDataSet);
            if (lDataSet.Tables[0].Rows.Count != 0)
            {
                requests = lDataSet.Tables[0].AsEnumerable().Select(row => new RequestEntity(row)).ToList();
            }
            con.Close();
            RequestList.DataSource = requests;
            RequestList.DataBind();
        }
    }

    protected void itemButton_Command(object sender, CommandEventArgs e)
    {
        string url = "~/ViewEditItem.aspx?ItemId=" + e.CommandArgument;
        Response.Redirect(url);
    }

    protected void ActionButton_Command(object sender, CommandEventArgs e)
    {
        string conString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        SqlConnection con = new SqlConnection(conString);
        SqlCommand com;
        if (e.CommandName.ToLower() == "confirm")
        {
            com = new SqlCommand("Update ExchangeDB..Request SET RequestComplete = 1 WHERE RequestId = @RequestId " +
                                 "UPDATE ExchangeDB..Item SET IsActive = 0 WHERE ItemId = (SELECT RequesterItemId FROM ExchangeDB..Request WHERE RequestId = @RequestId) " +
                                 "UPDATE ExchangeDB..Item SET IsActive = 0 WHERE ItemId = (SELECT ItemId FROM ExchangeDB..Request WHERE RequestId = @RequestId) " +
                                 "INSERT ExchangeDB..[Message](FromUserId, ToUserId, MessageBody, ItemId) " +
                                 "SELECT r.UserId, r.RequesterUserId, 'Confirmed exchange for item', r.ItemId " +
                                 "FROM ExchangeDB..Request r " +
                                 "WHERE r.RequestId = @RequestId", con);
            com.Parameters.Add("@RequestId", SqlDbType.Int).Value = Convert.ToInt32(e.CommandArgument);
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
        else
        {
            com = new SqlCommand("Delete ExchangeDB..Request WHERE RequestId = @RequestId " +
                                 "INSERT ExchangeDB..[Message](FromUserId, ToUserId, MessageBody, ItemId) " +
                                 "SELECT r.UserId, r.RequesterUserId, 'Rejected exchange for item', r.ItemId " +
                                 "FROM ExchangeDB..Request r " +
                                 "WHERE r.RequestId = @RequestId", con);
            com.Parameters.Add("@RequestId", SqlDbType.Int).Value = Convert.ToInt32(e.CommandArgument);
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
        Response.Redirect(HttpContext.Current.Request.Url.ToString());
    }
}