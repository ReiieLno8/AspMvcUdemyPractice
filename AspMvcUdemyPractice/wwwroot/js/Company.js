var dataTable;

$(document).ready(function () {
    loadDataTable();
});

/*to check api data add "/getall" in the link*/
function loadDataTable() {   /* details for api*/
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/company/getall' },
        "columns": [
            { data: 'name', "width": "10%" },
            { data: 'streetAddress', "width": "15%" },
            { data: 'city', "width": "6%" },
            { data: 'state', "width": "10%" },
            { data: 'postal', "width": "10%" },
            { data: 'phoneNumber', "width": "15%" },
            {
                data: 'id', "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                                <a href="/Admin/Company/CompanyUpsert?id=${data}" class="btn btn-outline-info mx-2"> <i class="bi bi-pencil-square"></i></a>
                                <a onClick=Delete('/Admin/Company/Delete/${data}') class="btn btn-outline-danger mx-2"> <i class="bi bi-trash3"></i></a>
                            </div>`
                }, "width": "15%"
            }
        ]
    });
}

/*for further explanation udemy section 6.103*/
function Delete(url) {
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
                success: function (data) {
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