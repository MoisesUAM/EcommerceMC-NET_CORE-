$(function () {

    validateForm();
    fillSelect2();
    $("#btnShowHideForm").on("click", function () {

        let formContainer = $("#formContainer").html();
        if (formContainer == "") {
            let template = `                  <form method="post">
                        <div class="mb-3 gap-3">
                            <div class="row">
                                <div class="col-6">
                                    <label for="startDate" class="form-label">Fecha Inicial</label>
                                    <input id="startDate" name="startDate" type="date" class="form-control"/>
                                </div>
                                <div class="col-6">
                                    <label for="endDate" class="form-label">Fecha Final</label>
                                    <input id="endDate" name="endDate" type="date" class="form-control"/>
                                </div>
                            </div>
                        </div>
                        <hr class="text-danger"/>
                        <div class="mb-3">
                            <label for="productList" class="form-label">Seleccione un producto</label>
                            <select id="productList" name="productList" class="form-select">
                            </select>
                        </div>
                        <hr class="text-danger" />
                        <div class="d-flex justify-content-start mb-3 gap-3">
                            <input id="btnGenerate" type="submit" value="Generar Reporte" class="form-control btn btn-outline-info" style="width:200px;" />
                            <a asp-action="Index" class="btn btn-outline-dark" style="width:200px;">Regresar</a>
                        </div>
                    </form>
            `;
            $("#formContainer").html(template);
            $("#btnShowHideForm").text("Ocultar Formulario");
            validateForm();
            fillSelect2();
  
        } else {

            let template = "";
            $("#formContainer").html(template);
            $("#btnShowHideForm").text("Mostrar Formulario");
        }
    });
});

function validateForm() {
    $("#btnGenerate").on("click", function () {
        let starDate = $("#startDate").val();
        let endDate = $("#endDate").val();
        let productId = document.getElementById("productList").value;

        if (starDate.toString() == '') {
            Swal.fire({
                title: "Alerta campo vacio!",
                text: "Debe seleccionar una fecha de inicio!",
                icon: "warning"
            });

            return false;
        }
        if (endDate.toString() == '') {
            Swal.fire({
                title: "Alerta campo vacio!",
                text: "Debe seleccionar una fecha final!",
                icon: "warning"
            });

            return false;
        }
        if (productId == '') {
            Swal.fire({
                title: "Alerta campo vacio!",
                text: "Debe seleccionar un producto",
                icon: "warning"
            });

            return false;
        }

    });
}

function fillSelect2() {
    $("#productList").select2({
        placeholder: "Seleccionar Producto",
        allowClear: true,
        theme: "bootstrap-5",
        ajax: {
            url: "/Inventory/Inventory/FindProductByTerm",
            contentType: "application/json; charset=utf-8",
            data: function (params) {
                var query = { term: params.term };
                return query;
            },
            processResults: function (result) {
                return {
                    results: $.map(result, function (item) {
                        return {
                            id: item.idProduct,
                            text: item.serialNumber + "  " + item.description
                        };
                    })
                };
            }
        }
    });
}