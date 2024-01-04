let categoryTbl;

$(document).ready(function () {
    loadCategoryTable();
});

function loadCategoryTable() {
    categoryTbl = $('#tblCategories').DataTable({
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
            "url": "/Admin/Category/GetAll"
        },
        "columns": [
            { "data": "name", "width": "20%" },
            { "data": "description", "width": "40%" },
            {
                "data": "estate",
                "render": function (data) {
                    if (data) {
                        return "Activo";
                    } else {
                        return "Inactivo";
                    }
                }, "width": "20%"
            },
            {
                "data": "idCategory",
                "render": function (data) {
                    return `
                     <div class="d-flex justify-content-around">
                          <a href="/Admin/Category/Upsert/${data}" role="button" class="btn btn-outline-primary" title="Editar" style="cursor:pointer;"><i class="bi bi-pencil-square"></i></a>
                          <a onclick=Delete("/Admin/Category/Delete/${data}") role="button" class="btn btn-outline-danger" title="Eliminar" style="cursor:pointer;"><i class="bi bi-trash3-fill"></i></a>
                     </div>
                    `;
                }, "width": "20%"
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
                        categoryTbl.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    })
}