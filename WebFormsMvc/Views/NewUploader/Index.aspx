<%@ Page Language="C#" %>


<html>

<head>

    <title>File Upload Test</title>
    <link rel="stylesheet" href="../../Content/CSS/FA5/css/fontawesome-all.min.css">
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css">
    <link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/css/toastr.min.css">
    <link rel="stylesheet" href="../../Content/CSS/sentry_client.css">


</head>

<body>
    <form method="post" enctype="multipart/form-data">

        <div class="uploader files upload-button"></div>

        <input type="submit" />

    </form>

    <script src="//code.jquery.com/jquery-3.3.1.min.js"></script>
    <script src="//cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>
    <script src="//stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
    <script src="//cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js"></script>
    <script type="text/javascript" src="../../Content/Scripts/SB_Uploader.js"></script>
    <script type="text/javascript">
        $('[data-toggle="tooltip"]').tooltip();
        $(".sb-check").click(function () {
            $(this).toggleClass('checked');
        });
        toastr.options = {
            "positionClass": "toast-top-center",
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }
        $(".uploader").SB_Uploader({
            filesallowed: 4
        });
        $("#prev").click(function () {
            pageSwitch('step2.html');
        });
        $("#next").click(function () {
            pageSwitch('step4.html');
        });
        function pageSwitch(url) {
            window.location.href = url;
        };
    </script>

</body>

</html>