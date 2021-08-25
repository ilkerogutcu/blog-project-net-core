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
                        url: 'Category/GetAll',
                        contentType: "application/json",
                        beforeSend: function () {
                            $('#categoriesTable').hide();
                            $('.spinner-border').show();
                        },
                        success: function (data) {
                            let categoryList = jQuery.parseJSON(data);
                            console.log(categoryList)

                            let tableBody = "";
                            $.each(categoryList.Data.$values,
                                function (index, category) {
                                    if (category.LastModifiedDate == null && category.LastModifiedBy == null) {
                                        category.LastModifiedDate = "Category has not been updated.";
                                        category.LastModifiedBy = "Category has not been updated.";
                                    }
                                    if (category.Status) {
                                        category.Status = "Active";
                                    } else {
                                        category.Status = "Not active";
                                    }
                                    category.CreatedDate = new Date(category.CreatedDate).toLocaleString()
                                    tableBody += `
                        <tr>
                            <td>${category.CategoryId}</td>
                            <td><img src="${category.ImageUrl}" alt="${category.CategoryName}" class="my-image-table"/></td>
                            <td>${category.CategoryName}</td>
                            <td>${category.Description}</td>
                            <td>${category.Status}</td>
                            <td>${category.CreatedBy}</td>
                            <td>${category.CreatedDate}</td>
                            <td>${category.LastModifiedBy}</td>
                            <td>${category.LastModifiedDate}</td>
                             <td>
                            <button class="btn btn-primary btn-sm btn-update" data-id="${category.CategoryId}"><span class="fas fa-edit"></span> Update</button>
                            <button class="btn btn-danger btn-sm btn-delete" data-id="${category.CategoryName}" style="margin-top: 7px;"><span class="fas fa-minus-circle"></span> Delete</button>
                            </td>
                        </tr>`;
                                });

                            $('#categoriesTable > tbody').replaceWith(tableBody);
                            $('.spinner-border').hide();
                            $('#categoriesTable').fadeIn(1400);
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
            dataToSend.append("File", files[0]);
            $.ajax({
                url: actionUrl,
                type: 'POST',
                data: dataToSend,
                processData: false,
                contentType: false,
                success: function (data) {
                    const categoryAddAjaxModel = jQuery.parseJSON(JSON.stringify(data));
                    let category = {
                        id: categoryAddAjaxModel.data.category.categoryId,
                        name: categoryAddAjaxModel.data.category.categoryName,
                        description: categoryAddAjaxModel.data.category.description,
                        imageUrl: categoryAddAjaxModel.data.category.imageUrl,
                        status: categoryAddAjaxModel.data.category.status,
                        createdBy: categoryAddAjaxModel.data.category.createdBy,
                        createdDate: categoryAddAjaxModel.data.category.createdDate,
                        lastModifiedDate: categoryAddAjaxModel.data.category.lastModifiedDate,
                        lastModifiedBy: categoryAddAjaxModel.data.category.lastModifiedBy
                    }
                    if (category.lastModifiedDate == null && category.lastModifiedBy == null) {
                        category.lastModifiedDate = "Category has not been updated.";
                        category.lastModifiedBy = "Category has not been updated.";
                    }
                    if (category.status) {
                        category.status = "Active";
                    } else {
                        category.status = "Not active";
                    }
                    const newFormBody = $('.modal-body', categoryAddAjaxModel.data.AddCategoryPartial);
                    placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                    placeHolderDiv.find('.modal').modal('hide');
                    const newTableRow = dataTable.row.add([
                        category.id,
                        `<img src="${category.imageUrl}" alt="${category.categoryName}" class="my-image-table" />`,
                        category.name,
                        category.description,
                        category.status,
                        category.createdBy,
                        `${new Date(category.createdDate).toLocaleString()}`,
                        category.lastModifiedDate,
                        category.lastModifiedBy,
                        '<button class="btn btn-primary btn-sm btn-update" data-id="${category.categoryName}"><span class="fas fa-edit"></span> Update</button> <button class="btn btn-danger btn-sm btn-delete" data-id="${category.categoryName}" style="margin-top: 7px;"><span class="fas fa-minus-circle"></span> Delete</button>'
                    ]).node();
                    dataTable.row(newTableRow).draw();
                    placeHolderDiv.find(".modal").modal('toggle');
                },
                error: function (err) {
                    toastr.error("Error", err);
                }
            })
        })
    });
    $(document).on('click', '.btn-delete', function (event) {
        event.preventDefault();
        const categoryName = $(this).attr('data-id');
        Swal.fire({
            title: 'Are you sure you want to delete this category?',
            text: "Selected category will be deleted.",
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
                        CategoryName: categoryName
                    },
                    url: 'Category/Delete',
                    success: function (data) {
                        const result = jQuery.parseJSON(data);
                        if (result.Success) {
                            Swal.fire(
                                'Deleted!',
                                'Category has been deleted.',
                                'success'
                            )
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
        const url = 'Category/Update';
        const placeHolderDiv = $('#modalPlaceHolder');
        $(document).on('click',
            '.btn-update',
            function (event) {
                event.preventDefault();
                const id = $(this).attr('data-id');
                $.get(url, {categoryId: id}).done(function (data) {
                    placeHolderDiv.html(data);
                    placeHolderDiv.find('.modal').modal('show');
                }).fail(function () {
                    toastr.error("Error")
                });
            });
        placeHolderDiv.on('click', '#btnUpdate', function (event) {
            event.preventDefault();
            const form = $('#form-update-category');
            const actionUrl = form.attr('action');
            const dataToSend = form.serialize();
            $.post(actionUrl, dataToSend).done(function (data) {
                const categoryUpdateAjaxModel = jQuery.parseJSON(JSON.stringify(data));
                console.log(categoryUpdateAjaxModel);
                let category = {
                    id: categoryUpdateAjaxModel.data.categoryDto.categoryId,
                    name: categoryUpdateAjaxModel.data.categoryDto.categoryName,
                    description: categoryUpdateAjaxModel.data.categoryDto.description,
                    imageUrl: categoryUpdateAjaxModel.data.categoryDto.imageUrl,
                    status: categoryUpdateAjaxModel.data.categoryDto.status,
                    createdBy: categoryUpdateAjaxModel.data.categoryDto.createdBy,
                    createdDate: categoryUpdateAjaxModel.data.categoryDto.createdDate,
                    lastModifiedDate: categoryUpdateAjaxModel.data.categoryDto.lastModifiedDate,
                    lastModifiedBy: categoryUpdateAjaxModel.data.categoryDto.lastModifiedBy
                }
                const newFormBody = $('.modal-body', categoryUpdateAjaxModel.CategoryUpdatePartial);
                placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                if (category.LastModifiedDate == null && category.LastModifiedBy == null) {
                    category.LastModifiedDate = "Category has not been updated.";
                    category.LastModifiedBy = "Category has not been updated.";
                }
                if (category.Status) {
                    category.Status = "Active";
                } else {
                    category.Status = "Not active";
                }
                category.CreatedDate = new Date(category.CreatedDate).toLocaleString()
                const newTableRow = `
                        <tr name="${category.CategoryId}">
                            <td>${category.CategoryId}</td>
                            <td><img src="${category.ImageUrl}" alt="${category.CategoryName}" class="my-image-table"/></td>
                            <td>${category.CategoryName}</td>
                            <td>${category.Description}</td>
                            <td>${category.Status}</td>
                            <td>${category.CreatedBy}</td>
                            <td>${category.CreatedDate}</td>
                            <td>${category.LastModifiedBy}</td>
                            <td>${category.LastModifiedDate}</td>
                             <td>
                            <button class="btn btn-primary btn-sm btn-update" data-id="${category.CategoryId}"><span class="fas fa-edit"></span> Update</button>
                            <button class="btn btn-danger btn-sm btn-delete" data-id="${category.CategoryName}" style="margin-top: 7px;"><span class="fas fa-minus-circle"></span> Delete</button>
                            </td>
                        </tr>`;
                const newTableRowObject = $(newFormBody);
                const categoryTableRow = $(`[name="${category.CategoryId}"]`)
                newTableRowObject.hide();
                categoryTableRow.replaceWith(newTableRowObject);
                newTableRowObject.fadeIn(3500)
                toastr.success("Successfull", "Category updated successfully")
                placeHolderDiv.find('.modal').modal('toggle');
            });
        });
    })
});
