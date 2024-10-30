const BASE_URL = 'https://localhost:7091/api';


let users = [];
let motorbikes = [];
let rentals = [];
const categories = ['Bike', 'Sport', 'Touring'];


async function loadData() {
    try {
        const [usersResponse, motorbikesResponse, rentalsResponse] = await Promise.all([
            fetch(`${BASE_URL}/User`),
            fetch(`${BASE_URL}/Motorbike`),
            fetch(`${BASE_URL}/Rental`)
        ]);

        if (!usersResponse.ok || !motorbikesResponse.ok || !rentalsResponse.ok) {
            throw new Error('Failed to fetch data from the server');
        }

        users = await usersResponse.json();
        motorbikes = await motorbikesResponse.json();
        rentals = await rentalsResponse.json();

        console.log('Data loaded successfully:', { users, motorbikes, rentals });


        const currentPath = window.location.pathname;

        if (currentPath.includes("d-customer.html")) {
            updateAvailableMotorbikes();
        }
        else {
            updateMotorbikeTable()

        }



        updateRentalRequests();
        updateUserInfo();
        updateOrderHistory();
        updateCustomerRentals();
        checkOverdueRentals();
    } catch (error) {
        console.error('Failed to load data:', error.message);
        alert('An error occurred while loading data. Please check the console for details.');
    }
}


async function login(username, password, role) {
    const response = await fetch(`${BASE_URL}/User/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password, role })
    });

    if (response.ok) {
        const user = await response.json();
        sessionStorage.setItem('currentUser', JSON.stringify(user));
        alert('Login successful');
        location.href = role === 'manager' ? 'd-manager.html' : 'd-customer.html';
        return true;
    } else {
        alert('Invalid Username or Password');
        return false;
    }
}

function populateCategoryDropdown() {
    const categorySelect = document.getElementById('category');
    if (categorySelect) {
        categorySelect.innerHTML = '';

        categories.forEach(category => {
            const option = document.createElement('option');
            option.value = category;
            option.textContent = category;
            categorySelect.appendChild(option);
        });
    }
}

async function register(username, password, nic, role) {
    const response = await fetch(`${BASE_URL}/User/register`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password, nic, role })
    });

    if (response.ok) {
        alert('Registration successful. Please login.');
        return true;
    } else {
        alert('Username already exists.');
        return false;
    }
}

function logout() {
    sessionStorage.removeItem('currentUser');
    alert('You have been logged out.');
    location.href = 'index.html';
}

function getCurrentUser() {
    return JSON.parse(sessionStorage.getItem('currentUser'));
}


async function addMotorbike(regNumber, brand, model, category, imageData) {
    try {
        const response = await fetch(`${BASE_URL}/Motorbike`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ regNumber, brand, model, category, imageData })
        });

        if (response.ok) {
            alert('Motorbike added successfully.');
            await loadData();
        } else {
            const errorMessage = await response.text();
            console.error('Failed to add motorbike:', errorMessage);
            alert('Failed to add motorbike. Please check the console for details.');
        }
    } catch (error) {
        console.error('An error occurred while adding the motorbike:', error.message);
        alert('An error occurred while adding the motorbike. Please check the console for details.');
    }
}

async function removeMotorbike(index) {
    const motorbike = motorbikes[index];
    const response = await fetch(`${BASE_URL}/Motorbike/${motorbike.motorbikeId}`, {
        method: 'DELETE'
    });

    if (response.ok) {
        alert('Motorbike removed successfully.');
        await loadData();
    } else {
        alert('Failed to remove motorbike.');
    }
}

// Rental management functions
async function rentMotorbike(regNumber) {
    const currentUser = getCurrentUser();
    if (!currentUser) {
        alert('Please login to request a rental.');
        return;
    }

    const rentalRequest = {
        regNumber,
        username: currentUser.username,
        requestDate: new Date().toISOString(),
        status: 'pending'
    };

    const response = await fetch(`${BASE_URL}/RentalRequest`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(rentalRequest)
    });

    if (response.ok) {
        alert('Rental request submitted.');
        await loadData();
    } else {
        alert('Failed to submit rental request.');
    }
}

async function approveRental(index) {
    const rentalRequests = await fetchData(`${BASE_URL}/RentalRequest`);
    const request = rentalRequests[index];
    const response = await fetch(`${BASE_URL}/RentalRequest/${request.id}/status`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ status: 'approved', approvalDate: new Date().toISOString() })
    });

    if (response.ok) {
        alert('Rental approved.');
        await loadData();
    } else {
        alert('Failed to approve rental.');
    }
}

function updateMotorbikeTable() {
    const motorbikeTableBody = document.getElementById('motorbikeTableBody');

    console.log(motorbikeTableBody)
    motorbikeTableBody.innerHTML = '';
    motorbikes.forEach((motorbike, index) => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${motorbike.regNumber}</td>
            <td>${motorbike.brand}</td>
            <td>${motorbike.model}</td>
            <td>${motorbike.category}</td>
            <td>
                <button class="btn btn-primary btn-sm" onclick="updateMotorbike(${index})">Update</button>
                <button class="btn btn-danger btn-sm" onclick="removeMotorbike(${index})">Remove</button>
            </td>`
            ;
        motorbikeTableBody.appendChild(row);
    });
}

