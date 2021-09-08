$(document).ready(function () {
    let dataTable = $('#accountsTable').DataTable({
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
                        url: 'Account/GetAll',
                        contentType: "application/json",
                        beforeSend: function () {
                            $('#accountsTable').hide();
                            $('.spinner-border').show();
                        },
                        success: function (data) {
                            let userList = jQuery.parseJSON(data);
                            dataTable.clear()

                            $.each(userList.$values,
                                function (index, user) {
                                    let lastModifiedDate = new Date(user.LastModifiedDate);
                                    if (lastModifiedDate.getFullYear() < 1900) {
                                        user.LastModifiedDate = "User has not been updated.";
                                        user.LastModifiedBy = "User has not been updated.";
                                    } else {
                                        user.LastModifiedDate = new Date(user.LastModifiedDate).toLocaleString()
                                    }
                                    if (user.Status) {
                                        user.Status = "Active";
                                    } else {
                                        user.Status = "Not active";
                                    }
                                    user.CreatedDate = new Date(user.CreatedDate).toLocaleString()
                                    dataTable.row.add([
                                        user.Id,
                                        `<img src="${user.ImageUrl}" alt="${user.Username}" class="my-image-table"/>`,
                                        user.FirstName,
                                        user.LastName,
                                        user.Username,
                                        user.Bio,
                                        user.Email,
                                        user.EmailConfirmed,
                                        user.CreatedBy,
                                        user.CreatedDate,
                                        user.Status,
                                        user.LastModifiedBy,
                                        user.LastModifiedDate,
                                        `<td style="display: flex;">
                            <button class="btn btn-primary btn-sm btn-update" data-id="${user.Id}"><span class="fas fa-edit"></span> Update</button>
                            <button class="btn btn-danger btn-sm btn-delete" data-id="${user.Id}" style="margin-left: 15px;"><span class="fas fa-minus-circle"></span> Delete</button>
                            </td>`
                                    ]);
                                });
                            dataTable.draw();
                            $('.spinner-border').hide();
                            $('#accountsTable').fadeIn(1400);
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
        const url = 'Account/SignUpAdmin';
        const placeHolderDiv = $('#modalPlaceHolder');
        $('#btnAdd').click(function () {
            $.get(url).done(function (data) {
                placeHolderDiv.html(data);
                placeHolderDiv.find(".modal").modal('show');
            });
        });

        placeHolderDiv.on('click', '#btnSave', function () {
            event.preventDefault();
            const files = $('#uploadFile').prop("files");
            const form = $('#form-sign-up-admin');
            const actionUrl = form.attr('action');
            const dataToSend = new FormData(form.get(0));
            dataToSend.append("Image", files[0]);
            $.ajax({
                url: actionUrl,
                type: 'POST',
                data: dataToSend,
                processData: false,
                contentType: false,
                success: function (data) {
                    const addUserAjaxModel = jQuery.parseJSON(data);
                    const newFormBody = $('.modal-body', addUserAjaxModel.AddUserPartial);
                    placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                    const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                    if (isValid) {
                        let user = {
                            id: addUserAjaxModel.SignUpResponse.Id,
                            photo: addUserAjaxModel.SignUpResponse.ImageUrl,
                            firstName: addUserAjaxModel.SignUpRequest.FirstName,
                            lastName: addUserAjaxModel.SignUpRequest.LastName,
                            username: addUserAjaxModel.SignUpResponse.UserName,
                            bio: addUserAjaxModel.SignUpRequest.Bio,
                            email: addUserAjaxModel.SignUpResponse.Email,
                            emailConfirmed: "False",
                            createdBy: addUserAjaxModel.SignUpResponse.CreatedBy,
                            createdDate: addUserAjaxModel.SignUpResponse.CreatedDate,
                            status: "True",
                            lastModifiedDate: "User has not been updated.",
                            lastModifiedBy: "User has not been updated.",
                        }
                        placeHolderDiv.find('.modal').modal('hide');

                        placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                        placeHolderDiv.find('.modal').modal('hide');
                        const newTableRow = dataTable.row.add([
                            user.id,
                            `<img src="${user.photo}" alt="${user.username}" class="my-image-table" />`,
                            user.firstName,
                            user.lastName,
                            user.username,
                            user.bio.substring(0, 20),
                            user.email,
                            user.emailConfirmed,
                            user.createdBy,
                            `${new Date(user.createdDate).toLocaleString()}`,
                            user.status,
                            user.lastModifiedDate,
                            user.lastModifiedBy,
                            `<td>
                                <button class="btn btn-primary btn-sm btn-update" data-id="${user.Id}"><span class="fas fa-edit"></span> Update</button> 
                                <button class="btn btn-danger btn-sm btn-delete" data-id="${user.Id}" style="margin-top: 7px;"><span class="fas fa-minus-circle"></span> Delete</button>
                            </td>`

                        ]).node();
                        dataTable.row(newTableRow).draw();
                        placeHolderDiv.find(".modal").modal('toggle');
                        toastr.success("Success", addUserAjaxModel.Message)
                    }
                    toastr.error("Error", "Form is not valid ");

                },
                error: function (err) {
                    toastr.error("Error", err);
                }
            })
        })
    });
    $(document).on('click', '.btn-delete', function (event) {
        event.preventDefault();
        const userId = $(this).attr('data-id');
        Swal.fire({
            title: 'Are you sure you want to delete this user?',
            text: `${userId} will be deleted.`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!',
            cancelButtonText: 'No, I dont want it.'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    data: {
                        id: userId
                    },
                    url: 'Account/Delete',
                    success: function (data) {
                        const result = jQuery.parseJSON(data);
                        if (result.Success) {
                            Swal.fire(
                                'Deleted!',
                                `${userId} has been deleted.`,
                                'success'
                            )
                            const tableRow = $(`[name="${userId}]"`)
                            tableRow.fadeOut(3500);
                        }
                    },
                    error: function (err) {
                        console.log(err);
                    }
                })
            }
        })
    })
    $(function () {
        const url = 'Account/Update';
        const placeHolderDiv = $('#modalPlaceHolder');
        $(document).on('click',
            '.btn-update',
            function (event) {
                event.preventDefault();
                const id = $(this).attr('data-id');
                console.log(id)
                $.get(url, {
                    id: id
                }).done(function (data) {
                    placeHolderDiv.html(data);
                    placeHolderDiv.find('.modal').modal('show');
                }).fail(function () {
                    toastr.error("Error")
                });
            });
        placeHolderDiv.on('click', '#btnUpdate', function (event) {
            event.preventDefault();
            const form = $('#form-update-user');
            const files = $('#uploadFile').prop("files");
            const actionUrl = form.attr('action');
            const dataToSend = new FormData(form.get(0));
            dataToSend.append("Image", files[0]);
            $.ajax({
                url: actionUrl,
                type: 'POST',
                data: dataToSend,
                processData: false,
                contentType: false,
                success: function (data) {
                    const updateUserAjaxModel = jQuery.parseJSON(data);

                    const newFormBody = $('.modal-body', updateUserAjaxModel.UpdateUserPartial);
                    placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                    const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                    if (isValid) {
                        const id = updateUserAjaxModel.UserResponse.Id;
                        const tableRow = $(`[name="${id}"]`);
                        let user = {
                            id: updateUserAjaxModel.UserResponse.Id,
                            imageUrl: updateUserAjaxModel.UserResponse.ImageUrl,
                            firstName: updateUserAjaxModel.UserResponse.FirstName,
                            lastName: updateUserAjaxModel.UserResponse.LastName,
                            email: updateUserAjaxModel.UserResponse.Email,
                            bio: updateUserAjaxModel.UserResponse.Bio,
                            username: updateUserAjaxModel.UserResponse.Username,
                            status: updateUserAjaxModel.UserResponse.Status,
                            createdBy: updateUserAjaxModel.UserResponse.CreatedBy,
                            createdDate: updateUserAjaxModel.UserResponse.CreatedDate,
                            lastModifiedDate: updateUserAjaxModel.UserResponse.LastModifiedDate,
                            lastModifiedBy: updateUserAjaxModel.UserResponse.LastModifiedBy,
                            emailConfirmed: updateUserAjaxModel.UserResponse.EmailConfirmed
                        }
                        if (user.Status) {
                            user.Status = "Active";
                        } else {
                            user.Status = "Not active";
                        }
                        user.CreatedDate = new Date(user.CreatedDate).toLocaleString();
                        placeHolderDiv.find('.modal').modal('hide');

                        dataTable.row(tableRow).data([
                            user.id,
                            `<img src="${user.imageUrl}" alt="${user.username}" class="my-image-table"/>`,
                            user.firstName,
                            user.lastName,
                            user.username,
                            user.bio,
                            user.email,
                            user.emailConfirmed,
                            user.createdBy,
                            user.createdDate,
                            user.status,
                            user.lastModifiedBy,
                            user.lastModifiedDate,
                            ` <button class="btn btn-primary btn-sm btn-update" data-id="${user.id}"><span class="fas fa-edit"></span> Update</button>
                       <button class="btn btn-danger btn-sm btn-delete" data-id="${user.id}" style="margin-left: 15px;"><span class="fas fa-minus-circle"></span> Delete</button>`

                        ]);
                        tableRow.attr("name", `${id}`);
                        dataTable.row(tableRow).invalidate();
                        toastr.success("Successfully", "User updated successfully.");
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