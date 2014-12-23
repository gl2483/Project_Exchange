using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for UserEntity
/// </summary>
public class UserEntity
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
	public UserEntity(int pUserId, string pUsername, string pPassword)
	{
        UserId = pUserId;
        Username = pUsername;
        Password = pPassword;
		//
		// TODO: Add constructor logic here
		//
	}
}