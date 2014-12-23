using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for Item
/// </summary>
public class Item
{ 
    public int ItemId { get; set; }
    public string ItemImage { get; set; }
    public string ItemName { get; set; }
    public string ItemDesc { get; set; }
    public string Category { get; set; }
    public DateTime CreatedOn { get; set; }
    public string UserName { get; set; }
    public bool IsActive { get; set; }
    public int UserId { get; set; }

    public Item(DataRow row)
	{
        ItemId = Convert.ToInt32(row["ItemId"]);
        ItemImage = row["Image"] == DBNull.Value ? "~/NotAvailable.jpg" : "ImageHandler.ashx?ItemId=" + row["ItemId"].ToString();
        ItemName = row["ItemName"].ToString();
        ItemDesc = row["ItemDesc"].ToString();
        Category = row["Category"].ToString();
        CreatedOn = Convert.ToDateTime(row["CreatedOn"]);
        UserName = row["UserName"].ToString();
        IsActive = Convert.ToBoolean(row["IsActive"]);
        UserId = Convert.ToInt32(row["UserId"]);
		//
		// TODO: Add constructor logic here
		//
	}
}