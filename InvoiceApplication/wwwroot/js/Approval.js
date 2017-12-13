$(document).ready(function () {

    var total = $("#info").find("tr").length;

    var noOfDocuments1 = parseInt($("#info tr:last-child td:nth-child(2)").text());
    var noOfDocuments = parseInt($("#docs").text());

    if (noOfDocuments === 0)
    {
        $(".btn-info").addClass("disabled");
    }
    $("#approve").click(function () {
        var invoiceId = $("#element td:nth-child(2)").text();
        var status = "Checked";
        var url = "/InvoiceAction/Check";
        var remarks = $("#remarks").val();

        doPostcall(invoiceId, status,url,remarks);
    });

    
    $("#decline").click(function () {
        var invoiceId = $("#element td:nth-child(2)").text();
        var status = "CheckRejected";
        var url = "/InvoiceAction/Check";
        var remarks = $("#remarks").val();
        if (remarks.length <= 0)
        {
            $('#errorMessages').html("Please enter remarks");
        }
        else {
            doPostcall(invoiceId, status, url, remarks);

        }
    });
    $("#warning").click(function () {
        var invoiceId = $("#element td:nth-child(2)").text();
        var status = "CheckPending";
        var url = "/InvoiceAction/Check";
        var remarks = $("#remarks").val();

        if (remarks.length <= 0) {
            $('#errorMessages').html("Please enter remarks");
        }
        else {
            doPostcall(invoiceId, status, url, remarks);

        }
    });


    $("#approveCat2").click(function () {
        var invoiceId = $("#element td:nth-child(2)").text();
        var status = "Approved";
        var url = "/InvoiceAction/Approve";
        var remarks = $("#remarks").val();
       
       
        doPostcall(invoiceId, status, url, remarks);

       
    });

    $("#declineCat2").click(function () {
        var invoiceId = $("#element td:nth-child(2)").text();
        var status = "ApproveRejected";
        var url = "/InvoiceAction/Approve";
        var remarks = $("#remarks").val();
        if (remarks.length <= 0) {
            $('#errorMessages').html("Please enter remarks");
        }
        else {
            doPostcall(invoiceId, status, url, remarks);

        }
    });
    $("#warningCat2").click(function () {
        var invoiceId = $("#element td:nth-child(2)").text();
        var status = "ApprovePending";
        var url = "/InvoiceAction/Approve";
        var remarks = $("#remarks").val();
        if (remarks.length <= 0) {
            $('#errorMessages').html("Please enter remarks");
        }
        else {
            doPostcall(invoiceId, status, url, remarks);

        }
    });



    var doPostcall = function (InvoiceId, status, url1,remarks) {
        var data = {
            InvoiceId: InvoiceId,
            Status: status,
            Remarks : remarks
        }
        $.ajax({
            headers: {
                'Accept': 'application/json'
            },
            type: "POST",
            url: url1,
            data: data,
            success: function (data) {
                $('#statuslabel').html(status);
                $('#statuslabel').css("background-color", data);
            },
            error: function (data) {
                alert("Error in updating");
            }

        });

    };

});