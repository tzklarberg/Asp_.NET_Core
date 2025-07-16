const apiUrl = "http://localhost:5187";
const token = document.cookie.split('; ').find(row => row.startsWith('AuthToken=')).split('=')[1];

if (!token) {
    alert("You are not authenticated. Please log in.");
    window.location.href = "../login.html";
}

// Fetch user details
async function fetchUserDetails() {
    const response = await fetch(`${apiUrl}/User/6`, {
        headers: {
            Authorization: `Bearer ${token}`
        }
    });

    if (response.ok) {
        const user = await response.json();
        document.getElementById("username").textContent = user.name;

        // Show admin link if the user is an admin
        if (user.userType === "ADMIN") {
            document.getElementById("adminLink").style.display = "block";
        }
    } else {
        alert("Failed to fetch user details.");
    }
}

// Fetch shoes
async function fetchShoes() {
    console.log("------------apiurl-----    "+apiUrl);

    const response = await fetch(`${apiUrl}/Shoes`, {

        headers: {
            Authorization: `Bearer ${token}`
        }
    });

    if (response.ok) {
        const shoes = await response.json();
        const tableBody = document.getElementById("shoesTableBody");
        tableBody.innerHTML = ""; // Clear the table

        shoes.forEach(shoe => {
            const row = document.createElement("tr");
            row.innerHTML = `
                <td>${shoe.id}</td>
                <td>${shoe.name}</td>
                <td>${shoe.isElegant}</td>
            `;
            tableBody.appendChild(row);
        });
    } else {
        alert("Failed to fetch shoes.");
    }
}

// Initial fetch
fetchUserDetails();
fetchShoes();