function updateAvailableMotorbikes() {
    console.log("hi")
    const availableMotorbikeTableBody = document.getElementById('availableMotorbikeTableBody');
    console.log(availableMotorbikeTableBody)
    availableMotorbikeTableBody.innerHTML = '';

    motorbikes.forEach((motorbike) => {
        const row = document.createElement('tr');


        const imageCell = motorbike.imageData
            ? `<img src="${motorbike.imageData}" style="width: 100px;">`
            : 'No image';

        row.innerHTML = `
            <td>${motorbike.regNumber}</td>
            <td>${motorbike.brand}</td>
            <td>${motorbike.model}</td>
            <td>${motorbike.category}</td>
            <td>${imageCell}</td>
            <td>
                <button class="btn btn-primary btn-sm" onclick="rentMotorbike('${motorbike.regNumber}')">
                    Request Rental
                </button>
            </td>
        `;

        availableMotorbikeTableBody.appendChild(row);
    });
}



document.addEventListener('DOMContentLoaded', async () => {
    try {

        await loadData();


        populateCategoryDropdown();


        updateUserInfo();
        updateMotorbikeTable();
        updateAvailableMotorbikes();
        checkOverdueRentals();
        updateOrderHistory();
        updateRentalRequests();
        updateCustomerRentals();
    } catch (error) {
        console.error('Initialization failed:', error.message);
    }


    const loginForm = document.getElementById('loginForm');
    const registerForm = document.getElementById('registerForm');
    const addMotorbikeForm = document.getElementById('addMotorbikeForm');
    const logoutBtn = document.getElementById('logoutBtn');
    const updateMotorbikeForm = document.getElementById('updateMotorbikeForm');

    if (loginForm) {
        loginForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            const username = document.getElementById('loginUsername').value;
            const password = document.getElementById('loginPassword').value;
            const role = document.getElementById('loginRole').value;
            if (await login(username, password, role)) {
                location.href = role === 'manager' ? 'd-manager.html' : 'd-customer.html';
            }
        });
    }

    if (registerForm) {
        registerForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            const username = document.getElementById('registerUsername').value;
            const password = document.getElementById('registerPassword').value;
            const nic = document.getElementById('registerNIC').value;
            const role = document.getElementById('registerRole').value;
            if (await register(username, password, nic, role)) {
                alert('Registration successful. Please login.');
                registerForm.reset();
            }
        });
    }

    if (addMotorbikeForm) {
        addMotorbikeForm.addEventListener('submit', async (e) => {
            e.preventDefault();
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
            addMotorbikeForm.reset();
        });
    }

    if (logoutBtn) {
        logoutBtn.addEventListener('click', () => {
            logout();
            location.href = 'index.html';
        });
    }

    if (updateMotorbikeForm) {
        updateMotorbikeForm.addEventListener('submit', saveUpdatedMotorbike);
    }
});

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
