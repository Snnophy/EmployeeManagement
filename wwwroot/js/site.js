$(document).ready(function() {
    // Unbind any previous event handlers to prevent duplicate handlers
    $('#employeeForm').off('submit').on('submit', function(e) {
        e.preventDefault();

        var id = $('#EmployeeId').val();
        var employee = {
            Id: id ? parseInt(id) : 0,
            Name: $('#Name').val(),
            Position: $('#Position').val(),
            Office: $('#Office').val(),
            Salary: parseFloat($('#Salary').val())
        };

        var url = id ? '/Employee/UpdateEmployee' : '/Employee/AddEmployee';
        var method = id ? 'PUT' : 'POST';

        $.ajax({
            url: url,
            type: method,
            contentType: 'application/json',
            data: JSON.stringify(employee),
            success: function(result) {
                if (id) {
                    // Update existing employee
                    var row = $(`tr[data-id="${id}"]`);
                    row.find('td:eq(1)').text(result.name);
                    row.find('td:eq(2)').text(result.position);
                    row.find('td:eq(3)').text(result.office);
                    row.find('td:eq(4)').text(parseFloat(result.salary).toFixed(2));
                } else {
                    // Add new employee
                    $('#employeesTable').append(
                        `<tr data-id="${result.id}">
                            <td>${result.id}</td>
                            <td>${result.name}</td>
                            <td>${result.position}</td>
                            <td>${result.office}</td>
                            <td>${parseFloat(result.salary).toFixed(2)}</td>
                            <td>
                                <button class="btn btn-warning btn-sm edit-btn" data-id="${result.id}">Edit</button>
                                <button class="btn btn-danger btn-sm delete-btn" data-id="${result.id}">Delete</button>
                            </td>
                        </tr>`
                    );
                }
                $('#employeeForm')[0].reset();
                $('#EmployeeId').val('');
                $('#submitBtn').text('Add Employee').prop('disabled', false);
            },
            error: function(xhr, status, error) {
                console.error('Error:', error);
                alert('Error processing request.');
                $('#submitBtn').prop('disabled', false); // Re-enable the button if there's an error
            }
        });
    });

    // Handle Edit Employee
    $(document).on('click', '.edit-btn', function() {
        var id = $(this).data('id');

        $.ajax({
            url: `/Employee/GetEmployee/${id}`,
            type: 'GET',
            success: function(result) {
                $('#EmployeeId').val(result.id);
                $('#Name').val(result.name);
                $('#Position').val(result.position);
                $('#Office').val(result.office);
                $('#Salary').val(result.salary);
                $('#submitBtn').text('Update Employee');
            },
            error: function(xhr, status, error) {
                console.error('Error:', error);
                alert('Error fetching employee details.');
            }
        });
    });

    // Handle Delete Employee
    $(document).on('click', '.delete-btn', function() {
        var id = $(this).data('id');

        $.ajax({
            url: '/Employee/DeleteEmployee',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(id), // Envía el ID como un valor numérico, no como un objeto con una propiedad 'id'
            success: function() {
                $(`tr[data-id="${id}"]`).remove();
            },
            error: function(xhr, status, error) {
                console.error('Error:', error);
                alert('Error deleting employee.');
            }
        });
    });


    // Load employees function
    function loadEmployees() {
        $.getJSON('/Employee/GetEmployees', function(data) {
            var tableBody = $('#employeesTable');
            tableBody.empty();
            $.each(data, function(i, employee) {
                tableBody.append(
                    `<tr data-id="${employee.id}">
                        <td>${employee.id}</td>
                        <td>${employee.name}</td>
                        <td>${employee.position}</td>
                        <td>${employee.office}</td>
                        <td>${parseFloat(employee.salary).toFixed(2)}</td>
                        <td>
                            <button class="btn btn-warning btn-sm edit-btn" data-id="${employee.id}">Edit</button>
                            <button class="btn btn-danger btn-sm delete-btn" data-id="${employee.id}">Delete</button>
                        </td>
                    </tr>`
                );
            });
        });
    }
});
