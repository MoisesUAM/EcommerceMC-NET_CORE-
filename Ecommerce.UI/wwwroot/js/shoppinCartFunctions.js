$(function () {
    $("#btnCheckout").on("click", function () {
        let names = $("#namesId").val();
        let phone = $("#phoneId").val();
        let shippingAddress = $("#shippinAddressId").val();
        let city = $("#cityId").val();
        let country = $("#countryId").val();

        if (names == '') {
            $("#namesId").focus();
            emptyFielAlert("Nombres");
            return false;
        }
        if (phone == '') {
            $("#phoneId").focus();
            emptyFielAlert("Telefono");
            return false;
        }
        if (shippingAddress == '') {
            $("#shippinAddressId").focus();
            emptyFielAlert("Direccion");
            return false;
        }
        if (city == '') {
            $("#cityId").focus();
            emptyFielAlert("Ciudad");
            return false;
        }
        if (country == '') {
            $("#countryId").focus();
            emptyFielAlert("Pais");
            return false;
        }

    });
});

function emptyFielAlert(name) {
    Swal.fire({
        title: "Campo " + name + " requerido",
        text: "Debe llenar este dato para continuar",
        icon: "error"

    });
}