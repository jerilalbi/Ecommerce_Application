$(document).ready(() => {

    $.get('/Admin/GetSales').done((data) => {

        if (data.success) {

            createSalesByMonthChart(data.salesMonth)
            createSalesByCategoryChart(data.salesCategory)
            createTopProductsChart(data.topProduct)
        } else {
            console.log('No data found or API failed')
        }
        
    });

})

function createSalesByMonthChart(salesData) {
    const ctx = document.getElementById('salesByMonthChart').getContext('2d');

    const rawData = salesData;
    const labels = rawData.map(x => x.month);
    const data = rawData.map(x => x.total);


    const myChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                data: data,
                borderColor: 'black',
                borderWidth: 5,
                pointBackgroundColor: '#1a3300'
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                title: {
                    display: true,
                    text: 'Sales by Month',
                    align: 'start',
                    color: '#000',
                    font: {
                        size: 18,
                        weight: 'bold'
                    }
                },
                legend: {
                    display: false,
                }
            },
            scales: {
                x: {
                    grid: { display: false },
                    ticks: { color: '#000', font: { weight: 'bold' } },
                },
                y: {
                    grid: { display: false },
                    ticks: { color: '#000', font: { weight: 'bold' } },
                    beginAtZero: true
                }
            }
        }
    });
}

function createSalesByCategoryChart(categoryData) {
    const ctx = document.getElementById('salesByCategoryChart').getContext('2d');

    const rawData = categoryData;
    const labels = rawData.map(x => x.category);
    const data = rawData.map(x => x.total);


    const myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                data: data,
                borderWidth: 1,
                backgroundColor: 'black',
            }]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: 'Sales by Category',
                    align: 'start',
                    color: '#000',
                    font: {
                        size: 18,
                        weight: 'bold'
                    }
                },
                legend: {
                    display: false,
                },
                datalabels: {
                    color: '#000',
                    font: { weight: 'bold', size: 14 },
                    formatter: (value, ctx) => {
                        let label = ctx.chart.data.labels[ctx.dataIndex];
                        return label + '\n' + value;
                    }
                },
            },
            scales: {
                x: {
                    grid: { display: false },
                    ticks: { color: '#000', font: { weight: 'bold' } },
                },
                y: {
                    grid: { display: false },
                    ticks: { color: '#000', font: { weight: 'bold' } },
                    beginAtZero: true
                }
            }
        }
    });
}

function createTopProductsChart(productData) {
    const ctx = document.getElementById('salesByChannelChart').getContext('2d');

    const rawData = productData;
    const labels = rawData.map(x => x.ProductName);
    const data = rawData.map(x => x.ProductCount);



    const myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                data: data,
                borderWidth: 1,
                backgroundColor: 'black'
            }]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: 'Top Selling Products',
                    align: 'start',
                    color: '#000',
                    font: {
                        size: 18,
                        weight: 'bold'
                    }
                },
                legend: {
                    display: false,
                }
            },
            scales: {
                x: {
                    grid: { display: false },
                    ticks: { color: '#000', font: { weight: 'bold' } },
                },
                y: {
                    grid: { display: false },
                    ticks: { color: '#000', font: { weight: 'bold' } },
                    beginAtZero: true
                }
            }
        }
    });
}
