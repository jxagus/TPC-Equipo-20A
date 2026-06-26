<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="alert alert-danger" role="alert">
            <h4 class="alert-heading">🚨 Ha ocurrido un error en el sistema</h4>
            <hr>
            <asp:Label ID="lblError" runat="server" CssClass="text-monospace small" style="white-space: pre-wrap;"></asp:Label>
        </div>
    </div>
</asp:Content>