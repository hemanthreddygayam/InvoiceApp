$(document).ready(function () {

    $("#approve").click(function () {
        var invoiceId = $("#element td:nth-child(2)").text();
        var status = "Approved";
        var url = "/CheckingAuthority/Index";

        doPostcall(invoiceId, status,url);
    });

    
    $("#decline").click(function () {
        var invoiceId = $("#element td:nth-child(2)").text();
        var status = "Rejected";
        var url = "/CheckingAuthority/Index";

        doPostcall(invoiceId, status,url);
    });
    $("#warning").click(function () {
        var invoiceId = $("#element td:nth-child(2)").text();
        var status = "Pending";
        var url = "/CheckingAuthority/Index";

        doPostcall(invoiceId, status);
    });


    $("#approveCat2").click(function () {
        var invoiceId = $("#element td:nth-child(2)").text();
        var status = "Approved";
        var url = "/ApproverAuthority/Index";

        doPostcall(invoiceId, status,url);
    });

    $("#declineCat2").click(function () {
        var invoiceId = $("#element td:nth-child(2)").text();
        var status = "Rejected";
        var url = "/ApproverAuthority/Index";
        doPostcall(invoiceId, status,url);
    });
    $("#warningCat2").click(function () {
        var invoiceId = $("#element td:nth-child(2)").text();
        var status = "Pending";
        var url = "/ApproverAuthority/Index";

        doPostcall(invoiceId, status,url);
    });



    var doPostcall = function (InvoiceId, status,url1) {
        var data = {
            InvoiceId: InvoiceId,
            Status: status
        }
        var updateStatus = $.ajax({
            type: "POST",
            url: url1,
            data: data,
            sucess: function (data) {
                alert("Updated Status");
                $('#info').hide();
                $('#buttuonsdiv').hide();

            },
            error: function (data) {
                alert("Error in updating");
                $('#info').hide();
                $('#buttuonsdiv').hide();
            }
           
        });
        
    }

});