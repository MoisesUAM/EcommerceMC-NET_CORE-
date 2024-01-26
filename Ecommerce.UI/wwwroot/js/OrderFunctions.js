let datatable;

$(function () {
    var url = window.location.search;
    if (url.includes("approved")) {
        loadTblOrders("GetAllOrders?status=approved");
    }
    else {
        if (url.includes("completed")) {
            loadTblOrders("GetAllOrders?status=completed");
        }
        else {
            loadTblOrders("GetAllOrders?status=all");
        }
    }
});

function loadTblOrders(url) {
    datatable = $('#tblDatos').DataTable({
        "language": {
            "lengthMenu": "Mostrar _MENU_ Registros Por Pagina",
            "zeroRecords": "Ningun Registro",
            "info": "Mostrar page _PAGE_ de _PAGES_",
            "infoEmpty": "no hay registros",
            "infoFiltered": "(filtered from _MAX_ total registros)",
            "search": "Buscar",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "ajax": {
            "url": "/Admin/Order/" + url
        },
        "columns": [
            { "data": "idOrder" },
            { "data": "clientName" },
            { "data": "phoneNumber" },
            { "data": "users.email" },
            { "data": "orderState" },
            {
                "data": "totalOrderAmount", "className": "text-end",
                "render": function (data) {
                    var formattedCurrency = data.toLocaleString("en-US", { style: "currency", currency: "USD" });
                    return formattedCurrency;
                }
            },
            {
                "data": "idOrder",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/Order/Details/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                <i class="bi bi-ticket-detailed"></i>
                            </a>                           
                        </div>
                        `;
                }
            }
        ]
    });
}

$(function () {

    $("#btnSendOrder").on("click", function () {
        let carrier = $("#carrierId").val();
        let trackingNumber = $("#trackingNumberId").val();
        if (carrier == '') {
            Swal.fire({
                title: "Carrier es un campo requerido",
                text: "No puede dejar en blanco el detalle del transportista",
                icon: "error"
            });
            $("#carrierId").focus();
            return false;
        }
        if (trackingNumber == '') {
            Swal.fire({
                title: "Numero de Envio es un campo requerido",
                text: "No puede dejar en blanco el numero de envio",
                icon: "error"
            });
            $("#trackingNumberId").focus();
            return false;
        }
    });
});