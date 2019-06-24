<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<WebFormsMvc.Models.Test>" %>

<!DOCTYPE html>

<html>
<head>
    <title>Test Page</title>
</head>
<body>
    <% = Model.Name %>

    <form action="/home/uploadfiles" method="post" enctype="multipart/form-data">

    <label for="file">Filename:</label>

    <input type="file" name="file" id="file" />
 
    <input type="submit" name="submit" value="Submit" />
</form>
</body>
</html>
