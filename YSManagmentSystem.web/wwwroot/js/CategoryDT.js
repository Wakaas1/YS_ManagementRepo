﻿$(document).ready(function () {
    $('#myTableCategory').DataTable(
        {
            "Processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true, // this is for disable filter (search box)
            "orderMulti": true, // for disable multiple column at once
            "bDestroy": true,
            "ajax": {
                "url": "/Category/GetAllCategory",
                "type": "POST",
                "datatype": "json"
            },
            "columnDefs": [{
                "order": [[0]],
                "targets": [0],
                "visible": false,
                "searchable": false,
                "language": {
                    "emptyTable": "No record found. Please click on Add User for create new record.",
                    "Processing": '<i class="fa fa- refresh fa- spin">Laoding...</i>'
                },
            }],
            columns: [
                { data: "id" },
                { data: "categoryName" },
                { data: "description" },
                
                {
                    data: "id", "render": function (data, type, row) {
                        return '<button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#exampleModal" href="" data-url="/Category/AddOrEditCategory?id=' + data + '" > <i class="fa fa-edit"></i></button>  <a href="#" class="btn btn-danger" onclick=DelProduct("' + data + '")><i class="fa fa-trash"></i></a>'
                    },
                    "Width": "autowidth"
                }
            ],
        });
});

$('#exampleModal').on('shown.bs.modal', function (event) {
    $('#catPopup').html();
    var url = '';
    url = event.relatedTarget.getAttribute('data-url');
    $.get(url)
        .done(function (response) {
            $('#catPopup').html(response);
        });
});

if ($.validator) {
    $.validator.setDefaults({
        ignore: [] // DON'T IGNORE PLUGIN HIDDEN SELECTS, CHECKBOXES AND RADIOS!!!
        ,
        highlight: function (element, errorClass) {
            $(element).removeClass(errorClass);
            $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
        },
        unhighlight: function (element, errorClass) {
            $(element).closest('.form-group').addClass('has-success').removeClass('has-error');
        },
        errorPlacement: function (error, element) {
            if (element.data().chosen) {
                {
                    element.next().after(error);
                }
            } else {
                if (element.parent().hasClass("input-group")) {
                    element.parent().next().after(error);
                } else {
                    element.after(error);
                }
            }
        }
    });

}
function validateEmpForm() {
    $('#step-form-horizontal').validate({
        rules: {
            name: { required: true },
            email: { required: true },
            roles_list: { required: true },


        },
        messages: {
            name: { required: 'Please provide User Name' },
            email: { required: 'Please provide User Email' },
            roles_list: { required: 'Roles assigned To User' },


        },
        submitHandler: function (form) {
            var form = $('#step-form-horizontal')[0];
            var formData = new FormData(form);
            $.ajax({
                url: form.action,
                data: formData,
                type: 'POST',
                contentType: false, // NEEDED, DON'T OMIT THIS (requires jQuery 1.6+)
                processData: false, // NEEDED, DON'T OMIT THIS
                success: function (response) {
                    if (response.status == 200) {
                        $("#exampleModal").modal("hide");
                        $.notify({
                            // options
                            title: '<strong>Success</strong>',
                            message: "<br>Record has been saved successfully",
                            icon: 'glyphicon glyphicon-ok'
                        },
                            {
                                // settings
                                element: 'body',
                                //position: null,
                                type: "success",
                                //allow_dismiss: true,
                                //newest_on_top: false,
                                showProgressbar: false,
                                placement: {
                                    from: "top",
                                    align: "center"
                                },
                                offset: 20,
                                spacing: 10,
                                z_index: 1051,
                                delay: 3300,
                                timer: 1000,

                            });
                        $('#myTableCategory').DataTable().ajax.reload();
                        //notify("saved", "App Design Pattern Mapping Updated Successfully !");
                    }
                    else {
                        $.notify({
                            // options
                            title: '<strong>Error</strong>',
                            message: "<br>There was error an occured.",
                            icon: 'glyphicon glyphicon-remove-sign',

                        }, {
                            // settings
                            element: 'body',
                            //position: null,
                            type: "danger",
                            //allow_dismiss: true,
                            //newest_on_top: false,
                            showProgressbar: false,
                            placement: {
                                from: "top",
                                align: "center"
                            },
                            offset: 20,
                            spacing: 10,
                            z_index: 1051,
                            delay: 3300,
                            timer: 1000,

                        });
                    }
                }, error: function () {
                }
            });
            return false;
        }
    });
}

function DelProduct(id) {
    if (confirm("Are you sure you want to delete ...?")) {
        Delete(id);
    } else {
        return false;
    }
}
function Delete(id) {
    var url = '@Url.Content("~/")' + "Product/DeleteProduct";

    $.post(url, { ID: id }, function (data) {
        if (data) {
            oTable = $('#myTableCategory').DataTable();
            oTable.draw();
        } else {
            alert("Something Went Wrong!");
        }
    });
}