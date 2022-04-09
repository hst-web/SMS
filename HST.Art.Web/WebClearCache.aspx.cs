using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HST.Utillity;

namespace HST.Art.Web
{
    public partial class WebClearCache : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CacheHelper.RemoveAll();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);           
            }

            Response.Write("Remove cache success");
            Response.Write(DateTime.Now.ToString());
        }
    }
}