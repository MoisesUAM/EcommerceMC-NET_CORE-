
let tblInventory;
$(document).ready(function () {
    console.log("Inventios en linea");
    loadInventoryTable();
});

function loadInventoryTable() {
    tblInventory = $('#tblInventory').DataTable({
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
            "url": "/Inventory/Inventory/GetAllStoresProducts"
        },
        "columns": [
            { "data": "stores.name" },
            {
                "data": "products",
                "render": function (data) {
                    let productDetails = data.serialNumber + ' ' + data.description;
                    return productDetails;
                }
            },
            {
                "data": "products.costPrice",
                "className": "text-end",
                "render": function (data) {
                    var formattedCurrency = data.toLocaleString("en-US", { style: "currency", currency: "USD" });
                    return formattedCurrency;
                }
                
            },
            { "data": "onHand", "className":"text-center" }
        ]

    });
}
