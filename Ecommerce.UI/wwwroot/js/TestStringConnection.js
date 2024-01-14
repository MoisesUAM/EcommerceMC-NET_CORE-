$(function () {

    console.log("Jquery on line");
    var currentURL = window.location.href;
    console.log("URL actual del cliente: " + currentURL);
});

$(function () {
    $('#formDataTest').on('submit', function (e) {
        e.preventDefault(); // Evita el envío predeterminado del formulario

        // Obtiene el valor del campo de entrada con el atributo "name" igual a "dataMessage"
        var urlClient = window.location.href;
        console.log("Mensaje ingresado por el usuario: " + urlClient);

        // Realiza una solicitud AJAX para enviar los datos al servidor
        $.ajax({
            type: "POST",
            url: "/Inventory/Test/DataTest",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(urlClient),
            success: function (response) {
                $('#alertMessage').html(response.message);
                $('#linkMessage').attr('href', response.message);
            },
            error: function () {
                toastr.error = "Error en metodo post Test revisar";
            }
        });
    });
});
