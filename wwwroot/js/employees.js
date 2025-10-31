let token = "";
const apiUrl = "/api/employees";

$(document).ready(() => {

    // Login
    $("#btnLogin").click(async () => {
        const username = $("#username").val();
        const password = $("#password").val();

        try {
            const res = await axios.post(`${apiUrl}/login`, { username, password });
            token = res.data.token;
            localStorage.setItem("jwt", token);

            $("#loginSection").hide();
            $("#crudSection").show();
            loadEmployees();
        } catch {
            alert("Invalid login");
        }
    });

    // Load employee list
    async function loadEmployees() {
        try {
            const res = await axios.get(apiUrl, {
                headers: { Authorization: `Bearer ${token}` }
            });
            renderTable(res.data);
        } catch {
            alert("Failed to load employees.");
        }
    }

    // Render table
    function renderTable(data) {
        const tbody = $("#empTableBody");
        tbody.empty();
        data.forEach(emp => {
            tbody.append(`
                <tr>
                    <td>${emp.id}</td>
                    <td>${emp.name}</td>
                    <td>${emp.position}</td>
                    <td>${emp.salary}</td>
                    <td>
                        <button class="btn btn-sm btn-warning me-2 btn-edit" data-id="${emp.id}">Edit</button>
                        <button class="btn btn-sm btn-danger btn-del" data-id="${emp.id}">Delete</button>
                    </td>
                </tr>
            `);
        });
    }

    // Save employee
    $("#btnSave").click(async () => {
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
            $("#empModal").modal("hide");
            loadEmployees();
        } catch {
            alert("Save failed!");
        }
    });

    // Edit employee
    $(document).on("click", ".btn-edit", async function () {
        const id = $(this).data("id");
        try {
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
        } catch {
            alert("Error loading data");
        }
    });

    // Delete employee
    $(document).on("click", ".btn-del", async function () {
        const id = $(this).data("id");
        if (confirm("Are you sure to delete this employee?")) {
            try {
                await axios.delete(`${apiUrl}/${id}`, {
                    headers: { Authorization: `Bearer ${token}` }
                });
                loadEmployees();
            } catch {
                alert("Delete failed!");
            }
        }
    });

});
