const apiUrl = "http://localhost:5187/Login"; // Replace with your login API URL

document.getElementById("loginForm").addEventListener("submit", async(e) => {
    e.preventDefault();
    const Name = document.getElementById("username").value;
    const Password = document.getElementById("password").value;

    const response = await fetch(apiUrl, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ Name, Password })
    });

    if (response.ok) {
        const data = await response.text();
        // Save the token in a cookie
        document.cookie = `AuthToken=${data}; `; //Secure; HttpOnly
        // Redirect to dashboard
        window.location.href = "/../dashboard.html";
    } else {
        document.getElementById("errorMessage").style.display = "block";
    }
});