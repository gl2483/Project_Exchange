using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

public partial class ImageGrid : System.Web.UI.UserControl
{
    public string Category;
    public string SearchString;
    protected void Page_Load(object sender, EventArgs e)
    {
        Category = Request.QueryString["Category"];
        SearchString = Request.QueryString["SearchString"];
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //Binds the Data Before Rendering
        BindData();
    }

    private void BindData()
    {
        ImageListView.DataSource = GetListOfImages();
        ImageListView.DataBind();

    }

    /// <summary>
    /// Gets list of images
    /// </summary>
    /// <returns></returns>
    private List<Item2> GetListOfImages()
    {
        var images = new List<string>();
        List<Item2> items = null;
        string conString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        SqlConnection con = new SqlConnection(conString);
        SqlCommand com;
        if (!string.IsNullOrEmpty(Category))
        {
            com = new SqlCommand("SELECT i.ItemId, ItemName, ItemDesc, ContentType, Image, Category, i.CreatedOn, UserName, IsActive, i.UserId, up.Preference " +
                                 "FROM ExchangeDB..Item i " +
                                 "JOIN ExchangeDB..Users u ON u.UserId = i.UserId " +
                                 "LEFT JOIN ExchangeDB..UserPreference up ON up.UserId = @UserId AND up.ItemId = i.ItemId " +
                                 "Where Category = @Category AND (up.UserPreferenceId IS NULL OR up.Preference = 1) AND i.IsActive = 1", con);
            com.Parameters.Add("@Category", SqlDbType.NVarChar).Value = Category;
            com.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["UserId"] == null ? 0 : Convert.ToInt32(Session["UserId"]);
        }
        else if (!string.IsNullOrEmpty(SearchString))
        {
            com = new SqlCommand("SELECT i.ItemId, ItemName, ItemDesc, ContentType, Image, Category, i.CreatedOn, UserName, IsActive, i.UserId, up.Preference " +
                                 "FROM ExchangeDB..Item i " +
                                 "JOIN ExchangeDB..Users u ON u.UserId = i.UserId " +
                                 "LEFT JOIN ExchangeDB..UserPreference up ON up.UserId = @UserId AND up.ItemId = i.ItemId " +
                                 "Where (up.UserPreferenceId IS NULL OR up.Preference = 1) AND i.IsActive = 1", con);
            com.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["UserId"] == null ? 0 : Convert.ToInt32(Session["UserId"]);
            string[] strArray = SearchString.Split(' ');
            com.CommandText += " AND (";
            for(int i = 0; i < strArray.Count(); i++)
            {
                com.CommandText += "i.ItemName Like '%'+ '" + strArray[i] + "' + '%' OR i.ItemDesc Like '%'+ '" + strArray[i] + "' + '%' ";
                if (i < strArray.Count() - 1)
                    com.CommandText += " OR ";
            }
            com.CommandText += ")";
        }
        else
        {
            com = new SqlCommand("SELECT i.ItemId, ItemName, ItemDesc, ContentType, Image, Category, i.CreatedOn, UserName, IsActive, i.UserId, up.Preference " +
                                 "FROM ExchangeDB..Item i " +
                                 "JOIN ExchangeDB..Users u ON u.UserId = i.UserId " +
                                 "LEFT JOIN ExchangeDB..UserPreference up ON up.UserId = @UserId AND up.ItemId = i.ItemId " +
                                 "WHERE (up.UserPreferenceId IS NULL OR up.Preference = 1) AND i.IsActive = 1", con);
            com.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["UserId"] == null ? 0 : Convert.ToInt32(Session["UserId"]);
        }
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
            items = lDataSet.Tables[0].AsEnumerable()
                .Select(row => new Item2(row, Session["UserId"] == null ? 0 : Convert.ToInt32(Session["UserId"])))
                    .ToList();
            con.Close();
        }
        return items;
    }

    private DataSet GetDataSet()
    {
        string conString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        SqlConnection con = new SqlConnection(conString);
        SqlCommand com = new SqlCommand("SELECT ItemId FROM ExchangeDB..Item", con);
        con.Open();
        //' execute the select statment 
        var lDataAdapter = new SqlDataAdapter(com);
        var lDataSet = new DataSet();
        lDataAdapter.Fill(lDataSet);
        con.Close();
        return lDataSet;
    }

    protected void titleLabel_Load(object sender, EventArgs e)
    {
        var titleLabel = sender as Label;
        if (titleLabel == null) return;

        titleLabel.Text = Session["UserId"] == null ? "Guest" : Session["UserId"].ToString();
    }

    protected void deleteLinkButton_Load(object sender, EventArgs e)
    {
        //In case of AdminMode, we would want to show the delete button 

        //which is not visible by iteself for Non-Admin users


    }

    protected void setItemPreference(object sender, CommandEventArgs e)
    {
        if (Session["UserId"] != null)
        {
            string conString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            SqlConnection con = new SqlConnection(conString);
            SqlCommand com = new SqlCommand("Insert ExchangeDB..UserPreference (UserId, ItemId, Preference) VALUES (@UserId, @ItemId, @Preference)", con);
            com.Parameters.Add("@UserId", SqlDbType.Int).Value = Convert.ToInt32(Session["UserId"]);
            com.Parameters.Add("@ItemId", SqlDbType.Int).Value = Convert.ToInt32(e.CommandArgument);
            com.Parameters.Add("@Preference", SqlDbType.Bit).Value = e.CommandName.ToUpper() == "LIKE" ? true: false;
            SqlCommand com2 = new SqlCommand("Insert ExchangeDB..Message (FromUserId, ToUserId, MessageBody, ItemId) VALUES (@FromUserId, (SELECT UserId FROM ExchangeDB..Item Where ItemId = @ItemId), @Message, @ItemId)", con);
            com2.Parameters.Add("@FromUserId", SqlDbType.Int).Value = Convert.ToInt32(Session["UserId"]);
            com2.Parameters.Add("@ItemId", SqlDbType.Int).Value = Convert.ToInt32(e.CommandArgument);
            com2.Parameters.Add("@Message", SqlDbType.NVarChar).Value = "liked your item";
            con.Open();
            try
            {
                com.ExecuteNonQuery();
                if (e.CommandName.ToUpper() == "LIKE")
                    com2.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            con.Close();
        }
    }

    protected void itemImageButton_Command(object sender, CommandEventArgs e)
    {
        Response.Redirect(e.CommandArgument as string);
    }

    protected void ImageListView_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        
    }

    protected void browseItem_Click(object sender, CommandEventArgs e)
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