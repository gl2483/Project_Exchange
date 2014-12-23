<%@ Page Title="View Item" Language="C#" AutoEventWireup="true" CodeFile="ViewEditItem.aspx.cs" Inherits="ViewEditItem" MasterPageFile="~/Site.master"%>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
<link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css"/>
 <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
<script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>
<script type="text/javascript">
    $(function () {
        var itemUserId = $("#MainContent_hiddenUserId").text();
        var currentUserId = $("#MainContent_currentUserId").text();

        if (itemUserId != currentUserId || itemUserId == "") {
            $("#PopupBtn").hide();
            $("#DeleteBtn").hide();
        }

        if ($("#MainContent_Status").text() == "Not Available") {
            $("#PopupBtn").hide();
            $("#DeleteBtn").hide();
        }

        $("#PopupBtn")
        .button()
        .click(function (event) {
            event.preventDefault();
            $("#editPopup").dialog({
                modal: true,
                width: 400,
                title: "UpdateItem",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                }
            });
            var itemName = $("#MainContent_ItemName").text();
            $("#MainContent_editName").val(itemName);
            var itemDesc = $("#MainContent_ItemDesc").text();
            $("#MainContent_editDesc").val(itemDesc);
            var itemCategory = $("#MainContent_ItemCategory").text();
            $("#MainContent_editCategory").val(itemCategory);
            $("#editPopup").dialog("open");
        });

        $("#DeleteBtn")
        .button()
        .click(function (event) {
            var itemId = $("#MainContent_hiddenItemId").text();
            $("#dialog-confirm").append("<strong>Confirm deleting this item?</strong>");
            $("#dialog-confirm").dialog({
                resizable: false,
                height: 140,
                modal: true,
                buttons: {
                    "Delete item": function () {
                        $.ajax({
                            type: "POST",
                            url: "ViewEditItem.aspx/DeleteItem",
                            data: "{pItemId:'" + itemId + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                $("#MainContent_Status").text("Not Available");
                                $("#MainContent_Status").css("color", "Red");
                                $("#DeleteBtn").hide()
                                $("#PopupBtn").hide()
                                $("#dialog-message").append("<strong>" + msg.d + "</strong>");
                                $("#dialog-message").dialog({
                                    modal: true,
                                    buttons: {
                                        Ok: function () {
                                            $(this).empty();
                                            $("#dialog-confirm").empty();
                                            $(this).dialog("close");
                                            $("#dialog-confirm").dialog("close");
                                        }
                                    }
                                });
                            }
                        });
                    },
                    Cancel: function () {
                        $(this).empty();
                        $(this).dialog("close");
                    }
                }
            });
        });


    });
</script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div>
        <table width="100%">
            <colgroup>
                <col width="50%" />
                <col width="50%" />
            </colgroup>
            <tr>
                <td>
                    <asp:Image ID="ItemImage" runat="server" CssClass="ViewImage"/>
                    <h2><b><asp:Label ID="ItemNotFound" runat="server" Visible="false" Text="Item Not Found"></asp:Label></b></h2>
                </td>
                <td valign="top">
                    <table>
                        <tr style="display:none"><td>
                            <asp:Label ID="hiddenItemId" runat="server"></asp:Label>
                        </td></tr>
                        <tr style="display:none"><td>
                            <asp:Label ID="hiddenUserId" runat="server"></asp:Label>
                        </td></tr>
                        <tr style="display:none"><td>
                            <asp:Label ID="currentUserId" runat="server"></asp:Label>
                        </td></tr>
                        <tr><td>
                            <asp:Label ID="ItemName" runat="server" Font-Bold="true" Font-Size="XX-Large"></asp:Label>
                        </td></tr>
                        <tr><td>
                            <asp:Label ID="ItemDesc" runat="server" Font-Size="X-Large"></asp:Label>
                        </td></tr>
                        <tr><td>
                            <asp:Label ID="UserName" runat="server" Font-Size="Larger"></asp:Label>
                        </td></tr>
                        <tr><td>
                            <asp:Label ID="CategoryLabel" runat="server" Font-Size="Larger">Category: </asp:Label><asp:Label ID="ItemCategory" runat="server" Font-Size="Larger"></asp:Label>
                        </td></tr>
                        <tr><td>
                            <asp:Label ID="StatusLabel" runat="server" Font-Size="Larger">Status: </asp:Label><asp:Label ID="Status" runat="server" Font-Size="Larger"></asp:Label>
                        </td></tr>
                        <tr><td>
                            <span id="PopupBtn" style="display:block; border:2px">Edit Item</span>
                        </td></tr>
                        <tr><td>
                            <span id="DeleteBtn" style="display:block; border:2px">Delete Item</span>
                        </td></tr>
                    </table>
                </td>
            </tr>
        </table>
        <fieldset>
        <legend>Related Items</legend>
        <div style="width:100%; overflow:scroll; overflow-x:hidden; height:500px">
            <asp:ListView runat="server" ID="RelatedItemView" ItemPlaceholderID="itemPlaceHolder" 
             GroupPlaceholderID="groupPlaceHolder"  GroupItemCount="5">
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
                  <asp:ImageButton ID="relatedItem" runat="server" 
                  ImageUrl='<%# Eval("ItemImage")%>' CssClass="RecentHistory" OnCommand="relatedItem_Click" CommandArgument='<%# Eval("ItemId")%>'
                  />
            </td>
            </tr>
            <tr>
            <td>
                <center><asp:LinkButton ID="relatedItemLink" runat="server" OnCommand="relatedItem_Click" CommandArgument='<%# Eval("ItemId")%>'><%# Eval("ItemName")%> <br /> <%# Eval("ItemDesc")%></asp:LinkButton></center>
            </td>
            </tr>
            </table>
            </td>
            </ItemTemplate>
            <EmptyItemTemplate>
                <td />
            </EmptyItemTemplate>
            <EmptyDataTemplate>
                <h3>No images available</h3>
            </EmptyDataTemplate>
        </asp:ListView>
        </div>
    </fieldset>
        <div id="editPopup" style="display: none">
            <table>
                <tr>
                    <td>
                        Item Name: 
                    </td>
                    <td>
                        <asp:TextBox ID="editName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                     <td>
                        Item Desc: 
                    </td>
                    <td>
                        <asp:TextBox ID="editDesc" runat="server" TextMode="MultiLine" Columns="30" Rows="3"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Category:
                    </td>
                    <td>
                        <asp:DropDownList AutoPostBack="False" ID="editCategory" runat="server">
                        <asp:ListItem Value="Book"> Book </asp:ListItem>
                          <asp:ListItem Value="Electronic"> Electronic </asp:ListItem>
                          <asp:ListItem Value="Video Game"> VideoGame </asp:ListItem>
                          <asp:ListItem Value="Music"> Music </asp:ListItem>
                          <asp:ListItem Value="Others"> Others </asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                     <td colspan="2"> 
                         <asp:FileUpload ID="EditImage" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                     <td style="float:right"> 
                         <asp:Button ID="Editbtn" runat="server" Text="Update" OnClick="UpdateItem"/>
                    </td>
                </tr>
                <tr>
                    <td> <asp:Label ID="updateMessage" runat="server" Text=""></asp:Label></td>
                </tr>
            </table>
        </div>
        
        <div id="dialog-message" style="display:none"></div>
        <div id="dialog-confirm" style="display:none"></div>
    </div>
</asp:Content>
