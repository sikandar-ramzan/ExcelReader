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
                alert("success")

                
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
        let isAdmin = $("#isAdminCheckbox").is(":checked"); // Check if the checkbox is checked

        let userData = {
            Username: username,
            Password: password,
            IsAdmin: isAdmin // Include the isAdmin flag in the data
        };

        // Determine the URL based on whether isAdmin is true or false
        let url = isAdmin
            ? "http://localhost:6769/api/auth/register-admin"
            : "http://localhost:6769/api/auth/register-user";

        $.ajax({
            url: url,
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify(userData),
            success: (data) => {
                // Handle a successful signup response here
                console.log("Signup successful!");
                console.log(data);
                alert("Signup successful!" + `\n${data}`);

                // Redirect or perform other actions as needed
            },
            error: (err) => {
                // Handle signup error here
                console.error("Signup failed!");
                console.error(err.responseText);
                alert(`Signup failed: ${err.responseText}`);
            }
        });
    });
});