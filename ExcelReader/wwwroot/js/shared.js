
$(document).ready(() => {
    console.log("document is ready")
    var loadDataBttn = $("#loadTableData");
    let headers;
    // Assuming you have a JWT token stored in a variable called 'jwtToken'
    let jwtToken = localStorage.getItem('jwtToken');
    if (jwtToken) {
        // Include the token in the request headers
        headers = {
            'Authorization': 'Bearer ' + jwtToken
        };
    }
    loadDataBttn.click(() => {
        $.ajax({
            url: "http://localhost:6769/it-request",
            method: "GET",
            cache: false,
            async: true,
            headers: headers,
            success: (data) => {
                console.log("datashata: ", data)
                tableData = data.map(row => [
                    row.itRequest.requestId,
                    row.itRequest.author,
                    row.itRequest.type,
                    row.itRequest.subject,
                    row.itRequest.body,
                    row.itRequest.sourceFileId,
                    row.itRequest.status,
                    row.userFile.filename,
                    row.userFile.owner,
                    row.userFile.uploadDate
                ])

                tableshable.rows.add(tableData).draw();
                loadDataBttn.prop('disabled', true)


              
            },
            error: (err) => {
                console.log(err)
                window.alert(err.responseText)
            }
        })


    })



})


/*$('#it-request-with-file-table').DataTable();
$('#example').DataTable({
    columns: [
        { title: 'Name' },
        { title: 'Position' },
        { title: 'Office' },
        { title: 'Extn.' },
        { title: 'Start date' },
        { title: 'Salary' }
    ],
    data: dataSet
});*/