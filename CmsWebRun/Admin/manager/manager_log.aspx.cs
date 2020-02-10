﻿using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Web;
using Cms.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cms.Common;
using System.Data.SqlClient;
using Cms.DBUtility;


public partial class Admin_manager_manager_log : System.Web.UI.Page
{
     protected int totalCount;
        protected int page;
        protected int pageSize;

        protected string keywords = string.Empty;
        Cms.Model.C_admin model = new Cms.Model.C_admin();
        public DataSet ds;
        public SqlDataAdapter dr;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.keywords = DTRequest.GetQueryString("keywords");
           
            if (!Page.IsPostBack)
            {
                //登录验证
                adminUser.GetLoginState();

                //登录信息
                HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["admin"];

                if (cookie != null)
                {
                    Application["adminname"] = (string)cookie.Values["adminname"];
                }
                else if (Session["adminname"] != null)
                {
                    Application["adminname"] = (string)Session["adminname"];
                }

                Cms.BLL.C_admin_log bll = new Cms.BLL.C_admin_log();
                string where = "select * from C_admin_log  order by id desc";
                this.AspNetPager1.AlwaysShow = true;
                this.AspNetPager1.PageSize = 10;
                this.AspNetPager1.RecordCount = bll.GetRecordCount("");
                this.RepeaterDataBind(where);

                bool blDelete = adminUser.setpurview("管理日志", "Delete");
                if (!blDelete)
                {
                    btnDelete.Visible = false;
                }
            }
        }

        #region 组合SQL查询语句==========================
        protected string CombSqlTxt(string _keywords)
        {
            StringBuilder strTemp = new StringBuilder();
            _keywords = _keywords.Replace("'", "");
            if (!string.IsNullOrEmpty(_keywords))
            {
                strTemp.Append(" and (user_name like  '%" + _keywords + "%' or action_type like '%" + _keywords + "%')");
            }

            return strTemp.ToString();
        }
        #endregion

        #region 数据读取======================================
        public void RepeaterDataBind(string whereStr)
        {
            dr = new SqlDataAdapter(whereStr, DbHelperSQL.connectionString);
            ds = new DataSet();
            dr.Fill(ds, AspNetPager1.PageSize * (AspNetPager1.CurrentPageIndex - 1), AspNetPager1.PageSize, "C_admin_log");
            this.rptList.DataSource = ds.Tables["C_admin_log"];
            this.rptList.DataBind();


        }
        protected void AspNetPager1_PageChanging(object src, Wuqi.Webdiyer.PageChangingEventArgs e)
        {
            this.AspNetPager1.CurrentPageIndex = e.NewPageIndex;
            string where = "select * from C_admin_log order by id desc";
            this.RepeaterDataBind(where.ToString());

        }
        #endregion

        //关健字查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("manager_log.aspx", "keywords={0}", txtKeywords.Text));
        }
              


        //批量删除
        protected void btnDelete_Click(object sender, EventArgs e)
        {

            Cms.BLL.C_admin_log bll = new Cms.BLL.C_admin_log();
            
            int sucCount = Cms.DBUtility.DbHelperSQL.ExecuteSql("delete from C_admin_log where DATEDIFF(day, add_time, getdate()) > 7");//修改
            adminUser.AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除管理日志" + sucCount + "条"); //记录日志
            JscriptMsg("删除日志" + sucCount + "条", Utils.CombUrlTxt("manager_log.aspx", "keywords={0}", this.keywords), "Success");
        }

        #region 提示框=========================================
        public void JscriptMsg(string msgtitle, string url, string msgcss)
        {
            string msbox = "parent.jsprint(\"" + msgtitle + "\", \"" + url + "\", \"" + msgcss + "\")";
            ClientScript.RegisterClientScriptBlock(Page.GetType(), "JsPrint", msbox, true);
        }
        #endregion
    
}