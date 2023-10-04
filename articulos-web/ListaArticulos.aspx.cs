﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using service;
using domain;
using System.Text.RegularExpressions;

namespace articulos_web
{
    public partial class ListaArticulos : System.Web.UI.Page
    {
        public CultureInfo ci = new CultureInfo("es-AR");
        public bool FiltroAvanzado { get; set; }
        public string error { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            FiltroAvanzado = checkFiltroAvanzado.Checked;
            if (!IsPostBack)
            {
                if (Session["search"] == null)
                {
                    FiltroAvanzado = false;
                
                    ProductoService service = new ProductoService();
                    Session.Add("productos", service.toList());
                    dgvArticulos.DataSource = Session["productos"];
                    dgvArticulos.DataBind();
                    ddlCriterio.Items.Add("Que Contenga");
                    ddlCriterio.Items.Add("Que Empiece Por");
                    ddlCriterio.Items.Add("Que Termine Por");
                    //ddlCriterio.SelectedItem = "Que Contenga";
                }
                else
                {
                    txtFiltro.Text = Session["search"].ToString();
                    filtrarVuelta(Session["search"].ToString());


                }


            }
            
        }

        protected void dgvArticulos_SelectionIndexChanged(object sender, EventArgs e)
        {
            string id = dgvArticulos.SelectedDataKey.Value.ToString();
            Response.Redirect("Producto.aspx?id=" + id);
        }

        protected void dgvArticulos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvArticulos.PageIndex = e.NewPageIndex;
            dgvArticulos.DataBind();
        }

        protected void filtro_TextChanged(object sender, EventArgs e)
        {
            filtrarTexto(txtFiltro.Text);   
        }

        public void filtrarTexto(string filtro)
        {
            if (txtFiltro.Text == "")
            {
                if (Session["search"] != null)
                {
                    if (checkFiltroAvanzado.Checked)
                    {
                        ProductoService service = new ProductoService();
                        dgvArticulos.DataSource = service.filtrar(ddlCampo.SelectedItem.ToString(), ddlCriterio.SelectedItem.ToString(), txtFiltro.Text, ddlImagen.SelectedItem.ToString());
                        dgvArticulos.DataBind();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "focusScript", "setFocusOnFilter();", true);
                        return;
                    }
                }
            }

            if (validarFiltro())
            {

                if (!checkFiltroAvanzado.Checked)
                {
                    List<Producto> lista = (List<Producto>)Session["productos"];
                    List<Producto> listaFiltrada = lista.FindAll(x => x.Nombre.ToUpper().Contains(txtFiltro.Text.ToUpper()));
                    dgvArticulos.DataSource = listaFiltrada;
                    Session.Add("filtro", listaFiltrada);
                    Session.Add("search", txtFiltro.Text);
                    dgvArticulos.DataBind();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "focusScript", "setFocusOnFilter();", true);
                }
                else
                {
                    ProductoService service = new ProductoService();
                    List<Producto> listaFiltrada = service.filtrar(ddlCampo.SelectedItem.ToString(), ddlCriterio.SelectedItem.ToString(), txtFiltro.Text, ddlImagen.SelectedItem.ToString());
                    dgvArticulos.DataSource = listaFiltrada;
                    Session.Add("filtro", listaFiltrada);
                    Session.Add("search", txtFiltro.Text);
                    dgvArticulos.DataBind();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "focusScript", "setFocusOnFilter();", true);
                }
            }
        }

        public void filtrarVuelta(string filtro)
        {
            if (txtFiltro.Text == "")
            {
                if (Session["search"] != null)
                {
                    if (checkFiltroAvanzado.Checked)
                    {
                        ProductoService service = new ProductoService();
                        dgvArticulos.DataSource = service.filtrar(ddlCampo.SelectedItem.ToString(), ddlCriterio.SelectedItem.ToString(), Session["search"].ToString(), ddlImagen.SelectedItem.ToString());
                        dgvArticulos.DataBind();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "focusScript", "setFocusOnFilter();", true);
                        return;
                    }
                }
            }

            if (validarFiltro())
            {

                if (!checkFiltroAvanzado.Checked)
                {
                    ProductoService service = new ProductoService();
                    List<Producto> lista = service.toList();
                    List<Producto> listaFiltrada = lista.FindAll(x => x.Nombre.ToUpper().Contains(Session["search"].ToString().ToUpper()));
                    dgvArticulos.DataSource = listaFiltrada;
                    Session.Add("filtro", listaFiltrada);
                    txtFiltro.Text = Session["search"].ToString();
                    Session.Remove("search");
                    dgvArticulos.DataBind();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "focusScript", "setFocusOnFilter();", true);
                }
                else
                {
                    ProductoService service = new ProductoService();
                    List<Producto> listaFiltrada = service.filtrar(ddlCampo.SelectedItem.ToString(), ddlCriterio.SelectedItem.ToString(), Session["search"].ToString(), ddlImagen.SelectedItem.ToString());
                    dgvArticulos.DataSource = listaFiltrada;
                    Session.Add("filtro", listaFiltrada);
                    txtFiltro.Text = Session["search"].ToString();
                    Session.Remove("search");
                    dgvArticulos.DataBind();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "focusScript", "setFocusOnFilter();", true);
                }
            }
        }

        private bool validarFiltro()
        {
            if (ddlCampo.SelectedItem.ToString() == "Precio")
            {
                if (Regex.IsMatch(txtFiltro.Text, @"[a-zA-Z]"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('El texto ingresado contiene letras. Por favor, ingrese solo numeros para filtrar por Precio. ');", true);
                    return false;
                }
            }

            return true;
        }

        protected void checkFiltroAvanzado_CheckedChanged(object sender, EventArgs e)
        {
            FiltroAvanzado = checkFiltroAvanzado.Checked;


            //if (checkFiltroAvanzado.Checked)
            //    FiltroAvanzado = true;
            //else
            //    FiltroAvanzado = false;

            
        }

        protected void ddlCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCriterio.Items.Clear();
            switch (ddlCampo.SelectedValue)
            {
                default: //Nombre, Marca o Categoria
                    ddlCriterio.Items.Add("Contiene");
                    ddlCriterio.Items.Add("Termina con");
                    ddlCriterio.Items.Add("Comienza con");
                    break;
                case "Precio":
                    ddlCriterio.Items.Add("Menor que");
                    ddlCriterio.Items.Add("Mayor que");
                    ddlCriterio.Items.Add("Igual que");
                    break;
            }
        }

        

    }
}