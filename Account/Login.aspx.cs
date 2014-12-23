using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Data.Sql;
using System.Security.Cryptography;
using System.Text;

public partial class Account_Login : System.Web.UI.Page
{
    public const string salt = "S@!t$";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            Console.WriteLine("Logging in");
        }
        RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
    }

    protected void LoginUser_Authenticate(object sender, System.Web.UI.WebControls.AuthenticateEventArgs e)
    {
        string userName = LoginUser.UserName;
        string password = LoginUser.Password;

        bool result = UserLogin(userName, password);
        if ((result))
        {
            e.Authenticated = true;
        }
        else
        {
            e.Authenticated = false;
        }
    }

    private bool UserLogin(string userName, string password)
    {
        // read the coonection string from web.config 
        string conString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        string saltedPassword = GenerateSaltedHash(password, salt);
        using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(conString))
        {
            //' declare the command that will be used to execute the select statement 
            SqlCommand com = new SqlCommand("SELECT UserId, UserName FROM ExchangeDB..Users WHERE UserName = @UserName AND UserPassword = @Password", con);

            // set the username and password parameters
            com.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = userName;
            com.Parameters.Add("@Password", SqlDbType.NVarChar).Value = saltedPassword;

            con.Open();
            //' execute the select statment 
            var lDataAdapter = new SqlDataAdapter(com);
            var lDataSet = new DataSet();
            lDataAdapter.Fill(lDataSet);

            //' check the result 
            if (lDataSet.Tables[0].Rows.Count == 0)
            {
                //invalid user/password , return flase 
                con.Close();
                return false;
            }
            else
            {
                DataRow row = lDataSet.Tables[0].Rows[0];
                string userid = row["UserId"].ToString();
                Session.Add("UserId", userid);
                // valid login
                con.Close();
                return true;
            }
        }
    }

    private static string GenerateSaltedHash(string pPlainText, string pSalt)
    {
        HashAlgorithm algorithm = new SHA256Managed();

        UTF8Encoding encoding = new System.Text.UTF8Encoding();
        byte[] plainText = encoding.GetBytes(pPlainText);
        byte[] salt = encoding.GetBytes(pSalt);

        byte[] plainTextWithSaltBytes =
          new byte[plainText.Length + salt.Length];

        for (int i = 0; i < plainText.Length; i++)
        {
            plainTextWithSaltBytes[i] = plainText[i];
        }
        for (int i = 0; i < salt.Length; i++)
        {
            plainTextWithSaltBytes[plainText.Length + i] = salt[i];
        }

        return Convert.ToBase64String(algorithm.ComputeHash(plainTextWithSaltBytes));
    }
}
