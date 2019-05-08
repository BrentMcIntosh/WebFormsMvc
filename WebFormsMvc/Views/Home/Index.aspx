<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<WebFormsMvc.Models.AgGrid.AgGridModel>" %>

<!DOCTYPE html>

<html>
<head>
    <title>Test Page</title>

    
    <script src="https://unpkg.com/ag-grid-enterprise@20.2.0/dist/ag-grid-enterprise.js"></script>
    
    <link rel="stylesheet" href="https://unpkg.com/ag-grid-enterprise@20.2.0/dist/styles/ag-grid.css">
    
    <link rel="stylesheet" href="https://unpkg.com/ag-grid-enterprise@20.2.0/dist/styles/ag-theme-balham.css">

    </head>
<body>

<div id="myGrid" style="height: 800px; width: 100%;" class="ag-theme-balham"></div>

<script>

    var renderSplitLink = function (params) {
        return '<a href="' + params.value + '">Split</a>';
    }

    var options = <% = Model.ToJsonCamel() %>;

    var actionColumn = {
        headerName: "Action",
        editable: false,
        field: "action",
        cellRenderer: renderSplitLink
    };

    var columnDefs = options.columns;

    columnDefs.push(actionColumn);

    var gridOptions = {
        defaultColDef: {
            sortable: true,
            resizable: true,
            filter: true
        },

        columnDefs: columnDefs,

        rowModelType: 'enterprise',

        enableSorting: true,
        
        enableFilter: true,
        
        pagination: true,

        paginationPageSize: 100
    };

    function EnterpriseDataSource() {}

    agGrid.LicenseManager.setLicenseKey("_Not_For_Production_Valid_Until_1_July_2019__MTU2MTkzNTYwMDAwMA==8ea846c82b3280fc8df9f911fc6de1b9");

    EnterpriseDataSource.prototype.getRows = function (params) {

        const jsonRequest = JSON.stringify(params.request, null, 2);
        
        console.log(jsonRequest);

        let httpRequest = new XMLHttpRequest();
        
        httpRequest.open('POST', options.callBackMethod);
        
        httpRequest.setRequestHeader("Content-type", "application/json");
        
        httpRequest.send(jsonRequest);
        
        httpRequest.onreadystatechange = () => {
        
            if (httpRequest.readyState === 4 && httpRequest.status === 200) {
            
                const result = JSON.parse(httpRequest.responseText);
                
                params.successCallback(result.data, result.lastRow);
            }
        };
    };

    document.addEventListener('DOMContentLoaded', function () {
        
        let gridDiv = document.querySelector('#myGrid');
        
        new agGrid.Grid(gridDiv, gridOptions);
        
        gridOptions.api.setEnterpriseDatasource(new EnterpriseDataSource());
    });

    function onPageSizeChanged(newPageSize) {
        
        var value = document.getElementById('page-size').value;
        
        gridOptions.api.paginationSetPageSize(Number(value));
    }

    function onGoToPage() {

        var page =  parseInt(document.getElementById("goToPage").value);

        gridOptions.api.paginationGoToPage(page);
    }

</script>

</body>
</html>
