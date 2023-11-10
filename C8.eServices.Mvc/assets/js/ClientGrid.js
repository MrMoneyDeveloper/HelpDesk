new gridjs.Grid({
    columns: [{
        name: 'RowId',
        hidden: true
    },
        "Company", "Country", "Contact Person", "Email", "Industry",
    {
        name: '',
        formatter: (_, row) =>
            gridjs.html(`<button type="button" onclick="window.location.href='/Onboarding/Details?q=${row.cells[0].data}'" class="btn btn-outline-info"><i class="uil uil-info-circle me-2"></i>View Details</button>`)
    }],
    sort: true,
    pagination: true,
    fixedHeader: true,
    height: "400px",
    data: () => {
        return new Promise((resolve) => {
            fetch('/Onboarding/loadClients',)
                .then(response => response.json())
                .then(data => {
                    const mappedData = data.map(item => [
                        item.Data,
                        item.Company,
                        item.Countr,
                        item.PrimaryContactPerson,
                        item.StatusValue,
                        item.Industr
                    ]);
                    resolve(mappedData);
                });

        });
    }
}).render(document.getElementById("table-fixed-header2"));