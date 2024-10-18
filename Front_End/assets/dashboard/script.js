// Global variables
let users = [];
let motorbikes = [];
let rentals = [];
const categories = ['Bike', 'Sport', 'Touring'];

const BASE_URL = 'https://localhost:7091/api'; 

async function loadData() {
    try {
        console.log('Starting to load data...');
        const [usersResponse, motorbikesResponse, rentalsResponse] = await Promise.all([
            fetch(`${BASE_URL}/user`),
            fetch(`${BASE_URL}/motorbike`),
            fetch(`${BASE_URL}/rental`)
        ]);

        console.log('Fetch responses:', {
            users: usersResponse.status,
            motorbikes: motorbikesResponse.status,
            rentals: rentalsResponse.status
        });

        if (!usersResponse.ok || !motorbikesResponse.ok || !rentalsResponse.ok) {
            throw new Error('Failed to fetch data from the server');
        }

        users = await usersResponse.json();
        motorbikes = await motorbikesResponse.json();
        rentals = await rentalsResponse.json();

        console.log('Data loaded successfully:', { users, motorbikes, rentals });

        updateUI();
        displayRawData(); // New function to display raw data for debugging
    } catch (error) {
        console.error('Failed to load data:', error);
        showAlert('An error occurred while loading data. Please check the console for details.');
    }
}

function updateUI() {
    updateMotorbikeTable();
    updateAvailableMotorbikes();
    updateRentalRequests();
    updateUserInfo();
    updateOrderHistory();
    updateCustomerRentals();
    checkOverdueRentals();
}

async function login(username, password, role) {
    try {
        console.log('Attempting login:', { username, role });
        const response = await fetch(`${BASE_URL}/user/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, password, role })
        });

        console.log('Login response status:', response.status);

        if (response.ok) {
            const user = await response.json();
            sessionStorage.setItem('currentUser', JSON.stringify(user));
            showAlert('Login successful');
            await loadData(); // Reload data after login
            return true;
        } else {
            showAlert('Invalid username or password');
            return false;
        }
    } catch (error) {
        console.error('Login failed:', error);
        showAlert('Login failed. Please try again.');
        return false;
    }
}

async function register(username, password, nic, role) {
    try {
        console.log('Attempting registration:', { username, nic, role });
        const response = await fetch(`${BASE_URL}/user/register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, password, nic, role })
        });

        console.log('Registration response status:', response.status);

        if (response.ok) {
            showAlert('Registration successful. Please login.');
            return true;
        } else {
            showAlert('Username already exists.');
            return false;
        }
    } catch (error) {
        console.error('Registration failed:', error);
        showAlert('Registration failed. Please try again.');
        return false;
    }
}

function logout() {
    sessionStorage.removeItem('currentUser');
    showAlert('You have been logged out.');
    location.href = 'index.html'; 
}

function getCurrentUser() {
    return JSON.parse(sessionStorage.getItem('currentUser'));
}

async function addMotorbike(regNumber, brand, model, category, imageData) {
    try {
        console.log('Adding motorbike:', { regNumber, brand, model, category });
        const response = await fetch(`${BASE_URL}/motorbike`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ regNumber, brand, model, category, imageData })
        });

        console.log('Add motorbike response status:', response.status);

        if (response.ok) {
            showAlert('Motorbike added successfully.');
            await loadData();
        } else {
            showAlert('Failed to add motorbike.');
        }
    } catch (error) {
        console.error('Failed to add motorbike:', error);
        showAlert('An error occurred while adding the motorbike.');
    }
}

async function removeMotorbike(motorbikeId) {
    try {
        console.log('Removing motorbike:', motorbikeId);
        const response = await fetch(`${BASE_URL}/motorbike/${motorbikeId}`, {
            method: 'DELETE'
        });

        console.log('Remove motorbike response status:', response.status);

        if (response.ok) {
            showAlert('Motorbike removed successfully.');
            await loadData(); 
        } else {
            showAlert('Failed to remove motorbike.');
        }
    } catch (error) {
        console.error('Failed to remove motorbike:', error);
        showAlert('An error occurred while removing the motorbike.');
    }
}

