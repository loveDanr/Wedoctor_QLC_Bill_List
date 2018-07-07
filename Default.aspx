<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" Async="true" %>

<%@ Register Assembly="DevExpress.Web.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>海口健康医疗服务平台账目明细</title>
    <link id="cssfile" href="style.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="favicon.ico" type="image/x-icon" />

</head>
<body onload="remove_loading();">
    <form id="form1" runat="server">
    <div class="Main" align="center">
     <div class="top">
          <table border="0" class="tftableTop">
                <tr>
                        <td colspan="4" align="center" style="font-family:'Microsoft YaHei UI';font-weight: bold; align-content:center; font-size: 28px;padding-bottom:30px; padding-left:100px;text-align:center;">&nbsp;&nbsp;海口健康医疗服务平台账目明细
                    </td>
                </tr>
                <tr style="width:auto">
                    <td align="right">
                        <label style="font-family:'Microsoft YaHei UI';font-weight: bold">
                            开始时间:</label>
                    </td>
                    <td>

                     <dx:ASPxTimeEdit id="txt_startDate" runat="server" displayformatstring="yyyy-MM-dd" editformat="Custom" editformatstring="yyyy-MM-dd" width="120px"></dx:ASPxTimeEdit>
                    </td>
                    <td align="right">
                     <label style="font-family:'Microsoft YaHei UI';font-weight: bold">
                            结束时间:</label>
                    </td>
                        <td>
                          <dx:ASPxTimeEdit id="txt_endDate" runat="server" displayformatstring="yyyy-MM-dd" editformat="Custom" editformatstring="yyyy-MM-dd" width="120px"></dx:ASPxTimeEdit>
                    </td>
                        <td>
                            <asp:Button ID="btnSearch" runat="server" Text="查询" onclick="btnSearch_Click" CssClass="button button-3d button-caution" /></td><td><asp:Button ID="download_btn" runat="server" Text="下载" 
             CssClass="button button-3d button-caution" onclick="download_btn_Click" /></td>
                </tr>
              <tr align="center">
                  <td colspan="4" align="center" style="font-weight: bold; font-size: 15px;padding-bottom:5px; padding-left:100px;text-align:center;color:#ff0000">提示：对账数据的最大显示日期为当天的前一天！</br>(由于微信后台账单生成时间限制，</br>前一天账单会在第二天11点后生成！)
                  <dx:ASPxLabel ID="TotalDealCount" runat="server" Text="" Font-Size="15px" Font-Bold="True" Visible="false"></dx:ASPxLabel>&nbsp;&nbsp;<dx:ASPxLabel ID="TotalDealMoneyCount" runat="server" Text="" Font-Size="15px" Font-Bold="True" Visible="false"></dx:ASPxLabel></td>
              </tr>
              <tr align="center">
                  <td colspan="4" align="center" style="font-weight: bold; font-size: 15px;padding-bottom:5px; padding-left:100px;text-align:center;color:#ff0000"><dx:ASPxLabel runat="server" ID="HIS_TotalDealCount" Font-Bold="True" Font-Size="15px" Visible="false"></dx:ASPxLabel>   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<dx:ASPxLabel runat="server" ID="HIS_TotalFeeCount" Font-Size="15px" Font-Bold="True" Visible="false"></dx:ASPxLabel></td>
              </tr>
               <tr align="center">
                  <td colspan="4" align="center" style="font-weight: bold; font-size: 15px;padding-bottom:5px; padding-left:100px;text-align:center;color:#ff0000"><dx:ASPxLabel runat="server" ID="Notice" Font-Bold="True" Font-Size="15px" Text=""></dx:ASPxLabel></td>
              </tr>
            </table>
   
    </div>
                <div class="bottom2">
    
        <asp:GridView ID="GridView_Count" CssClass="msgtable" runat="server" 
            EmptyDataText="无" ShowFooter="True" OnRowDataBound="GridView_Count_RowDataBound" OnRowCreated="GridView_Count_RowCreated">
 
                <HeaderStyle CssClass="DataGridFixedHeader" BorderStyle="Solid" />
        </asp:GridView>
     
    </div>
    <div class="bottom">
    <table border="0" class="tftableTop">
          <tr>
              <td colspan="4"  style="font-weight: bold; font-size: 15px;padding-bottom:5px; padding-left:100px;text-align:left; float:left;color:#ff0000"><dx:ASPxLabel runat="server" ID="DetailsListTitle" Font-Bold="True" Font-Size="15px" Text=""></dx:ASPxLabel></td>
              
              </tr>
          </table>
        <asp:GridView ID="GridView" CssClass="msgtable" runat="server" 
            EmptyDataText="无" ShowFooter="True" OnRowDataBound="GridView_RowDataBound" 
            OnRowCreated="GridView_RowCreated" >
                <HeaderStyle CssClass="DataGridFixedHeader" BorderStyle="Solid" />
        </asp:GridView>
      
    </div>
      <%--OnRowDataBound="GridView_RowDataBound" 
            OnRowCreated="GridView_RowCreated"--%>
        
    </div>
    
    </form>
</body>
</html>
