$(document).ready(function () {
    rechargePage();
});


function rechargePage() {
    $('#itemPerPage').on('change', function () {
        let selectedValue = $(this).val();
        console.log(selectedValue);
        let url = '/?itemsPerPage=' + selectedValue + '&pageNumber=1';
        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                var productContent = $(data).find('#productsContainer').html();
                $('#productsContainer').html(productContent);
                $('#itemPerPage').val(selectedValue);
            },
            error: function () {
                console.log('Sucedio un error');
            }
        });
    });

}

