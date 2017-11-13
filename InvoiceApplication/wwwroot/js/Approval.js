$(document).ready(function () {

    $("#approve").click(function () {
        var claimId = $("#claimId").val();
        var status = "Approved";
        var url = "/CheckingAuthority/Index";

        doPostcall(claimId, status,url);
    });

    $("#decline").click(function () {
        var claimId = $("#claimId").val();
        var status = "Rejected";
        var url = "/CheckingAuthority/Index";

        doPostcall(claimId, status,url);
    });
    $("#warning").click(function () {
        var claimId = $("#claimId").val();
        var status = "Pending";
        var url = "/CheckingAuthority/Index";

        doPostcall(claimId, status);
    });


    $("#approveCat2").click(function () {
        var claimId = $("#claimId").val();
        var status = "Approved";
        var url = "/ApproverAuthority/Index";

        doPostcall(claimId, status,url);
    });

    $("#declineCat2").click(function () {
        var claimId = $("#claimId").val();
        var status = "Rejected";
        var url = "/ApproverAuthority/Index";
        doPostcall(claimId, status,url);
    });
    $("#warningCat2").click(function () {
        var InvoiceId = $("#claimId").val();
        var status = "Pending";
        var url = "/ApproverAuthority/Index";

        doPostcall(claimId, status,url);
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
            success: function (data) {

            },
           
        });
        updateStatus.fail(function () {

        })
    }

});