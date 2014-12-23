<%@ WebHandler Language="C#" Class="ImageHandler" %>

using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public class ImageHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        string itemId = context.Request.QueryString["ItemId"];
        if (string.IsNullOrEmpty(itemId))
        {
            //Set a default imageID
            //imageid = "1";
        }
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
        connection.Open();

        SqlCommand command = new SqlCommand("select Image from ExchangeDB..Item where ItemId = " + itemId, connection);
        SqlDataReader dr = command.ExecuteReader();
        dr.Read();
        //context.Response.ContentType = "text/plain";
        if(dr[0] != DBNull.Value)
            context.Response.BinaryWrite((Byte[])dr[0]);
        connection.Close();

        context.Response.End();  
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}