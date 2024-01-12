
$('#multiselectRoles').bsMultiSelect({
    cssPatch: {
        choices: { columnCount: '3' },
    }
});

$('#multiselectRoles').on('change', function () {
    let myElementSelect = (this);
    let itemsSelected = [];
    let firtsOption = $('#multiselectRoles option#firtsSelect');
    
    $('#multiselectRoles option:selected').each(function () {
        if (!($(this).val() === "-Seleccione el o lo Roles-"))
        {
            itemsSelected.push($(this).val());
        }
    });

    if (itemsSelected.length == 0) {
        toastr.error('Debe seleccionar al menos un rol');
    }

    console.log(itemsSelected);
    console.log(itemsSelected.length);
    console.log(firtsOption.html())
});