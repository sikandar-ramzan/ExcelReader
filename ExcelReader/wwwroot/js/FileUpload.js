$(document).ready(() => {
    $("#uploadExcelFile").submit((e) => {
        e.preventDefault();
        let form_data = new FormData($("#uploadExcelFile")[0]);
        let headers;

        let jwtToken = localStorage.getItem('jwtToken');
        if (jwtToken) {

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
            headers: headers,

            success: (data) => {
                window.alert(data);
            },
            error: (err) => {
                console.log("err", err)
                if (!jwtToken || err.status == 401)
                {
                    window.alert(err.responseText);
                }
                else if (jwtToken && err.status == 401)
                {
                    window.alert("Token Expired! Sign In Again");
                } else if (err.responseText)
                {
                    window.alert(err.responseText);
                }
                else {
                    window.alert("UnExpected Error Occured")

                }
            }
        });
    });
});