﻿/*if js is not working try hard reload  CTRL + SHIFT + R*/
var dataTable;

$(document).ready(function () {
    loadDataTable();
});

/*To get the Api control add /getall to the url*/
function loadDataTable() {   /* details for api*/
    dataTable = $('#tblData').DataTable({ 
        "ajax": { url: '/admin/product/getall' },
        "columns": [
            { data: 'title', "width": "10%" },
            { data: 'isbn', "width": "15%" },
            { data: 'author', "width": "6%" },
            { data: 'listPrice', "width": "10%" },
            { data: 'category.name', "width": "15%" },
            {
                data: 'id', "render": function (data) {
                    return `<div class="text-center" role="group">
                                <a href="/Admin/Product/ProductUpsert?id=${data}" class="btn btn-outline-info mx-2"> <i class="bi bi-pencil-square"></i></a>
                                <a onClick=Delete('/Admin/Product/Delete?id=${data}') class="btn btn-outline-danger mx-2"> <i class="bi bi-trash3"></i></a>
                            </div>`
                }, "width": "15%"
            }
        ]
    });
}

/*for further explanation udemy section 6.103*/
function Delete(url)
{
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data)
                {
                    Swal.fire(
                        'Deleted!',
                        'Your file has been deleted.',
                        'success'
                    )
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}