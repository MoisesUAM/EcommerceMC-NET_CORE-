
let tblInventory;
$(function () {
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
            "url": "/Inventory/Inventory/GetAllInventories"
        },
        "columns": [
            { "data": "serialNumber" },
            { "data": "description" },
            { "data": "category.name" },
            { "data": "brand.name" },
            {
                "data": "price",
                "className": "text-end",
                "render": function (data) {
                    //var formattedCurrency = data.toLocaleString("es-CR", { style: "currency", currency: "CRC" });
                    var formattedCurrency = data.toLocaleString("en-US", { style: "currency", currency: "USD" });
                    return formattedCurrency;
                }
            },
            {
                "data": "estate",
                "render": function (data) {
                    if (data) {
                        return "Activo";
                    } else {
                        return "Inactivo";
                    }
                }
            },
            {
                "data": "idProduct",
                "render": function (data) {
                    return `
                     <div class="d-flex justify-content-around">
                          <a href="/Admin/Product/Upsert/${data}" role="button" class="btn btn-outline-primary" title="Editar" style="cursor:pointer;"><i class="bi bi-pencil-square"></i></a>
                          <a onclick=Delete("/Admin/Product/Delete/${data}") role="button" class="btn btn-outline-danger" title="Eliminar" style="cursor:pointer;"><i class="bi bi-trash3-fill"></i></a>
                     </div>
                    `;
                }
            }
        ]

    });
}
