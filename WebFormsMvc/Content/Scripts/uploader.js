var Uploader = (function($, Dropzone, candidates, successActions) {
    /*
        Notes:

        [11/11/2015] - The variables candidates and successActions are a way to work around
            the page construction that occurs in the MVC ASP.Net framework.  This library is injected into the
            footer section of the shared layout through the hosting view.  However, the uploaders created via 
            partial view need to be registered to the uploader library.  Since the uploader javascript
            library is unavailable, the following must be performed to add a successful action trigger or to
            register a candidate uploader.

            {{}} - represents an argument value that would be resolved in the current context of use

            Candidate Uploader:
                var Uploader = Uploader || {};
                Uploader.candidates = Uploader.candidates || [];
                Uploader.candidates.push({{Json Serialized Version of Uploader View Model}});

            Successful Action Trigger:
                var Uploader = Uploader || {};
                Uploader.successActions = Uploader.successActions || [];
                Uploader.successAtions.push({
                    key: {{key used to identify the particular uploader. Matches the key used in the candidate}},
                    selector: {{a valid jquery selector to target the object the action will be performed on}},
                    action: {{the name of the action to be executed}},
                    parameters: {{an array of arguments passed in to the action}}
                });

                If a value from the success response of the server is needed the parameters may contain, ( {{Key}},
                 {{FileUrl}}, {{ResponseMessage}}, {{Success}} ), and they will be replaced with associated value
                 in the response.

            Effectively, a property of candidates || successActions is created in the namespace of Uploader prior to
            the javascript library being available.  When the module loads, values contained in the known properties 
            are attached to local variables and manipulated as needed.

        [11/16/2015] - Legacy support (browsers not supporting the HTML5 FileAPI) has been implemented, but has a few 
            quirks that must be considered when expecting legacy use to occur.  The file upload occurs as a form submit
            post. In browsers such as IE8 and 9, the file object is protected from access and can't be sent within an
            AJAX post to the server.  The response currently is expected to be a redirect to the page hosting the upload,
            causing a full refresh.  On the server side it is important that for legacy support an endpoint with a redirect
            type of response be implemented and pointed to when using the uploader.  Non legacy browsers will continue to
            send the post via ajax and expect a response object containing information about the storeage of the file.

        
    */

    //Set value of _debugMode to true to log critical values of the module
    var _debugMode = false;

    _debugMode && console.log('Version of jQuery loaded to module is: ' + $.fn.jquery);

    _debugMode && console.log('Version of Dropzone is: ' + Dropzone.version);

    //Dropzone checks the following aspects of the browser for determination of support
    //Failing any will result in the fallback being used
    _debugMode && console.log('Has window.File: ' + ((window.File) ? true : false));
    _debugMode && console.log('Has window.FileReader: ' + ((window.FileReader) ? true : false));
    _debugMode && console.log('Has window.FileList: ' + ((window.FileList) ? true : false));
    _debugMode && console.log('Has window.Blob: ' + ((window.Blob) ? true : false));
    _debugMode && console.log('Has window.FormData: ' + ((window.FormData) ? true : false));
    _debugMode && console.log('Has document.querySelector: ' + ((document.querySelector) ? true : false));
    _debugMode && console.log('Hyperlink element property "classList" nonexistent: ' + ((!("classList" in document.createElement("a"))) ? true : false));
    _debugMode && console.log("Dropzone's blacklisted browsers: " + Dropzone.blacklistedBrowsers);

    _debugMode && console.log('Dropzone supports this browser: ' + Dropzone.isBrowserSupported());

    //holder for configurations
    var _candidates = candidates || [];

    //container for the uploaders
    var _instances = _instances || [];

    //container for actions that need to be executed on a succesful upload.
    //the assumption is that the actions will be outside of the uploader
    var _successActions = successActions || [];

    _debugMode && console.log(successActions);

    var _defaultConfig = {
        // Record Values
        HasStored: null,
        Key: null,
        StoredUrl: '/folderPath/filename',    //Location of the stored file

        //Actions
        ActionSave: '.uploader__action--save',
        ActionSelect: '.uploader__action--select',
        ActionRemove: '.uploader__action--remove',
        ActionDelete: '.uploader__action--delete',
        
        //Template Elements
        ClassTemplate: 'uploader__drop-zone-template--file',
        ClassTemplateLegacy: 'uploader__template--legacy',
        ContainerId: '#uploader',   //Main Container Id
        ContainerCompatible: '.uploader__drop-zone--compatible',
        ContainerDefault: '.uploader__default', //Used to mark elements for removal from the template at load
        ContainerError: '.uploader__error',
        ContainerInput: '.uploader__action--select',
        ContainerFilename: '.uploader__filename',
        ContainerFormLegacy: '.uploader__file-form',
        ContainerPreviews: '#uploader__previews',   //Mapped to dropzone's previews container (previewsContainer). Must be Id.
        ContainerProgress: '.uploader__progress',
        ContainerTemplate: '.uploader__drop-zone-template--file',
        ContainerTemplateLegacy: '.uploader__template--legacy',
        ContainerThumbnail: '.uploader__thumbnail',

        //Options
        ActionsOnSuccess: [],   //Actions to perform upon a successful upload
        FileCountExceededMessage: null, //Mapped to dropzone's dictMaxFilesExceeded
        FileSizeExceededMessage: null,  //Mapped to dropzone's dictFileTooBig
        FileTypeInvalidMessage: null,   //Mapped to dropzone's dictInvalidFileType
        ForceLegacy: false,
        HeaderVisible: true,
        Heading: null,
        IsImage: false,
        LegacyBrowserInstruction: null, //Mapped to dropzone's dictFallbackText
        LegacyBrowserMessage: null, //Mapped to dropzone's dictFallbackMessage
        RequestParameters: {},  //Optional Dictionary of Key Value Pairs Added to Server Requests
        SelectionMaxFiles: 1,   //Mapped to dropzone's max file count (maxFiles)
        SelectionMaxSize: 1000000,  //Mapped to dropzone's maxFilesize.  Converted to MB from Bytes
        SelectionSaveUrl: '#',  //Mapped to dropzone's upload url (url).
        SelectionSaveUrlLegacy: '#', 
        SelectionTypeFilter: 'application/*,audio/*,chemical/*,image/*,text/*,video/*,x-conference/*',  //default accepts most file types (acceptedFiles)
        ServerResponseErrorMessage: null,   //Mapped to dropzone's dictResponseError
        StoredDeleteAllowed: true,  //Enables the delete request action
        StoredDeleteHidden: false,  //Hides the delete action from view
        StoredDeleteUrl: '#'   //Url to request deletion of the existing stored file
    };
    
    function Config(config){
        var _config = _defaultConfig;
        
        config = config || {};
        
        for (var prop in config) {
            if (typeof _config[prop] === 'undefined') {
                console.error('Configuration option "' + prop + '" not found.');
            } else {
                _config[prop] = config[prop];
            }
        }
        
        return _config;
    }
    
    function Selectors(config){
        config = config || {};
        
        var _selectors = {
            ActionDelete: config.ContainerId.concat(' ', config.ActionDelete),
            ActionSave: config.ContainerId.concat(' ', config.ActionSave),
            ActionSelect: config.ContainerId.concat(' ', config.ActionSelect),
            ActionRemove: config.ContainerId.concat(' ', config.ActionRemove),
            ContainerCompatible: config.ContainerId.concat(' ', config.ContainerCompatible),
            ContainerDefault: config.ContainerId.concat(' ', config.ContainerDefault),
            ContainerError: config.ContainerId.concat(' ', config.ContainerError),
            ContainerInput: config.ContainerId.concat(' ', config.ContainerInput),
            ContainerFilename: config.ContainerId.concat(' ', config.ContainerFilename),
            ContainerFormLegacy: config.ContainerId.concat(' ', config.ContainerFormLegacy),
            ContainerPreviews: config.ContainerId.concat(' ', config.ContainerPreviews),
            ContainerProgress: config.ContainerId.concat(' ', config.ContainerProgress),
            ContainerTemplate: config.ContainerId.concat(' ', config.ContainerTemplate),
            ContainerTemplateLegacy: config.ContainerId.concat(' ', config.ContainerTemplateLegacy),
            ContainerThumbnail: config.ContainerId.concat(' ', config.ContainerThumbnail)
        };
        
        return _selectors;
    }
    
    function BuildLegacy(config, selectors){
        var handler = {};
        
        $(selectors.ContainerCompatible).remove();

        //create hidden inputs for supporting request parameters
        var inputFileButton = $(selectors.ContainerInput);

        _debugMode && console.log(config.RequestParameters);

        var requestParameters = config.RequestParameters;
        for (var prop in requestParameters) {
            $('<input>').attr({
                type: 'hidden',
                name: prop,
                value: requestParameters[prop]
            }).appendTo(inputFileButton);
        }

        $(document).on('change', (selectors.ContainerInput + ' :file'), function() {
            var input = $(this);
            var numFiles = input.get(0).files ? input.get(0).files.length : 1;
            var label = input.val().replace(/\\/g, '/').replace(/.*\//, '');

            input.trigger('fileselected', [numFiles, label]);
        });

        $(selectors.ContainerInput + ' :file').on('fileselected', function(event, numFiles, label) {
            $(selectors.ActionRemove).removeClass('hidden');
            $(selectors.ActionSave).removeClass('hidden');

            var labelElement = $(selectors.ContainerFilename);
            var labelValue = numFiles > 1 ? numFiles + ' files selected' : label;

            _debugMode && console.log(labelElement);
            _debugMode && console.log(labelValue);

            if (labelElement.length) {
                labelElement.html(labelValue);

                if (!IsValidType(label, config.SelectionTypeFilter)) {
                    handler.error(config.FileTypeInvalidMessage);
                } else {
                    $(selectors.ContainerError).addClass('hidden');
                    $(selectors.ContainerError).html('');
                }
            }
        });

        handler.error = function (message) {
            var errorElement = $(selectors.ContainerError);

            errorElement.removeClass('hidden');
            errorElement.html(message);
        };

        handler.removeSelection = function () {
            var fileInput = $(selectors.ContainerInput + ' :file');
            fileInput.replaceWith(fileInput.clone(true));
            
            $(selectors.ContainerFilename).html('[None Selected]');
            $(selectors.ActionRemove).addClass('hidden');
            $(selectors.ActionSave).addClass('hidden');
        };

        handler.deleteFile = function() {
            _debugMode && console.log("server delete action triggered");
            _debugMode && console.error("Uploader deleteFile not implemented.");
        };

        $(selectors.ActionDelete).on("click", handler.deleteFile);
        $(selectors.ActionRemove).on("click", handler.removeSelection);
        
        return handler;
    }
    
    function BuildDropzone(config, selectors){
        var handler = {};
        
        var _previewNode = $(selectors.ContainerTemplate);
        _previewNode.id = config.Key;
        
        var _previewTemplate = _previewNode.parent().clone()
            .find(config.ContainerTemplate).removeClass(config.ClassTemplate).end()
            .find(config.ContainerDefault).remove().end()
            .html();
        
        handler.dropzone = new Dropzone(selectors.ContainerPreviews, {
            acceptedFiles: config.SelectionTypeFilter,
            autoQueue: false,
            clickable: selectors.ActionSelect,
            dictFallbackMessage: config.LegacyBrowserMessage,
            dictFallbackText: config.LegacyBrowserInstruction,
            dictFileTooBig: config.FileSizeExceededMessage,
            dictInvalidFileType: config.FileTypeInvalidMessage,
            dictMaxFilesExceeded: config.FileCountExceededMessage,
            dictResponseError: config.ServerResponseErrorMessage,
            filesizeBase: 1024,
            maxFiles: config.SelectionMaxFiles,
            maxFilesize: (config.SelectionMaxSize / 1024 / 1024),
            params: config.RequestParameters,
            previewsContainer: config.ContainerPreviews,
            previewTemplate: _previewTemplate,
            thumbnailWidth: parseInt($(selectors.ContainerThumbnail).css('width'), 10),
            thumbnailHeight: parseInt($(selectors.ContainerThumbnail).css('height'), 10),
            url: config.SelectionSaveUrl
        });

        _debugMode && console.log("Max File Size (Base 1024) for " + config.SelectionMaxSize
            + ' bytes in MB is: ' + (config.SelectionMaxSize / 1024 / 1024));
        
        handler.dropzone.sbConfig = $.extend(true, {}, config);
        handler.dropzone.sbSelectors = $.extend(true, {}, selectors);
        
        // emitted by dropzone when a file is added to the queue
        handler.dropzone.on("addedfile", function(file) {
            if (this.files.length > 0) {
                $(this.sbSelectors.ContainerTemplate).addClass('hidden');
                $(this.sbSelectors.ActionSave).removeClass('hidden');
                $(this.sbSelectors.ActionRemove).removeClass('hidden');
                $(this.sbSelectors.ActionDelete).attr('disabled', 'disabled');
                $(this.sbSelectors.ActionSave).removeClass('btn-success');
                $(this.sbSelectors.ActionSave).removeClass('btn-danger');
            }

            if (this.files.length > this.sbConfig.SelectionMaxFiles) {
                this.removeFile(this.files[0]);
            }
        });

        // emitted by dropzone if any error occurs (during selection or upload)
        handler.dropzone.on("error", function(file, errorMessage) {
            $(this.sbSelectors.ContainerError).removeClass('hidden');
            $(this.sbSelectors.ContainerProgress).addClass('hidden');

            _debugMode && console.log(file);
            _debugMode && console.log(errorMessage);

            if (typeof file != 'undefined' && file.accepted === true) {
                $(this.sbSelectors.ActionSave).addClass('btn-danger');
            }
        });

        // emitted by dropzone as the progress of the upload changes
        handler.dropzone.on("totaluploadprogress", function(progress) {
            $(this.sbSelectors.ContainerProgress).css('width', progress + '%');
        });

        // emitted by dropzone just before a file is uploaded
        handler.dropzone.on("sending", function(file, xhr, data) {
            $(this.sbSelectors.ContainerProgress).removeClass('hidden');
            $(this.sbSelectors.ActionSave).attr('disabled', 'disabled');
        });

        // emitted by dropzone at the completion of a successful file upload
        handler.dropzone.on("success", function(file, serverResponse) {
            _debugMode && console.log(file);
            _debugMode && console.log(serverResponse);

            if (typeof file !== 'undefined') {
                if (serverResponse['Success']) {
                    $(this.sbSelectors.ActionSave).addClass('btn-success');

                    _debugMode && console.log(_successActions);

                    var actions = $.grep(_successActions, function(value, i) {
                        return value.key === handler.dropzone.sbConfig.Key;
                    });

                    var actions = $.grep(actions, function(action, i) {
                        action.parameters = $.map(action.parameters, function(parameter, i) {
                            return parameter.replace('{{Key}}', (serverResponse['Key'] || ''))
                                .replace('{{FileUrl}}', (serverResponse['FileUrl'] || '#'))
                                .replace('{{ResponseMessage}}', (serverResponse['ResponseMessage'] || ''))
                                .replace('{{Success}}', (serverResponse['Success'] || ''));
                        });
                        return action;
                    });

                    _debugMode && console.log(actions);

                    for (var i = 0; i < actions.length; i++) {
                        var curAction = actions[i];
                        _debugMode && console.log(curAction);
                        $(curAction.selector)[curAction.action].apply($(curAction.selector), curAction.parameters);
                    }
                } else {
                    $(this.sbSelectors.ContainerError).removeClass('hidden');
                    $(this.sbSelectors.ContainerProgress).addClass('hidden');
                    $(this.sbSelectors.ActionSave).addClass('btn-danger');

                    $(this.sbSelectors.ContainerError).text(serverResponse['ResponseMessage'] || 'Upload Failed. Reason Unknown.');
                }
            }
        });
        
        // emitted by dropzone when the upload process completes
        handler.dropzone.on("queuecomplete", function(progress) {
            $(this.sbSelectors.ContainerProgress).addClass('hidden');
            $(this.sbSelectors.ActionSave).removeAttr('disabled');
        });

        // emitted by dropzone when all files have been removed from the queue
        handler.dropzone.on("reset", function() {
            $(this.sbSelectors.ContainerTemplate).removeClass('hidden');
            $(this.sbSelectors.ContainerError).addClass('hidden');
            $(this.sbSelectors.ActionSave).addClass('hidden');
            $(this.sbSelectors.ActionSave).removeClass('btn-success');
            $(this.sbSelectors.ActionSave).removeClass('btn-danger');
            $(this.sbSelectors.ActionRemove).addClass('hidden');

            if ((this.sbConfig.HasStored && this.sbConfig.StoredDeleteAllowed)) {
                $(this.sbSelectors.ActionDelete).removeAttr('disabled');
            }
        });

        _debugMode && console.log(handler.dropzone);

        handler.saveSelection = function () {
            handler.dropzone.enqueueFiles(handler.dropzone.getFilesWithStatus(Dropzone.ADDED));
        };

        handler.removeSelection = function () {
            handler.dropzone.removeAllFiles(true);
        };

        handler.deleteFile = function () {
            _debugMode && console.log("server delete action triggered");
            _debugMode && console.error("Uploader deleteFile not implemented.");
        };

        //bind Action click events
        $(selectors.ActionDelete).on("click", handler.deleteFile);
        $(selectors.ActionSave).on("click", handler.saveSelection);
        $(selectors.ActionRemove).on("click", handler.removeSelection);

        return handler;
    }
    
    function File(config) {
        var fileHandler = {};
        
        var _config = Config(config);
        var _selectors = Selectors(_config);

        if (Dropzone.isBrowserSupported() && !_config.ForceLegacy){
            $(_selectors.ContainerTemplateLegacy).remove();
            $(_selectors.ContainerTemplate).siblings().remove();
            
            fileHandler = BuildDropzone(_config, _selectors);
        } else {
            
            fileHandler = BuildLegacy(_config, _selectors);
        }
        
        _debugMode && console.log(fileHandler);

        if (_config.HasStored && _config.StoredDeleteAllowed) {
            $(_selectors.ActionDelete).removeAttr('disabled');
        }

        if (_config.StoredDeleteHidden) {
            $(_selectors.ActionDelete).addClass('hidden');
        }

        return {
            save: fileHandler.saveSelection,
            remove: fileHandler.removeSelection,
            deleteFile: fileHandler.deleteFile
        };
    }

    function Image(config) {
        config = config || {};

        config.ClassTemplate = 'uploader__drop-zone-template--image';
        config.ContainerTemplate = '.uploader__drop-zone-template--image';

        return File.call(this, config);
    }

    //default configuration property
    function Defaults() {
        return _defaultConfig;
    }

    function IsValidType(filename, acceptedFiles) {
        var fileExtension, validType, _i, _len;

        if (!acceptedFiles) {
            return true;
        }
        acceptedFiles = acceptedFiles.split(",");
        fileExtension = filename.substr((~-filename.lastIndexOf(".") >>> 0) + 1);

        _debugMode && console.log('Accepted File Types:', acceptedFiles);
        _debugMode && console.log('File Extension: ', fileExtension);

        for (_i = 0, _len = acceptedFiles.length; _i < _len; _i++) {
            validType = acceptedFiles[_i];
            validType = validType.trim();
            if (validType.charAt(0) === ".") {
                if (fileExtension.toLowerCase().indexOf(validType.toLowerCase(), fileExtension.length - validType.length) !== -1) {
                    return true;
                }
            }
        }
        return false;
    }

    $(document).ready(function () {
        for (i = 0; i < _candidates.length; ++i) {
            //create upload instances from candidate configurations
            var containerId = 
                _candidates[i].ContainerId || 
                ("#uploader" + _candidates[i].Key);

            var previewsId = 
                _candidates[i].ContainerPreviews ||
                '#' + $(containerId).find('.uploader__previews').first().attr('id');

            _candidates[i].ContainerId = containerId;
            _candidates[i].ContainerPreviews = previewsId;

            _debugMode && console.log('Current Upload Candidate: ' + containerId);

            if (_candidates[i]['IsImage']) {
                _instances.push(Image(_candidates[i]));
            } else {
                _instances.push(File(_candidates[i]));
            }

            //setup tooltips
            var tooltipSelect = containerId + ' [data-toggle="tooltip"]';

            $(tooltipSelect).tooltip();

            $(tooltipSelect).mouseenter(function () {
                var tip = $(this);
                tip.tooltip('show');
                setTimeout(function () {
                    tip.tooltip('hide');
                }, 2000);
            });

            $(tooltipSelect).mouseleave(function () {
                $(this).tooltip('hide');
            });
        }
    });

    return {
        Defaults: Defaults,
        File: File,
        Image: Image,
        Version: "20151211-0832"
    };
})(jQuery, Dropzone, Uploader.candidates, Uploader.successActions);