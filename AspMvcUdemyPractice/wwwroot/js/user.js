/*if js is not working try hard reload  CTRL + SHIFT + R*/

var dataTable;

$(document).ready(function () {
    loadDataTable();
});

/*to check api data add "/getall" in the link*/
function loadDataTable() {   /* details for api*/
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/user/getall' },
        "columns": [
            { data: 'name', "width": "10%" },
            { data: 'email', "width": "15%" },
            { data: 'phoneNumber', "width": "6%" },
            { data: 'company.name', "width": "10%" },
            { data: 'role', "width": "5%" },
            {
                data: { id: 'id', lockoutEnd: 'lockoutEnd' }, "render": function (data)
                {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();

                    if (lockout > today)
                    {
                        return `<div class="text-center">
                                     <a onclick=LockUnlock('${data.id}') class="btn btn-outline-warning btn-sm" style="cursor:pointer;"> 
                                        <i class="bi bi-lock-fill"></i>
                                    Lock </a>
                                    <a href="/admin/user/rolemanagement?userId=${data.id}" class="btn btn-outline-info btn-sm" style="cursor:pointer;"> 
                                        <i class="bi bi-pencil-fill"></i>
                                    Permission </a>
                                </div>`
                    }
                    else
                    {
                        return `<div class="text-center" role="group">
                                    <a onclick=LockUnlock('${data.id}') class="btn btn-outline-info btn-sm" style="cursor:pointer;"> 
                                        <i class="bi bi-unlock-fill"></i>
                                    Unlock </a>
                                    <a href="/admin/user/rolemanagement?userId=${data.id}" class="btn btn-outline-info btn-sm" style="cursor:pointer;"> 
                                        <i class="bi bi-pencil-fill"></i>
                                    Permission </a>
                                </div>`
                    }
                }, "width": "15%"
            }
        ]
    });
}

function LockUnlock(id)
{
    $.ajax
        ({
            type: "POST",
            url: '/Admin/User/LockUnlock',
            data: JSON.stringify(id),
            contentType: "application/json",
            success: function (data)
            {
                if (data.success)
                {
                    toastr.success(data.message);
                    dataTable.ajax.reload();
                }
            }
        })
}