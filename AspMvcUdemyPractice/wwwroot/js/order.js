/*if js is not working try hard reload  CTRL + SHIFT + R*/
var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    }
    else {
        if (url.includes("completed")) {
            loadDataTable("completed");
        }
        else {
            if (url.includes("pending")) {
                loadDataTable("pending");
            }
            else {
                if (url.includes("approved")) {
                    loadDataTable("approved");
                }
                else {
                    loadDataTable("all");
                }
            }
        }
    }
});

/*To get the Api control add /getall to the url*/
function loadDataTable(status) {   /* details for api*/
    dataTable = $('#tblData').DataTable({ 
        "ajax": { url: '/admin/order/getall?status='+status },
        "columns": [
            { data: 'id', "width": "5%" },
            { data: 'name', "width": "10%" },
            { data: 'phoneNumber', "width": "6%" },
            { data: 'applicationUser.email', "width": "15%" },
            { data: 'orderStatus', "width": "10%" },
            { data: 'orderTotal', "width": "10%" },
            {
                data: 'id', "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                                <a href="/Admin/order/details?id=${data}" class="btn btn-outline-info mx-2"> <i class="bi bi-pencil-square"></i></a>
                            </div>`
                }, "width": "10%"
            }
        ]
    });
}

