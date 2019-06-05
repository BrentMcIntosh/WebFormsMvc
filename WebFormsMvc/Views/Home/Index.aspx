<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<WebFormsMvc.Models.AgGrid.AgGridModel>" %>

<!DOCTYPE html>

<html>
<head>
    <title>Test Page</title>
    
    <script type="text/javascript" src="https://unpkg.com/ag-grid-enterprise@20.2.0/dist/ag-grid-enterprise.js"></script>
    
    </head>
<body>

<div id="myGrid" style="height: 800px; width: 100%;" class="ag-theme-balham"></div>

<script>
    var options = <% = Model.ToJsonCamel() %>;

    var gridOptions = {

        columnDefs: options.columns,

        rowModelType: 'serverSide',

        pagination: true,

        paginationPageSize: 20
    };

    function ServerSideDataSource() {}

    agGrid.LicenseManager.setLicenseKey("_Not_For_Production_Valid_Until_1_July_2019__MTU2MTkzNTYwMDAwMA==8ea846c82b3280fc8df9f911fc6de1b9");

    ServerSideDataSource.prototype.getRows = function (params) {

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
        
        gridOptions.api.setServerSideDatasource(new ServerSideDataSource());
    });

</script>

</body>
</html>
