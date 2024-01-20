//Select2

$("#idProduct").select2({
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
                        text: item.serialNumber + "  " + item.description,
                    };
                }),
            };
        },
    },
});

$("#btnAgregar").on("click", function () {

    console.log("Click Recibido");

    let cantidad = $("#cantidadId").val();
    let producto = $("#idProduct").val();

    if (cantidad.toString() === "" || cantidad < 1) {
        Swal.fire({
            title: "Atencion!",
            text: "No selecciono un cantidad valida",
            icon: "error"
        });
        return false;
    }

    if (producto.toString() === "") {
        Swal.fire({
            title: "Atencion!",
            text: "No selecciono el producto",
            icon: "error"
        });

        return false;
    }
});