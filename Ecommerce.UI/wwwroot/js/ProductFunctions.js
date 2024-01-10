﻿

let productTbl;

$(document).ready(function () {
    loadBrandTable();
});

function loadBrandTable() {
    productTbl = $('#tblProducts').DataTable({
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
            "url": "/Admin/Product/GetAll"
        },
        "columns": [
            { "data": "serialNumber" },
            { "data": "description"},
            { "data": "category.name"},
            { "data": "brand.name"},
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

function Delete(url) {
    Swal.fire({
        title: 'Esta Seguro de esta Accion?',
        text: "Este cambio es irreversible!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Si Elimnar!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        productTbl.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    })
}
