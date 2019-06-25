<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WebFormsMvc.Models.UploaderViewModel>" %>

<div class="row uploader" id="uploader1">
    <div class="col-md-12">
        <div class="panel panel-default">
            <div class="panel-heading">
                <span class="panel-title">The Panel Title</span>
            </div>
            <div class="panel-body uploader__template--legacy">
                <!-- Legacy Template for older Browsers -->
                <div class="row">
                    <div class="col-xs-5 col-sm-4 col-md-3 col-lg-6">
                        <div class="pull-left">
                            <!-- Name Label (resource file value) -->
                            <b>Name Label (resource file value) :</b>
                        </div>
                        <!-- Area for File Thumbnail -->
                        <div class="pull-left uploader__thumbnail uploader__thumbnail__file">
                            <!-- Text pulled from resource file -->
                            <p class="uploader__drop-zone__content">
                                <em class="uploader__filename">["DefaultFileName"]</em>
                            </p>
                        </div>
                    </div>
                    <div class="col-xs-7 col-sm-8 col-md-9 col-lg-6">
                        <form action="<% = Url.Action("UploadRequiredDocument", "Home") %>" class="uploader__file-form" enctype="multipart/form-data" method="post">
                            <div class="btn-group btn-group-sm pull-left" role="group">
                                <div class="btn-group-vertical btn-group-sm pull-left" role="group">
                                    <div class="btn btn-default uploader__action-input uploader__action--select" data-placement="top" data-toggle="tooltip" data-trigger="manual" title="Select" type="button">
                                        <span class="glyphicon glyphicon-file uploader__action-icon" aria-hidden="true"></span>
                                        <input type="file" name="file" multiple>
                                    </div>
                                    <button class="btn btn-default uploader__action--delete" data-placement="right" data-toggle="tooltip" data-trigger="manual" disabled="disabled" title="Delete" type="button">
                                        <span class="glyphicon glyphicon-trash uploader__action-icon" aria-hidden="true">
                                        </span>
                                    </button>
                                </div>
                                <button class="btn btn-default hidden uploader__action--remove" data-placement="top" data-toggle="tooltip" data-trigger="manual" title="Remove" type="button">
                                    <span class="glyphicon glyphicon-remove-circle uploader__action-icon" aria-hidden="true">
                                    </span>
                                </button>
                                <div class="btn btn-default hidden uploader__action-input uploader__action--save" data-placement="right" data-toggle="tooltip" data-trigger="manual" title="Save" type="button">
                                    <span class="glyphicon glyphicon-ok-circle uploader__action-icon" aria-hidden="true">
                                    </span>
                                    <input type="submit" value="submit">
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12">
                        <span class="hidden label label-danger uploader__error">
                        </span>
                    </div>
                </div>
                <!-- End of Legacy Template -->
            </div>
            <div class="panel-body uploader__drop-zone--compatible">
                <div class="row">
                    <div class="col-xs-5 col-sm-4 col-md-3 col-lg-6 uploader__previews" id="uploader__previews1">
                        <div class="row uploader__drop-zone-template--image">
                            <!-- Image Template -->
                            <div class="col-sm-12">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="pull-left">
                                            <!-- Preview Label -->
                                            <b>Preview Label :</b>
                                        </div>
                                        <!-- Area for Image Thumbnail -->
                                        <div class="pull-right uploader__thumbnail uploader__thumbnail__image">

                                            <div class="hidden progress uploader__progress">
                                                <div class="progress-bar progress-bar-success" data-dz-uploadprogress style="width: 0%;"></div>
                                            </div>
                                            <!-- Text pulled from resource file -->
                                            <p class="hidden uploader__default uploader__drop-zone__content">
                                                Default Drop Message
                                            </p>
                                            <span class="uploader__drop-zone__content">
                                                <img src="http://placehold.it/225x200" data-dz-thumbnail />
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <p class="uploader__wrap-text">
                                            <!-- Name Label -->
                                            <b> Name Label:</b>
                                            <em data-dz-name>["DefaultFileName"]</em>
                                        </p>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <br />
                                        <span class="hidden label label-danger uploader__error" data-dz-errormessage>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <!-- Image Template - End -->
                        </div>
                        <div class="row uploader__drop-zone-template--file">
                            <!-- File Template -->
                            <div class="col-sm-12">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="pull-left">
                                            <!-- Name Label (resource file value) -->
                                            <b>Name Label (resource file value) :</b>
                                        </div>
                                        <!-- Area for File Thumbnail -->
                                        <div class="pull-left uploader__thumbnail uploader__thumbnail__file">

                                            <div class="hidden progress uploader__progress">
                                                <div class="progress-bar progress-bar-success" data-dz-uploadprogress style="width: 0%;"></div>
                                            </div>
                                            <!-- Text pulled from resource file -->
                                            <p class="uploader__default uploader__drop-zone__content">
                                                <em>Default Drop Message</em>
                                            </p>
                                            <span class="hidden uploader__drop-zone__content">
                                                <img data-dz-thumbnail />
                                            </span>
                                            <p class="uploader__drop-zone__content">
                                                <em data-dz-name></em>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <br />
                                        <span class="hidden label label-danger uploader__error" data-dz-errormessage>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <!-- File Template - End -->
                        </div>
                    </div>
                    <div class="col-xs-7 col-sm-8 col-md-9 col-lg-6">
                        <!-- Actions Here
                        Actions: Select File(s), Unselect File(s), Delete File(s), Save File(s)
                        -->
                        <div class="btn-group btn-group-sm pull-left" role="group">
                            <div class="btn-group-vertical btn-group-sm pull-left" role="group">
                                <button class="btn btn-default uploader__action--select" data-placement="top" data-toggle="tooltip" data-trigger="manual" title="Select" type="button">
                                    <span class="glyphicon glyphicon-file uploader__action-icon" aria-hidden="true">
                                    </span>
                                </button>
                                <button class="btn btn-default uploader__action--delete" data-placement="right" data-toggle="tooltip" data-trigger="manual" disabled="disabled" title="Delete" type="button">
                                    <span class="glyphicon glyphicon-trash uploader__action-icon" aria-hidden="true">
                                    </span>
                                </button>
                            </div>
                            <button class="btn btn-default hidden uploader__action--remove" data-placement="top" data-toggle="tooltip" data-trigger="manual" title="Remove" type="button">
                                <span class="glyphicon glyphicon-remove-circle uploader__action-icon" aria-hidden="true">
                                </span>
                            </button>
                            <button class="btn btn-default hidden uploader__action--save" data-placement="right" data-toggle="tooltip" data-trigger="manual" title="Save" type="button">
                                <span class="glyphicon glyphicon-ok-circle uploader__action-icon" aria-hidden="true">
                                </span>
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<!--
    Script to add configuration to Uploader namespace. Is pulled in to the Uploader javascript module.
-->
<script>
    var Uploader = Uploader || {};
    Uploader.candidates = Uploader.candidates || [];
    Uploader.candidates.push(<% = Model.ToJson() %>);
</script>