
$("#btnLines").on("click", function () {
    console.log("Click recibido");
    let container = "";
        container = `
        <div class="row">
           <h5 class="text-center text-info fw-bold">Detalle de productos</h5>
           <hr class="text-danger" />
           <div class="d-flex justify-content-evenly mb-3 gap-2">
               <div class="flex-grow-1 flex-lg-grow-1">
                     <div class="mb-3">
                        <select id="idProduct" name="idProduct" class="form-select">
                        </select>
                     </div>
                  </div>
                <div class="d-inline-flex gap-1">
              <div class="mb-3">
               <input style="max-width:105px;" id="cantidadId" name="cantidadId" type="number" placeholder="Cantidad" min="1" class="form-control-sm" />
             </div>
           <div class="mb-3">
            <button type="submit" class="btn btn-sm btn-primary" onfocus="false" id="btnAgregar"><i class="bi bi-bag-plus-fill"></i> Agregar producto</button>
            <button id="btnCancel" type="reset" class="btn btn-sm btn-dark" onfocus="false"><i class="bi bi-arrow-return-left"></i> Cancelar</button>
         </div>
        </div>
      </div>
   </div>

   <script>
   $("#btnAgregar").on("click", function () {
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
                            text: item.serialNumber + "  " + item.description
                        };
                    })
                };
            }
        }
});

$("#btnCancel").on("click", function () {
            
            $("#btnLines").attr("disabled", false);
            $("#detailsProductosForm").html("");
        });

</script>
        `;
$("#detailsProductosForm").html(container);
$("#btnLines").attr("disabled", true);

});



