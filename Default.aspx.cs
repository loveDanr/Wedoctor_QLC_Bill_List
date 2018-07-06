using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Collections;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net.Mail;

public partial class _Default : System.Web.UI.Page
{
    public int HIS_RowsCount=0;
    public int sum_totalCount = 0;
    public decimal sum_TotalGet = 0;
    public decimal sum_Total_Refund = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        //testTime = "2018-06-19";
        if (!IsPostBack)
        {
            txt_startDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            txt_endDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            //System.Threading.Thread LoadServiceData = new System.Threading.Thread(new System.Threading.ThreadStart(LoadFromWebservice));
            //LoadServiceData.Start();

}
    }
        private void LoadFromWebservice()
    {
        //定义一个定时器，并开启和配置相关属性
        System.Timers.Timer Wtimer = new System.Timers.Timer(1);//执行任务的周期
        Wtimer.Elapsed += new System.Timers.ElapsedEventHandler(GetDate);
        Wtimer.Enabled = true;
        Wtimer.AutoReset = true;
    }
     void GetDate(object sender, System.Timers.ElapsedEventArgs e)
    {
         int intHour = e.SignalTime.Hour;
        int intMinute = e.SignalTime.Minute;
        int intSecond = e.SignalTime.Second;
        // 定制时间； 比如 在10：30 ：00 的时候执行某个函数
        int iHour = 18;
        int iMinute =34;
        int iSecond = 00;
        // 设置　每天的１０：３０：００开始执行程序
        if (intHour == iHour && intMinute == iMinute && intSecond == iSecond)
        { 
              txt_startDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
        List<string> _request = getReq(txt_startDate.Text.Trim(), txt_endDate.Text.Trim());
        //RefreshCheckData(_request,);
        }
      
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {   DateTime t1 = Convert.ToDateTime(txt_startDate.Text.Trim());
        DateTime t2 = Convert.ToDateTime(txt_endDate.Text.Trim());
        DateTime t3 =  Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        DateTime t5 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
        string dateCompare = txt_startDate.Text.Trim() + " "+"11:00:00";
        DateTime t4 = Convert.ToDateTime(dateCompare);
        if (DateTime.Compare(t1, t2) > 0)
        {
            //strErr += "截止日期必须在发布日期之后！";
            //HttpContext.Current.Response.Write(" <script>alert('没有数据可导出！');");
           // Response.Write(" <script>function window.onload() {alert( ' 弹出的消息' ); } </script> ");
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('开始时间大于结束时间啦！');</script>");
        } if (DateTime.Compare(t4, t3) > 0)
        {
            //strErr += "截止日期必须在发布日期之后！";
            //HttpContext.Current.Response.Write(" <script>alert('没有数据可导出！');");
            // Response.Write(" <script>function window.onload() {alert( ' 弹出的消息' ); } </script> ");
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('平台账单由于微信后台账单生成时间限制导致账单尚未生成,\\n  请于11点后查看！！');</script>");

        }
        //else if (DateTime.Compare(t2, t5) <= 0)
        //{
        //    Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('时间选择有误，请重新选择！');</script>");
        //}
        else
        {
            string _requesthis = gethisReq(txt_startDate.Text.Trim(), txt_endDate.Text.Trim());
            List<string> _request = getReq(txt_startDate.Text.Trim(), txt_endDate.Text.Trim());
            //string request = gethisReq(txt_startDate.Text.Trim(), txt_endDate.Text.Trim());
            RefreshCheckData(_request,_requesthis);
        }
    }


    protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
        {
            //保持列不变形
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                //方法一：
                e.Row.Cells[i].Text = e.Row.Cells[i].Text;
                e.Row.Cells[i].Wrap = false;
                //方法二：
                //e.Row.Cells[i].Text = "<nobr>&nbsp;" + e.Row.Cells[i].Text + "&nbsp;</nobr>";            
            }

        }
        //如果是绑定数据行 
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    //鼠标经过时，行背景色变 
        //    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#E6F5FA'");
        //    //鼠标移出时，行背景色变 
        //    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF'");

        //}

        if (e.Row.RowIndex >= 0)
        {


            sum_TotalGet += Convert.ToDecimal(e.Row.Cells[8].Text);
            sum_Total_Refund += Convert.ToDecimal(e.Row.Cells[9].Text);
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = (sum_totalCount).ToString("F2");
            e.Row.Cells[3].Text = (sum_TotalGet - sum_Total_Refund).ToString("F2");
            if (this.GridView.DataSource != "")
            {
                TotalDealCount.Text = e.Row.Cells[1].Text;
                TotalDealMoneyCount.Text = e.Row.Cells[3].Text;
            }
            else
            {
                TotalDealCount.Text = "无数据";
                TotalDealMoneyCount.Text = "无数据";
            }
        }
    }
    protected void GridView_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            TableCellCollection tcc = e.Row.Cells;
            tcc.Clear();
            e.Row.Cells.Add(new TableCell());
            e.Row.Cells[0].Attributes.Add("colspan", "4");
            e.Row.Cells[0].Text = "总交易数";
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells.Add(new TableCell());

            e.Row.Cells[1].Text = "";//
            e.Row.Cells.Add(new TableCell());

            e.Row.Cells[2].Text = "总收入";//
            e.Row.Cells.Add(new TableCell());

            e.Row.Cells[3].Text = "";//
            e.Row.Cells.Add(new TableCell());

            e.Row.Cells[4].Attributes.Add("colspan", "8");
            e.Row.Cells[4].Text = "";
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells.Add(new TableCell());
        }
        if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            e.Row.Cells[1].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            e.Row.Cells[2].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            e.Row.Cells[3].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            e.Row.Cells[4].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            e.Row.Cells[10].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            e.Row.Cells[11].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            e.Row.Cells[16].Visible = false;
            e.Row.Cells[17].Visible = false;
            e.Row.Cells[18].Visible = false;
            e.Row.Cells[19].Visible = false;
        }
        //if (e.Row.RowType == DataControlRowType.Header)
        //{
        //    e.Row.Cells[1].Text = "3333";
        //}


    }

    /// <summary>
    /// 导出到Excel
    /// </summary>
    /// <param name="gv"></param>
    void toExcel(GridView gv, string fileName)
    {
        Response.Charset = "UTF-8";
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
        string style = @"<style> .text { mso-number-format:\@; } </script> ";
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=" + fileName + ".xls");
        Response.ContentType = "application/excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        gv.RenderControl(htw);
        Response.Write(style);
        Response.Write(sw.ToString());
        Response.End();
        }
        
    
 
    /// <summary>
    /// 这个重写貌似是必须的
    /// </summary>
    /// <param name="control"></param>
    public override void VerifyRenderingInServerForm(Control control) { }
    #region "页面加载中效果"
    /// <summary>
    /// 页面加载中效果
    /// </summary>
    public static void initJavascript()
    {
        HttpContext.Current.Response.Write(" <script language=JavaScript type=text/javascript>");
        HttpContext.Current.Response.Write("var t_id = setInterval(animate,20);");
        HttpContext.Current.Response.Write("var pos=0;var dir=2;var len=0;");
        HttpContext.Current.Response.Write("function animate(){");
        HttpContext.Current.Response.Write("var elem = document.getElementById('progress');");
        HttpContext.Current.Response.Write("if(elem != null) {");
        HttpContext.Current.Response.Write("if (pos==0) len += dir;");
        HttpContext.Current.Response.Write("if (len>32 || pos>79) pos += dir;");
        HttpContext.Current.Response.Write("if (pos>79) len -= dir;");
        HttpContext.Current.Response.Write(" if (pos>79 && len==0) pos=0;");
        HttpContext.Current.Response.Write("elem.style.left = pos;");
        HttpContext.Current.Response.Write("elem.style.width = len;");
        HttpContext.Current.Response.Write("}}");
        HttpContext.Current.Response.Write("function remove_loading() {");
        HttpContext.Current.Response.Write(" this.clearInterval(t_id);");
        HttpContext.Current.Response.Write("var targelem = document.getElementById('loader_container');");
        HttpContext.Current.Response.Write("targelem.style.display='none';");
        HttpContext.Current.Response.Write("targelem.style.visibility='hidden';");
        HttpContext.Current.Response.Write("}");
        HttpContext.Current.Response.Write("</script>");
        HttpContext.Current.Response.Write("<style>");
        HttpContext.Current.Response.Write("#loader_container {text-align:center; position:absolute; top:20%;width:100%; left: 0;}");
        //HttpContext.Current.Response.Write("#loader {font-family:Tahoma, Helvetica, sans; font-size:13.5px; color:#000000; background-color:#FFFFFF; padding:10px 0 16px 0; margin:0 auto; display:block; width:130px;height: 40px; border:1px solid #5a667b; text-align:left; z-index:2;}");
        //HttpContext.Current.Response.Write("#progress {height:5px; font-size:5px; width:5px; position:relative; top:1px; left:0px; background-color:#8894a8;}");
        HttpContext.Current.Response.Write("#loader_bg {background-color:#e4e7eb; position:relative; top:8px; left:8px; height:7px; width:113px; font-size:1px;}");
        HttpContext.Current.Response.Write("</style>");
        HttpContext.Current.Response.Write("<div id=loader_container>");
        //HttpContext.Current.Response.Write("<div id=loader>");
        HttpContext.Current.Response.Write("<div align=center><img src='/loading.gif'/></div>");
        //HttpContext.Current.Response.Write("<div id=loader_bg> <div id=progress></div> </div>"); 
        HttpContext.Current.Response.Write("</div></div>");
        HttpContext.Current.Response.Flush();
    }
    //
    #endregion

    protected void download_btn_Click(object sender, EventArgs e)
    {
        if (this.GridView.Rows.Count != null && this.GridView.Rows.Count != 0)
        {
            toExcel(this.GridView, txt_startDate.Text.Trim() + "至" + txt_endDate.Text.Trim() + "对账表");
        }
        else
        {
            return;
            // Response.Write("<script>alert('没有数据可导出！')</script>");
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('没有数据可导出！');</script>");
            //Response.Write(" <script>function window.onload() {alert( ' 弹出的消息' ); } </script> ");
         
        }
    }
    /// <summary>
    /// 求datatable列的和
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="ColumnName"></param>
    /// <returns></returns>
    double ColumnSum(DataTable dt, string ColumnName,string ColumnName2)
    {
        double d = 0; double e = 0; double f = 0;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i][ColumnName2].ToString() == "")
            {

                dt.Rows[i][ColumnName2] = 0;
            }
        }
        foreach (DataRow row in dt.Rows)
        {

            d += double.Parse(row[ColumnName].ToString());
           
            e += double.Parse(row[ColumnName2].ToString());

        }
        f = d - e;
        return f;

    }
    // 将系统类库提供的异步方法利用async封装起来

 
    #region  刷新数据明细数据
    /// <summary>
    /// 刷新数据明细数据
    /// </summary>
    public async void RefreshCheckData(List<string> request, string request_his)
    {
        initJavascript(); System.Data.DataTable DtAll = new System.Data.DataTable();
        System.Data.DataTable dt_wy = new System.Data.DataTable(); dt_wy.TableName = "ACCOUNT_LIST";
        System.Data.DataTable dt_his = new System.Data.DataTable(); DataTable dtResulthis = new DataTable();
        DataTable dtWYResulthis = new DataTable();
        Task<string> t1 = gethisXml(request_his);
        string strxmlhis = t1.Result;//gethisXml(request_his);
        double Counthis = 0; double Amounthis = 0;
       
        dt_his = GetDBdata.XmlToDataTable(strxmlhis);
        HIS_RowsCount = dt_his.Rows.Count;
        foreach (DataRow dr in dtResulthis.Rows)
        {
            Counthis += double.Parse(dr["Counthis"].ToString());
            Amounthis += double.Parse(dr["Amounthis"].ToString());
        }

        HIS_TotalFeeCount.Text = Convert.ToDecimal(Amounthis).ToString("f2") + "元";
        HIS_TotalDealCount.Text = Convert.ToString(Counthis) + "笔";
        HIS_TotalFeeCount.Visible = false; HIS_TotalDealCount.Visible = false;
        #region datatable创建

        dt_wy.Columns.Add("交易时间", typeof(string));
        dt_wy.Columns.Add("业务平台流水", typeof(string));

        dt_wy.Columns.Add("平台交易流水", typeof(string));
        dt_wy.Columns.Add("业务退款流水", typeof(string));
        dt_wy.Columns.Add("平台退款流水", typeof(string));
        dt_wy.Columns.Add("交易金额", typeof(decimal));
        dt_wy.Columns.Add("退款金额", typeof(decimal));
        dt_wy.Columns.Add("补贴金额", typeof(decimal));
        dt_wy.Columns.Add("实付金额", typeof(decimal));
        dt_wy.Columns.Add("实退金额", typeof(decimal));
        dt_wy.Columns.Add("第三方支付交易流水号", typeof(string));
        dt_wy.Columns.Add("第三方支付退款流水号", typeof(string));
        dt_wy.Columns.Add("支付类型", typeof(string));
        dt_wy.Columns.Add("交易状态", typeof(string));
        dt_wy.Columns.Add("退款状态", typeof(string));
        dt_wy.Columns.Add("商户名称", typeof(string));
        dt_wy.Columns.Add("商品名称", typeof(string));
        dt_wy.Columns.Add("商户号", typeof(string));
        dt_wy.Columns.Add("接入应用ID", typeof(string));
        dt_wy.Columns.Add("第三方商户号", typeof(string));
        //string his_dealTime,his_orderId,his_fee,wy_dealTime,wy_orderId,wy_fee;
        #endregion
        try
        {
            Task<List<string[]>> t2 = getDataList(request);
            var end =  await getDataList(request);
            List<string[]> response = end;// getDataList(request);
            List<string> result = new List<string>();
            int _count = response.Count;
            sum_totalCount = _count;

            for (int k = 0; k < _count; k++)
            {
                dt_wy.Rows.Add(response[k]);
            }
             
            this.GridView.DataSource = dt_wy.DefaultView;
            this.GridView.DataBind();
            for (int i = dt_his.Rows.Count - 1; i >= 0; i--)
            {
                for (int k = 0; k < dt_wy.Rows.Count; k++)
                {
                    if (dt_his.Rows[i][1].ToString() == dt_wy.Rows[k][1].ToString())
                    {
                        GridView.Rows[k].BackColor = System.Drawing.Color.Green;
                        //Logging.WriteHISlog("记录日志：","HIS的ORDER_ID="+dt_his.Rows[i][3].ToString()+ "\r\n"+"微医的平台订单号="+ dt_wy.Rows[k][11].ToString() + "\r\n" + "HIS的TRANS_NO="+dt_his.Rows[i][13].ToString()+"微医的HOSP_ORDER_ID = "+ dt_wy.Rows[k][1].ToString() + "\r\n" + "HIS的TRANS_TYPE=" + dt_his.Rows[i][1].ToString() + "\r\n" + "微医的ORDER_TYPE=" + dt_wy.Rows[k][2].ToString() + "");
                    }

                }

            }
            #region 统计所有数据
            dtResulthis = GetDBdata.GetResult(dt_his);
            dtWYResulthis = GetDBdata.GetResult(dt_wy);
            DtAll = GetDBdata.UniteDataTable(dtResulthis, dtWYResulthis, "合并Dt");
            DtAll.Columns.AddRange(new DataColumn[] { new DataColumn("different", typeof(double)) });
            DataRow drw = DtAll.NewRow();
            double different_money = 0.00;
            for (int i = 0; i < DtAll.Rows.Count; i++)
            {
                foreach (DataRow dr in DtAll.Rows)
                {

                    different_money = different_money + (Convert.ToDouble(DtAll.Rows[i]["Amounthis"]) - Convert.ToDouble(DtAll.Rows[i]["wxAmount"]));
                    DtAll.Rows[i]["different"] = different_money.ToString("f2");
                    different_money = 0;


                }

            }


            this.GridView_Count.DataSource = DtAll.DefaultView;
            this.GridView_Count.DataBind();
            #endregion
            #region 账不平的变红色

            for (int k = 0; k < DtAll.Rows.Count; k++)
            {
                if (DtAll.Rows[k][6].ToString() != "0")
                {
                    GridView_Count.Rows[k].BackColor = System.Drawing.Color.Red;
                    //Logging.WriteHISlog("记录日志：","HIS的CARD_NO="+dt_his.Rows[i][3].ToString()+ "\r\n"+"微医的HOSP_PATIENT_ID="+ dt_wy.Rows[k][11].ToString() + "\r\n" + "HIS的TRANS_NO="+dt_his.Rows[i][13].ToString()+"微医的HOSP_ORDER_ID = "+ dt_wy.Rows[k][1].ToString() + "\r\n" + "HIS的TRANS_TYPE=" + dt_his.Rows[i][1].ToString() + "\r\n" + "微医的ORDER_TYPE=" + dt_wy.Rows[k][2].ToString() + "");
                }

            }

            #endregion

        }

        catch (Exception Exc)
        {
            Logging.WriteBuglog(Exc);
            return;
        }
        finally
        {

            string LogXml = GetDBdata.DataTable2Xml(dt_wy);
            Logging.WriteWYlog(txt_startDate.Text.Trim() + "至" + txt_endDate.Text.Trim() + "的明细日志", LogXml);
            Logging.WriteHISlog(txt_startDate.Text + "至" + txt_endDate.Text + "的日志", strxmlhis);

        }


    }

    #endregion
    /// <summary>
    /// 获取his交易数据
    /// </summary>
    /// <returns></returns>
    public async Task<string>  gethisXml(string request)
    {
        string response = GetDBdata.gethis(request);
        return response;
    }

    /// <summary>
    /// 获取平台交易明细账数据
    /// </summary>
    /// <returns></returns>
    public async Task<List<string[]>>  getDataList(List<string> listdate)
    {
        List<string[]> list = new List<string[]>();
        List<string[]> response = new List<string[]>();

        foreach (var date in listdate)
        {
            
            list =  GetDBdata.getPtData(date);
            if (list.Count != 0)
            {
                list.RemoveAt(0);
                list.RemoveAt((list.Count - 3));
                list.RemoveAt((list.Count - 2));
                list.RemoveAt((list.Count - 1));

                foreach (var str in list)
                {
                    response.Add(str);
                }
            }
           
        }
        return response;
    }
    #region  获取平台交易总账数据
    /// <summary>
    /// 获取平台交易总账数据
    /// </summary>
    /// <returns></returns>
    public List<string[]> getData(List<string> listdate)
    {
        List<string[]> list = new List<string[]>();
        List<string[]> response = new List<string[]>();

        foreach (var date in listdate)
        {
            list = GetDBdata.getPtData(date);
            if (list.Count != 0)
            {
                string s1 = list[1][0].ToString().Split(' ')[0].Replace("/", "-");
                string[] ss = s1.Split('-');
                if (ss[1].Length == 1)
                {
                    ss[1] = "0" + ss[1];
                }
                if (ss[2].Length == 1)
                {
                    ss[2] = "0" + ss[2];
                }
                s1 = ss[0] + "-" + ss[1] + "-" + ss[2];
                string s2 = (Convert.ToDouble(list[(list.Count - 1)][1]) - Convert.ToDouble(list[(list.Count - 1)][2])).ToString();
                string s3 = list[(list.Count - 1)][0].ToString();

                string[] str = { s1, s2, s3 };
                response.Add(str);
            }
        }
        return response;
    }
    #endregion

    /// <summary>
    /// 获取his入参
    /// </summary>
    public string gethisReq(string begindata, string enddata)
    {
        string req = string.Empty;
        string request = string.Empty;
        string pagesize = ConfigurationManager.AppSettings["PageSize"];//ReadIni.ReadIniValue("LOCALHOST_SERVERS", "PageSize");//HIS接口数据查询量
        string hiskey = ConfigurationManager.AppSettings["Hiskey"];//ReadIni.ReadIniValue("LOCALHOST_SERVERS", "Hiskey");//His密匙
        string service = ConfigurationManager.AppSettings["Service"];//ReadIni.ReadIniValue("LOCALHOST_SERVERS", "Service");//接口名称
        string partner = ConfigurationManager.AppSettings["Partner"];//ReadIni.ReadIniValue("LOCALHOST_SERVERS", "Partner");//接入方接口用户
        string hospitalCode = ConfigurationManager.AppSettings["HospitalCode"];//ReadIni.ReadIniValue("LOCALHOST_SERVERS", "HospitalCode");//接入医疗机构编码


        req = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<request>
  <startDate><![CDATA[" + begindata + @" ]]></startDate>
  <endDate><![CDATA[" + enddata + @"]]></endDate>
  <currentPage><![CDATA[1]]></currentPage>
  <pageSize><![CDATA[" + pagesize + @"]]></pageSize>
</request>";

        string encryptKey = hiskey;
        string encryptString = req;
        req = AES.Encrypt(encryptKey, encryptString);

        IDictionary<string, string> dict = new Dictionary<string, string>();
        string sign = string.Empty;

        dict.Add("service", service);
        dict.Add("partner", partner);
        dict.Add("hospitalCode", hospitalCode);
        dict.Add("inputCharset", "utf-8");
        dict.Add("dataFormat", "xml");
        dict.Add("requestEncrypted", req);
        string signString = GetDBdata.CreateLinkString(GetDBdata.SortDictPara(dict));
        signString += "&key=" + encryptKey;
        sign = GetDBdata.Encode(signString).ToUpper();

        request = @"<root>
                          <service><![CDATA[" + service + @"]]></service>
                          <partner><![CDATA[" + partner + @"]]></partner>
                          <hospitalCode><![CDATA[" + hospitalCode + @"]]></hospitalCode>
                          <inputCharset><![CDATA[utf-8]]></inputCharset>
                          <dataFormat><![CDATA[xml]]></dataFormat>
                          <sign><![CDATA[" + sign + @"]]></sign>
                          <signType><![CDATA[md5]]></signType>
                          <requestEncrypted><![CDATA[" + req + @"]]></requestEncrypted>
                        </root>";
        return request;
    }



    /// <summary>
    /// 获取平台入参
    /// </summary>
    public List<string> getReq(string begindata, string enddata)
    {
        string request = string.Empty;
        string date = string.Empty;
        DateTime dtend = Convert.ToDateTime(enddata + " 00:00:00");
        DateTime dtbegin = Convert.ToDateTime(begindata + " 00:00:00");
        TimeSpan tsend = new TimeSpan(dtend.Ticks);
        TimeSpan tsbegin = new TimeSpan(dtbegin.Ticks);
        TimeSpan ts = tsend.Subtract(tsbegin).Duration();
        int n = ts.Days + 1;

        List<string> listdate = new List<string>();
        for (int i = 0; i < n; i++)
        {
            date = dtbegin.AddDays(i).ToString("yyyyMMdd");
            listdate.Add(date);
        }
        return listdate;
    }
    private int sum_Total_count, sum_Total_count_wy;
    private double sum_Total_his_amount, sum_Total_amoun_wy, different_money;
    protected void GridView_Count_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
        {
            //保持列不变形
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Wrap = false;
                //方法二：
                //e.Row.Cells[i].Text = "<nobr>&nbsp;" + e.Row.Cells[i].Text + "&nbsp;</nobr>";            
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = "挂号日期";
                e.Row.Cells[1].Text = "HIS交易记录数";
                e.Row.Cells[2].Text = "HIS交易金额";
                e.Row.Cells[3].Text = "微医交易日期";
                e.Row.Cells[4].Text = "微医交易记录数";
                e.Row.Cells[5].Text = "微医交易金额";
                e.Row.Cells[6].Text = "双方差额";
            }
            if (e.Row.RowIndex >= 0)
            {


                sum_Total_count += Convert.ToInt32(e.Row.Cells[1].Text);
                sum_Total_his_amount += Convert.ToDouble(e.Row.Cells[2].Text);
                sum_Total_count_wy += Convert.ToInt32(e.Row.Cells[4].Text);
                sum_Total_amoun_wy += Convert.ToDouble(e.Row.Cells[5].Text);
                different_money += Convert.ToDouble(e.Row.Cells[6].Text);

            }


        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = sum_Total_count.ToString();
            e.Row.Cells[2].Text = sum_Total_his_amount.ToString("F2");
            e.Row.Cells[4].Text = sum_Total_count_wy.ToString();
            e.Row.Cells[5].Text = sum_Total_amoun_wy.ToString("f2");
            e.Row.Cells[6].Text = different_money.ToString("f2");

        }
        #region 提升语内容设置
        if (e.Row.Cells[6].Text == "0.00")
        {
            DetailsListTitle.Visible = false;
            TotalDealCount.Visible = false;
            TotalDealMoneyCount.Visible = false;
            HIS_TotalDealCount.Visible = false;
            HIS_TotalFeeCount.Visible = false;
            GridView.Visible = false;
            string str = HIS_TotalFeeCount.Text.Trim();
            if (txt_startDate.Text == txt_endDate.Text)
            {
                Notice.Text = "恭喜！" + txt_startDate.Text.Trim() + "的账平啦！\r\r\n 总金额是：" + str.Replace("HIS总金额", "￥");
            }
            if (txt_startDate.Text != txt_endDate.Text)
            {
                Notice.Text = "恭喜！" + txt_startDate.Text.Trim() + "至" + txt_endDate.Text.Trim() + "的账平啦！\r\r\n 总金额是：" + str.Replace("HIS总金额", "￥");
            }
        }
        else if (e.Row.Cells[6].Text != "0.00")
        {
            TotalDealCount.Visible = false;
            TotalDealMoneyCount.Visible = false;
            HIS_TotalDealCount.Visible = false;
            HIS_TotalFeeCount.Visible = false;
            Notice.Text = "注意！有账不平啦！";
            GridView.Visible = true;
            DetailsListTitle.Visible = true;
            if (txt_startDate.Text == txt_endDate.Text)
            {
                DetailsListTitle.Text = txt_startDate.Text.Trim() + "的明细单。 备注：绿色是匹配成功的订单，白色是HIS没有的订单";
            }
            else if (txt_startDate.Text != txt_endDate.Text)
            {
                DetailsListTitle.Text = txt_startDate.Text.Trim() + "至" + txt_endDate.Text.Trim() + "的明细单。 备注：绿色是匹配成功的订单，白色是HIS没有的订单";
            }
        }

        #endregion

    }

    protected void GridView_Count_RowCreated(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            //e.Row.Cells[0].Attributes.Add("colspan", "3");
            e.Row.Cells[0].Text = "合计";

            //e.Row.Cells[1].Text = "";//



            //e.Row.Cells[2].Font.Bold = true;


            //e.Row.Cells[3].Text = "";//


            //e.Row.Cells[4].Text = "";//


            //e.Row.Cells[5].Text = "";//


            //e.Row.Cells[6].Text = "";
            //e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;

        }
        if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
        {
            #region 设置GridView列的属性为文本，便于导出excel时显示正常
            e.Row.Cells[0].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            e.Row.Cells[1].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            e.Row.Cells[2].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            e.Row.Cells[3].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            e.Row.Cells[4].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            e.Row.Cells[6].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            #endregion
        }
    }
}