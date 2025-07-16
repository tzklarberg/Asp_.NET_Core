const apiUrl = "http://localhost:5187";
const token = document.cookie.split('; ').find(row => row.startsWith('AuthToken='))?.split('=')[1];

if (!token) {
    alert("You are not authenticated. Please log in.");
    window.location.href = "/../login.html"; 
}

// Fetch all users
async function fetchUsers() {
    const response = await fetch(`${apiUrl}/User`, {
        headers: {
            Authorization: `Bearer ${token}`
        }
    });

    if (response.ok) {
        const users = await response.json();
        const tableBody = document.getElementById("usersTableBody");
        tableBody.innerHTML = ""; // Clear the table

        users.forEach(user => {
            const row = document.createElement("tr");
            row.innerHTML = `
                <td>${user.id}</td>
                <td>${user.name}</td>
                <td>${user.email}</td>
            `;
            tableBody.appendChild(row);
        });
    } else {
        alert("Failed to fetch users.");
    }
}

// Initial fetch
fetchUsers();