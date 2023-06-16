﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="CartasDeArticulos.aspx.cs" Inherits="articulos_web.CartasDeArticulos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="text-center">Grilla
    </h1>
    <div class="container">
        <div class="row row-cols-1 row-cols-md-3 g-4">

            <%
                foreach (domain.Producto prod in ListaProducto)
                {%>

            <div class="col">
                <div class="card h-100">
                    <img src="<%: prod.ImagenUrl %>" class="card-img-top" alt="..." style="max-height:300px; max-width: 300px;">
                    <div class="card-body">
                        <h5 class="card-title"><%: prod.Nombre %></h5>
                        <p class="card-text"><%: prod.Descripcion %></p>
                        <a href="Producto.aspx?id=<%: prod.Id %>" class="btn btn-primary">Detalle</a>
                    </div>
                </div>
            </div>
            <%  } %>
        </div>
    </div>
</asp:Content>
