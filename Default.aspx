<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Register TagPrefix="cc" TagName="ImageControl" Src="~/ImageGrid.ascx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
<link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css"/>
 <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
<script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>
<script type="text/javascript">
    $(function () {
        var currentUserId = $("#MainContent_currentUserId").text();

        if (currentUserId == "") {
            $("#historyfieldset").hide();
        }

        $("#searchBtn")
        .button()
        .click(function (event) {
            event.preventDefault();
            window.location = window.location.pathname + "?SearchString=" + $("#SearchBox").val();
        });
    });
</script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div style="display:none">
        <asp:Label runat="server" ID="currentUserId"></asp:Label>
    </div>
    <table width="100%">
        <colgroup>
            <col width="30%"/>
            <col width="70%" />
        </colgroup>
        <tr>
            <td valign="top">
                <table>
                    <tr>
                        <td><table width="100%">
                            <colgroup><col width="65" /><col width="35%" /></colgroup>
                            <tr><td><input ID="SearchBox" type="text" placeholder="Search here" /></td><td><span id="searchBtn" style="display:block; width:100%;border-radius: 8px;height:30px; text-align:center; vertical-align:middle">Search</span></td></tr>
                        </table></td>    
                    </tr>
                    <tr>
                        <td><asp:Menu ID="Filter" runat="server" Orientation="Vertical" CssClass="filter">
                            <Items>
                                <asp:MenuItem NavigateUrl="~/Default.aspx" Text="All"/>
                                <asp:MenuItem NavigateUrl="~/Default.aspx?Category=Book" Text="Books"/>
                                <asp:MenuItem NavigateUrl="~/Default.aspx?Category=Electronic" Text="Electronic"/>
                                <asp:MenuItem NavigateUrl="~/Default.aspx?Category=Video Game" Text="Video Game"/>
                                <asp:MenuItem NavigateUrl="~/Default.aspx?Category=Music" Text="Music"/>
                                <asp:MenuItem NavigateUrl="~/Default.aspx?Category=Others" Text="Others"/>
                            </Items>
                        </asp:Menu></td>
                    </tr>
                </table>
            </td>
            <td>
                <cc:ImageControl runat="server" ID="MyImageGrid"  />
            </td>
        </tr>
    </table>
    <fieldset id="historyfieldset">
        <legend>Recently viewed</legend>
        <div style="width:100%; overflow:scroll; overflow-x:hidden; height:500px">
            <asp:ListView ID="RecentHistory" runat="server" ItemPlaceholderID="itemPlaceHolder" Visible = "false" GroupPlaceholderID="groupPlaceHolder"  GroupItemCount="5">
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
                      <asp:ImageButton ID="recentItemButton" runat="server" ImageUrl='<%# Eval("ItemImage")%>' CssClass="RecentHistory" OnCommand="recentItemButton_Command" CommandArgument='<%# Eval("ItemId")%>'/>
                </td>
                </tr>
                <tr>
                <td>
                    <center><asp:LinkButton ID="recentItemLink" runat="server" OnCommand="recentItemButton_Command" CommandArgument='<%# Eval("ItemId")%>'><%# Eval("ItemName")%> <br /> <%# Eval("ItemDesc")%></asp:LinkButton></center>
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
