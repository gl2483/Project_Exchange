<%@ Page Language="C#" Title="Request" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Request.aspx.cs" Inherits="Request" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div style="width:100%; height:100%; overflow:scroll; overflow-x:hidden; float:right">
            <asp:ListView ID="RequestList" runat="server" ItemPlaceholderID="itemPlaceHolder">
                <LayoutTemplate>
                    <table style="width:100%">
                        <table style="width:100%">
                            <colgroup>
                                <col width="15%" />
                                <col width="25%" />
                                <col width="10%" />
                                <col width="15%" />
                                <col width="25%" />
                                <col width="10%" />
                            </colgroup>
                            <tr>
                                <td></td>
                                <td align="center" colspan="2"><div class="large">Your Item</div></td>
                                <td></td>
                                <td align="center" colspan="2"><div class="large">Suggested Item</div></td>
                            </tr>
                        </table>
                    <tr>
                        <td>
                        <div id="itemPlaceHolder" runat="server"></div>
                        </td>
                    </tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                <table style="width:100%">
                <colgroup>
                    <col width="15%" />
                    <col width="25%" />
                    <col width="10%" />
                    <col width="15%" />
                    <col width="25%" />
                    <col width="10%" />
                </colgroup>
                <tr>
                <td align="center" valign="middle">
                    <asp:Button ID="ConfirmBtn" runat="server" CssClass="likeButton" Text="Confirm" Visible='<%# !Convert.ToBoolean(Eval("IsMyRequest"))%>' OnCommand="ActionButton_Command" CommandArgument='<%# Eval("RequestId")%>' CommandName="Confirm"/>
                    <asp:Button ID="RejectBtn" runat="server" CssClass="dislikeButton" Text="Reject" Visible='<%# !Convert.ToBoolean(Eval("IsMyRequest"))%>' OnCommand="ActionButton_Command" CommandArgument='<%# Eval("RequestId")%>' CommandName="Reject"/>
                    <asp:Label ID="StatusLabel" runat="server" Visible='<%# Eval("IsMyRequest")%>' CssClass="StatusLabel">Pending Confirmation</asp:Label>
                </td>
                <td>
                      <asp:ImageButton ID="itemImage1" runat="server" ImageUrl='<%# Eval("MyImage")%>' CssClass="RequestImage" OnCommand="itemButton_Command" CommandArgument='<%# Eval("MyItemId")%>'/>
                </td>
                <td>
                    <asp:LinkButton ID="ItemLink1" runat="server" OnCommand="itemButton_Command" CommandArgument='<%# Eval("MyItemId")%>'><%# Eval("MyItemName")%> <br /> <%# Eval("MyItemDesc")%></asp:LinkButton>
                </td>
                <td valign="middle" align="center">
                    <%--<div style="width:200px; height:200px; vertical-align:middle">--%>
                        <asp:Image ID="ImageX" runat="server" ImageUrl="~/Xarrows.png" Height="60px"/>
                    <%--</div>--%>
                </td>
                <td>
                      <asp:ImageButton ID="itemImage2" runat="server" ImageUrl='<%# Eval("Image")%>' CssClass="RequestImage"  OnCommand="itemButton_Command" CommandArgument='<%# Eval("ItemId")%>'/>
                </td>
                <td>
                    <asp:LinkButton ID="ItemLink2" runat="server" OnCommand="itemButton_Command" CommandArgument='<%# Eval("ItemId")%>'><%# Eval("ItemName")%> <br /> <%# Eval("ItemDesc")%></asp:LinkButton>
                </td>
                </tr>
                <tr>
                <td></td>
                <td></td>
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
</asp:Content>
