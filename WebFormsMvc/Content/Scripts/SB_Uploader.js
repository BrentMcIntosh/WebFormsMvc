    (function ($) {
        $.fn.SB_Uploader = function (options) {

            var settings = $.extend({
                    filesallowed: 1,
                    maxFileSize: 100000000,
                    instructions: "Click to upload file",
                    _id: 'fileUpload'
                },
                options);


            var _this = $(this),
                _numOfFiles = 0;
            _this.append('<div class="uploader_trigger"></div>');
            _this.parent().append('<input  multiple class="upload_box" id="' + settings._id + '" type="file" /> ');


            function buildItem(val, size) {
                _numOfFiles++;
                console.log(_numOfFiles)
                return '<div class="uploaded_item"><i class="fas fa-cog fa-spin"></i><strong>' + val + '</strong><div class="item_size">(' + Math.ceil(size) + 'MB)</div> <a class="remove_item" href="#"><i class="fas fa-times"></i></a></div>';
            }

            function clicked() {
                $('#' + settings._id).trigger('click');
            }

            if (settings.instructions != "") {
                $(this).append('<div class="uploader_instructions">' + settings.instructions + '</div>');
            }


            $('.uploader_trigger').on('click', function () {
                if (_numOfFiles < settings.filesallowed) {
                    clicked();
                } else {
                    toastr.error('You have exceeded the number of files allowed (' + settings.filesallowed + ') for upload.');
                }

            });

            $('#' + settings._id).on('change', function () {
                var length = this.files.length;
                if (length <= settings.filesallowed) {
                    for (var i = 0; i < length; i++) {
                        var size = this.files[i].size / 1024 / 1024,
                            vals = this.files[i].name,
                            val = vals.length ? vals.split('\\').pop() : '',
                            fileInfo = this.files[i];

                        if (size > settings.maxFileSize) {
                            toastr.warning('Your file is too large. Please, do not exceed ' + settings.maxFileSize + 'MB');

                        } else {
                            _this.append(buildItem(val, size));
                            toastr.success('Your files have been added to the uploader.<br> Click the <strong>SAVE</strong> button to complete uploading.');
                        }
                    }

                } else {
                    toastr.error('You have exceeded the number of files allowed (' + settings.filesallowed + ') for upload.');

                }
            });

            _this.on('click', '.remove_item', function () {
                $(this).parent().remove();
                $('#' + settings._id).val(null);
                _numOfFiles--;
            })

        };
    }(jQuery));

