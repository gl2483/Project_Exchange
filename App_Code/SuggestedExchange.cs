using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for SuggestedExchange
/// </summary>
public class SuggestedExchange
{
    public int MyItemId { get; set; }
    public string MyItemName { get; set; }
    public string MyItemDesc { get; set; }
    public string MyContentType { get; set; }
    public string MyImage { get; set; }
    public string MyCategory { get; set; }
    public int MyUserId { get; set; }
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public string ItemDesc { get; set; }
    public string ContentType { get; set; }
    public string Image { get; set; }
    public string Category { get; set; }
    public int UserId { get; set; }

    private const string notAvail = "data:image/jpg;base64,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";

	public SuggestedExchange(DataRow row)
	{
        MyItemId = Convert.ToInt32(row["MyItemId"]);
        MyItemName = row["MyItemName"].ToString();
        MyItemDesc = row["MyItemDesc"].ToString();
        MyImage = row["MyImage"] == DBNull.Value ? notAvail : "data:" + row["MyContentType"].ToString() + ";base64," + Convert.ToBase64String((byte[])row["MyImage"]);
        MyCategory = row["MyCategory"].ToString();
        MyUserId = Convert.ToInt32(row["MyUserId"]);
        ItemId = Convert.ToInt32(row["ItemId"]);
        ItemName = row["ItemName"].ToString();
        ItemDesc = row["ItemDesc"].ToString();
        Image = row["Image"] == DBNull.Value ? notAvail : "data:" + row["ContentType"].ToString() + ";base64," + Convert.ToBase64String((byte[])row["Image"]);
        Category = row["Category"].ToString();
        UserId = Convert.ToInt32(row["UserId"]);
	}
}