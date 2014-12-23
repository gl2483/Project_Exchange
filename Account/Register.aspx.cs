using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Data.Sql;
using System.Security.Cryptography;
using System.Text;

public partial class Account_Register : System.Web.UI.Page
{

    public const string salt = "S@!t$";

    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterUser.ContinueDestinationPageUrl = Request.QueryString["ReturnUrl"];
    }

    protected void RegisterUser_CreatedUser(object sender, EventArgs e)
    {
        FormsAuthentication.SetAuthCookie(RegisterUser.UserName, false /* createPersistentCookie */);

        string continueUrl = RegisterUser.ContinueDestinationPageUrl;
        if (String.IsNullOrEmpty(continueUrl))
        {
            continueUrl = "~/";
        }
        Response.Redirect(continueUrl);
    }

    protected void CreateAccountButton_Click(object sender, EventArgs e)
    {
        string username = RegisterUser.UserName;
        string password = RegisterUser.Password;
        string email = RegisterUser.Email;
        if (CheckCreateUser(username))
        {
            CreateUser(username, password, email);
            Response.Redirect("~/Default.aspx");
        }
        else
            CreateUserResult.Text = "Duplicate username";
    }

    private bool CreateUser(string userName, string password, string email)
    {
        string conString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(conString))
        {
            string saltedPassword = GenerateSaltedHash(password, salt);
            //' declare the command that will be used to execute the select statement 
            SqlCommand com = new SqlCommand("INSERT ExchangeDB.dbo.Users(Username, UserPassword, Email) VALUES(@UserName,@UserPass,@Email)", con);
            // set the username and password parameters
            com.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = userName;
            com.Parameters.Add("@UserPass", SqlDbType.NVarChar).Value = saltedPassword;
            com.Parameters.Add("@Email", SqlDbType.NVarChar).Value = email;
            con.Open();
            //' execute the select statment 
            try
            {
                com.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                CreateUserResult.Text = e.Message;
                return false;
            }
            return true;
            con.Close();
        }
    }

    private bool CheckCreateUser(string userName)
    {
        // read the coonection string from web.config 
        string conString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(conString))
        {
            //' declare the command that will be used to execute the select statement 
            SqlCommand com = new SqlCommand("SELECT UserId, UserName FROM ExchangeDB..Users WHERE UserName = @UserName", con);
            // set the username and password parameters
            com.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = userName;
            con.Open();
            //' execute the select statment 
            var lDataAdapter = new SqlDataAdapter(com);
            var lDataSet = new DataSet();
            lDataAdapter.Fill(lDataSet);

            //' check the result 
            if (lDataSet.Tables[0].Rows.Count == 0)
            {
                con.Close();
                return true;
            }
            else
            {
                con.Close();
                return false;
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
