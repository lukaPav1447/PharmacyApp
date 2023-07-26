$(document).ready(function () {
    $("#recipeTable").DataTable({

        "responsive": true,
        "bRetrieve": true,
        "serverSide": true,
        "bAutoWidth": true,
        "ordering": true,
        "bDestroy": true,

        "ajax": {
            "url": "/Recipetype/GetRecipeTypes",
            "type": "POST",
            "datatype": "json"
        },

        "columnDefs":
            [{
                "targets": [0],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [1],
                "searchable": false,
                "orderable": false
            },
            {
                "targets": [2],
                "searchable": false,
                "orderable": false
            },
            {
                "targets": [3],
                "searchable": false,
                "orderable": false
            }],

        "columns": [
            { "data": "recipetypeid", "name": "Recipetypeid", "autoWidth": true },
            { "data": "name", "name": "name", "autoWidth": true },
            { "data": "pricemodifier", "title": "pricemodifier", "name": "pricemodifier", "autoWidth": true },

            {
                "render": function (data, type, full, meta) { return '<a class="btn btn-info" href="/Demo/Edit/' + full.CustomerID + '">Edit</a>'; }
            },
            //{
            //    data: null, render: function (data, type, row) {
            //        return "<a href='#' class='btn btn-danger' onclick=DeleteData('" + row.CustomerID + "'); >Delete</a>";
            //    }
            //},

        ]


    });
});  