using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

public partial class ViewEditItem : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Item item = null;
            List<Item> items = null;
            string itemId = Request.QueryString["ItemId"];
            string conString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            SqlConnection con = new SqlConnection(conString);
            SqlCommand com = new SqlCommand("SELECT ItemId, ItemName, ItemDesc, ContentType, Image, Category, CreatedOn, UserName, IsActive, i.UserId FROM ExchangeDB..Item i JOIN ExchangeDB..Users u ON u.UserId = i.UserId WHERE ItemId = @ItemId", con);
            com.Parameters.Add("@ItemId", SqlDbType.Int).Value = Convert.ToInt32(itemId);
            con.Open();
            var lDataAdapter = new SqlDataAdapter(com);
            var lDataSet = new DataSet();
            lDataAdapter.Fill(lDataSet);
            if (lDataSet.Tables[0].Rows.Count == 0)
            {
                ItemName.Visible = false;
                ItemImage.Visible = false;
                ItemDesc.Visible = false;
                ItemCategory.Visible = false;
                CategoryLabel.Visible = false;
                UserName.Visible = false;
                Status.Visible = false;
                ItemNotFound.Visible = true;
                //con.Close();
            }
            else
            {
                item = lDataSet.Tables[0].AsEnumerable().Select(row => new Item(row)).FirstOrDefault();
                hiddenItemId.Text = item.ItemId.ToString();
                hiddenUserId.Text = item.UserId.ToString();
                currentUserId.Text = Session["UserId"] == null? string.Empty : Session["UserId"].ToString();
                ItemName.Text = item.ItemName;
                ItemDesc.Text = item.ItemDesc;
                ItemCategory.Text = item.Category;
                ItemImage.ImageUrl = item.ItemImage;
                UserName.Text = "By: " + item.UserName;
                Status.Text = (item.IsActive ? "Available" : "Not Available");
                Status.ForeColor = (item.IsActive ? System.Drawing.Color.Green : System.Drawing.Color.Red);
                //con.Close();
            }

            SqlCommand relatedItem = new SqlCommand("SELECT top 10 ItemId, ItemName, ItemDesc, ContentType, Image, Category, CreatedOn, UserName, IsActive, i.UserId FROM ExchangeDB..Item i JOIN ExchangeDB..Users u ON u.UserId = i.UserId WHERE (i.Category = @Category OR u.UserId = @UserId) AND i.IsActive = 1 Order by i.CreatedOn Desc", con);
            relatedItem.Parameters.Add("@UserId", SqlDbType.Int).Value = item == null ? 0 : item.UserId;
            relatedItem.Parameters.Add("@Category", SqlDbType.NVarChar).Value = item == null ? "" : item.Category;
            var _DataAdapter = new SqlDataAdapter(relatedItem);
            var _DataSet = new DataSet();
            _DataAdapter.Fill(_DataSet);
            items = _DataSet.Tables[0].AsEnumerable().Select(row => new Item(row)).ToList();
            RelatedItemView.DataSource = items;
            RelatedItemView.DataBind();
            con.Close();
        }
    }

    protected void UpdateItem(object sender, EventArgs e)
    {
        string filePath = EditImage.PostedFile.FileName;
        string filename = Path.GetFileName(filePath);
        string ext = Path.GetExtension(filename);
        string contenttype = String.Empty;
        //Set the contenttype based on File Extension
        switch (ext)
        {
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
        if (contenttype != String.Empty || !string.IsNullOrEmpty(filename))
        {
            Stream fs = EditImage.PostedFile.InputStream;
            BinaryReader br = new BinaryReader(fs);
            Byte[] bytes = br.ReadBytes((Int32)fs.Length);
            MemoryStream ms = new MemoryStream(bytes);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            MemoryStream newMs = new MemoryStream();

            ResizeImage(returnImage, 300).Save(newMs, System.Drawing.Imaging.ImageFormat.Jpeg);
            Byte[] newBytes = newMs.ToArray();
            //insert the file into database
            SqlCommand cmd;
            if(bytes.Length > 0)
            {
                string strQuery = "Update ExchangeDB..Item" +
                                  " Set ItemName = @ItemName, ItemDesc = @ItemDesc, Category = @Category, ContentType = @ContentType, Image = @Image" +
                                  " Where ItemId = @ItemId";

                cmd = new SqlCommand(strQuery);
                cmd.Parameters.Add("@ItemId", SqlDbType.Int).Value = Convert.ToInt32(hiddenItemId.Text);
                cmd.Parameters.Add("@ItemName", SqlDbType.NVarChar).Value = editName.Text;
                cmd.Parameters.Add("@ItemDesc", SqlDbType.NVarChar).Value = editDesc.Text;
                cmd.Parameters.Add("@ContentType", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(contenttype) ? (object)DBNull.Value : contenttype;
                cmd.Parameters.Add("@Image", SqlDbType.Binary).Value = newBytes.Length > 0 ? newBytes : (object)DBNull.Value;
                cmd.Parameters.Add("@Category", SqlDbType.NVarChar).Value = editCategory.SelectedValue;
            }
            else{
                string strQuery = "Update ExchangeDB..Item" +
                                  " Set ItemName = @ItemName, ItemDesc = @ItemDesc, Category = @Category" +
                                  " Where ItemId = @ItemId";

                cmd = new SqlCommand(strQuery);
                cmd.Parameters.Add("@ItemId", SqlDbType.Int).Value = Convert.ToInt32(hiddenItemId.Text);
                cmd.Parameters.Add("@ItemName", SqlDbType.NVarChar).Value = editName.Text;
                cmd.Parameters.Add("@ItemDesc", SqlDbType.NVarChar).Value = editDesc.Text;
                cmd.Parameters.Add("@Category", SqlDbType.NVarChar).Value = editCategory.SelectedValue;
            }

            if (InsertUpdateData(cmd))
            {
                updateMessage.ForeColor = System.Drawing.Color.Green;
                updateMessage.Text = "Item Updated Successfully";
                Response.Redirect("~/ViewEditItem.aspx?ItemId=" + hiddenItemId.Text);
            }

        }
        else
        {
            updateMessage.ForeColor = System.Drawing.Color.Red;
            updateMessage.Text = "File format not recognised.";
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
            updateMessage.ForeColor = System.Drawing.Color.Red;
            updateMessage.Text = ex.Message;
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    protected void relatedItem_Click(object sender, CommandEventArgs e)
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

    [System.Web.Services.WebMethod]
    public static string DeleteItem(int pItemId)
    {
        string strQuery = "Update ExchangeDB..Item" +
                                  " Set IsActive = 0" +
                                  " Where ItemId = @ItemId";
        SqlCommand cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add("@ItemId", SqlDbType.Int).Value = pItemId;
        string conString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        SqlConnection con = new SqlConnection(conString);
        cmd.CommandType = CommandType.Text;
        cmd.Connection = con;
        con.Open();
        try
        {
            cmd.ExecuteNonQuery();
            con.Close();
            return "Item deleted";
        }
        catch (Exception ex)
        {
            con.Close();
            return ex.Message;
        }
    }
}