<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Home.aspx.cs" Inherits="Home" %>
    
<%--<%@ Register TagPrefix="cc" TagName="ItemControl" Src="~/YourItems.ascx" %>--%>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
<script type="text/javascript" src="http://code.jquery.com/jquery-1.10.2.js"></script>
<script type="text/javascript">
    function btn_click() {
        $.post("Home.aspx/btnUpload_Click");
    }
</script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<asp:ValidationSummary ID="UploadValidationSummary" runat="server" CssClass="failureNotification" 
                 ValidationGroup="UploadValidationGroup"/>
<table style="width:100%">
<colgroup>
    <col width="47%"/>
    <col width="5%"/>
    <col width="47%"/>
</colgroup>
    <tr valign="top"><td>
    <div>
        <fieldset class="Upload">
            <legend>Upload Item</legend>
            <table>
            <tr>
                <td><asp:Label ID="ItemNameLabel" runat="server" AssociatedControlID="ItemName">ItemName:</asp:Label></td>
                <td><asp:TextBox ID="ItemName" runat="server" CssClass="textEntry"></asp:TextBox>
                <asp:RequiredFieldValidator ID="ItemNameRequired" runat="server" ControlToValidate="ItemName" 
                        CssClass="failureNotification" ErrorMessage="Item Name is required." ToolTip="User Name is required." 
                        ValidationGroup="UploadValidationGroup">*</asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td><asp:Label ID="ItemDescriptionLabel" runat="server" AssociatedControlID="ItemDescription">ItemDescription:</asp:Label></td>
                <td><asp:TextBox ID="ItemDescription" runat="server" CssClass="textEntry"></asp:TextBox>
                <asp:RequiredFieldValidator ID="ItemDescriptionRequired" runat="server" ControlToValidate="ItemDescription" 
                        CssClass="failureNotification" ErrorMessage="Item Description is required." ToolTip="Item Description is required." 
                        ValidationGroup="UploadValidationGroup">*</asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td><asp:Label ID="CategoryDropDownLabel" runat="server" AssociatedControlID="CategoryDropDown">Category:</asp:Label></td>
                <td><asp:DropDownList AutoPostBack="False" ID="CategoryDropDown" runat="server">
                
                <asp:ListItem Selected="True" Value="Book"> Book </asp:ListItem>
                  <asp:ListItem Value="Electronic"> Electronic </asp:ListItem>
                  <asp:ListItem Value="Video Game"> VideoGame </asp:ListItem>
                  <asp:ListItem Value="Music"> Music </asp:ListItem>
                  <asp:ListItem Value="Others"> Others </asp:ListItem>
                </asp:DropDownList></td>
            </tr>
            <tr>
                <td colspan="2"><asp:FileUpload ID="UploadImage" runat="server" /></td>
            </tr>
            <tr>
                <td><asp:Button ID="Upload_btn" runat="server" Text="Upload" OnClick="btnUpload_Click" /></td>
            </tr>
            <tr>
                <td> <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></td>
            </tr>
            </table>
        </fieldset>
    </div>
    </td>
    <td></td>
    <td>
    <div>
        <fieldset>
            <legend>Messages</legend>
            <div style="height:350px; overflow:scroll; overflow-x:hidden">
                <asp:ListView ID="MessageList" runat="server" ItemPlaceholderID="itemPlaceHolder">
                    <LayoutTemplate>
                            <div id="itemPlaceHolder" runat="server"></div>
                    </LayoutTemplate>
                    <ItemTemplate>
                    <table>
                        <tr>
                        <td>
                            User <b><%# Eval("FromUserName").ToString() %></b> <%# Eval("MessageBody").ToString()%> 
                            <asp:HyperLink ID="MessageItem" runat="server" NavigateUrl='<%# Eval("ItemLink") %>'><%# Eval("ItemName") %></asp:HyperLink> On <%# Eval("CreatedOn").ToString() %>
                        </td>
                        </tr>
                    </table>
                    </ItemTemplate>
                    <EmptyItemTemplate>
                        <td />
                    </EmptyItemTemplate>
                    <EmptyDataTemplate>
                        <h3>No Item available</h3>
                    </EmptyDataTemplate>
                </asp:ListView>
            </div>
        </fieldset>
    </div>
    </td></tr>
</table>
    <fieldset>
        <legend>Your Items</legend>
        <div style="overflow:scroll; overflow-x:hidden; height:500px">
            <asp:ListView ID="ItemList" runat="server" ItemPlaceholderID="itemPlaceHolder" OnItemCommand="ItemListView_ItemCommand" GroupPlaceholderID="groupPlaceHolder"  GroupItemCount="5">
                <LayoutTemplate>
                    <table width="100%">
                        <colgroup>
                            <col width=20% />
                            <col width=20% />
                            <col width=20% />
                            <col width=20% />
                            <col width=20% />
                        </colgroup>
                        <div runat="server" id="groupPlaceHolder">
                        </div>
                    </table>
                </LayoutTemplate>
                <GroupTemplate>
                <tr>
                    <div id="itemPlaceHolder" runat="server"></div>
                </tr>
                </GroupTemplate>
                <ItemTemplate>
                <td>
                <table>
                <tr>
                <td>
                      <asp:ImageButton ID="itemImageButton" runat="server" ImageUrl='<%# Eval("ItemImage")%>' CssClass="RecentHistory" OnCommand="itemButton_Command" CommandArgument='<%# Eval("ItemId")%>'/>
                </td>
                </tr>
                <tr>
                <td>
                    <center><asp:LinkButton ID="ItemLink" runat="server" OnCommand="itemButton_Command" CommandArgument='<%# Eval("ItemId")%>'><%# Eval("ItemName")%> <br /> <%# Eval("ItemDesc")%></asp:LinkButton></center>
                </td>
                </tr>
                </table>
                </td>
                </ItemTemplate>
                <EmptyItemTemplate>
                    <td />
                </EmptyItemTemplate>
                <EmptyDataTemplate>
                    <h3>No Item available</h3>
                </EmptyDataTemplate>
            </asp:ListView>
        </div>
    </fieldset>
</asp:Content>
