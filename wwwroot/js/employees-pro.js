let token = "";
let userRole = "";
let table;
const apiUrl = "/api/employees";

$(document).ready(() => {

    // LOGIN
    $("#btnLogin").click(async () => {
        const username = $("#username").val();
        const password = $("#password").val();

        try {
            const res = await axios.post(`${apiUrl}/login`, { username, password });
            token = res.data.token;
            userRole = parseJwt(token).role;
            localStorage.setItem("jwt", token);

            Swal.fire({
                icon: "success",
                title: "Login Successful!",
                text: `Welcome ${username} (${userRole})`,
                showConfirmButton: false,
                timer: 1200
            });

            $("#loginSection").hide();
            $("#crudSection").fadeIn();
            $("#userInfo").text(`👤 ${username} (${userRole})`);

            if (userRole !== "Admin") {
                $("#btnAdd").hide();
            }

            loadEmployees();
        } catch {
            Swal.fire("Error", "Invalid credentials!", "error");
        }
    });

    // LOAD EMPLOYEES
    async function loadEmployees() {
        try {
            const res = await axios.get(apiUrl, {
                headers: { Authorization: `Bearer ${token}` }
            });

            if (table) table.destroy(); // reset DataTable

            table = $("#empTable").DataTable({
                data: res.data,
                columns: [
                    { data: "id" },
                    { data: "name" },
                    { data: "position" },
                    { data: "salary" },
                    {
                        data: null,
                        render: (data) => {
                            if (userRole === "Admin") {
                                return `
                                    <button class="btn btn-sm btn-warning btn-edit me-1" data-id="${data.id}">Edit</button>
                                    <button class="btn btn-sm btn-danger btn-del" data-id="${data.id}">Delete</button>
                                `;
                            } else {
                                return `<span class="text-muted">View Only</span>`;
                            }
                        }
                    }
                ]
            });
        } catch {
            Swal.fire("Error", "Failed to load employees.", "error");
        }
    }

    // OPEN MODAL ADD
    $("#btnAdd").click(() => {
        $("#empId").val("");
        $("#empName").val("");
        $("#empPosition").val("");
        $("#empSalary").val("");
        $("#empModalLabel").text("Add Employee");
        new bootstrap.Modal($("#empModal")).show();
    });

    // SAVE (ADD/EDIT)
    $("#btnSave").click(async () => {
        if (userRole !== "Admin") return;

        const id = $("#empId").val();
        const emp = {
            id: id || 0,
            name: $("#empName").val(),
            position: $("#empPosition").val(),
            salary: parseFloat($("#empSalary").val())
        };

        try {
            if (id) {
                await axios.put(apiUrl, emp, {
                    headers: { Authorization: `Bearer ${token}` }
                });
            } else {
                await axios.post(apiUrl, emp, {
                    headers: { Authorization: `Bearer ${token}` }
                });
            }

            Swal.fire("Success", "Employee saved!", "success");
            $("#empModal").modal("hide");
            loadEmployees();
        } catch {
            Swal.fire("Error", "Save failed!", "error");
        }
    });

    // EDIT
    $(document).on("click", ".btn-edit", async function () {
        if (userRole !== "Admin") return;

        const id = $(this).data("id");
        const res = await axios.get(apiUrl, {
            headers: { Authorization: `Bearer ${token}` }
        });
        const emp = res.data.find(e => e.id === id);

        if (emp) {
            $("#empId").val(emp.id);
            $("#empName").val(emp.name);
            $("#empPosition").val(emp.position);
            $("#empSalary").val(emp.salary);
            $("#empModalLabel").text("Edit Employee");
            new bootstrap.Modal($("#empModal")).show();
        }
    });

    // DELETE
    $(document).on("click", ".btn-del", function () {
        if (userRole !== "Admin") return;

        const id = $(this).data("id");

        Swal.fire({
            title: "Are you sure?",
            text: "This employee will be deleted.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Yes, delete it!"
        }).then(async (result) => {
            if (result.isConfirmed) {
                try {
                    await axios.delete(`${apiUrl}/${id}`, {
                        headers: { Authorization: `Bearer ${token}` }
                    });
                    Swal.fire("Deleted!", "Employee has been removed.", "success");
                    loadEmployees();
                } catch {
                    Swal.fire("Error", "Delete failed!", "error");
                }
            }
        });
    });

    // Parse JWT (đọc role từ token)
    function parseJwt(token) {
        try {
            const base64Url = token.split('.')[1];
            const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
            const jsonPayload = decodeURIComponent(atob(base64).split('').map(c =>
                '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2)
            ).join(''));
            return JSON.parse(jsonPayload);
        } catch {
            return {};
        }
    }
});
