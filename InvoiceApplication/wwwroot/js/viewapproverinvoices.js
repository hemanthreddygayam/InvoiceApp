$(document).ready(function () {

    $("#datetimepicker1").datetimepicker({
        format: "D/M/Y"
    });
    $("#datetimepicker2").datetimepicker({
        format: "D/M/Y"
    });

    //$('#searchinvoice').on('click', function () {
    //    var status = $('#status').val();
    //    var from = $('#from').val();
    //    var to = $('#to').val();

    //    from = from === "" ? "0001-01-01" : from;
    //    to = to === "" ? "0001-01-01" : to;

    //    var results = $('#results').val();

    //    var data = {
    //        Status: status,
    //        From: from,
    //        To: to,
    //        Results: results,
    //        page: 0
    //    };
    //    $.ajax({
    //        headers: {
    //            'Accept': 'application/json',
    //            'Content-Type': 'application/json'
    //        },
    //        type: 'POST',
    //        url: "/ApproverAuthority/GetInvoices/",
    //        data: JSON.stringify(data),
    //        success: function (invoices) {
    //            console.log(invoices);
    //            var options = '';
    //            $.each(invoices, function (index, invoice) {
    //                console.log(invoice);
    //                options += '<tr>';
    //                options += '<td>' + invoice.invoiceNo + '</td>';
    //                options += '<td>' + invoice.vesselName + '</td>';
    //                options += '<td>' + invoice.customerName + '</td>';
    //                options += '<td>' + invoice.accountDate + '</td>';
    //                options += '<td>' + invoice.currencyCode + '</td>';
    //                options += '<td>' + invoice.exchangeRate + '</td>';
    //                options += '<td>' + invoice.totalAmt + '</td>';
    //                options += '<td><a href="/ApproverAuthority/Index/?invoiceId=' + invoice.invoiceId + '" class="btn btn-warning">View Details</a></td>';
    //                options += '</tr>';
    //            });
    //            $('#invoice-info').html(options);
    //        },
    //        error: function (response) {
    //            $('#errorModal').modal('show');
    //        }
    //    });

    //});

});