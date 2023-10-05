﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace articulos_web
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null)
            {
                lblddCuentaExtra.Text = "Algo Aqui";
            }
            else
            {
                lblddCuentaExtra.Text = "Cerrar Sesion";

            }
        }

        protected void lblddCuentaExtra_Click(object sender, EventArgs e)
        {
            Session.Remove("user");
            Response.Redirect("Login.aspx", false);
        }
    }
}