async function rentMotorbike(regNumber) {
    try {
        const currentUser = getCurrentUser();
        if (!currentUser) {
            showAlert('Please login to request a rental.');
            return;
        }

        console.log('Renting motorbike:', { regNumber, username: currentUser.username });

        const rentalRequest = {
            regNumber,
            username: currentUser.username,
            requestDate: new Date().toISOString(),
            status: 'pending'
        };

        const response = await fetch(`${BASE_URL}/rentalrequest`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(rentalRequest)
        });

        console.log('Rent motorbike response status:', response.status);

        if (response.ok) {
            showAlert('Rental request submitted.');
            await loadData(); 
        } else {
            showAlert('Failed to submit rental request.');
        }
    } catch (error) {
        console.error('Failed to rent motorbike:', error);
        showAlert('An error occurred while requesting rental.');
    }
}

async function approveRental(rentalRequestId) {
    try {
        console.log('Approving rental:', rentalRequestId);
        const response = await fetch(`${BASE_URL}/rentalrequest/${rentalRequestId}/status`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ status: 'approved', approvalDate: new Date().toISOString() })
        });

        console.log('Approve rental response status:', response.status);

        if (response.ok) {
            showAlert('Rental approved.');
            await loadData();
        } else {
            showAlert('Failed to approve rental.');
        }
    } catch (error) {
        console.error('Failed to approve rental:', error);
        showAlert('An error occurred while approving the rental.');
    }
}

function showAlert(message) {
    console.log('Alert:', message);
    const alertElement = document.getElementById('alertMessage');
    if (alertElement) {
        alertElement.textContent = message;
        alertElement.style.display = 'block';
        setTimeout(() => {
            alertElement.style.display = 'none';
        }, 3000);
    } else {
        alert(message); 
    }
}

function updateMotorbikeTable() {
    console.log('Updating motorbike table');
    const tableBody = document.querySelector('#motorbikeTable tbody');
    if (!tableBody) {
        console.error('Motorbike table body not found');
        return;
    }

    tableBody.innerHTML = '';
    motorbikes.forEach((bike, index) => {
        const row = tableBody.insertRow();
        row.innerHTML = `
            <td>${bike.regNumber}</td>
            <td>${bike.brand}</td>
            <td>${bike.model}</td>
            <td>${bike.category}</td>
            <td><button onclick="removeMotorbike(${bike.motorbikeId})">Remove</button></td>
        `;
    });
    console.log('Motorbike table updated with', motorbikes.length, 'bikes');
}

function updateAvailableMotorbikes() {
    console.log('Updating available motorbikes');
    const availableBikes = document.getElementById('availableBikes');
    if (!availableBikes) {
        console.error('Available bikes container not found');
        return;
    }

    availableBikes.innerHTML = '';
    const availableMotorbikes = motorbikes.filter(bike => !bike.isRented);
    availableMotorbikes.forEach(bike => {
        const bikeElement = document.createElement('div');
        bikeElement.className = 'bike-item';
        bikeElement.innerHTML = `
            <h3>${bike.brand} ${bike.model}</h3>
            <p>Registration: ${bike.regNumber}</p>
            <p>Category: ${bike.category}</p>
            <button onclick="rentMotorbike('${bike.regNumber}')">Rent</button>
        `;
        availableBikes.appendChild(bikeElement);
    });
    console.log('Available motorbikes updated with', availableMotorbikes.length, 'bikes');
}

function updateRentalRequests() {
    console.log('Updating rental requests');
    const requestsList = document.getElementById('rentalRequests');
    if (!requestsList) {
        console.error('Rental requests list not found');
        return;
    }

    requestsList.innerHTML = '';
    const pendingRentals = rentals.filter(rental => rental.status === 'pending');
    pendingRentals.forEach(rental => {
        const requestElement = document.createElement('li');
        requestElement.innerHTML = `
            <p>User: ${rental.username}</p>
            <p>Motorbike: ${rental.regNumber}</p>
            <p>Date: ${new Date(rental.requestDate).toLocaleDateString()}</p>
            <button onclick="approveRental(${rental.rentalId})">Approve</button>
        `;
        requestsList.appendChild(requestElement);
    });
    console.log('Rental requests updated with', pendingRentals.length, 'requests');
}

function updateUserInfo() {
    console.log('Updating user info');
    const userInfoElement = document.getElementById('userInfo');
    if (!userInfoElement) {
        console.error('User info element not found');
        return;
    }

    const currentUser = getCurrentUser();
    if (currentUser) {
        userInfoElement.innerHTML = `
            <p>Username: ${currentUser.username}</p>
            <p>Role: ${currentUser.role}</p>
            <button onclick="logout()">Logout</button>
        `;
    } else {
        userInfoElement.innerHTML = '<p>Not logged in</p>';
    }
    console.log('User info updated');
}

