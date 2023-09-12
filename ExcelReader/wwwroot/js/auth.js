$(document).ready(() => {
    $("#sign-in-form").submit((e) => {
        e.preventDefault();


        let username = $("#inputEmail").val();
        let password = $("#inputPassword").val();


        let userData = {
            Username: username,
            Password: password
        };


        $.ajax({
            url: "http://localhost:6769/api/auth/login",
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify(userData),
            success: (data) => {
                localStorage.setItem('jwtToken', data.token);
                console.log("Login successful!");
                console.log(data);
                alert("Logged in Successfully")


            },
            error: (err) => {

                console.error("Login failed!");
                console.error(err.responseText);
                alert("Login failed. Please check your credentials.");
            }
        });
    });


    $("#sign-up-form").submit((e) => {
        e.preventDefault();

        let username = $("#inputEmail").val();
        let password = $("#inputPassword").val();
        let isAdmin = $("#isAdminCheckbox").is(":checked");

        let userData = {
            Username: username,
            Password: password,
            IsAdmin: isAdmin
        };

        let url = isAdmin
            ? "http://localhost:6769/api/auth/register-admin"
            : "http://localhost:6769/api/auth/register-user";

        $.ajax({
            url: url,
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify(userData),
            success: (data) => {
                console.log("Signup successful!");
                console.log(data);
                alert("Signup successful!" + `\n${data}`);
            },
            error: (err) => {
                console.error("Signup failed!");
                console.error(err.responseText);
                alert(`Signup failed: ${err.responseText}`);
            }
        });
    });
});