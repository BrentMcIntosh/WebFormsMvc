<%@ Page Language="C#" %>

<!DOCTYPE html>

<html>
<head>
<title>Dropzone!</title>
<%--<link rel="stylesheet" href="../../Content/CSS/basic.css" />
<link rel="stylesheet" href="../../Content/CSS/DropzoneStyle.css" />--%>
<link rel="stylesheet" href="../../Content/CSS/dropzone.css" />
<link rel="stylesheet" href="../../Content/CSS/FA5/css/fontawesome-all.min.css">
<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css">
<link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/css/toastr.min.css">
<link rel="stylesheet" href="../../Content/CSS/sentry_client.css">

<script src="../../Content/Scripts/DZX.js"></script>
</head>
<body>
<form action="<% = Url.Action("Index", "DropzonePlain") %>" class="dropzone uploader files upload-button" id="family">


</form>

    <script src="//code.jquery.com/jquery-3.3.1.min.js"></script>
    <script src="//cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>
    <script src="//stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
    <script src="//cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js"></script>

       
</body>

</html>