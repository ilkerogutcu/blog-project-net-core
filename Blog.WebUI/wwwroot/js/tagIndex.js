$(document).ready(function() {
    let dataTable = $('#tagsTable').DataTable({
        dom: "<'row'<'col-sm-3'l><'col-sm-6 text-center'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [{
            text: 'Add',
            attr: {
                id: "btnAdd",
            },
            className: 'btn btn-success',
            action: function(e, dt, node, config) {}
        }, ]
    });

    $(function() {
        const url = 'Tag/Create';
        const placeHolderDiv = $('#modalPlaceHolder');
        $('#btnAdd').click(function() {
            $.get(url).done(function(data) {
                placeHolderDiv.html(data);
                placeHolderDiv.find(".modal").modal('show');
            });
        });

        placeHolderDiv.on('click', '#btnSave', function() {
            event.preventDefault();
            const form = $('#form-create-tag');
            const actionUrl = form.attr('action');
            const dataToSend = new FormData(form.get(0));
            $.ajax({
                url: actionUrl,
                type: 'POST',
                data: dataToSend,
                processData: false,
                contentType: false,
                success: function(data) {
                    const createTagAjaxModel = jQuery.parseJSON(data);
                    const newFormBody = $('.modal-body', createTagAjaxModel.CreateTagPartial);
                    placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                    const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                    console.log(createTagAjaxModel.TagDto);
                    if (isValid) {
                        let tag = {
                            id: createTagAjaxModel.TagDto.Id,
                            name: createTagAjaxModel.TagDto.Name,
                            createdDate: createTagAjaxModel.TagDto.CreatedDate,
                            createdBy: createTagAjaxModel.TagDto.CreatedBy,
                            lastModifiedBy: createTagAjaxModel.TagDto.LastModifiedBy,
                            lastModifiedDate: createTagAjaxModel.TagDto.LastModifiedDate
                        };
                        placeHolderDiv.find('.modal').modal('hide');

                        placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                        placeHolderDiv.find('.modal').modal('hide');
                        const newTableRow = dataTable.row.add([
                            tag.id,
                            tag.name,
                            tag.createdBy,
                            tag.createdDate,
                            tag.lastModifiedBy,
                            tag.lastModifiedDate,
                            `<td>
                                <button class="btn btn-primary btn-sm btn-update" data-id="${tag.name}"><span class="fas fa-edit"></span> Update</button>  <button class="btn btn-danger btn-sm btn-delete" data-id="${tag.id}" style="margin-top: 7px;"><span class="fas fa-minus-circle"></span> Delete</button> 
                            </td>`
                        ]).node();
                        dataTable.row(newTableRow).draw();
                        placeHolderDiv.find(".modal").modal('toggle');
                        toastr.success("Success", createTagAjaxModel.Message)
                    } else {
                        let summaryText = "";
                        $('#validation-summary > ul > li').each(function() {
                            let text = $(this).text();
                            summaryText = `*${text}\n`;
                        });
                        toastr.warning(summaryText);
                    }
                },
                error: function(err) {
                    toastr.error("Error", err);
                }
            })
        })
    });
    $(document).on('click', '.btn-delete', function(event) {
        event.preventDefault();
        const tagName = $(this).attr('data-id');
        Swal.fire({
            title: 'Are you sure you want to delete this category?',
            text: `${tagName} will be deleted.`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!',
            cancelButtonText: 'No, I dont want it.'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: 'DELETE',
                    dataType: 'json',
                    data: {
                        Name: tagName
                    },
                    url: 'Tag/Delete',
                    success: function(data) {
                        const result = jQuery.parseJSON(data);
                        if (result.Success) {
                            Swal.fire(
                                'Deleted!',
                                `${tagName} has been deleted.`,
                                'success'
                            )
                            const tableRow = $(`[name="${tagName}]"`)
                            tableRow.fadeOut(3500);
                        } else {
                            toastr.warning(result.Message);
                        }
                    },
                    error: function(err) {
                        console.log(err);
                    }
                })
            }
        })
    })
    $(function() {
        const url = 'Tag/Update';
        const placeHolderDiv = $('#modalPlaceHolder');
        $(document).on('click',
            '.btn-update',
            function(event) {
                event.preventDefault();
                const id = $(this).attr('data-id');
                $.get(url, {
                    id: id
                }).done(function(data) {
                    placeHolderDiv.html(data);
                    placeHolderDiv.find('.modal').modal('show');
                }).fail(function() {
                    toastr.error("Error")
                });
            });
        placeHolderDiv.on('click', '#btnUpdate', function(event) {
            event.preventDefault();
            const form = $('#form-update-tag');
            const actionUrl = form.attr('action');
            const dataToSend = new FormData(form.get(0));
            console.log(dataToSend);
            $.ajax({
                url: actionUrl,
                type: 'POST',
                data: dataToSend,
                processData: false,
                contentType: false,
                success: function(data) {
                    const updateTagAjaxModel = jQuery.parseJSON(data);

                    const newFormBody = $('.modal-body', updateTagAjaxModel.UpdateTagPartial);
                    placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                    const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                    console.log(updateTagAjaxModel.TagDto);
                    if (isValid) {
                        let tag = {
                            name: updateTagAjaxModel.TagDto.Name
                        }
                        const tableRow = $(`[name="${tag.name}"]`);
                        placeHolderDiv.find('.modal').modal('hide');
                        dataTable.row(tableRow).data([
                            tag.id,
                            tag.name,
                            tag.createdBy,
                            tag.createdDate,
                            tag.lastModifiedBy,
                            tag.lastModifiedDate,
                            `<td>
                                <button class="btn btn-primary btn-sm btn-update" data-id="${tag.name}"><span class="fas fa-edit"></span> Update</button>  <button class="btn btn-danger btn-sm btn-delete" data-id="${tag.id}" style="margin-top: 7px;"><span class="fas fa-minus-circle"></span> Delete</button> 
                            </td>`
                        ]);
                        tableRow.attr("name", `${role.name}`);
                        dataTable.row(tableRow).invalidate();
                        toastr.success("Successfully", "Tag updated successfully.");
                    } else {
                        let summaryText = "";
                        $('#validation-summary > ul > li').each(function() {
                            let text = $(this).text();
                            summaryText = `*${text}\n`;
                        });
                        toastr.warning(summaryText);
                    }
                },
                error: function(error) {
                    console.log(error);
                }
            });
        });
    })
});