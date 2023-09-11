$(document).ready(() => {
    $("#uploadExcelFile").submit((e) => {
        e.preventDefault();
        let form_data = new FormData($("#uploadExcelFile")[0]);
        let headers;
        // Assuming you have a JWT token stored in a variable called 'jwtToken'
        let jwtToken = localStorage.getItem('jwtToken');
        if (jwtToken) {
            // Include the token in the request headers
            headers = {
                'Authorization': 'Bearer ' + jwtToken
            };
        }

        $.ajax({
            url: "http://localhost:6769/it-request",
            method: "POST",
            cache: false,
            dataType: 'json',
            data: form_data,
            contentType: false,
            processData: false,
            headers: headers, // Include the JWT token in the request headers

            success: (data) => {
                window.alert(data);
            },
            error: (err) => {
                window.alert(err.responseText);
                console.log("error: ", err);
            }
        });
    });
});