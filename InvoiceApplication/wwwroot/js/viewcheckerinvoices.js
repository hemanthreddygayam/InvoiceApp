$(document).ready(function () {

    $("#datetimepicker1").datetimepicker({
    });
    $("#datetimepicker2").datetimepicker({
    });

        //$('.page').on('click', function () {
        //    var val = $(this).text();
        //    clickCall(val);
        //});

        //$('#searchinvoice').on('click', function () {
        
        //    clickCall(1);
        //});

        //var clickCall = function (pageNumber) {
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
        //        Results: pageNumber,
        //        page: 0
        //    };

        //    window.location.href = '/CheckingAuthority/GetInvoices/?Status=' + status + "&From=" + from + "&To=" + to + "&Results=" + pageNumber + "&page=0";
        //    //$.ajax({
        //    //    type: 'GET',
        //    //    url: "/CheckingAuthority/GetInvoices/",
        //    //    data: data,
        //    //    success: function (invoices) {
        //    //        //console.log(invoices);
        //    //        //var options = '';
        //    //        //$.each(invoices.invoiceResults, function (index, invoice) {
        //    //        //    console.log(invoice);
        //    //        //    options += '<tr>';
        //    //        //    options += '<td>' + invoice.invoiceNo + '</td>';
        //    //        //    options += '<td>' + invoice.vesselName + '</td>';
        //    //        //    options += '<td>' + invoice.customerName + '</td>';
        //    //        //    options += '<td>' + invoice.accountDate + '</td>';
        //    //        //    options += '<td>' + invoice.currencyCode + '</td>';
        //    //        //    options += '<td>' + invoice.exchangeRate + '</td>';
        //    //        //    options += '<td>' + invoice.totalAmt + '</td>';

        //    //        //    options += '<td><a href="/CheckingAuthority/Index/?invoiceId=' + invoice.invoiceId + '" class="btn btn-warning">View Details</a></td>';
        //    //        //    options += '</tr>';
        //    //        //});
        //    //        //$('#invoice-info').html(options);

        //    //        //buttonsCode = ""
        //    //        //for (var i = 1; i <= invoices.totalNumberOfRecords; i++)
        //    //        //{
        //    //        //    buttonsCode += '<button class="btn btn-primary page">'+ i+'</button>';
        //    //        //}
        //    //        //$("#pageInfo").html(buttonsCode);
        //    //    },
        //    //    error: function (response) {
        //    //        $('#errorModal').modal('show');
        //    //    }
        //    //});
        //};

});