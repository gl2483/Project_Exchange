<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ImageGrid.ascx.cs" Inherits="ImageGrid" %>

<div style="overflow:scroll; overflow-x:hidden; height:800px">
<asp:ListView runat="server" ID="ImageListView" ItemPlaceholderID="itemPlaceHolder" 

     GroupPlaceholderID="groupPlaceHolder" 
    OnItemCommand="ImageListView_ItemCommand" GroupItemCount="3">
    <LayoutTemplate>
        <%--<h1>
            <asp:Label Text="" runat="server" ID="titleLabel" OnLoad="titleLabel_Load" />

        </h1>--%>
        <table width="100%">
                    <colgroup>
                        <col width=33% />
                        <col width=33% />
                        <col width=33% />
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
        <%--<asp:ImageButton ID="itemImageButton" runat="server" 

          ImageUrl='<%# "ImageHandler.ashx?ItemId="+ Eval("ItemId") %>' Width="320" Height="240" 

          />--%><%--OnCommand="itemImageButton_Command"--%>

          <%--CommandArgument="<%# Container.DataItem %>" --%><%--Container.DataItem--%>
          <asp:ImageButton ID="itemImageButton" runat="server" OnCommand="browseItem_Click" CommandArgument='<%# Eval("ItemId")%>'
          ImageUrl='<%# Eval("ItemImage")%>' CssClass="GridImage"

          />
    </td>
    </tr>
    <tr>
    <td>
        <%--<asp:LinkButton ID="deleteLinkButton" runat="server" CommandName="Remove" 

          CommandArgument="<%# Container.DataItem %>" Text="Delete" Visible="true" 

          OnLoad="deleteLinkButton_Load"  />--%>
          <center><asp:Button ID="Likebtn" runat="server" Text="Like" CssClass="likeButton" Visible='<%# Eval("showLike") %>' OnCommand="setItemPreference" CommandArgument='<%# Eval("ItemId")%>' CommandName="Like" Enabled='<%# Eval("showLike").ToString() == Eval("showDislike").ToString() %>'/>
          <asp:Button ID="Dislikebtn" runat="server" Text="DisLike" CssClass="dislikeButton" Visible='<%# Eval("showDislike") %>' OnCommand="setItemPreference" CommandArgument='<%# Eval("ItemId")%>'  CommandName="DisLike"/></center>
    </td>
    </tr>
    <tr>
        <td><center><asp:LinkButton ID="browseItemLink" runat="server" OnCommand="browseItem_Click" CommandArgument='<%# Eval("ItemId")%>'><%# Eval("ItemName")%> <br /> <%# Eval("ItemDesc")%></asp:LinkButton></center></td>
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
    <InsertItemTemplate>
        <%--<p>
            <asp:Label Text="Please upload an image" runat="server" ID="imageUploadLabel" />
            <asp:FileUpload runat="server" ID="imageUpload" OnLoad="imageUpload_Load" />
            <asp:Button ID="uploadButton" Text="Upload" runat="server" />
        </p>--%>
       <%-- <p>
            <asp:Label Text="" runat="server" ID="imageUploadStatusLabel" />
        </p>--%>
    </InsertItemTemplate>
</asp:ListView>
</div>
<asp:DataPager ID="lvDataPager1" runat="server" PagedControlID="ImageListView" PageSize="12">
    <Fields>
        <asp:NumericPagerField ButtonType="Link" />
    </Fields>
</asp:DataPager>