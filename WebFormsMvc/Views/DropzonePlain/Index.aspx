<%@ Page Language="C#" %>

<!DOCTYPE html>

<html>
<head>
<title>Dropzone!</title>
<%--<link rel="stylesheet" href="../../Content/CSS/basic.css" />
<link rel="stylesheet" href="../../Content/CSS/DropzoneStyle.css" />--%>
<link rel="stylesheet" href="../../Content/CSS/dropzone.css" />
<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
<link rel="stylesheet" href="../../Content/CSS/FA5/css/fontawesome-all.min.css">
<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css">
<link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/css/toastr.min.css">
<link rel="stylesheet" href="../../Content/CSS/sentry_client.css">

   <style>
    label, input { display:block; }
    input.text { margin-bottom:12px; width:95%; padding: .4em; }
    fieldset { padding:0; border:0; margin-top:25px; }
    h1 { font-size: 1.2em; margin: .6em 0; }
    div#users-contain { width: 350px; margin: 20px 0; }
    div#users-contain table { margin: 1em 0; border-collapse: collapse; width: 100%; }
    div#users-contain table td, div#users-contain table th { border: 1px solid #eee; padding: .6em 10px; text-align: left; }
    .ui-dialog .ui-state-error { padding: .3em; }
    .validateTips { border: 1px solid transparent; padding: 0.3em; }
  </style>

<script src="../../Content/Scripts/NotDropZone.js"></script>
</head>
<body>
<form action="<% = Url.Action("Index", "DropzonePlain") %>" class="dropzone uploader files upload-button" id="family">


</form>








 
 
<div id="dialog-form" title="Create new user">
  <p class="validateTips">All form fields are required.</p>
 
  <form>
    


      <div class="col-sm-12">
                                                    <div class="input-group mb-3">
                                                        <div class="input-group-prepend">
                                                            <div class="input-group-text">Type</div>
                                                        </div>



                                                        <select name="StepThreeViews[0].DocumentType" id="StepThreeViews[0].DocumentType" class="form-control">
                                        <option>-- Choose --</option>
                                        
                                        <option value="4491">
                                            Affidavit</option>
                                        
                                        <option value="4499">
                                            Bill - employee name</option>
                                        
                                        <option value="4500">
                                            Bill - spouse name</option>
                                        
                                        <option value="4487">
                                            Birth certificate - ee as parent</option>
                                        
                                        <option value="4489">
                                            Birth certificate - spouse as parent</option>
                                        
                                        <option value="4498">
                                            Birth Date Verification</option>
                                        
                                        <option value="4490">
                                            Court order/Adoption decree/Divorce decree</option>
                                        
                                        <option value="4494">
                                            Fax Cover Sheet</option>
                                        
                                        <option value="4501">
                                            Full-time Student Verification</option>
                                        
                                        <option value="4496">
                                            Invalid Document</option>
                                        
                                        <option value="4486">
                                            Joint marital document</option>
                                        
                                        <option value="4485">
                                            Marriage certificate</option>
                                        
                                        <option value="4495">
                                            Other</option>
                                        
                                        <option value="4493">
                                            QMCSO</option>
                                        
                                        <option value="4497">
                                            Returned Mail</option>
                                        
                                        <option value="4488">
                                            Statement of Disability</option>
                                        
                                        <option value="4492">
                                            Unknown</option>
                                        
                                    </select>


                                                        
                                                    </div>
                                                </div>


<div class="col-sm-12">
<div class="input-group mb-3">
<div class="input-group-prepend">
<div class="input-group-text">Description</div>
</div>






<textarea name="StepThreeViews[0].DocumentDescription" class="form-control" id="StepThreeViews[0].DocumentDescription" aria-label="With textarea"></textarea>
</div>
</div>
 
<input type="submit" tabindex="-1" style="position:absolute; top:-1000px">
  </form>
</div>
 
 






    <script src="//code.jquery.com/jquery-3.3.1.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script src="//cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>
    <script src="//stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
    <script src="//cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js"></script>
     <script>

        function FileRemoved(element) {

            console.log(element);
        }



          
  $( function() {
    var dialog, form,
 
      // From http://www.whatwg.org/specs/web-apps/current-work/multipage/states-of-the-type-attribute.html#e-mail-state-%28type=email%29
      emailRegex = /^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/,
      name = $( "#name" ),
      email = $( "#email" ),
      password = $( "#password" ),
      allFields = $( [] ).add( name ).add( email ).add( password ),
      tips = $( ".validateTips" );
 
    function updateTips( t ) {
      tips
        .text( t )
        .addClass( "ui-state-highlight" );
      setTimeout(function() {
        tips.removeClass( "ui-state-highlight", 1500 );
      }, 500 );
    }
 
    function checkLength( o, n, min, max ) {
      if ( o.val().length > max || o.val().length < min ) {
        o.addClass( "ui-state-error" );
        updateTips( "Length of " + n + " must be between " +
          min + " and " + max + "." );
        return false;
      } else {
        return true;
      }
    }
 
    function checkRegexp( o, regexp, n ) {
      if ( !( regexp.test( o.val() ) ) ) {
        o.addClass( "ui-state-error" );
        updateTips( n );
        return false;
      } else {
        return true;
      }
    }
 
    function addUser() {
      var valid = true;
      allFields.removeClass( "ui-state-error" );
 
      valid = valid && checkLength( name, "username", 3, 16 );
      valid = valid && checkLength( email, "email", 6, 80 );
      valid = valid && checkLength( password, "password", 5, 16 );
 
      valid = valid && checkRegexp( name, /^[a-z]([0-9a-z_\s])+$/i, "Username may consist of a-z, 0-9, underscores, spaces and must begin with a letter." );
      valid = valid && checkRegexp( email, emailRegex, "eg. ui@jquery.com" );
      valid = valid && checkRegexp( password, /^([0-9a-zA-Z])+$/, "Password field only allow : a-z 0-9" );
 
      if ( valid ) {
        $( "#users tbody" ).append( "<tr>" +
          "<td>" + name.val() + "</td>" +
          "<td>" + email.val() + "</td>" +
          "<td>" + password.val() + "</td>" +
        "</tr>" );
        dialog.dialog( "close" );
      }
      return valid;
    }
 
    dialog = $( "#dialog-form" ).dialog({
      autoOpen: false,
      height: 400,
      width: 350,
      modal: true,
      buttons: {
        "Create an account": addUser,
        Cancel: function() {
          dialog.dialog( "close" );
        }
      },
      close: function() {
        form[ 0 ].reset();
        allFields.removeClass( "ui-state-error" );
      }
    });
 
    form = dialog.find( "form" ).on( "submit", function( event ) {
      event.preventDefault();
      addUser();
    });
 
    $( "#create-user" ).button().on( "click", function() {
      dialog.dialog( "open" );
    });
  } );
  </script>

       
</body>

</html>