using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

public partial class Home : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var images = new List<string>();
            List<Item2> items = null;
            List<Message> messages = null;
            //string notAvail = "data:image/jpg;base64,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";
            string conString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            SqlConnection con = new SqlConnection(conString);
            SqlCommand com = new SqlCommand("SELECT ItemId, ItemName, ItemDesc, ContentType, Image, Category, CreatedOn, UserName, IsActive, i.UserId, 1 AS Preference FROM ExchangeDB..Item i JOIN ExchangeDB..Users u ON u.UserId = i.UserId WHERE u.UserId = @UserId And i.IsActive = 1", con);
            com.Parameters.Add("@UserId", SqlDbType.Int).Value = Convert.ToInt32(Session["UserId"]);
            SqlCommand com2 = new SqlCommand("SELECT Top 15 fu.UserId As FromUserId, fu.UserName As FromUserName, tu.UserId As ToUserId, tu.UserName As ToUserName, i.ItemId, i.ItemName, m.MessageBody, m.CreatedOn FROM ExchangeDB..[Message] m JOIN ExchangeDB..Users fu ON fu.UserId = m.FromUserId JOIN ExchangeDB..Users tu ON tu.UserId = m.ToUserId LEFT JOIN ExchangeDB..Item i ON i.ItemId = m.ItemId WHERE m.ToUserId = @UserId Order by m.CreatedON Desc", con);
            com2.Parameters.Add("@UserId", SqlDbType.Int).Value = Convert.ToInt32(Session["UserId"]);
            con.Open();
            var lDataAdapter = new SqlDataAdapter(com);
            var lDataSet = new DataSet();
            lDataAdapter.Fill(lDataSet);
            var lDataAdapter2 = new SqlDataAdapter(com2);
            var lDataSet2 = new DataSet();
            lDataAdapter2.Fill(lDataSet2);
            if (lDataSet.Tables[0].Rows.Count != 0)
            {
                items = lDataSet.Tables[0].AsEnumerable().Select(row => new Item2(row, Session["UserId"] == null ? 0 : Convert.ToInt32(Session["UserId"]))).ToList();
            }
            ItemList.DataSource = items;
            ItemList.DataBind();

            if (lDataSet2.Tables[0].Rows.Count != 0)
            {
                messages = lDataSet2.Tables[0].AsEnumerable().Select(row => new Message(row)).ToList();
            }
            MessageList.DataSource = messages;
            MessageList.DataBind();

            con.Close();

        }
    }

    protected void itemButton_Command(object sender, CommandEventArgs e)
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

    protected void ItemListView_ItemCommand(object sender, ListViewCommandEventArgs e)
    {

    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        // Read the file and convert it to Byte Array
        string filePath = UploadImage.PostedFile.FileName;
        string filename = Path.GetFileName(filePath);
        string ext = Path.GetExtension(filename);
        string contenttype = String.Empty;
        //Set the contenttype based on File Extension
        switch (ext.ToLower())
        {
            case ".jpeg":
            case ".jpg":
                contenttype = "image/jpg";
                break;
            case ".png":
                contenttype = "image/png";
                break;
            case ".gif":
                contenttype = "image/gif";
                break;
            case ".bmp":
                contenttype = "image/bmp";
                break;

        }

        if (contenttype != String.Empty || string.IsNullOrEmpty(filename))
        {
            Stream fs = UploadImage.PostedFile.InputStream;
            BinaryReader br = new BinaryReader(fs);
            Byte[] bytes = br.ReadBytes((Int32)fs.Length);
            Byte[] newBytes = new byte[0];
            if (!string.IsNullOrEmpty(filename))
            {
                MemoryStream ms = new MemoryStream(bytes);
                System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
                MemoryStream newMs = new MemoryStream();

                ResizeImage(returnImage, 300).Save(newMs, System.Drawing.Imaging.ImageFormat.Jpeg);
                newBytes = newMs.ToArray();
            }
            //insert the file into database
            string strQuery = "insert into ExchangeDB..Item(UserId, ItemName, ItemDesc, ContentType, Image, Category)" +
               " values (@UserId, @ItemName, @ItemDesc, @ContentType, @Image, @Category)";

            SqlCommand cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Convert.ToInt32(Session["UserId"].ToString());
            cmd.Parameters.Add("@ItemName", SqlDbType.NVarChar).Value = ItemName.Text;
            cmd.Parameters.Add("@ItemDesc", SqlDbType.NVarChar).Value = ItemDescription.Text;
            cmd.Parameters.Add("@ContentType", SqlDbType.NVarChar).Value = "image/jpg";
            cmd.Parameters.Add("@Image", SqlDbType.Binary).Value = newBytes.Length > 0 ? newBytes : (object)DBNull.Value;
            cmd.Parameters.Add("@Category", SqlDbType.NVarChar).Value = CategoryDropDown.SelectedValue;

            if (InsertUpdateData(cmd))
            {
                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Text = "Item Uploaded Successfully";
                Response.Redirect("~/Home.aspx");
            }

        }
        else
        {
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Text = "File format not recognised.";
        }
    }



    private Boolean InsertUpdateData(SqlCommand cmd)
    {
        string conString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        SqlConnection con = new SqlConnection(conString);
        cmd.CommandType = CommandType.Text;
        cmd.Connection = con;
        try
        {
            con.Open();
            cmd.ExecuteNonQuery();
            return true;
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Text = ex.Message;
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    public static System.Drawing.Image ResizeImage(System.Drawing.Image img, int minsize)
    {
        var size = img.Size;
        if (size.Height > minsize && size.Width > minsize)
        {
            if (size.Width >= size.Height)
            {
                // Could be: if (size.Height < minsize) size.Height = minsize;
                size.Height = minsize;
                size.Width = (size.Height * img.Width + img.Height - 1) / img.Height;
            }
            else
            {
                size.Width = minsize;
                size.Height = (size.Width * img.Height + img.Width - 1) / img.Width;
            }
            return new System.Drawing.Bitmap(img, size);
        }
        return img;
    }
}
