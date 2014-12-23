using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for Message
/// </summary>
public class Message
{
    public int FromUserId { get; set; }
    public string FromUserName { get; set; }
    public int ToUserId { get; set; }
    public string ToUserName { get; set; }
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public string MessageBody { get; set; }
    public DateTime CreatedOn { get; set; }
    public string ItemLink { get; set; }

	public Message(DataRow row)
	{
        FromUserId = Convert.ToInt32(row["FromUserId"]);
        FromUserName = row["FromUserName"].ToString();
        ToUserId = Convert.ToInt32(row["ToUserId"]);
        ToUserName = row["ToUserName"].ToString();
        ItemId = row["ItemId"] == DBNull.Value ? 0 : Convert.ToInt32(row["ItemId"]);
        ItemName = row["ItemName"] == DBNull.Value ? string.Empty : row["ItemName"].ToString();
        MessageBody = row["MessageBody"].ToString();
        CreatedOn = Convert.ToDateTime(row["CreatedOn"]);
        ItemLink = "~/ViewEditItem.aspx?ItemId=" + (row["ItemId"] == DBNull.Value ? 0 : Convert.ToInt32(row["ItemId"])).ToString();
	}
}