<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Carrito.aspx.cs" Inherits="OrdinarioTienda.Views.Carrito" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Carrito</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f5f5f5;
            margin: 0;
            padding: 0;
        }

        .container {
            max-width: 800px;
            margin: 20px auto;
            background: #fff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }

        h1 {
            color: #333;
        }

        .articulo {
            border-bottom: 1px solid #ccc;
            padding: 10px 0;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

            .articulo:last-child {
                border-bottom: none;
            }

            .articulo p {
                margin: 0;
                color: #777;
            }

        .boton {
            background-color: #dc3545;
            color: white;
            border: none;
            padding: 8px 12px;
            border-radius: 5px;
            cursor: pointer;
        }

            .boton:hover {
                background-color: #c82333;
            }

        .total {
            font-weight: bold;
            margin-top: 20px;
            color: #333;
        }

        .inputCantidad {
            width: 50px;
            padding: 5px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>Carrito de Compras</h1>
            <asp:Repeater ID="RepeaterCarrito" runat="server">
                <ItemTemplate>
                    <div class="articulo">
                        <p><%# Eval("Nombre") %> - $<%# Eval("Precio", "{0:F2}") %></p>

                        <!-- Input para actualizar la cantidad -->
                        <input type="number" value='<%# Eval("Cantidad_Disponible") %>' class="inputCantidad"
                            onchange="ActualizarCantidad(<%# Eval("ID_Articulo") %>, this.value)" />

                        <!-- Botón para eliminar artículo -->
                        <asp:Button ID="btnEliminar" runat="server" CssClass="boton"
                            Text="Eliminar" CommandArgument='<%# Eval("ID_Articulo") %>'
                            OnCommand="btnEliminar_Command" />
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <p class="total">Total: $<asp:Label ID="lblTotal" runat="server" Text="0.00"></asp:Label></p>

            <asp:Button ID="btnVaciar" runat="server" Text="Vaciar Carrito" CssClass="boton"
                OnClick="btnVaciar_Click" />
            <asp:Button ID="btnRegresarArticulos" runat="server" Text="Regresar a Artículos" CssClass="boton"
                OnClick="btnRegresarArticulos_Click" />

            <asp:Button ID="btnFinalizarCompra" runat="server" Text="Finalizar Compra" CssClass="boton"
                OnClick="btnFinalizarCompra_Click" />
        </div>
    </form>

    <script type="text/javascript">
        // Función para actualizar la cantidad de un artículo en el carrito
        function ActualizarCantidad(idArticulo, cantidad) {
            var url = 'Carrito.aspx/ActualizarArticulo';
            var data = {
                'idArticulo': idArticulo,
                'cantidad': cantidad
            };

            fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            })
                .then(response => response.json())
                .then(data => {
                    window.location.reload();
                });
        }
    </script>
</body>
</html>



