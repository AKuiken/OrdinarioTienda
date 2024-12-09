<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Articulos.aspx.cs" Inherits="OrdinarioTienda.Views.Articulos" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Gestión de Artículos</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f5f5f5;
            margin: 0;
            padding: 0;
        }

        .container {
            max-width: 900px;
            margin: 20px auto;
            background: #fff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }

        h1 {
            color: #333;
            text-align: center;
            margin-bottom: 30px;
        }

        .form-crud {
            margin-bottom: 30px;
        }

            .form-crud input,
            .form-crud textarea {
                width: 100%;
                padding: 10px;
                margin-bottom: 10px;
                border: 1px solid #ddd;
                border-radius: 4px;
            }

                .form-crud button,
                .form-crud input[type="submit"] {
                    padding: 10px 15px;
                    background-color: #4CAF50;
                    color: white;
                    border: none;
                    border-radius: 4px;
                    cursor: pointer;
                }

                    .form-crud button:hover {
                        background-color: #45a049;
                    }

        .articulo {
            border-bottom: 1px solid #ddd;
            margin-bottom: 15px;
            padding-bottom: 10px;
        }

        .boton-agregar {
            background-color: #4CAF50;
            color: white;
            border: none;
            padding: 5px 10px;
            cursor: pointer;
        }

        .btn-carrito {
            background-color: #008CBA;
            color: white;
            padding: 10px 15px;
            text-align: center;
            border: none;
            border-radius: 4px;
        }

            .boton-agregar:hover,
            .btn-carrito:hover {
                background-color: #005f7f;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>Gestión de Artículos</h1>

            <!-- Formulario para agregar/editar artículo -->
            <div class="form-crud">
                <h2>Agregar/Actualizar Artículo</h2>
                <asp:HiddenField ID="hfArticuloId" runat="server" />
                <asp:TextBox ID="txtNombre" runat="server" placeholder="Nombre del artículo"></asp:TextBox>
                <asp:TextBox ID="txtMarca" runat="server" placeholder="Marca"></asp:TextBox>
                <asp:TextBox ID="txtPrecio" runat="server" placeholder="Precio"></asp:TextBox>
                <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine" placeholder="Descripción"></asp:TextBox>
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="form-crud-button" OnClick="btnGuardar_Click" />
            </div>

            <!-- Repeater de artículos existentes -->
            <asp:Repeater ID="RepeaterArticulos" runat="server">
                <ItemTemplate>
                    <div class="articulo">
                        <h3><%# Eval("Nombre") %></h3>
                        <p>Marca: <%# Eval("Marca") %></p>
                        <p>Precio: $<%# Eval("Precio", "{0:F2}") %></p>
                        <p><%# Eval("Descripcion") %></p>

                        <!-- Botón para agregar al carrito -->
                        <asp:Button ID="btnAgregarCarrito" runat="server" CssClass="boton-agregar" Text="Agregar al Carrito"
                            CommandArgument='<%# Eval("ID_Articulo") %>' OnCommand="btnAgregarCarrito_Command" />
                        <asp:Button ID="btnEliminar" runat="server" CssClass="boton-agregar" Text="Eliminar"
                            CommandArgument='<%# Eval("ID_Articulo") %>' OnCommand="btnEliminar_Command" />
                        <asp:Button ID="btnEditar" runat="server" CssClass="boton-agregar" Text="Editar"
                            CommandArgument='<%# Eval("ID_Articulo") %>' OnCommand="btnEditar_Command" />
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <asp:Button ID="btnCarrito" runat="server" CssClass="btn-carrito" Text="Ir al Carrito"
                PostBackUrl="Carrito.aspx" />
            <asp:Label ID="lblMensaje" runat="server" ForeColor="Green"></asp:Label>
        </div>
    </form>
</body>
</html>



