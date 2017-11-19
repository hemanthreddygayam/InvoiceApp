$(document).ready(function () {
    $('.viewfile').on('click', function () {

        var id = $(this).attr('id');
        $.ajax({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            type: 'POST',
            url: "/Invoices/GetFilePath/",
            data: JSON.stringify(id),
            success: function (response) {

                console.log(response);
                console.log(response.fileName);
                var filepath = response.filePath + response.fileName;
                $("#viewpdf").html('<embed class="viewer" src="' + filepath + '" />');
                console.log(navigator.userAgent);
            },
            error: function (response) {
                $('#errorModal').modal('show');
            }
        });
    });

    $('.toggler').on('click', function () {
        var id = $(this).attr('id');
        //Remove text_ from id and return the number
        id = id.substr(5, id.length);
        if ($(this).html() == '<span class="glyphicon glyphicon-triangle-bottom"></span>Hide File') {
            $(this).html('<span class="glyphicon glyphicon-triangle-top"></span>Show File');
            $('#document_' + id).empty();
        }
        else {
            $('.toggler').each(function () {
                $(this).html('<span class="glyphicon glyphicon-triangle-top"></span>Show File');
            });

            $('.fileviewer').each(function () {
                $(this).empty();
            });
            $.ajax({
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                type: 'POST',
                url: "/Invoices/GetFilePath/",
                data: JSON.stringify(id),
                success: function (response) {
                    var filepath = response.filePath + response.fileName;
                    $("#document_" + id).html("<object data='" + filepath + "' type='application/pdf' class='viewer' ><p> Your web browser doesn't have a PDF plugin. Instead you can <a href= '" + filepath + "'> click here to download the PDF file.</a ></p></object >");
                    $("#text_" + id).html('<span class="glyphicon glyphicon-triangle-bottom"></span>Hide File');
                },
                error: function (response) {
                    $('#errorModal').modal('show');
                }
            });
        }
    });
});