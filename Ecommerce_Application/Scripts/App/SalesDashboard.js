$(document).ready(() => {

    if (!window.dashboardSalesData) {

        $('#loadingSpinnerSales').show();

        $.get('/Admin/GetSales').done((data) => {

            if (data.success) {
                window.dashboardSalesData = data
                createSalesByMonthChart(data.salesMonth)
                createSalesByCategoryChart(data.salesCategory)
                createTopProductsChart(data.topProduct)
            } else {
                console.log('No data found or API failed')
            }
        }).always(() => {
            $('#loadingSpinnerSales').hide();
        })
    } else {
        createSalesByMonthChart(window.dashboardSalesData.salesMonth)
        createSalesByCategoryChart(window.dashboardSalesData.salesCategory)
        createTopProductsChart(window.dashboardSalesData.topProduct)
    }

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
                    borderColor: '#36A2EB',
                    borderWidth: 5,
                    pointBackgroundColor: '#36A2EB'
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

        const chartColors = [
            '#36A2EB',
            '#FF6384',
            '#FFCD56',
            '#4BC0C0',
            '#9966FF',
            '#FF9F40',
            '#A0522D',
            '#2ECC40',
            '#FF69B4',
            '#6A5ACD'
        ];

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
                    backgroundColor: chartColors,
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

        const chartColors = [
            '#36A2EB',
            '#FF6384',
            '#FFCD56',
            '#4BC0C0',
            '#9966FF',
            '#FF9F40',
            '#A0522D',
            '#2ECC40',
            '#FF69B4',
            '#6A5ACD'
        ];

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
                    backgroundColor: chartColors
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

})
