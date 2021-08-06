$(document).ready(function() {
    const dataTable = $('#categoriesTable').DataTable({
        dom: "<'row'<'col-sm-3'l><'col-sm-6 text-center'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [{
            text: 'Add',
            attr: {
                id: "btnAdd",
            },
            className: 'btn btn-success',
            action: function(e, dt, node, config) {
            }
        },
            {
                text: 'Refresh',
                className: 'btn btn-warning',
                action: function(e, dt, node, config) {
                    alert('Yenile butonuna basıldı.');
                }
            }
        ]
    });

    $(function() {
        const url = 'Category/Add';
        const placeHolderDiv = $('#modalPlaceHolder');
        $('#btnAdd').click(function() {
            $.get(url).done(function(data) {
                placeHolderDiv.html(data);
                placeHolderDiv.find(".modal").modal('show');
            });
        });

        placeHolderDiv.on('click', '#btnSave', function() {
            event.preventDefault();
            const files = $('#uploadFile').prop("files");
            const form = $('#form-create-category');
            const actionUrl = form.attr('action');
            const dataToSend = new FormData(form.get(0));
            dataToSend.append("File", files[0]);
            $.ajax({
                url: actionUrl,
                type: 'POST',
                data: dataToSend,
                processData: false,
                contentType: false,
                success: function(data) {
                    const categoryAddAjaxModel = jQuery.parseJSON(JSON.stringify(data));
                    if( categoryAddAjaxModel.data.category.lastModifiedDate==null && categoryAddAjaxModel.data.category.lastModifiedBy==null)
                    {
                        categoryAddAjaxModel.data.category.lastModifiedDate="Category has not been updated.";
                        categoryAddAjaxModel.data.category.lastModifiedBy = "Category has not been updated.";

                    }
                    const newFormBody = $('.modal-body', categoryAddAjaxModel.data.AddCategoryPartial);
                    placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                    placeHolderDiv.find('.modal').modal('hide');
                    console.log(categoryAddAjaxModel.data.category);
                    const newTableRow = dataTable.row.add([
                        `<img src="${categoryAddAjaxModel.data.category.imageUrl}" alt="${categoryAddAjaxModel.data.category.categoryName}" class="my-image-table" />`,
                        categoryAddAjaxModel.data.category.categoryName,
                        categoryAddAjaxModel.data.category.description,
                        categoryAddAjaxModel.data.category.status,
                        categoryAddAjaxModel.data.category.createdBy,
                        `${new Date(categoryAddAjaxModel.data.category.createdDate).toLocaleString()}`,
                        categoryAddAjaxModel.data.category.lastModifiedDate,
                        categoryAddAjaxModel.data.category.lastModifiedBy,
                    ]).node();
                    dataTable.row(newTableRow).draw();
                    toastr.success('Successfully', 'Added successfully');
                },
                error: function(err) {
                    console.log(err)
                    let summaryText = "";
                    $('#validation-summary > ul > li ').each(function() {
                        let text = $(this).text();
                        summaryText = `#${text}\n`;
                    });
                    toastr.error(categoryAddAjaxModel.message);
                }
            })
        })
    });
});