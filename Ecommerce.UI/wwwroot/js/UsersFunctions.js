let tblUsers;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    tblUsers = $('#tblUsers').DataTable({
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
            "url": "/Admin/User/GetAllUsers"
        },
        "columns": [
            { "data": "email" },
            { "data": "name" },
            { "data": "lastName" },
            { "data": "phoneNumber" },
            {
                "data": "roles",
                "render": function (data) {
                    let items = "";

                    data.forEach(function (item) {
                        items += `
                                 <div class="card bg-primary bg-opacity-75 ms-1 me-1" style="min-width:110px;">
                                   <p class="card-text text-center text-light fw-bold">${item}</p>
                                  </div>
                        `;
                    });

                    var template = `
                     <div class="d-flex flex-row justify-content-center" style="min-width:fit-content">`+items+
                    `</div>`;

                    return template;
                   
                }
            },
            {
                "data": {
                    id: "id",
                    lockoutEnd: "lockoutEnd"

                },
                render: function (data) {
                    let currentTime = new Date().getTime();
                    let timeLockoutEnd = new Date(data.lockoutEnd).getTime();
                    let locked = (currentTime < timeLockoutEnd);
                    if (locked) {
                        return `
                              <div class="text-center text-danger">
                                 <a role="button" class="btn btn-danger text-white" onclick=LockUnlock("${data.id}") style="font-size:12px;">
                                   <i class="bi bi-lock-fill"></i> <span class="details-hidden">Desbloquear</span>
                                 </a>
                              </div>
                        `;
                    } else {
                        return `
                              <div class="text-center text-danger">
                                 <a role="button" class="btn btn-success text-white" onclick=LockUnlock("${data.id}") style="font-size:12px;">
                                   <i class="bi bi-unlock-fill"></i> <span class="details-hidden">Bloquear</span>
                                 </a>
                              </div>
                        `;
                    }

                }, width:"100"
            }
        ]
    });
}

function LockUnlock(id) {

    console.log(id);
            $.ajax({
                type: "POST",
                url: "/Admin/User/UserActionLock",
                data: JSON.stringify(id),
                contentType: "application/json",
                success: function (data) {
                    console.log(data);
                    if (data.success) {
                        toastr.success(data.message);
                        tblUsers.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                       // tblUsers.ajax.reload();

                    }
                }
            });
}