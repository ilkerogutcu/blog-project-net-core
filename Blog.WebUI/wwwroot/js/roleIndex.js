$(document).ready(function () {
    let dataTable = $('#rolesTable').DataTable({
        dom: "<'row'<'col-sm-3'l><'col-sm-6 text-center'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [{
            text: 'Add',
            attr: {
                id: "btnAdd",
            },
            className: 'btn btn-success',
            action: function (e, dt, node, config) {
            }
        },
            {
                text: 'Get All',
                className: 'btn btn-warning',
                action: function (e, dt, node, config) {
                    e.preventDefault();
                    $.ajax({
                        type: 'GET',
                        url: 'Role/GetAll',
                        contentType: "application/json",
                        beforeSend: function () {
                            $('#rolesTable').hide();
                            $('.spinner-border').show();
                        },
                        success: function (data) {
                            let roleList = jQuery.parseJSON(data);
                            dataTable.clear()

                            $.each(roleList.$values,
                                function (index, role) {
                                    dataTable.row.add([
                                        role.Id,
                                        role.Name,
                                        `<td style="display: flex;">
                            <button class="btn btn-primary btn-sm btn-update" data-id="${role.Name}"><span class="fas fa-edit"></span> Update</button>
                            <button class="btn btn-danger btn-sm btn-delete" data-id="${role.Name}" style="margin-left: 15px;"><span class="fas fa-minus-circle"></span> Delete</button>
                            </td>`
                                    ]);
                                });
                            dataTable.draw();
                            $('.spinner-border').hide();
                            $('#rolesTable').fadeIn(1400);
                            toastr.success('Successfull');
                        },
                        error: function (err) {
                            toastr.error("Fetch data failed");
                        }
                    });
                    e.preventDefault();
                }
            },
        ]
    });

    $(function () {
        const url = 'Role/Create';
        const placeHolderDiv = $('#modalPlaceHolder');
        $('#btnAdd').click(function () {
            $.get(url).done(function (data) {
                placeHolderDiv.html(data);
                placeHolderDiv.find(".modal").modal('show');
            });
        });

        placeHolderDiv.on('click', '#btnSave', function () {
            event.preventDefault();
            const form = $('#form-create-role');
            const actionUrl = form.attr('action');
            const dataToSend = new FormData(form.get(0));
            $.ajax({
                url: actionUrl,
                type: 'POST',
                data: dataToSend,
                processData: false,
                contentType: false,
                success: function (data) {
                    const createRoleAjaxModel = jQuery.parseJSON(data);
                    const newFormBody = $('.modal-body', createRoleAjaxModel.CreateRolePartial);
                    placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                    const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                    if (isValid) {
                        let role = {
                            id: createRoleAjaxModel.RoleDto.Id,
                            name: createRoleAjaxModel.RoleDto.Name
                        }
                        placeHolderDiv.find('.modal').modal('hide');

                        placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                        placeHolderDiv.find('.modal').modal('hide');
                        const newTableRow = dataTable.row.add([
                            role.id,
                            role.name,
                            `<td>
                                <button class="btn btn-primary btn-sm btn-update" data-id="${role.name}"><span class="fas fa-edit"></span> Update</button> 
                                <button class="btn btn-danger btn-sm btn-delete" data-id="${role.name}" style="margin-top: 7px;"><span class="fas fa-minus-circle"></span> Delete</button>
                            </td>`

                        ]).node();
                        dataTable.row(newTableRow).draw();
                        placeHolderDiv.find(".modal").modal('toggle');
                        toastr.success("Success", createRoleAjaxModel.Message)
                    }
                    toastr.error("Error", "Form is not valid ");
                },
                error: function (err) {
                    toastr.error("Error", err);
                }
            })
        })
    });
    // $(document).on('click', '.btn-delete', function(event) {
    //     event.preventDefault();
    //     const userId = $(this).attr('data-id');
    //     Swal.fire({
    //         title: 'Are you sure you want to delete this user?',
    //         text: `${userId} will be deleted.`,
    //         icon: 'warning',
    //         showCancelButton: true,
    //         confirmButtonColor: '#3085d6',
    //         cancelButtonColor: '#d33',
    //         confirmButtonText: 'Yes, delete it!',
    //         cancelButtonText: 'No, I dont want it.'
    //     }).then((result) => {
    //         if (result.isConfirmed) {
    //             $.ajax({
    //                 type: 'POST',
    //                 dataType: 'json',
    //                 data: {
    //                     id: userId
    //                 },
    //                 url: 'Account/Delete',
    //                 success: function(data) {
    //                     const result = jQuery.parseJSON(data);
    //                     if (result.Success) {
    //                         Swal.fire(
    //                             'Deleted!',
    //                             `${userId} has been deleted.`,
    //                             'success'
    //                         )
    //                         const tableRow = $(`[name="${userId}]"`)
    //                         tableRow.fadeOut(3500);
    //                     }
    //                 },
    //                 error: function(err) {
    //                     console.log(err);
    //                 }
    //             })
    //         }
    //     })
    // })
    $(function () {
        const url = 'Role/Update';
        const placeHolderDiv = $('#modalPlaceHolder');
        $(document).on('click',
            '.btn-update',
            function (event) {
                event.preventDefault();
                const name = $(this).attr('data-id');
                console.log(name)
                $.get(url, {
                    name: name
                }).done(function (data) {
                    placeHolderDiv.html(data);
                    placeHolderDiv.find('.modal').modal('show');
                }).fail(function () {
                    toastr.error("Error")
                });
            });
        placeHolderDiv.on('click', '#btnUpdate', function (event) {
            event.preventDefault();
            const form = $('#form-update-role');
            const actionUrl = form.attr('action');
            const dataToSend = new FormData(form.get(0));
            $.ajax({
                url: actionUrl,
                type: 'POST',
                data: dataToSend,
                processData: false,
                contentType: false,
                success: function (data) {
                    const updateRoleAjaxModel = jQuery.parseJSON(data);

                    const newFormBody = $('.modal-body', updateRoleAjaxModel.UpdateRolePartial);
                    placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                    const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                    if (isValid) {
                        let role = {
                            id: updateRoleAjaxModel.RoleDto.Id,
                            name: updateRoleAjaxModel.RoleDto.Name
                        }
                        const tableRow = $(`[name="${role.id}"]`);

                        placeHolderDiv.find('.modal').modal('hide');

                        dataTable.row(tableRow).data([
                            role.id,
                            role.name,
                            ` <button class="btn btn-primary btn-sm btn-update" data-id="${role.name}"><span class="fas fa-edit"></span> Update</button>
                       <button class="btn btn-danger btn-sm btn-delete" data-id="${role.name}" style="margin-left: 15px;"><span class="fas fa-minus-circle"></span> Delete</button>`

                        ]);
                        tableRow.attr("name", `${role.id}`);
                        dataTable.row(tableRow).invalidate();
                        toastr.success("Successfully", "Role updated successfully.");
                    } else {
                        let summaryText = "";
                        $('#validation-summary > ul > li').each(function () {
                            let text = $(this).text();
                            summaryText = `*${text}\n`;
                        });
                        toastr.warning(summaryText);
                    }
                },
                error: function (error) {
                    console.log(error);
                }
            });
        });
    })
});