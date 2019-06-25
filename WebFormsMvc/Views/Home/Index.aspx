<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<WebFormsMvc.Models.UploaderViewModel>" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>

<html>

<head>

    <title>File Upload Test</title>
    <link rel="stylesheet" href="../../Content/CSS/FA5/css/fontawesome-all.min.css">
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css">
    <link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/css/toastr.min.css">
    <link rel="stylesheet" href="../../Content/CSS/sentry_client.css">


</head>

<body>
   
    <% Html.RenderPartial("Uploader", Model); %>

    <script src="//code.jquery.com/jquery-3.3.1.min.js"></script>
    <script src="//cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>
    <script src="//stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
    <script src="//cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js"></script>
    <script src="../../Content/Scripts/dropzone.js"></script>
    <script src="../../Content/Scripts/uploader.js"></script>
    <script type="text/javascript">

        var Uploader = Uploader || {};
        
        Uploader.successActions = Uploader.successActions || [];

        Uploader.successActions.push({
            key: 1,
            selector: '1',
            action: 'modal',
            parameters: ['hide']
        });

        Uploader.successActions.push({
            key: 2,
            selector: '[data-target="2"]',
            action: 'addClass',
            parameters: ['hidden']
        });

        Uploader.successActions.push({
            key: 3,
            selector: '[requireddocumentidlink="3"] a',
            action: 'attr',
            parameters: ['href', '{{FileUrl}}']
        });

        Uploader.successActions.push({
            key: 4,
            selector: '[requireddocumentidlink="4"]',
            action: 'removeClass',
            parameters: ['hidden']
        });

        Uploader.successActions.push({
            key: 5,
            selector: '[requireddocumentid="5"]',
            action: 'html',
            parameters: ['<% = DateTime.Now.ToShortDateString() %>']
        });

    </script>

</body>

</html>