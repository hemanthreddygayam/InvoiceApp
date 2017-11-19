$(document).ready(function () {
    $('#searchinvoice').on('click', function () {
        var status = $('#status').val();
        var from = $('#from').val();
        var to = $('#to').val();
        var results = $('#results').val();

        var data = {
            Status: status,
            From: from,
            To: to,
            Results: results,
            page: 0
        };
        $.ajax({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            type: 'POST',
            url: "/CheckingAuthority/GetInvoices/",
            data: JSON.stringify(data),
            success: function (invoices) {
                console.log(invoices);
                var options = '';
                $.each(invoices, function (index, invoice) {
                    console.log(invoice);
                    options += '<tr>';
                    options += '<td>' + invoice.invoiceId+ '</td>';
                    options += '<td>' + invoice.invoiceDate+ '</td>';
                    options += '<td>' + invoice.dueDate + '</td>';
                    options += '<td>' + invoice.customerName + '</td>';
                    options += '<td>' + invoice.totalAmt + ' ' + invoice.currencyCode + '</td>';
                    options += '<td><a href="/CheckingAuthority/Index/' + invoice.invoiceId + '" class="btn btn-primary">View Details</a></td>';
                    options += '</tr>';
                });
                $('#invoice-info').html(options);
            },
            error: function (response) {
                $('#errorModal').modal('show');
            }
        });

    });

});