function updateOrderHistory() {
    console.log('Updating order history');
    const orderHistoryElement = document.getElementById('orderHistory');
    if (!orderHistoryElement) {
        console.error('Order history element not found');
        return;
    }

    const currentUser = getCurrentUser();
    if (!currentUser) {
        console.log('No current user, skipping order history update');
        return;
    }

    orderHistoryElement.innerHTML = '';
    const userRentals = rentals.filter(rental => rental.username === currentUser.username);
    userRentals.forEach(rental => {
        const orderElement = document.createElement('div');
        orderElement.innerHTML = `
            <p>Motorbike: ${rental.regNumber}</p>
            <p>Status: ${rental.status}</p>
            <p>Date: ${new Date(rental.requestDate).toLocaleDateString()}</p>
        `;
        orderHistoryElement.appendChild(orderElement);
    });
    console.log('Order history updated with', userRentals.length, 'orders');
}

function updateCustomerRentals() {
    console.log('Updating customer rentals');
    const customerRentalsElement = document.getElementById('customerRentals');
    if (!customerRentalsElement) {
        console.error('Customer rentals element not found');
        return;
    }

    customerRentalsElement.innerHTML = '';
    const approvedRentals = rentals.filter(rental => rental.status === 'approved');
    approvedRentals.forEach(rental => {
        const rentalElement = document.createElement('div');
        rentalElement.innerHTML = `
            <p>User: ${rental.username}</p>
            <p>Motorbike: ${rental.regNumber}</p>
            <p>Start Date: ${new Date(rental.approvalDate).toLocaleDateString()}</p>
        `;
        customerRentalsElement.appendChild(rentalElement);
    });
    console.log('Customer rentals updated with', approvedRentals.length, 'rentals');
}

function checkOverdueRentals() {
    console.log('Checking overdue rentals');
    const overdueRentalsElement = document.getElementById('overdueRentals');
    if (!overdueRentalsElement) {
        console.error('Overdue rentals element not found');
        return;
    }

    const currentDate = new Date();
    const overdueRentals = rentals.filter(rental => {
        const rentalDate = new Date(rental.approvalDate);
        const daysPassed = (currentDate - rentalDate) / (1000 * 60 * 60 * 24);
        return rental.status === 'approved' && daysPassed > 7;
    });

    overdueRentalsElement.innerHTML = '';
    overdueRentals.forEach(rental => {
        const overdueElement = document.createElement('div');
        overdueElement.innerHTML = `
            <p>User: ${rental.username}</p>
            <p>Motorbike: ${rental.regNumber}</p>
            <p>Start Date: ${new Date(rental.approvalDate).toLocaleDateString()}</p>
        `;
        overdueRentalsElement.appendChild(overdueElement);
    });
    console.log('Overdue rentals updated with', overdueRentals.length, 'rentals');
}

function displayRawData() {
    console.log('Displaying raw data');
    const rawDataContainer = document.getElementById('rawData');
    if (!rawDataContainer) {
        console.error('Raw data container not found');
        return;
    }

    rawDataContainer.innerHTML = `
        <h3>Raw Data (for debugging)</h3>
        <h4>Users:</h4>
        <pre>${JSON.stringify(users, null, 2)}</pre>
        <h4>Motorbikes:</h4>
        <pre>${JSON.stringify(motorbikes, null, 2)}</pre>
        <h4>Rentals:</h4>
        <pre>${JSON.stringify(rentals, null, 2)}</pre>
    `;
    console.log('Raw data displayed');
}

document.addEventListener('DOMContentLoaded', async function () {
    console.log('DOM content loaded');
    try {
        await loadData();
        setupEventListeners();
    } catch (error) {
        console.error('Initialization failed:', error);
    }
});

function setupEventListeners() {
    console.log('Setting up event listeners');
    const loginForm = document.getElementById('loginForm');
    const registerForm = document.getElementById('registerForm');
    const addMotorbikeForm = document.getElementById('addMotorbikeForm');
    const logoutBtn = document.getElementById('logoutBtn');

    if (loginForm) {
        loginForm.addEventListener('submit', handleLogin);
    } else {
        console.warn('Login form not found');
    }

    if (registerForm) {
        registerForm.addEventListener('submit', handleRegister);
    } else {
        console.warn('Register form not found');
    }

    if (addMotorbikeForm) {
        addMotorbikeForm.addEventListener('submit', handleAddMotorbike);
    } else {
        console.warn('Add motorbike form not found');
    }

    if (logoutBtn) {
        logoutBtn.addEventListener('click', logout);
    } else {
        console.warn('Logout button not found');
    }

    console.log('Event listeners setup complete');
}

