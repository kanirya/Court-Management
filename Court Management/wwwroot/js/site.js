// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
        // Initialize tooltips
    document.addEventListener('DOMContentLoaded', function() {
            var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
                return new bootstrap.Tooltip(tooltipTriggerEl)
            });

    // Search functionality
    const searchInput = document.getElementById('searchInput');
    const statusFilter = document.getElementById('statusFilter');
    const dateFilter = document.getElementById('dateFilter');
    const resetFilters = document.getElementById('resetFilters');
    const tableRows = document.querySelectorAll('.case-table tbody tr');

    function filterTable() {
                const searchTerm = searchInput.value.toLowerCase();
    const statusValue = statusFilter.value;
    const dateValue = dateFilter.value;

                tableRows.forEach(row => {
                    const title = row.querySelector('h6').textContent.toLowerCase();
    const description = row.querySelector('small').textContent.toLowerCase();
    const status = row.querySelector('.badge').textContent;
    const dateCreated = row.querySelectorAll('td')[2].textContent;

    let showRow = true;

    // Search filter
    if (searchTerm && !title.includes(searchTerm) && !description.includes(searchTerm)) {
        showRow = false;
                    }

    // Status filter
    if (statusValue && status !== statusValue) {
        showRow = false;
                    }

    // Date filter (simplified - would need more logic for actual date filtering)
    if (dateValue) {
        // This is a placeholder for actual date filtering logic
        // You would need to implement proper date comparison based on your requirements
    }

    row.style.display = showRow ? '' : 'none';
                });
            }

    searchInput.addEventListener('input', filterTable);
    statusFilter.addEventListener('change', filterTable);
    dateFilter.addEventListener('change', filterTable);

    resetFilters.addEventListener('click', function() {
        searchInput.value = '';
    statusFilter.value = '';
    dateFilter.value = '';
                tableRows.forEach(row => {
        row.style.display = '';
                });
            });
        });