async function handleLogin(e) {
    e.preventDefault();
    console.log('Login form submitted');
    const username = document.getElementById('loginUsername').value;
    const password = document.getElementById('loginPassword').value;
    const role = document.getElementById('loginRole').value;
    if (await login(username, password, role)) {
        console.log('Login successful, redirecting');
        location.href = role === 'manager' ? 'd-manager.html' : 'd-customer.html';
    } else {
        console.log('Login failed');
    }
}

async function handleRegister(e) {
    e.preventDefault();
    console.log('Register form submitted');
    const username = document.getElementById('registerUsername').value;
    const password = document.getElementById('registerPassword').value;
    const nic = document.getElementById('registerNIC').value;
    const role = document.getElementById('registerRole').value;
    if (await register(username, password, nic, role)) {
        console.log('Registration successful');
        showAlert('Registration successful. Please login.');
        e.target.reset();
    } else {
        console.log('Registration failed');
    }
}

async function handleAddMotorbike(e) {
    e.preventDefault();
    console.log('Add motorbike form submitted');
    const regNumber = document.getElementById('regNumber').value;
    const brand = document.getElementById('brand').value;
    const model = document.getElementById('model').value;
    const category = document.getElementById('category').value;
    const imageFile = document.getElementById('bikeImage').files[0];

    if (imageFile) {
        const reader = new FileReader();
        reader.onload = async function (event) {
            await addMotorbike(regNumber, brand, model, category, event.target.result);
        };
        reader.readAsDataURL(imageFile);
    } else {
        await addMotorbike(regNumber, brand, model, category, null);
    }
    e.target.reset();
}

// Utility functions for debugging
function logNetworkRequests() {
    const originalFetch = window.fetch;
    window.fetch = function() {
        console.log('Fetch request:', arguments[0], arguments[1]);
        return originalFetch.apply(this, arguments).then(response => {
            console.log('Fetch response:', response.url, response.status);
            return response;
        });
    };
    console.log('Network request logging enabled');
}

function checkLocalStorage() {
    console.log('Local Storage contents:');
    for (let i = 0; i < localStorage.length; i++) {
        const key = localStorage.key(i);
        console.log(key, localStorage.getItem(key));
    }
}

function checkSessionStorage() {
    console.log('Session Storage contents:');
    for (let i = 0; i < sessionStorage.length; i++) {
        const key = sessionStorage.key(i);
        console.log(key, sessionStorage.getItem(key));
    }
}

// Call these functions at the end of the DOMContentLoaded event listener
document.addEventListener('DOMContentLoaded', async function () {
    console.log('DOM content loaded');
    try {
        logNetworkRequests();
        await loadData();
        setupEventListeners();
        checkLocalStorage();
        checkSessionStorage();
        console.log('Initialization complete');
    } catch (error) {
        console.error('Initialization failed:', error);
    }
});

// Export functions for testing if needed
if (typeof module !== 'undefined' && module.exports) {
    module.exports = {
        login,
        register,
        loadData,
        addMotorbike,
        removeMotorbike,
        rentMotorbike,
        approveRental
    };
}

// start: Tab
(function () {
    const panes = document.querySelectorAll("[data-tab-pane]");
    const pages = document.querySelectorAll("[data-tab-page]");
    panes.forEach(function (item, i) {
        item.addEventListener("click", function (e) {
            e.preventDefault();
            const target = document.querySelector(
                '[data-tab-page="' + item.dataset.tabPane + '"]'
            );
            const active = document.querySelector("[data-tab-pane].active");
            if (active) {
                const activeIndex = Array.from(panes).indexOf(active);
                panes.forEach(function (el, x) {
                    el.classList.remove("active");
                    el.classList.toggle("before", x < i);
                    el.classList.toggle("after", x > i);
                    el.classList.toggle(
                        "slow",
                        Math.abs(activeIndex - x) > 0 && item !== el
                    );
                    el.style.setProperty(
                        "--delay",
                        `${active === el
                            ? 0
                            : (Math.abs(activeIndex - x) - 1) * 150
                        }ms`
                    );
                });
            }
            if (target) {
                pages.forEach(function (el) {
                    el.classList.remove("active");
                });
                target.classList.add("active");
            }
            item.classList.add("active");
        });
    });
})();
// end: Tab
